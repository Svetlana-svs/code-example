//  Service.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include <tchar.h>
#include <stdio.h>
#include <conio.h>
#include <windows.h>

#include "DFFViewer.h"
#include "Helpers.h"

// Application
const TCHAR g_szVersion[] = L"1.3.0";
const TCHAR g_szServiceName[] = L"FaceDetector";
const int g_nBufferSize = MAX_PATH-1;
TCHAR g_szLogFile[MAX_PATH];
bool g_bIsConsole;

VOID AppExecute(LPCTSTR lpCmdLineData);
VOID Install(LPCTSTR pPath);
VOID UnInstall();
BOOL LaunchServiceStop();
BOOL LaunchServiceStart();

// Business process
VOID StartServiceDispatcherLoop();
VOID RunBusinessProcessLoop();

// Windows Service interface
VOID WINAPI ServiceMain(DWORD dwArgc, LPTSTR *lpszArgv);
VOID WINAPI ServiceHandler(DWORD fdwControl);
SERVICE_STATUS_HANDLE	g_hServiceStatusHandle; 
SERVICE_STATUS			g_stServiceStatus; 

// synchronization
CRITICAL_SECTION	g_csLog;
SyncInt	g_nControlRequest, g_nControlState;

// Aplication entry point
int _tmain(int argc, _TCHAR* argv[])
{
	TCHAR szCmdLineData[g_nBufferSize+1];
	if(argc >= 2)
		wcsncpy(szCmdLineData, argv[1], g_nBufferSize);
	
	DWORD code = 0; 
	try
	{
		AppExecute(szCmdLineData);
	}
	catch (TCHAR *err)
	{
		DWORD code = GetLastError();
		LogDebugString2(err, code);
	}
	return code;
}

VOID AppExecute(LPCTSTR lpCmdLineData)
{
	::InitializeCriticalSection(&g_csLog);
	g_nControlRequest = 0;
	g_nControlState = 0;

	// initialize variables for .exe and .log file names
	TCHAR pModuleFile[g_nBufferSize+1];
	DWORD dwSize = GetModuleFileName(NULL, pModuleFile, g_nBufferSize);
	pModuleFile[dwSize] = 0;

	// make path for the log file
	wcsncpy(g_szLogFile, pModuleFile, g_nBufferSize);
	LPTSTR lpDot = wcsrchr(g_szLogFile, '.');
	if (lpDot)
	{
		size_t nRest = wcslen(++lpDot);
		wcsncpy(lpDot, L"log", nRest);
	}

#ifdef FD_TRIAL
	g_bIsConsole = true;
	g_nControlRequest = 1;
	RunBusinessProcessLoop(); // wait until business process does finish
#else
	// make path for service executable
	wcsncat(pModuleFile, L" -s", g_nBufferSize - wcslen(pModuleFile));

	g_bIsConsole = false;
	if(_wcsicmp(L"-i", lpCmdLineData) == 0)
		Install(pModuleFile);
	else if(_wcsicmp(L"-u", lpCmdLineData) == 0)
		UnInstall();
	else if(_wcsicmp(L"-r", lpCmdLineData) == 0)
		LaunchServiceStart();
	else if(_wcsicmp(L"-k", lpCmdLineData) == 0)
		LaunchServiceStop();
	else if(_wcsicmp(L"-s", lpCmdLineData) == 0)
		StartServiceDispatcherLoop(); // will be launched by SCM
	else // run as stanalone
	{
		g_bIsConsole = true;
		g_nControlRequest = 1;
		RunBusinessProcessLoop(); // wait until business process does finish
	}
#endif
}

VOID Install(LPCTSTR pPath)
{  
	SC_HANDLE schSCManager = OpenSCManager( NULL, NULL, SC_MANAGER_CREATE_SERVICE); 
	if (schSCManager == 0)
	{
		LogDebugString2(L"ERROR: OpenSCManager failed: ", GetLastError());
		return;
	}

	SC_HANDLE schService = CreateService
	( 
		schSCManager,	/* SCManager database      */ 
		g_szServiceName,			/* name of service         */ 
		g_szServiceName,			/* service name to display */ 
		SERVICE_ALL_ACCESS,        /* desired access          */ 
		SERVICE_WIN32_OWN_PROCESS|SERVICE_INTERACTIVE_PROCESS , /* service type            */ 
		SERVICE_AUTO_START,      /* start type              */ 
		SERVICE_ERROR_NORMAL,      /* error control type      */ 
		pPath,			/* service's binary        */ 
		NULL,                      /* no load ordering group  */ 
		NULL,                      /* no tag identifier       */ 
		NULL,                      /* no dependencies         */ 
		NULL,                      /* LocalSystem account     */ 
		NULL
	);                     /* no password             */ 
	if (schService == 0) 
		LogDebugString2(L"ERROR: Failed to create service: ", GetLastError());
	else
	{
		TCHAR pTemp[1024];
		swprintf(pTemp, 1024, L"ERROR: Service %s installed: ", g_szServiceName);
		LogDebugString2(pTemp);
	}

	CloseServiceHandle(schService); 
}

