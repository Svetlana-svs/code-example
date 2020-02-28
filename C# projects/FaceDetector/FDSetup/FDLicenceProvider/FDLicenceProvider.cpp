// FDLicenceProvider.cpp : Defines the initialization routines for the DLL.
//

#include "stdafx.h"
#include "FDLicenceProvider.h"
#include <wincrypt.h>


// Macro that to verify validate arguments.
#define VALIDATE(p) ((HRESULT)((p) > 0) && ((p) != NULL))

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

//
//TODO: If this DLL is dynamically linked against the MFC DLLs,
//		any functions exported from this DLL which call into
//		MFC must have the AFX_MANAGE_STATE macro added at the
//		very beginning of the function.
//
//		For example:
//
//		extern "C" BOOL PASCAL EXPORT ExportedFunction()
//		{
//			AFX_MANAGE_STATE(AfxGetStaticModuleState());
//			// normal function body here
//		}
//
//		It is very important that this macro appear in each
//		function, prior to any calls into MFC.  This means that
//		it must appear as the first statement within the 
//		function, even before any object variable declarations
//		as their constructors may generate calls into the MFC
//		DLL.
//
//		Please see MFC Technical Notes 33 and 58 for additional
//		details.
//


// CFDLicenceProviderApp

BEGIN_MESSAGE_MAP(CFDLicenceProviderApp, CWinApp)
END_MESSAGE_MAP()


// CFDLicenceProviderApp construction

CFDLicenceProviderApp::CFDLicenceProviderApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}


// The one and only CFDLicenceProviderApp object

CFDLicenceProviderApp theApp;

const GUID CDECL BASED_CODE _tlid =
		{ 0x30D7F6AA, 0xC8F4, 0x43BD, { 0x87, 0x9D, 0xA9, 0x2F, 0x80, 0xE, 0x43, 0x93 } };
const WORD _wVerMajor = 1;
const WORD _wVerMinor = 0;


// CFDLicenceProviderApp initialization

BOOL CFDLicenceProviderApp::InitInstance()
{
	CWinApp::InitInstance();

	// Register all OLE server (factories) as running.  This enables the
	//  OLE libraries to create objects from other applications.
	COleObjectFactory::RegisterAll();

	return TRUE;
}

// DllGetClassObject - Returns class factory

STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return AfxDllGetClassObject(rclsid, riid, ppv);
}


// DllCanUnloadNow - Allows COM to unload DLL

STDAPI DllCanUnloadNow(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	return AfxDllCanUnloadNow();
}


// DllRegisterServer - Adds entries to the system registry

STDAPI DllRegisterServer(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	if (!AfxOleRegisterTypeLib(AfxGetInstanceHandle(), _tlid))
		return SELFREG_E_TYPELIB;

	if (!COleObjectFactory::UpdateRegistryAll())
		return SELFREG_E_CLASS;

	return S_OK;
}


// DllUnregisterServer - Removes entries from the system registry

STDAPI DllUnregisterServer(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	if (!AfxOleUnregisterTypeLib(_tlid, _wVerMajor, _wVerMinor))
		return SELFREG_E_TYPELIB;

	if (!COleObjectFactory::UpdateRegistryAll(FALSE))
		return SELFREG_E_CLASS;

	return S_OK;
}

string CFDLicenceProviderApp::GetKey()
{
    return "803818418613523217301962325115210516219391049722581123532213451164129162216463";
}

__declspec(dllexport)HRESULT Encrypt(byte* inEncryptArray, int sizeIn, byte* outEncryptArray, int sizeOut, int* sizeEncryptData)
{
    if (!(VALIDATE(sizeIn)&& VALIDATE(sizeOut) && VALIDATE(inEncryptArray) && VALIDATE(outEncryptArray)))
    {
        return E_INVALIDARG;
    }
    DATA_BLOB dataIn;
    DATA_BLOB dataEncrypt;
    DATA_BLOB dataKey;
    DWORD len = 0;
    BOOL bRes = true;
    BYTE* dataOut = NULL;
    try
    {
        string key = CFDLicenceProviderApp::GetKey();
        dataIn.pbData = inEncryptArray;
        dataIn.cbData = sizeIn;

        dataKey.pbData = (BYTE*) key.c_str();
        dataKey.cbData = key.length();
 
        bRes = CryptProtectData(&dataIn, L"FaceDetector", &dataKey, NULL, NULL, 0, &dataEncrypt);
        if (!bRes)
        {
            return GetLastError();
        }

        if (sizeOut < dataEncrypt.cbData)
        {
            *sizeEncryptData = (int)dataEncrypt.cbData;
            return E_OUTOFMEMORY;
        }
        else
        {
            bRes = CryptBinaryToStringA(dataEncrypt.pbData, dataEncrypt.cbData, CRYPT_STRING_BASE64, NULL, &len);
            if (!bRes)
            {
                return GetLastError();
            }
            dataOut = new BYTE[len];
            bRes = CryptBinaryToStringA(dataEncrypt.pbData, dataEncrypt.cbData, CRYPT_STRING_BASE64, (LPSTR)dataOut, &len);
            if (!bRes)
            {
                if (dataOut != NULL)
                {
                    delete[] dataOut;
                }
                return GetLastError();
            }

            *sizeEncryptData = (int)len;
            memcpy(outEncryptArray, dataOut, len);
            delete[] dataOut;
        }
    }
    catch(...)
    {
        if (dataOut != NULL)
        {
            delete[] dataOut;
        }
        return E_UNEXPECTED;
    }
    
    return S_OK;
}

__declspec(dllexport)HRESULT Decrypt(byte* inEncryptArray, int sizeIn, byte* outEncryptArray, int sizeOut, int* sizeEncryptData)
{
    if (!(VALIDATE(sizeIn) && VALIDATE(sizeOut) && VALIDATE(inEncryptArray) && VALIDATE(outEncryptArray)))
    {
        return E_INVALIDARG;
    }
    DATA_BLOB dataIn;
    DATA_BLOB dataOut;
    DATA_BLOB dataKey;
    DWORD len = 0;
    BOOL bRes = true;
    BYTE *dataEncr = NULL;
    try
    {
        string key = CFDLicenceProviderApp::GetKey();
        dataKey.pbData = (BYTE*) key.c_str();
        dataKey.cbData = key.length();


        bRes = CryptStringToBinaryA((LPCSTR)inEncryptArray, sizeIn, CRYPT_STRING_BASE64, NULL, &len, 0, 0);
        if (!bRes)
        {
            return GetLastError();
        }
        dataEncr = new BYTE[len];
        bRes = CryptStringToBinaryA((LPCSTR)inEncryptArray, sizeIn, CRYPT_STRING_BASE64, dataEncr, &len, 0, 0);
        if (!bRes)
        {
            if (dataEncr != NULL)
            {
                delete[] dataEncr;
            }
            return GetLastError();
        }
        dataIn.pbData = dataEncr;
        dataIn.cbData = len;
        dataEncr = NULL;
        bRes = CryptUnprotectData(&dataIn, NULL, &dataKey, NULL, NULL, 0, &dataOut);

        if (!bRes)
        {
            return GetLastError();;
        }
      
        if (sizeOut < dataOut.cbData)
        {
            return E_OUTOFMEMORY;
        }
        else
        {
            memcpy(outEncryptArray, dataOut.pbData, dataOut.cbData);
        }    
        *sizeEncryptData = (int)dataOut.cbData;
    }
    catch(...)
    {
        if (dataEncr != NULL)
        {
            delete[] dataEncr;
        }
        return E_UNEXPECTED;
    }
    
    return S_OK;
}
