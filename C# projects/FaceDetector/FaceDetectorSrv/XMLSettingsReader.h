#pragma once

#include <map>

#import <msxml6.dll> raw_interfaces_only

#pragma comment( lib, "FDLicenceProvider.lib" )
extern "C" __declspec(dllimport) HRESULT Decrypt(byte* inEncryptArray, int sizeIn, byte* outEncryptArray, int sizeOut, int* sizeEncryptData);

using namespace std;

typedef map<wstring,wstring> MapStringString;

// Macro that releases a COM object if not NULL.
#define SAFE_RELEASE(p)     do { if ((p)) { (p)->Release(); (p) = NULL; } } while(0)
// Macro that calls a COM method returning HRESULT value.
#define CHK_HR(stmt, lbl)        do { hr=(stmt); if (FAILED(hr)) goto lbl; } while(0)

IXMLDOMDocument* GetXMLSettingsDoc();
IXMLDOMNode* FindFirstChildByName(IXMLDOMNode* pParrentNode, wstring strName);
bool FillSettingsSection(MapStringString& mapSettings, IXMLDOMNode* pSettingNode);
