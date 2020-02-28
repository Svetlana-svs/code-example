// FDLicenceProvider.h : main header file for the FDLicenceProvider DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols
#include <string>

using namespace std;

 extern "C" __declspec(dllexport) HRESULT Encrypt(byte* inEncryptArray, int sizeIn, byte* outEncryptArray, int sizeOut, int* sizeEncryptData);
 extern "C" __declspec(dllexport) HRESULT Decrypt(byte* inEncryptArray, int sizeIn, byte* outEncryptArray, int sizeOut, int* sizeEncryptData);


// CFDLicenceProviderApp
// See FDLicenceProvider.cpp for the implementation of this class
//

class CFDLicenceProviderApp : public CWinApp
{
public:
	CFDLicenceProviderApp();

// Overrides
public:
	virtual BOOL InitInstance();
    static string GetKey();

	DECLARE_MESSAGE_MAP()
};
