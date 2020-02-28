// RE_DLL_STAT.h : main header file for the RE_DLL_STAT DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CRE_DLL_STATApp
// See RE_DLL_STAT.cpp for the implementation of this class
//

class CRE_DLL_STATApp : public CWinApp
{
public:
	CRE_DLL_STATApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
