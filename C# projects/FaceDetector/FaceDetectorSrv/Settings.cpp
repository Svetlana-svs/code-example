#include "StdAfx.h"
#include "Settings.h"
#include "Helpers.h"

Settings::Settings(void)
{
}

Settings::~Settings(void)
{
	Database.clear();
	GeViScope.clear();
	Verilook.Detection.clear();
	Verilook.Recognition.clear();
	for (int i = 0; i < VIDEOCHANNUM; i++)
		Channels[i].clear();
}

bool Settings::Load()
{
	HRESULT hr = S_OK;
	IXMLDOMDocument* pXMLDoc = NULL;
	IXMLDOMElement* pElmRoot = NULL;
	IXMLDOMNode* pGeviscope = NULL;
	IXMLDOMNode* pVerilook = NULL;
	IXMLDOMNode* pVL_Detection = NULL;
	IXMLDOMNode* pVL_Recognition = NULL;
	IXMLDOMNode* pDatabase = NULL;
	IXMLDOMNode* pChannels = NULL;
	IXMLDOMNode* pChannel = NULL;

	bool res = false;

	pXMLDoc = GetXMLSettingsDoc();
	if (!pXMLDoc) goto CleanUp1;

	CHK_HR(pXMLDoc->get_documentElement(&pElmRoot), CleanUp1);

	// load GeViScope settings
	pGeviscope = FindFirstChildByName(pElmRoot, L"geviscope");
	if (!pGeviscope) goto CleanUp1;
	if (!FillSettingsSection(GeViScope, pGeviscope)) goto CleanUp1;

	// load Verilook settings
	pVerilook = FindFirstChildByName(pElmRoot, L"verilook");
	if (!pVerilook) goto CleanUp1;

	// load Verilook Detection settings
	pVL_Detection = FindFirstChildByName(pVerilook, L"detection");
	if (!pVL_Detection) goto CleanUp1;
	if (!FillSettingsSection(Verilook.Detection, pVL_Detection)) goto CleanUp1;

	// load Verilook Recognition settings
	pVL_Recognition = FindFirstChildByName(pVerilook, L"recognition");
	if (!pVL_Recognition) goto CleanUp1;
	if (!FillSettingsSection(Verilook.Recognition, pVL_Recognition)) goto CleanUp1;

	// load database settings
	pDatabase = FindFirstChildByName(pElmRoot, L"database");
	if (!pDatabase) goto CleanUp1;
	if (!FillSettingsSection(Database, pDatabase)) goto CleanUp1;

	// load channels settings
	pChannels = FindFirstChildByName(pElmRoot, L"channels");
	if (!pGeviscope) goto CleanUp1;
	for (int i = 0; i < VIDEOCHANNUM; i++)
	{
		wchar_t buff[20];
		swprintf(buff, 20, L"ch%u", i);
		pChannel = FindFirstChildByName(pChannels, buff);
		if (pChannel)
		{
			FillSettingsSection(Channels[i], pChannel);
			pChannel->Release(); pChannel = NULL;
			res = true;
		}
	}

CleanUp1:
	SAFE_RELEASE(pElmRoot);
	SAFE_RELEASE(pChannels);
	SAFE_RELEASE(pChannel);
	SAFE_RELEASE(pGeviscope);
	SAFE_RELEASE(pVerilook);
	SAFE_RELEASE(pVL_Detection);
	SAFE_RELEASE(pVL_Recognition);
	SAFE_RELEASE(pDatabase);
	SAFE_RELEASE(pXMLDoc);

	return res;
}
