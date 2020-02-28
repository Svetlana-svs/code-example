#include "stdafx.h"

#include <windows.h>
#include <time.h>

#include "Helpers.h"

extern CRITICAL_SECTION	g_csLog;
extern TCHAR g_szLogFile[];

string I2string(int i)
{
	char buf[65];
	_itoa_s(i, buf, 65, 10);
	return buf;
}

wstring I2wstring(int i)
{
	wchar_t buf[65];
	_itow_s(i, buf, 65, 10);
	return buf;
}

wstring GetLastErrorString(DWORD dwError)
{
	if (!dwError)
		return L"";

	LPVOID lpMsgBuf;

	FormatMessage(
		FORMAT_MESSAGE_ALLOCATE_BUFFER | 
		FORMAT_MESSAGE_FROM_SYSTEM,
		NULL,
		dwError,
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		(LPTSTR) &lpMsgBuf,
		0, NULL );
 
    wstring msg = (LPTSTR)lpMsgBuf;
    LocalFree(lpMsgBuf);
	return msg;
}

wstring FormatWString(const wchar_t* fmt, ...)
{
	size_t ln = wcslen(fmt)+1024;
	wchar_t *buf = new wchar_t[ln];
	va_list vl;
	va_start(vl, fmt);
	vswprintf(buf, ln, fmt, vl);
	va_end(vl);
	wstring dst(buf);
	delete[] buf;
	return dst;
}

wstring D2string(double d)
{
	wchar_t buf[10];
	swprintf(buf, 10, L"%4.1f", d);
	return buf;
}

wstring StringToWString(const string str)
{
	size_t origsize = strlen(str.c_str()) + 1;
	size_t convertedChars = 0;
	wchar_t wchr[50];
	mbstowcs_s(&convertedChars, wchr, origsize, str.c_str(), _TRUNCATE);
	return wchr;
}

void OutputConsoleString(LPCTSTR str)
{
	// save console colors
	CONSOLE_SCREEN_BUFFER_INFO csbiInfo;
    HANDLE hStdout = GetStdHandle(STD_OUTPUT_HANDLE); 
	GetConsoleScreenBufferInfo(hStdout, &csbiInfo);
	// output to console
	if (wcsstr(str, L"ERROR"))
		SetConsoleTextAttribute(hStdout, FOREGROUND_RED | FOREGROUND_INTENSITY);
	else if (wcsstr(str, L"SUCCESS"))
		SetConsoleTextAttribute(hStdout, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_INTENSITY);
	wprintf(str);
	// restore console colors
	SetConsoleTextAttribute(hStdout, csbiInfo.wAttributes);
}

wstring GetSysErrorString(LPCTSTR pMsgPrefix, DWORD dwError)
{
	return pMsgPrefix + GetLastErrorString(dwError) + L"\r\n";
}

void WriteToLogFile(LPCTSTR pMsgPrefix, DWORD dwError /*= 0*/)
{
	// write error or other information into log file
	::EnterCriticalSection(&g_csLog);
	try
	{
		// get current time
		time_t ltime;
		struct tm today;
		time( &ltime );
		_localtime64_s(&today, &ltime);
		TCHAR szNow[25];
		wcsftime(szNow, 25, L"%c", &today);
		// write log in file
		FILE* pLog = _wfopen(g_szLogFile, L"a");
		fwprintf(pLog, L"\n%s   %s", szNow, pMsgPrefix); 
		if (dwError)
			fwprintf(pLog, L":  %s", GetLastErrorString(dwError).c_str()); 
		fclose(pLog);
	} catch(...) {}
	::LeaveCriticalSection(&g_csLog);
}

void LogDebugString2(LPCTSTR pMsgPrefix, DWORD dwError/* = 0*/)
{
	wstring str = GetSysErrorString(pMsgPrefix, dwError);
	LogDebugString(str);
}

/*NRect TPlcRect2NRect(const TPlcRect srcRect)
{
	NRect dstRect;
	dstRect.X = srcRect.Left;
	dstRect.Y = srcRect.Top;
	dstRect.Height = srcRect.Bottom - srcRect.Top;
	dstRect.Width = srcRect.Right - srcRect.Left;
	return dstRect;
}*/

/*TPlcRect NRect2TPlcRect(const NRect srcRect)
{
	TPlcRect dstRect;
	dstRect.Left = srcRect.X;
	dstRect.Top = srcRect.Y;
	dstRect.Bottom = srcRect.Y + srcRect.Height;
	dstRect.Right = srcRect.X + srcRect.Width;
	return dstRect;
}*/

/*NSizeType GetGSCPitchSize(TDecompBufferFormat fmt)
{
	switch (fmt)
	{
	case dbfFourCC_YV12:
		return 0;
	case dbfFourCC_UYVY:
		return 0;
	case dbfRGB24:
		return 24;
	case dbfRGB32:
		return 32;
	case dbfYonly:
		return 8;
	default:
		return 0;
	}
}*/