VOID UnInstall()
{
	SC_HANDLE schSCManager = OpenSCManager( NULL, NULL, SC_MANAGER_ALL_ACCESS); 
	if (schSCManager == 0) 
		LogDebugString2(L"ERROR: OpenSCManager failed: ", GetLastError());
	else
	{
		SC_HANDLE schService = OpenService( schSCManager, g_szServiceName, SERVICE_ALL_ACCESS);
		if (schService == 0) 
			LogDebugString2(L"ERROR: OpenService failed: ", GetLastError());
		else
		{
			if (!DeleteService(schService)) 
				LogDebugString2(L"ERROR: Failed to delete service: ", GetLastError());
			else 
			{
				TCHAR pTemp[1024];
				swprintf(pTemp, 1024, L"Service %s removed", g_szServiceName);
				LogDebugString2(pTemp);
			}
			CloseServiceHandle(schService); 
		}
		CloseServiceHandle(schSCManager);	
	}
	DeleteFile(g_szLogFile);
}

BOOL LaunchServiceStart() 
{ 
	// run service with given name
	SC_HANDLE schSCManager = OpenSCManager( NULL, NULL, SC_MANAGER_ALL_ACCESS); 
	if (schSCManager == 0) 
	{
		LogDebugString2(L"ERROR: OpenSCManager failed: ", GetLastError());
		return FALSE;
	}

	// open the service
	SC_HANDLE schService = OpenService( schSCManager, g_szServiceName, SERVICE_ALL_ACCESS);
	if (schService == 0) 
	{
		LogDebugString2(L"ERROR: OpenService failed: ", GetLastError());
		CloseServiceHandle(schSCManager); 
		return FALSE;
	}

	// call StartService to run the service
	BOOL bSvcStart = StartService(schService, 0, (const TCHAR**)NULL);
	CloseServiceHandle(schService); 
	CloseServiceHandle(schSCManager); 

	if (!bSvcStart)
	{
		LogDebugString2(L"ERROR: StartService failed: ", GetLastError());
		return FALSE;
	}
	else
		return TRUE;
}

BOOL LaunchServiceStop() 
{ 
	// kill service with given name
	SC_HANDLE schSCManager = OpenSCManager( NULL, NULL, SC_MANAGER_ALL_ACCESS); 
	if (schSCManager == 0) 
	{
		LogDebugString2(L"ERROR: OpenSCManager failed: ", GetLastError());
		return FALSE;
	}

	// open the service
	SC_HANDLE schService = OpenService( schSCManager, g_szServiceName, SERVICE_ALL_ACCESS);
	if (schService == 0) 
	{
		LogDebugString2(L"ERROR: OpenService failed: ", GetLastError());
		CloseServiceHandle(schSCManager); 
		return FALSE;
	}

	// call ControlService to kill the given service
	SERVICE_STATUS status;
	BOOL bSvcStatus = ControlService(schService,SERVICE_CONTROL_STOP,&status);
	CloseServiceHandle(schService); 
	CloseServiceHandle(schSCManager); 

	if (!bSvcStatus)
	{
		LogDebugString2(L"ERROR: ControlService failed: ", GetLastError());
		return FALSE;
	}
	else
		return TRUE;
}


VOID StartServiceDispatcherLoop()
{
	g_nControlRequest = 1;
	SERVICE_TABLE_ENTRY stServiceStartTable[] = 
	{
		{(LPTSTR)g_szServiceName, ServiceMain},
		{NULL, NULL}
	};
	// query system to run service
	if (!StartServiceCtrlDispatcher(stServiceStartTable)) // does not return until all running services in the process have terminated
		LogDebugString2(L"ERROR: StartServiceCtrlDispatcher failed: ", GetLastError());
}

