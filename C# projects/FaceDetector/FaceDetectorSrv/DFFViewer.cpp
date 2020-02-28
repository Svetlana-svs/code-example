
#include "stdafx.h"

#include <atldbcli.h>

#include <ncore.h>

#include "DFFViewer.h"
#include "Helpers.h"
#include "Person.h"


static const wstring s_ViewerError	= L"[FaceDetector::DFFViewer::ERROR]: ";
static const wstring s_ViewerInfo	= L"[FaceDetector::DFFViewer::info]: ";
static const wstring s_ViewerChannel = L"[FaceDetector::DFFViewer::Channel(";
static const wstring s_Info			= L")::info]: ";
static const wstring s_Error		= L")::ERROR]: ";


// callback function for connect progress display
bool __stdcall ConnectProgressCB(void* Instance, unsigned __int32 Progress, unsigned __int32 MaxProgress)
{
	return TRUE;
}

// callback is called from offscreen viewer object after picture has been decompressed
bool __stdcall NewOffscreenImageCallbackCB(void* Instance,
                                           const HGscDecompBuffer OffscreenBufferHandle,	// Handle to the DecompBuffer object..
                                           const TRect& SrcRect,							// Rect of decompressed buffer
                                           const TPicData& PicData,						    // Picture Information
                                           const TViewerStatus& ViewerStatus,				// Viewer status
                                           const TEventData* MscEventData)					// return value defines if user was able to handle image
{
	if(Instance == NULL)
		return TRUE;

	// calling the callback method of the main window object instance
	DFFViewer::Channel* channel = (DFFViewer::Channel*)Instance;
	return channel->NewOffscreenImageCallback(OffscreenBufferHandle, SrcRect, PicData, ViewerStatus, MscEventData);
}

// callback is called from offscreen viewer object before picture is decompressed
bool __stdcall NewOffscreenImageAcceptCallbackCB(void* Instance,
                                                 HGscDecompBuffer& OffscreenBufferHandle,  // Buffer to decompress, default buffer can be overridden..
											     const TPicData& PicData,                  // Picture Information
                                                 const TViewerStatus& ViewerStatus,        // Viewer status
                                                 const TEventData* MscEventData)           // return value defines if user wants the image to be decompressed
{
	if(Instance == NULL)
		return TRUE;

	// calling the callback method of the main window object instance
	DFFViewer::Channel* channel = (DFFViewer::Channel*)Instance;
	return channel->NewOffscreenImageAcceptCallback(OffscreenBufferHandle, PicData, ViewerStatus, MscEventData);
}

// callback function for PLC notifications
void __stdcall PLCCallbackCB(void* Instance, TGscPlcNotificationType NotificationType, HGscPlcNotification Notification)
{
	if(Instance == NULL)
		return;

	// calling the callback method of the main window object instance
	DFFViewer::Channel* channel = (DFFViewer::Channel*)Instance;
	channel->PLCCallback(NotificationType, Notification);
}

DWORD WINAPI ProcessImageThreadChannel(LPVOID Instance)
{
	DFFViewer::Channel* channel = (DFFViewer::Channel*)Instance;
	return channel->ProcessImageThread();
}

// helper class to store media channel informations
class TMediaChannelDescriptor 
{
public:
	__int64	m_MediaChannelID;
	unsigned __int32	m_GlobalNumber;
	wstring	m_Description;
	wstring	m_Name;

	TMediaChannelDescriptor()
	{
		m_MediaChannelID = -1;
		m_GlobalNumber = 0;
	}

	TMediaChannelDescriptor(__int64	AMediaChannelID, unsigned __int32 AGlobalNumber,	wstring ADescription, wstring AName)
	{
		m_MediaChannelID = AMediaChannelID;
		m_GlobalNumber = AGlobalNumber;
		m_Description = ADescription;
		m_Name = AName;
	}
};


	//////// class DFFViewer ////////////////

DFFViewer::DFFViewer()
{
	// initialize COM
	CoInitialize(NULL); 

	InitializeCriticalSection(&m_cs);
	
	for (int i = 0; i < VIDEOCHANNUM; i++)
	{
		m_hWaitForPictureTheads[i] = NULL;
		m_Channels[i].SetParent(this, i);
	}
}

DFFViewer::~DFFViewer()
{
	CoUninitialize();

	for (int i = 0; i < VIDEOCHANNUM; i++)
		CloseHandle(m_hWaitForPictureTheads[i]);

	wstring message = s_ViewerInfo + L"~DFFViewer()\r\n";
	LogDebugString(message);
}

