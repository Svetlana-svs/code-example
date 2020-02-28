#pragma once

#ifndef DFFViewerH
#define DFFViewerH

#include "stdafx.h"

#include <CriticalSection.h>
#include <GscMediaPlayer.h>

#include "DetectFacialFeatures.h"
#include "Settings.h"

#pragma comment(lib, "GscActions.lib")

// #define VIDEOCHANNUM	= Number of video channels. Defined in Settings.h

using namespace GeviScopeSDKHelperClasses;
using namespace GeviScope;

class TMediaChannelDescriptor;

class DFFViewer
{
public:

	//////// class Channel //////////
	class Channel
	{
	public:
		Channel();
		~Channel();
		void SetParent(DFFViewer* pParent, unsigned int nChannel) { m_pParent = pParent; m_nChannel = nChannel; m_FaceCache.nChannel = nChannel; }

		// callback is called from offscreen viewer object after picture has been decompressed
		bool NewOffscreenImageCallback(const HGscDecompBuffer OffscreenBufferHandle,	// Handle to the DecompBuffer object..
									   const TRect& SrcRect,							// Rect of decompressed buffer
									   const TPicData& PicData,							// Picture Information
									   const TViewerStatus& ViewerStatus,				// Viewer status
									   const TEventData* MscEventData);					// return value defines if user was able to handle image

		// callback is called from offscreen viewer object before picture is decompressed
		bool NewOffscreenImageAcceptCallback(HGscDecompBuffer& OffscreenBufferHandle,	// Buffer to decompress, default buffer can be overridden..
											 const TPicData& PicData,					// Picture Information
											 const TViewerStatus& ViewerStatus,			// Viewer status
											 const TEventData* MscEventData);			// return value defines if user wants the image to be decompressed

		void PLCCallback(TGscPlcNotificationType NotificationType, HGscPlcNotification Notification);

		void LoadSettings(MapStringString& mapSettings);
		void ConnectMedia();
		bool ProcessImage();
		DWORD ProcessImageThread();

private:
		unsigned int		m_nChannel;
		wstring				m_strChannel;
		bool				m_bInitialized;
		bool				m_bEnabled;
		bool				m_bMatchNeeded;
		bool				m_isConnected;
		bool				m_isPlaying;

		wstring				m_strGVSHost;
		wstring				m_strGVSLogin;
		wstring				m_strGVSPassword;
		
		HGscServer			m_GscServer;			// handle to a server object
		HGscViewer			m_GscOffscreenViewer;	// handle to a viewer object
	    HGscPLC				m_GscPLC;				// handle to a PLC object
		HGscDecompBuffer	m_GscDecompBuffer1;		// handles to two decompression buffer objects
		HGscDecompBuffer	m_GscDecompBuffer2;		// during the image in one decompression buffer is rendered or processed,
		HGscDecompBuffer	m_NewPicDecompBuffer;	// the next image can allready be decompressed in the other
		bool				m_NewPicDecompressed;	// decompression buffer
		CriticalSection		m_CSBuffer;				// make the Flag "m_NewPicDecompressed" thread-safe
		CriticalSection		m_CSBuffer1;			// make the first decompression buffer thread-safe
		CriticalSection		m_CSBuffer2;			// make the second decompression buffer thread-safe

		HANDLE				m_hWaitForPictureEvent; //

		void CreateOffscreenViewer();
		void DestroyOffscreenViewer();
		void ConnectToGSServer();
		void DisconnectFromServer(bool ConnectionGotLost = false);
		void CreatePLC();
		void DestroyPLC(bool ConnectionGotLost = false);
		//void CheckDimentions(const TRect& SrcRect);
		bool GetMediaChannel(TMediaChannelDescriptor& mediaChannel);

		DFFViewer*	m_pParent;
		/*DWORD		m_AWidth;
		DWORD		m_AHeight;*/

		DetectFacialFeatures	m_FaceDetector;
		FaceCache				m_FaceCache;
	};

	//////// class DFFViewer ////////////////

	DFFViewer();
	~DFFViewer();

	void ConnectMedia();
	bool LoadSettings();
	bool LoadFaceGallery();

	//HBITMAP GetHBitmap(unsigned int whichChannel)
	//{ return whichChannel < VIDEOCHANNUM ? m_Channels[whichChannel].m_hMemBmp : NULL; }		// handle to a windows memory bitmap
	//HDC GetHDC(unsigned int whichChannel)
	//{ return whichChannel < VIDEOCHANNUM ? m_Channels[whichChannel].m_hMemDC : 0; }		// handle to a windows memory bitmap
	//bool IsPlaying(unsigned int whichChannel) { return whichChannel < VIDEOCHANNUM ? m_Channels[whichChannel].m_isPlaying : NULL; }

    //bool GetLockLastReceivedPic(unsigned int whichChannel)
	//{ return whichChannel < VIDEOCHANNUM ? m_Channels[whichChannel].GetLockLastReceivedPic() : false; }
	//void UnlockLastReceivedPic(unsigned int whichChannel)
	//{ if (whichChannel < VIDEOCHANNUM) m_Channels[whichChannel].UnlockLastReceivedPic(); }
	//DetectionResult* GetDetectionResult(unsigned int whichChannel)
	//{ return whichChannel < VIDEOCHANNUM ? &m_Channels[whichChannel].m_detectedFaces : NULL; }

	HANDLE m_hWaitForPictureTheads[VIDEOCHANNUM];	//

private:
	Settings m_Settings;

	CRITICAL_SECTION	m_cs;
	wstring				m_strGVSHost;
	wstring				m_strGVSLogin;
	wstring				m_strGVSPassword;
	ParamDetection		m_paramDetection;
	ParamRecognition	m_paramRecognition;

	FaceGallery m_FaceGallery;

	Channel m_Channels[VIDEOCHANNUM];
};


#endif // DFFViewerH
