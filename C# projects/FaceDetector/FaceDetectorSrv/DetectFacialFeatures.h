#pragma once

#include "FaceGallery.h"
#include "VerilookUtils.h"
#include "Settings.h"

struct ParamDetection
{
	NDouble	ConfidenceThr;		// Specifies the threshold which is considered when looking for faces in an image. For each face candidate confidence parameter is calculated. With higher threshold value faces are selected more strictly by the face detection routines. Must be in range [0..100]. The default value is 50.
	NByte	QualityThr;			// Specifies the threshold which is considered when extracting facial features from the image. With higher threshold better quality of face image is required to successfully extract facial features. The value of this parameter can be in range [0..255]. The default value is 128.
	NBool	FavorLargest;		// If set to true the largest face found in an image will be used for facial feature extraction instead of the one with the highest face confidence threshold score.
	NInt	MaxIOD;				// Specifies the maximum distance between eyes in face. Faces which have greater distance between eyes than this parameter, won't be returned by the face detection routines. Must be in range [10..10000]. The default value is 4000.
	NInt	MinIOD;				// Specifies the minimum distance between eyes in face. Faces which have smaller distance between eyes than this parameter, won't be returned by the face detection routines. Must be in range [10..10000]. The default value is 40.
	NBool	UseLivnCheck;		// If set to true liveness check is performed while doing facial feature extraction from an image stream.
	NDouble	LivenessThr;		// This threshold checks if faces are extracted from a live image stream. The higher the value of this parameter, the more strictly the stream is checked if the face in it is real (for example if it's not forged by showing a photo of a person in front a camera). The value of this parameter can be in range [0..100]. The default value is 50.
	NShort	MaxRollAngleDev;	// Defines maximum roll angle deviation from frontal face in degrees which is considered when looking for faces in an image. Must be in range [0..180]. The default value is 15.
	NInt	RecPerTempl;		// Sets the maximum number of records an extraction function can return in one NLTemplate. The value of this parameter can be in range [1..16]. The default value is 4.
};

struct ParamRecognition
{
	//NDouble	MatchThr;			// !!!CURRENTLY NOT AVAILABLE!!! Specifies the template matching threshold. Must be in range [0..100]. The default value is 60 .
	wstring	MatchSpeed;			// Identifier specifying matching speed parameter. Parameter value can be one of the 'low', 'high'.
};

class DetectionResult
{
public:
	NInt facesCount;
	NleFace *faces;
	FaceTemplate* faceTemplates;
	FaceCache faceCache;

	DetectionResult();
	~DetectionResult();
	void clear();
};

class DetectFacialFeatures
{
public:
	DetectFacialFeatures();
	~DetectFacialFeatures();
	bool Initialize(Settings& settings, bool bMatch);
	bool IsValid() { return m_bValid; }
	// bitmap based
	bool DetectBitmap(HBITMAP hBmp, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectBitmapInfo(BITMAPINFO* pBitmapInfo, NSizeType bitmapInfoSize, const void* pBits, NSizeType bitsSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	// memory based
	bool DetectImage(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectTiff(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectJpeg(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectJpeg2K(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectPng(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectWsq(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectBmp(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectIHead(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	// stream based
	bool DetectImage(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectTiff(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectJpeg(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectJpeg2K(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectPng(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectWsq(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectBmp(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectIHead(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);

private:
	bool m_bInited;
	bool m_bValid;
	bool m_bMatch;
	VerilookUtils m_vlUtils;
	HNLExtractor m_extractor;
	HNMatcher m_matcher;

	void LoadSettings(Settings& settings, ParamDetection& paramsDetect, ParamRecognition& paramsMatch);
	bool SetExtractorParams(ParamDetection& params);
	bool SetMatcherParams(ParamRecognition& params);
	bool DetectMemory(const void * pBuffer, NSizeType bufferSize, HNImageFormat hImageFormat, LPCTSTR pStreamType, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectStream(HNStream hStream, HNImageFormat hImageFormat, LPCTSTR pStreamType, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool DetectNImage(HNImage hImage, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC);
	bool ProcessTemplate(DetectionResult& detectedFaces, HNGrayscaleImage hGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, int nFace);
	bool ExtractTemplateParams(HNLTemplate& templ, NBool isProbe, FaceTemplate& faceTemplate);
	bool FindMatchingTemplate(FaceGallery& faceCache, FaceTemplate& faceTemplate, int& indexCurrent);
};