void DFFViewer::ConnectMedia()
{
	for (int i = 0; i < VIDEOCHANNUM; i++)
		m_Channels[i].ConnectMedia();
}

bool DFFViewer::LoadSettings()
{
	wstring message = s_ViewerInfo + L"LoadSettings\r\n";
	LogDebugString(message);

	if (!m_Settings.Load())
		return false;

	EnterCriticalSection(&m_cs); 
	m_strGVSHost = m_Settings.GeViScope[L"hostname"];
	m_strGVSLogin = m_Settings.GeViScope[L"login"];
	m_strGVSPassword = m_Settings.GeViScope[L"password"];
	LeaveCriticalSection(&m_cs); 

	for (int i = 0; i < VIDEOCHANNUM; i++)
		m_Channels[i].LoadSettings(m_Settings.Channels[i]);

	return true;
}

bool DFFViewer::LoadFaceGallery()
{
	wstring message = s_ViewerInfo + L"LoadFaceGallery\r\n";
	LogDebugString(message);

	CPerson person;
	person.SetDataSourceProperties(m_Settings);

	if (FAILED(person.OpenAll()))
	{
		wstring message = s_ViewerError + L"LoadFaceGallery failed\r\n";
		LogDebugString(message);
		return false;
	}

	while (person.MoveNext() == S_OK)
	{
		FaceTemplate faceTemplate;
		wchar_t szName[500];
		swprintf(szName, 500, L"%s %s, #%u", person.m_Fname, person.m_Name, person.m_Id);
		faceTemplate.strName = szName;
		faceTemplate.buffer = new NByte[person.m_dwTemplateLength];
		memcpy(faceTemplate.buffer, person.m_Template, person.m_dwTemplateLength);
		faceTemplate.bufferSize = person.m_dwTemplateLength;
		m_FaceGallery.AddTemplate(faceTemplate);
	}
	
	person.CloseAll();

	return true;
}

	//////// class Channel //////////

DFFViewer::Channel::Channel()
{
	m_GscServer = NULL;
	m_GscOffscreenViewer = NULL;
	m_GscPLC = NULL;

	m_bInitialized = false;
	m_bEnabled = false;
	m_bMatchNeeded = false;
	m_isConnected = false;
	m_isPlaying = false;

	m_hWaitForPictureEvent = NULL;
}

DFFViewer::Channel::~Channel()
{
	wstring message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"~Channel()\r\n";
	LogDebugString(message);
	m_CSBuffer.Leave();
	m_CSBuffer1.Leave();
	m_CSBuffer2.Leave();
	DestroyOffscreenViewer();
	DisconnectFromServer();
	g_nControlState = 0;
}

void DFFViewer::Channel::LoadSettings(MapStringString& mapSettings)
{
	EnterCriticalSection(&m_pParent->m_cs);
	m_strGVSHost = m_pParent->m_strGVSHost;
	m_strGVSLogin = m_pParent->m_strGVSLogin;
	m_strGVSPassword = m_pParent->m_strGVSPassword;
	LeaveCriticalSection(&m_pParent->m_cs);

	wstring message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"LoadSettings\r\n";
	LogDebugString(message);

	m_strChannel = mapSettings[L"channel"];
	m_bEnabled = mapSettings[L"enabled"].compare(L"true") == 0;
	m_bMatchNeeded = mapSettings[L"match"].compare(L"true") == 0;
	m_bInitialized = true;
}