// Service Thread walker 
VOID WINAPI ServiceMain(DWORD dwArgc, LPTSTR *lpszArgv)
{
	DWORD   status = 0; 
    DWORD   specificError = 0xfffffff; 
 
    g_stServiceStatus.dwServiceType        = SERVICE_WIN32; 
    g_stServiceStatus.dwCurrentState       = SERVICE_START_PENDING; 
    g_stServiceStatus.dwControlsAccepted   = SERVICE_ACCEPT_STOP | SERVICE_ACCEPT_SHUTDOWN | SERVICE_ACCEPT_PAUSE_CONTINUE; 
    g_stServiceStatus.dwWin32ExitCode      = 0; 
    g_stServiceStatus.dwServiceSpecificExitCode = 0; 
    g_stServiceStatus.dwCheckPoint         = 0; 
    g_stServiceStatus.dwWaitHint           = 0; 
 
    g_hServiceStatusHandle = RegisterServiceCtrlHandler(g_szServiceName, ServiceHandler); 
    if (g_hServiceStatusHandle == 0) 
    {
		LogDebugString2(L"ERROR: RegisterServiceCtrlHandler failed: ", GetLastError());
        return; 
    } 
 
    // Initialization complete - report running status 
    g_stServiceStatus.dwCurrentState       = SERVICE_RUNNING; 
    g_stServiceStatus.dwCheckPoint         = 0; 
    g_stServiceStatus.dwWaitHint           = 0;  
    if (!SetServiceStatus(g_hServiceStatusHandle, &g_stServiceStatus)) 
		LogDebugString2(L"ERROR: SetServiceStatus failed: ", GetLastError());

	RunBusinessProcessLoop(); // wait until business process does finish
}

// Service handler callback
VOID WINAPI ServiceHandler(DWORD fdwControl)
{
	switch (fdwControl) 
	{
		case SERVICE_CONTROL_STOP:
		case SERVICE_CONTROL_SHUTDOWN:
			g_stServiceStatus.dwWin32ExitCode = 0; 
			g_stServiceStatus.dwCurrentState  = SERVICE_STOPPED; 
			g_stServiceStatus.dwCheckPoint    = 0; 
			g_stServiceStatus.dwWaitHint      = 0;
			// terminate process started by this service before shutdown
			g_nControlRequest = 0;
			while (g_nControlState > 0)
				Sleep(500);
			break; 
		case SERVICE_CONTROL_PAUSE:
			g_stServiceStatus.dwCurrentState = SERVICE_PAUSED; 
			g_nControlRequest = 2;
			break;
		case SERVICE_CONTROL_CONTINUE:
			g_stServiceStatus.dwCurrentState = SERVICE_RUNNING; 
			g_nControlRequest = 1;
			break;
		case SERVICE_CONTROL_INTERROGATE:
			break;
		default:
			if(fdwControl >= 128 && fdwControl < 256)	// Custom control code
			{
				LogDebugString2(L"Custom control code...");
			}
			else
			{
				TCHAR pTemp[1024];
				swprintf(pTemp, 1024, L"Unrecognized opcode %d", fdwControl);
				LogDebugString2(pTemp);
			}
	};

	if (!SetServiceStatus(g_hServiceStatusHandle,  &g_stServiceStatus)) 
		LogDebugString2(L"ERROR: SetServiceStatus failed: ", GetLastError());
}

//////////////////////////////////////
/// main application loop
//////////////////////////////////////


VOID RunBusinessProcessLoop()
{
	LogDebugString2(FormatWString(L"Face Detector %s . Starting business process...", g_szVersion).c_str());

	DFFViewer viewer;
	viewer.LoadSettings();
	viewer.LoadFaceGallery();
	viewer.ConnectMedia();

	if (g_bIsConsole)
	{
		// wait for ctrl-c input
		while (_getch() != 0x03);
		g_nControlRequest = 0;
		while (g_nControlState > 0)
			Sleep(100);
	}
	else
	{
		// wait until all locked buffers will be released
		DWORD dwRes = ::WaitForMultipleObjects(VIDEOCHANNUM, viewer.m_hWaitForPictureTheads, TRUE, INFINITE);
		switch (dwRes) 
		{
			// All thread objects were signaled
			case WAIT_OBJECT_0: 
				LogDebugString2(L"All threads ended, cleaning up for application exit...\r\n");
				break;

			// An error occurred
			default: 
				LogDebugString2(L"ERROR: WaitForMultipleObjects failed: %s\r\n", GetLastError());
		}
	}
}
