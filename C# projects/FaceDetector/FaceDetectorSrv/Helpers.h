#pragma once

#include <string>
#include <winbase.h>
#include <gscactionstypes.h>
#include <NGeometry.h>
#include <stdio.h>
#include <tchar.h>

#include "Synchronization.h"

using namespace std;
using namespace GscPLC;

extern bool g_bIsConsole;
extern SyncInt	g_nControlRequest, g_nControlState;

string I2string(int i);
wstring I2wstring(int i);
wstring GetLastErrorString(DWORD dwError);
wstring FormatWString(const wchar_t* fmt, ...);
wstring D2string(double d);
wstring StringToWString(const string str);
wstring GetSysErrorString(LPCTSTR pMsgPrefix, DWORD dwError);
void WriteToLogFile(LPCTSTR pMsgPrefix, DWORD dwError = 0);
void LogDebugString2(LPCTSTR pMsgPrefix, DWORD dwError = 0);
void OutputConsoleString(LPCTSTR str);
void WriteLog(LPCSTR pMsgPrefix, DWORD dwError /*= 0*/);

//NRect TPlcRect2NRect(const TPlcRect srcRect);
//TPlcRect NRect2TPlcRect(const NRect srcRect);
//NSizeType GetGSCPitchSize(TDecompBufferFormat fmt);

#ifdef _DEBUG
#define LogDebugString(s) if (g_bIsConsole) OutputConsoleString((s).c_str()); else OutputDebugString((s).c_str())
	//#define LogDebugString(s) if (g_bIsConsole) printf(N_T((s).c_str())); else WriteToLogFile((s).c_str(), 0)	
#else  // !_DEBUG
	#define LogDebugString(s) ;
#endif //  _DEBUG