void DFFViewer::Channel::CreateOffscreenViewer()
{
	EnterCriticalSection(&m_pParent->m_cs);
	m_FaceDetector.Initialize(m_pParent->m_Settings, m_bMatchNeeded);
	LeaveCriticalSection(&m_pParent->m_cs);

	//DestroyOffscreenViewer();

	wstring message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"CreateOffscreenViewer\r\n";
	LogDebugString(message);

	// create Wait For Picture event
	wstring strEvent = m_strChannel + L"_WaitForPictureEvent";
	m_hWaitForPictureEvent = ::CreateEvent(NULL, FALSE, FALSE, strEvent.c_str()); 
	if (!m_hWaitForPictureEvent)
	{
		message = s_ViewerChannel + I2wstring(m_nChannel) + s_Error + GetSysErrorString(L"CreateEvent failed: %s\r\n", GetLastError());
		LogDebugString(message);
	}

	// create Wait For Picture tread
	m_pParent->m_hWaitForPictureTheads[m_nChannel] = ::CreateThread(NULL, 0, ProcessImageThreadChannel, this, 0, NULL);
	if (!m_pParent->m_hWaitForPictureTheads[m_nChannel])
	{
		message = s_ViewerChannel + I2wstring(m_nChannel) + s_Error + GetSysErrorString(L"CreateThread failed: %s\r\n", GetLastError());
		LogDebugString(message);
	}

	// create two decompression buffer object instances
	m_GscDecompBuffer1 = GMPCreateDecompBuffer();
	m_GscDecompBuffer2 = GMPCreateDecompBuffer();
	// set buffers size
	m_GscDecompBuffer1->SetBufferSize(1, 1, dbfRGB24);
	m_GscDecompBuffer2->SetBufferSize(1, 1, dbfRGB24);

	// create the offscreen viewer object instance
	m_GscOffscreenViewer = GMPCreateOffscreenViewer(m_GscDecompBuffer1);
	m_GscOffscreenViewer->SetOffscreenViewerSize(1, 1, false);

	// set callbacks of the offscreen viewer objects
	m_GscOffscreenViewer->SetCustomDrawCallBack(NULL, NULL);
	m_GscOffscreenViewer->SetOffscreenViewerCallBack(NewOffscreenImageCallbackCB, this);
	m_GscOffscreenViewer->SetOffscreenViewerAcceptCallBack(NewOffscreenImageAcceptCallbackCB, this);
}

void DFFViewer::Channel::DestroyOffscreenViewer()
{
	if (m_GscOffscreenViewer != NULL)
	{
		wstring message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"DestroyOffscreenViewer\r\n";
		LogDebugString(message);

		m_GscOffscreenViewer->Disconnect(TRUE);
		m_GscOffscreenViewer->SetCustomDrawCallBack(NULL, NULL);
		m_GscOffscreenViewer->SetOffscreenViewerCallBack(NULL, NULL);
		m_GscOffscreenViewer->SetOffscreenViewerAcceptCallBack(NULL, NULL);
		m_GscOffscreenViewer->Destroy();
		m_GscOffscreenViewer = NULL;
	}
	if (m_GscDecompBuffer1 != NULL)
	{
		m_GscDecompBuffer1->Destroy();
		m_GscDecompBuffer1 = NULL;
	}
	if (m_GscDecompBuffer2 != NULL)
	{
		m_GscDecompBuffer2->Destroy();
		m_GscDecompBuffer2 = NULL;
	}
	m_NewPicDecompBuffer = NULL;
	m_NewPicDecompressed = false;

	if (m_hWaitForPictureEvent)
	{
		CloseHandle(m_hWaitForPictureEvent);
		m_hWaitForPictureEvent = NULL;
	}
}

void DFFViewer::Channel::ConnectToGSServer()
{
	//DisconnectFromServer();
	m_isConnected = true;

	if (m_GscServer == NULL)
		// create a server object instance
		m_GscServer = DBICreateRemoteServer();
	// encode the password
	wstring strHostname = m_pParent->m_strGVSHost;
	wstring strLogin = m_pParent->m_strGVSLogin;
	wstring strPassword = m_pParent->m_strGVSPassword;
	string EncodedPassword = DBIEncodeString(strPassword);
	// initialize the connection parameters  
	TGscServerConnectParams ConnectParams(strHostname.c_str(), strLogin.c_str(), EncodedPassword.c_str());
	m_GscServer->SetConnectParams(ConnectParams);
	// connect to the server
	TConnectResult ConnectResult = m_GscServer->Connect(&ConnectProgressCB, this);
	wstring message;
	if(ConnectResult != connectOk)
	{
		message = s_ViewerChannel + I2wstring(m_nChannel) + s_Error + L"ConnectToGSServer failed!\r\n";
		m_isConnected = false;
	}
	else
	{
		if (m_GscPLC == NULL)
			CreatePLC();
		message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + FormatWString(L"ConnectToGSServer succeed: server %s login %s\r\n", strHostname.c_str(), strLogin.c_str());
	}

	LogDebugString(message);
}

void DFFViewer::Channel::DisconnectFromServer(bool ConnectionGotLost /*= false*/)
{
	m_isConnected = false;
	m_isPlaying = false;

	DestroyOffscreenViewer();
	DestroyPLC(ConnectionGotLost);

	if(m_GscServer != NULL)
	{
		wstring message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"DisconnectFromServer\r\n";
		LogDebugString(message);

		m_GscServer->Disconnect(INFINITE);
		m_GscServer->Destroy();
		m_GscServer = NULL;
	}
}

