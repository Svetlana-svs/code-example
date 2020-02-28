#include "stdafx.h"

#include <windows.h>
#include <atlbase.h>

#include "Helpers.h"
#include "XMLSettingsReader.h"


// Helper function to create a DOM instance. 
bool CreateAndInitDOM(IXMLDOMDocument **ppDoc)
{
	HRESULT hr = CoCreateInstance(CLSID_DOMDocument, NULL, CLSCTX_INPROC_SERVER, IID_IXMLDOMDocument, (LPVOID*)ppDoc);
	if (FAILED(hr))
		return false;

	// these methods should not fail so don't inspect result
    (*ppDoc)->put_async(VARIANT_FALSE);  
    (*ppDoc)->put_validateOnParse(VARIANT_FALSE);
    (*ppDoc)->put_resolveExternals(VARIANT_FALSE);
    return true;
}

bool GetXMLAttributeValue(IXMLDOMNamedNodeMap* pAttributes, LPCTSTR pszName, wstring &strval)
{
	CComBSTR bstrName = pszName;
	CComVariant varValue;
	IXMLDOMNode* pAttribute = NULL;
	bool bSuccess = false;
	if (SUCCEEDED(pAttributes->getNamedItem(bstrName, &pAttribute)) && pAttribute)
	{
		if (SUCCEEDED(pAttribute->get_nodeValue(&varValue)))
		{
			strval = varValue.bstrVal;
			bSuccess = true;
		}
		pAttribute->Release();
	}

	return bSuccess;
}

IXMLDOMNode* FindFirstChildByName(IXMLDOMNode* pParrentNode, wstring strName)
{
	if (!pParrentNode)
		return NULL;

	CComBSTR bstrName;
	HRESULT hr = S_OK;
	IXMLDOMNodeList* pXMLNodeList = NULL;
	IXMLDOMNode* pSettingNode = NULL;

	CHK_HR(pParrentNode->get_childNodes(&pXMLNodeList), CleanUp3);

	long settingsCount;

	if (FAILED(pXMLNodeList->get_length(&settingsCount)))
		return NULL;

	for (long i = 0; i < settingsCount; i++)
		if (SUCCEEDED(pXMLNodeList->get_item(i, &pSettingNode)) 
			&& SUCCEEDED(pSettingNode->get_nodeName(&bstrName)))
		{
			wstring str = bstrName.m_str;
			if (str.compare(strName) == 0)
				break;
		}

CleanUp3:
	SAFE_RELEASE(pXMLNodeList);

	return pSettingNode;
}
wstring DecryptData(wstring wstrValue)
{
    if (wstrValue.empty())
        return L"";
    
    HRESULT hr;
    char *pData = NULL;
    byte *pDataInput = NULL;
    byte *pDataOut = NULL;
    wchar_t *pDataDecrypt = NULL;
    string strDecryptData;
    wstring wstrDecryptData =  L"";
    int lenOut = 0;
    int lenDecryptData =0;

    int lenIn = WideCharToMultiByte(CP_UTF8,0, wstrValue.c_str(), -1, NULL, 0, NULL, NULL);
    if (lenIn == 0) goto CleanUp4;
    pData = new char[lenIn];

    int lenDataMultiByte =   WideCharToMultiByte(CP_UTF8, 0, wstrValue.c_str(), -1, pData, lenIn, NULL, NULL);
    if (lenDataMultiByte == 0) goto CleanUp4;
    
    pDataInput = new byte[lenIn];
    for (int i = 0; i < lenIn; i++)
    {
        pDataInput[i] = (byte)pData[i];
    }

    pDataOut = new byte[lenIn];
    lenOut = lenIn;

    hr = Decrypt(pDataInput, lenIn, pDataOut, lenOut, &lenDecryptData);
    if (hr != S_OK) goto CleanUp4;
    
    for (int i = 0; i < lenDecryptData; i++)
    {
        strDecryptData = strDecryptData + (char)pDataOut[i];
    }

    lenDecryptData = MultiByteToWideChar(CP_UTF8, 0, strDecryptData.c_str(), -1, 0, 0);
    pDataDecrypt = new wchar_t[lenDecryptData];

    MultiByteToWideChar(CP_UTF8, 0, strDecryptData.c_str(), -1, pDataDecrypt, lenDecryptData);
    wstrDecryptData = pDataDecrypt;
    
    if (pData != NULL) {delete[] pData; pData = NULL;}
    if (pDataInput != NULL) {delete[] pDataInput; pDataInput = NULL;}
    if (pDataOut != NULL) {delete[] pDataOut; pDataOut = NULL;}
    if (pDataDecrypt != NULL) {delete[] pDataDecrypt; pDataDecrypt = NULL;}
    
    
CleanUp4: if (pData != NULL) delete[] pData;
          if (pDataInput != NULL) delete[] pDataInput;
          if (pDataOut != NULL) delete[] pDataOut;

    return wstrDecryptData;
}
bool FillSettingsSection(MapStringString& mapSettings, IXMLDOMNode* pSettingNode)
{
	if (!pSettingNode)
		return false;

	HRESULT hr = S_OK;
	IXMLDOMNodeList* pXMLNodeList = NULL;
	IXMLDOMNode* pNode = NULL;
	IXMLDOMNamedNodeMap* pAttributes = NULL;
	long settingsCount = 0;

	CHK_HR(pSettingNode->get_childNodes(&pXMLNodeList), CleanUp2);
	CHK_HR(pXMLNodeList->get_length(&settingsCount), CleanUp2);

	bool bRes = true;
	for (long i = 0; i < settingsCount; i++)
	{
		bRes = false;
		CComBSTR bstrName;
		CHK_HR(pXMLNodeList->get_item(i, &pNode), CleanUp2);
		CHK_HR(pNode->get_nodeName(&bstrName), CleanUp2);
		if (bstrName == L"add")
		{
			CHK_HR(pNode->get_attributes(&pAttributes), CleanUp2);
			bRes = true;
			wstring strName, strValue;
			if (GetXMLAttributeValue(pAttributes, L"stname", strName)
					&& GetXMLAttributeValue(pAttributes, L"stvalue", strValue)
					&& !strName.empty())
			{
        		CComBSTR bstrParentNodeName;
                pSettingNode->get_nodeName(&bstrParentNodeName);
                if (bstrParentNodeName == L"database")
                {
                    mapSettings[strName] = DecryptData(strValue);
                }
                else
                {
                    mapSettings[strName] = strValue;
                }
			}
			pAttributes->Release(); pAttributes = NULL;
		}
		pNode->Release(); pNode = NULL;
	}

CleanUp2:
	SAFE_RELEASE(pAttributes);
	SAFE_RELEASE(pNode);
	SAFE_RELEASE(pXMLNodeList);

	return bRes;
}

IXMLDOMDocument* GetXMLSettingsDoc()
{
	HRESULT hr = S_OK;
	IXMLDOMDocument* pXMLDoc = NULL;

    VARIANT_BOOL varStatus;
	CComVariant varFileName = L"FDSettings.xml";

	if(!CreateAndInitDOM(&pXMLDoc))
		return NULL;
    // XML file name to load
    CHK_HR(pXMLDoc->load(varFileName, &varStatus), CleanUp);
    if (varStatus != VARIANT_TRUE)
		goto CleanUp;

	return pXMLDoc;

CleanUp:
	SAFE_RELEASE(pXMLDoc);
	return NULL;
}