bool DFFViewer::Channel::GetMediaChannel(TMediaChannelDescriptor& mediaChannel)
{
	wstring message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"GetMediaChannel\r\n";
	LogDebugString(message);

	if(m_GscServer == NULL)
		return false;

	if (!m_isConnected)
		return false;

    vector<TMediaChannelRecordEx> MediaChannelList;

    GMPQueryMediaChannelList(m_GscServer, ctGscServer, mtServer, MediaChannelList);

    for (vector<TMediaChannelRecordEx>::iterator it = MediaChannelList.begin(); it != MediaChannelList.end(); ++it)
    {
        if((*it).IsActive)
        {
			if (m_strChannel.compare((*it).Name) == 0)
			{
				mediaChannel.m_MediaChannelID = (*it).ChannelID;
				mediaChannel.m_GlobalNumber = (*it).GlobalNumber;
				mediaChannel.m_Description = (*it).Desc;
				mediaChannel.m_Name = (*it).Name;

				message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"GetMediaChannel: MediaChannel found\r\n";
				LogDebugString(message);
				return true;
			}
        }
    }

	message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"GetMediaChannel: MediaChannel NOT found\r\n";
	LogDebugString(message);

	return false;
}

void DFFViewer::Channel::ConnectMedia()
{
	if (!m_bInitialized)
		return;

	if (!m_bEnabled)
	{
		DisconnectFromServer();
		return;
	}

	if (m_strChannel.length() == 0)
		return;

	ConnectToGSServer();

	if(m_GscServer == NULL)
		return;

	if (!m_isConnected)
		return;

	wstring message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"ConnectMedia\r\n";
	LogDebugString(message);

	if (m_GscOffscreenViewer == NULL)
		CreateOffscreenViewer();

	TViewerStatus ViewerStatus;
	m_GscOffscreenViewer->GetStatus(ViewerStatus);

	TMediaChannelDescriptor SelectedMediaChannel;
	if (GetMediaChannel(SelectedMediaChannel))
	{
		if (ViewerStatus.MediaChID != SelectedMediaChannel.m_MediaChannelID)
		{
			// initialize the offscreen viewer parameters
			TMPConnectData ViewerConnectData;
			memset(&ViewerConnectData, 0, sizeof(ViewerConnectData));
			ViewerConnectData.StructSize = sizeof(ViewerConnectData);
			ViewerConnectData.Connection = reinterpret_cast<void*>(m_GscServer);
			ViewerConnectData.ServerType = ctGscServer;
			ViewerConnectData.MediaType = mtServer;
			ViewerConnectData.MediaChDesc = NULL;
			ViewerConnectData.DisableAudio = true;
			ViewerConnectData.PlayLoop = false;
			ViewerConnectData.Wait = false; 
			ViewerConnectData.MediaChID = SelectedMediaChannel.m_MediaChannelID;
			// connect the offscreen viewer with a media channel and set its playmode (stream, ...)
			m_GscOffscreenViewer->ConnectDB(ViewerConnectData, pmPlayStream, NULL, NULL, NULL);

			message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"ConnectMedia: Start monitoring...\r\n";
		}
		else
		{
			// only set the playmode of the allready connected viewer
			m_GscOffscreenViewer->SetPlayMode(pmPlayStream);

			message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"ConnectMedia: Restart monitoring...\r\n";
		}
		m_isPlaying = true;
	}
	else
		message = s_ViewerChannel + I2wstring(m_nChannel) + s_Error + L"ConnectMedia: MediaChannel not found!\r\n";

	LogDebugString(message);
}

bool DFFViewer::Channel::ProcessImage()
{
	if (!m_isConnected)
		return false;

	if (g_nControlRequest != 1)
		return false;

	wstring message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"ProcessImage\r\n";
	LogDebugString(message);

    m_CSBuffer.Enter();
	// check if new decompressed picture available
    if (m_NewPicDecompressed)
    {
		//message = s_ViewerChannel + I2wstring(m_nChannel) + s_Info + L"GetLockLastReceivedPic: new picture ready to be locked\r\n";
		//LogDebugString(message);

	    if(m_NewPicDecompBuffer == m_GscDecompBuffer2)
            m_CSBuffer2.Enter();
        else
            m_CSBuffer1.Enter();

        m_NewPicDecompressed = false;

        // new decompressed picture available
	    void* BufPointer;
        DWORD BufWidth, BufHeight, BufPitch;

	    // get a pointer to the bits of the decompressed image
	    m_NewPicDecompBuffer->GetBufPointer(BufPointer, BufWidth, BufHeight, BufPitch);

		// get a bitmap info header, that fits to the decompressed image
		BITMAPINFO bmpInfo;
	    m_NewPicDecompBuffer->GetBitmapInfoHdr(bmpInfo.bmiHeader);

		bool b = m_FaceDetector.DetectBitmapInfo(&bmpInfo, sizeof(BITMAPINFO), BufPointer, bmpInfo.bmiHeader.biSizeImage, false, m_FaceCache, m_pParent->m_FaceGallery, m_GscPLC);

	    if(m_NewPicDecompBuffer == m_GscDecompBuffer2)
            m_CSBuffer2.Leave();
        else
            m_CSBuffer1.Leave();
	    m_CSBuffer.Leave();
		return true;
    }
	else
	{
		message = s_ViewerChannel + I2wstring(m_nChannel) + s_Error + L"ProcessImage: no picture available\r\n";
		LogDebugString(message);
	    m_CSBuffer.Leave();
		return false;
	}
}

bool DFFViewer::Channel::NewOffscreenImageCallback(const HGscDecompBuffer OffscreenBufferHandle, const TRect& SrcRect,
										 const TPicData& PicData, const TViewerStatus& ViewerStatus,
										 const TEventData* MscEventData)
{
	// new decompressed picture available

	if (g_nControlRequest == 1)
	{
		m_CSBuffer.Enter();
		++g_nControlState;
	}

	if(OffscreenBufferHandle == m_GscDecompBuffer2)
		m_CSBuffer2.Leave();
	else
		m_CSBuffer1.Leave();

	--g_nControlState ; // was increased in NewOffscreenImageAcceptCallback

	if (g_nControlRequest == 1)
	{
		m_NewPicDecompBuffer = OffscreenBufferHandle;
		m_NewPicDecompressed = true;
		m_CSBuffer.Leave();
		--g_nControlState;
		if (!SetEvent(m_hWaitForPictureEvent))	// notify waiting thread about image is ready
		{
			wstring message = s_ViewerChannel + I2wstring(m_nChannel) + s_Error + L"SetEvent failed: %s\r\n";
			LogDebugString2(message.c_str(), GetLastError());
		}
	}

	return true;
}

bool DFFViewer::Channel::NewOffscreenImageAcceptCallback(HGscDecompBuffer& OffscreenBufferHandle, 
												const TPicData& PicData, const TViewerStatus& ViewerStatus, 
												const TEventData* MscEventData)
{
	if (g_nControlRequest != 1)
		return true;

	++g_nControlState; // will be decreased in NewOffscreenImageCallback

	// switch between the both decompression buffers
	if(OffscreenBufferHandle == m_GscDecompBuffer2)
    {
        m_CSBuffer1.Enter();
		OffscreenBufferHandle = m_GscDecompBuffer1;
    }
    else
    {
        m_CSBuffer2.Enter();
		OffscreenBufferHandle = m_GscDecompBuffer2;
    }

	return true;
}

DWORD DFFViewer::Channel::ProcessImageThread()
{
	while (g_nControlRequest > 0)
	{
		::WaitForSingleObject(m_hWaitForPictureEvent, INFINITE);
		ProcessImage();
	}
	return 0;
}

void DFFViewer::Channel::CreatePLC()
{
	DestroyPLC();
	// create the PLC object instance
	m_GscPLC = m_GscServer->CreatePLC();
	if(m_GscPLC != NULL)
	{
		// open a callback for handling server notifications (actions, events, ...)
		m_GscPLC->OpenPushCallback(PLCCallbackCB, this);
		// listen to all actions and all events in the callback
		m_GscPLC->SubscribeActionsAll();
		m_GscPLC->SubscribeEventsAll();
		m_FaceCache.SendCustomAction(m_GscPLC, L"PLC server created");
	}
}

void DFFViewer::Channel::DestroyPLC(bool ConnectionGotLost /*= false*/)
{
	if (m_GscPLC != NULL)
	{
		if (!ConnectionGotLost)
		{
			m_GscPLC->UnsubscribeAll();
			m_GscPLC->CloseCallback();
		}
		m_GscPLC->Destroy();
		m_GscPLC = NULL;
	}
}

void DFFViewer::Channel::PLCCallback(TGscPlcNotificationType NotificationType, HGscPlcNotification Notification)
{
	wstring message;
	bool NewPLCNotificationAdded = false;

	switch (NotificationType)
	{
	case plcnPushCallbackLost:
		// connection break down
		DisconnectFromServer(true);
		message = s_ViewerChannel + I2wstring(m_nChannel) + s_Error + L"Connection to GSC server lost!\r\n";
		LogDebugString(message);
		break;
	}

	// free the notification object instance
	Notification->Destroy();
}
