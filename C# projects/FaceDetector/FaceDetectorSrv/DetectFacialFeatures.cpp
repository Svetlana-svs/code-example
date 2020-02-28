#include "stdafx.h"

#include <windows.h>

#include <bmp.h>

#include "Helpers.h"
#include "DetectFacialFeatures.h"

static const wstring s_FD = L"[FaceDetector::DetectFacialFeatures::";
static const wstring s_Error = s_FD + L"ERROR]: ";
static const wstring s_Info = s_FD + L"info]: ";

///////////////////////////////////////////////////
// DetectionResult
///////////////////////////////////////////////////

DetectionResult::DetectionResult()
{
	facesCount = 0;
	faces = NULL;
	faceTemplates = NULL;
}

DetectionResult::~DetectionResult()
{
	clear();
}

void DetectionResult::clear()
{
	if (faces)
	{
		NFree(faces);
		faces = NULL;
	}
	if (faceTemplates)
	{
		for (NInt i = 0; i < facesCount; i++)
			faceTemplates[i].Clear();
		delete[] faceTemplates;
		faceTemplates = NULL;
	}
	facesCount = 0;
}


///////////////////////////////////////////////////
// DetectFacialFeatures
///////////////////////////////////////////////////

#define VL_LICENSES_COUNT	2
static const NChar * s_pszLicenses[VL_LICENSES_COUNT] = { N_T("FacesExtractor"), N_T("FacesMatcher") };

DetectFacialFeatures::DetectFacialFeatures()
{
	NCoreOnStart();
	m_bInited = false;
	m_bValid = false;
	m_bMatch = false;
	m_extractor = NULL;
	m_matcher = NULL;
}

DetectFacialFeatures::~DetectFacialFeatures()
{
	if (m_extractor)
		NObjectFree(m_extractor);
	if (m_matcher)
		NObjectFree(m_matcher);
	m_vlUtils.releaseLicense(s_pszLicenses, VL_LICENSES_COUNT);
	NCoreOnExitEx(NFalse);
}

bool DetectFacialFeatures::Initialize(Settings& settings, bool bMatch)
{
	m_bMatch = bMatch;

	ParamDetection paramsDetect;
	ParamRecognition paramsMatch;

	LoadSettings(settings, paramsDetect, paramsMatch);

	if (m_bInited)
		return m_bValid;
	m_bInited = true;

	const wstring msg = s_Error + L"Initialize failed: ";
	NResult result = N_OK;

	// check Verilook license
	result = m_vlUtils.obtainLicense(s_pszLicenses, VL_LICENSES_COUNT);
	m_bValid = NSucceeded(result);
	if (!m_bValid)
	{
		wstring message = msg + m_vlUtils.GetLastErrorStr() + L"\r\n";
		LogDebugString(message);
		return false;
	}

	// create extractor
	result = NleCreate(&m_extractor);
	m_bValid = NSucceeded(result);
	if (!m_bValid)
	{
		wstring message = msg + L"NleCreate() failed (result: %s)!\r\n";
		message = FormatWString(message.c_str(), m_vlUtils.GetErrorString(result).c_str());
		LogDebugString(message);
		return false;
	}
	if (!SetExtractorParams(paramsDetect))
		return false;

	// create matcher
	result = NMCreate(&m_matcher);
	m_bValid = NSucceeded(result);
	if (!m_bValid)
	{
		wstring message = msg + L"NMCreate() failed (result: %s)!\r\n";
		message = FormatWString(message.c_str(), m_vlUtils.GetErrorString(result).c_str());
		LogDebugString(message);
		return false;
	}
	if (!SetMatcherParams(paramsMatch))
		return false;

	wstring message = s_Info + L"Virilook is successfilly initialized\r\n";
	LogDebugString(message);
	return true;
}

void DetectFacialFeatures::LoadSettings(Settings& settings, ParamDetection& paramsDetect, ParamRecognition& paramsMatch)
{
	paramsDetect.ConfidenceThr = _wtof(settings.Verilook.Detection[L"ConfidenceThr"].c_str());
	paramsDetect.FavorLargest = settings.Verilook.Detection[L"FavorLargest"].compare(L"true") == 0;
	paramsDetect.LivenessThr = _wtof(settings.Verilook.Detection[L"LivenessThr"].c_str());
	paramsDetect.MaxIOD = _wtoi(settings.Verilook.Detection[L"MaxIOD"].c_str());
	paramsDetect.MinIOD = _wtoi(settings.Verilook.Detection[L"MinIOD"].c_str());
	paramsDetect.MaxRollAngleDev = _wtoi(settings.Verilook.Detection[L"MaxRollAngleDev"].c_str());
	paramsDetect.QualityThr = _wtoi(settings.Verilook.Detection[L"QualityThr"].c_str());
	paramsDetect.RecPerTempl = _wtoi(settings.Verilook.Detection[L"RecPerTempl"].c_str());
	paramsDetect.UseLivnCheck = settings.Verilook.Detection[L"UseLivnCheck"].compare(L"true") == 0;
	paramsMatch.MatchSpeed = settings.Verilook.Recognition[L"MatchSpeed"];
	//paramsMatch.MatchThr = _wtof(settings.Verilook.Recognition[L"MatchThr"].c_str());
}

bool DetectFacialFeatures::SetExtractorParams(ParamDetection& params)
{
	NResult hr;
	const wstring msg = s_Error + L"NObjectSetParameterEx for extractor failed: (result: %s)\r\n";
	if (NFailed(hr = NObjectSetParameterEx(m_extractor, NLEP_FACE_CONFIDENCE_THRESHOLD, N_TYPE_DOUBLE, &params.ConfidenceThr, sizeof(params.ConfidenceThr)))) goto lbl_SetExtractorParams_1;
	if (NFailed(hr = NObjectSetParameterEx(m_extractor, NLEP_FACE_QUALITY_THRESHOLD, N_TYPE_BYTE, &params.QualityThr, sizeof(params.QualityThr)))) goto lbl_SetExtractorParams_1;
	if (NFailed(hr = NObjectSetParameterEx(m_extractor, NLEP_FAVOR_LARGEST_FACE, N_TYPE_BOOL, &params.FavorLargest, sizeof(params.FavorLargest)))) goto lbl_SetExtractorParams_1;
	if (NFailed(hr = NObjectSetParameterEx(m_extractor, NLEP_MAX_IOD, N_TYPE_INT, &params.MaxIOD, sizeof(params.MaxIOD)))) goto lbl_SetExtractorParams_1;
	if (NFailed(hr = NObjectSetParameterEx(m_extractor, NLEP_MIN_IOD, N_TYPE_INT, &params.MinIOD, sizeof(params.MinIOD)))) goto lbl_SetExtractorParams_1;
	if (NFailed(hr = NObjectSetParameterEx(m_extractor, NLEP_USE_LIVENESS_CHECK, N_TYPE_BOOL, &params.UseLivnCheck, sizeof(params.UseLivnCheck)))) goto lbl_SetExtractorParams_1;
	if (NFailed(hr = NObjectSetParameterEx(m_extractor, NLEP_LIVENESS_THRESHOLD, N_TYPE_DOUBLE, &params.LivenessThr, sizeof(params.LivenessThr)))) goto lbl_SetExtractorParams_1;
	if (NFailed(hr = NObjectSetParameterEx(m_extractor, NLEP_MAX_ROLL_ANGLE_DEVIATION, N_TYPE_SHORT, &params.MaxRollAngleDev, sizeof(params.MaxRollAngleDev)))) goto lbl_SetExtractorParams_1;
	if (NFailed(hr = NObjectSetParameterEx(m_extractor, NLEP_MAX_RECORDS_PER_TEMPLATE, N_TYPE_INT, &params.RecPerTempl, sizeof(params.RecPerTempl)))) goto lbl_SetExtractorParams_1;
	return true;
lbl_SetExtractorParams_1:
	LogDebugString(FormatWString(msg.c_str(), m_vlUtils.GetErrorString(hr).c_str()));
	return false;
}

bool DetectFacialFeatures::SetMatcherParams(ParamRecognition& params)
{
	NResult hr;
	const wstring msg = s_Error + L"NObjectSetParameterEx for matcher failed: (result: %s)\r\n";
	//NDouble matchThr = params.MatchThr / 100; // actual range is [0..1]
	//if (NFailed(hr = NObjectSetParameterEx(m_matcher, NLMP_MATCHING_THRESHOLD, N_TYPE_DOUBLE, &matchThr, sizeof(matchThr)))) goto lbl_SetMatcherParams_1;
	NInt nMatchSpeed = 0;
	if (params.MatchSpeed.compare(L"low") == 0)
		nMatchSpeed = nlmsLow;
	else if (params.MatchSpeed.compare(L"high") == 0)
		nMatchSpeed = nlmsHigh;
	if (NFailed(hr = NObjectSetParameterWithPartEx(m_matcher, NM_PART_FACES + NLSM_PART_NLM, NLMP_MATCHING_SPEED, N_TYPE_INT, &nMatchSpeed, sizeof(nMatchSpeed)))) goto lbl_SetMatcherParams_1;
	return true;
lbl_SetMatcherParams_1:
	LogDebugString(FormatWString(msg.c_str(), m_vlUtils.GetErrorString(hr).c_str()));
	return false;
}

bool DetectFacialFeatures::DetectBitmap(HBITMAP hBmp, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	const wstring msg = s_Error + L"DetectBitmap failed: ";
	if (!m_bValid)
	{
		wstring message = msg + L"DetectFacialFeatures is not initialized";
		LogDebugString(message);
		return false;
	}

	HNImage image;
	NResult result;

	// read image
	result = NImageCreateFromHBitmap(hBmp, 0, &image);
	if (NFailed(result))
	{
		wstring message = msg + L"NImageCreateFromHBitmap() failed (result: %s)!\r\n";
		message = FormatWString(message.c_str(), m_vlUtils.GetErrorString(result).c_str());
		LogDebugString(message);
		return false;
	}
	
	return DetectNImage(image, bIsGrayscale, faceCache, faceGallery, pGscPLC);
}

bool DetectFacialFeatures::DetectBitmapInfo(BITMAPINFO* pBitmapInfo, NSizeType bitmapInfoSize, const void* pBits, NSizeType bitsSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	const wstring msg = s_Error + L"DetectBitmapInfo failed: ";
	if (!m_bValid)
	{
		wstring message = msg + L"DetectFacialFeatures is not initialized!\r\n";
		LogDebugString(message);
		return false;
	}

	HNImage image;
	NResult result;

	// read image
	result = NImageCreateFromBitmapInfoAndBits(pBitmapInfo, bitmapInfoSize, pBits, bitsSize, 0, &image);
	if (NFailed(result))
	{
		wstring message = msg + L"NImageCreateFromBitmapInfoAndBits() failed (result: %s)!\r\n";
		message = FormatWString(message.c_str(), m_vlUtils.GetErrorString(result).c_str());
		LogDebugString(message);
		return false;
	}
	
	return DetectNImage(image, bIsGrayscale, faceCache, faceGallery, pGscPLC);
}

#define _DETECT_IMAGE_(fmt)																	\
	HNImageFormat format;																	\
	NResult result;																			\
	result = NImageFormatGet##fmt(&format);													\
	if (NFailed(result))																	\
	{																						\
		wstring message = s_Error + L"NImageFormatGet";										\
		message += L#fmt;																	\
		message += L"() failed (result: %s)!\r\n";											\
		message = FormatWString(message.c_str(), m_vlUtils.GetErrorString(result).c_str());	\
		LogDebugString(message);															\
		return false;																		\
	}

#define _DETECT_IMAGE_MEM_(fmt)		\
	_DETECT_IMAGE_(fmt)				\
	return DetectMemory(pBuffer, bufferSize, format, L#fmt, bIsGrayscale, faceCache, faceGallery, pGscPLC);


bool DetectFacialFeatures::DetectImage(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	return DetectMemory(pBuffer, bufferSize, NULL, L"<automatic>", bIsGrayscale, faceCache, faceGallery, pGscPLC);
}

bool DetectFacialFeatures::DetectTiff(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_MEM_(Tiff)
}

bool DetectFacialFeatures::DetectJpeg(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_MEM_(Jpeg)
}

bool DetectFacialFeatures::DetectJpeg2K(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_MEM_(Jpeg2K)
}

bool DetectFacialFeatures::DetectPng(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_MEM_(Png)
}

bool DetectFacialFeatures::DetectWsq(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_MEM_(Wsq)
}

bool DetectFacialFeatures::DetectBmp(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_MEM_(Bmp)
}

bool DetectFacialFeatures::DetectIHead(const void * pBuffer, NSizeType bufferSize, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_MEM_(IHead)
}

bool DetectFacialFeatures::DetectMemory(const void * pBuffer, NSizeType bufferSize, HNImageFormat hImageFormat, LPCTSTR pStreamType, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	const wstring msg = s_Error + L"DetectMemory failed: ";

	HNImage image;
	NSizeType size;
	HNImageInfo info;
	NResult result;

	// read image
	result = NImageCreateFromMemory(pBuffer, bufferSize, hImageFormat, 0, &size, &info, &image);
	if (NFailed(result))
	{
		wstring message = msg + L"NImageCreateFromMemory() failed for %s (result: %s)!\r\n";
		message = FormatWString(message.c_str(), pStreamType, m_vlUtils.GetErrorString(result).c_str());
		LogDebugString(message);
		return false;
	}
	
	return DetectNImage(image, bIsGrayscale, faceCache, faceGallery, pGscPLC);
}

#define _DETECT_IMAGE_STEAM_(fmt)	\
	_DETECT_IMAGE_(fmt)				\
	return DetectStream(hStream, format, L#fmt, bIsGrayscale, faceCache, faceGallery, pGscPLC);

bool DetectFacialFeatures::DetectImage(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	return DetectStream(hStream, NULL, L"<automatic>", bIsGrayscale, faceCache, faceGallery, pGscPLC);
}

bool DetectFacialFeatures::DetectTiff(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_STEAM_(Tiff)
}

bool DetectFacialFeatures::DetectJpeg(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_STEAM_(Jpeg)
}

bool DetectFacialFeatures::DetectJpeg2K(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_STEAM_(Jpeg2K)
}

bool DetectFacialFeatures::DetectPng(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_STEAM_(Png)
}

bool DetectFacialFeatures::DetectWsq(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_STEAM_(Wsq)
}

bool DetectFacialFeatures::DetectBmp(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_STEAM_(Bmp)
}

bool DetectFacialFeatures::DetectIHead(HNStream hStream, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	_DETECT_IMAGE_STEAM_(IHead)
}

bool DetectFacialFeatures::DetectStream(HNStream hStream, HNImageFormat hImageFormat, LPCTSTR pStreamType, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	const wstring msg = s_Error + L"DetectStream failed: ";

	HNImage image;
	HNImageInfo info;
	NResult result;

	// read image
	result = NImageCreateFromStream(hStream, hImageFormat, 0, &info, &image);
	if (NFailed(result))
	{
		wstring message = msg + L"NImageCreateFromStream() failed for %s (result: %s)!\r\n";
		message = FormatWString(message.c_str(), pStreamType, m_vlUtils.GetErrorString(result).c_str());
		LogDebugString(message);
		return false;
	}
	
	return DetectNImage(image, bIsGrayscale, faceCache, faceGallery, pGscPLC);
}

bool DetectFacialFeatures::DetectNImage(HNImage hImage, bool bIsGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, HGscPLC pGscPLC)
{
	const wstring msg = s_Error + L"DetectNImage failed: ";

	NResult result;
	DetectionResult detectedFaces;
	int i;

	HNGrayscaleImage hGrayscale;

	if (bIsGrayscale)
		hGrayscale = (HNGrayscaleImage)hImage;
	else
	{
		// convert image to grayscale
		result = NImageToGrayscale(hImage, &hGrayscale);
		NObjectFree(hImage);
		if (NFailed(result))
		{
			wstring message = msg + L"NImageToGrayscale() failed (result: %s)!\r\n";
			message = FormatWString(message.c_str(), m_vlUtils.GetErrorString(result).c_str());
			LogDebugString(message);
			return false;
		}
	}

	// detect all faces in image that are suitable for face recognition
	detectedFaces.clear();
	result = NleDetectFaces(m_extractor, hGrayscale, &detectedFaces.facesCount, &detectedFaces.faces);
	if (NFailed(result))
	{
		wstring message = msg + L"NleDetectFaces() failed (result: %s)!\r\n";
		message = FormatWString(message.c_str(), m_vlUtils.GetErrorString(result).c_str());
		LogDebugString(message);
		return false;
	}

	detectedFaces.faceTemplates = new FaceTemplate[detectedFaces.facesCount]; // this array will then be deleted by DetectionResult clear() or destructor

	faceCache.InitializeFrame(); // mark all templates in gallery as unprocessed
	// traverse through all found faces on frame
	bool bRes = true;
	for (i = 0; i < detectedFaces.facesCount; ++i)
		bRes = bRes && ProcessTemplate(detectedFaces, hGrayscale, faceCache, faceGallery, i);
	
	NObjectFree(hGrayscale);

	// fire GVS Actions on new or expired faces
	faceCache.FinalizeFrame(pGscPLC);

	if (bRes)
	{
		wstring message = s_Info + L"DetectFacialBitmap: Extracted and processed %i faces\r\n";
		message = FormatWString(message.c_str(), detectedFaces.facesCount);
		LogDebugString(message);
	}
	return bRes;
}

bool DetectFacialFeatures::ProcessTemplate(DetectionResult& detectedFaces, HNGrayscaleImage hGrayscale, FaceCache& faceCache, FaceGallery& faceGallery, int nFace)
{
	NResult result;
	HNLTemplate hTemplate;
	NleExtractionStatus status;
	const wstring msg = s_Error + L"ProcessTemplate failed: ";
	bool bRes = true;

	// get objects from collection
	FaceTemplate& faceTemplate = detectedFaces.faceTemplates[nFace]; // here faceTemplate is still empty
	NleFace* pFace = &detectedFaces.faces[nFace];

	// detect eyes for current face
	result = NleDetectFacialFeatures(m_extractor, hGrayscale, pFace, &faceTemplate.details); // fill datails
	if (NFailed(result) || (faceTemplate.details.EyesAvailable == 0))
	{
		bRes = false;
		wstring message = msg + L"Eye detection failed for face %u of %u (result: %%s)\r\n";
		message = FormatWString(message.c_str(), nFace, detectedFaces.facesCount);
		if (NFailed(result))
			message = FormatWString(message.c_str(), m_vlUtils.GetErrorString(result).c_str());
		else // if (faceTemplate.details.EyesAvailable == 0)
			message = FormatWString(message.c_str(), L"no eyes available");
		LogDebugString(message);
	}
	else // eyes are successfully detected
	{
		result = NleExtractUsingDetails(m_extractor, hGrayscale, &faceTemplate.details, &status, &hTemplate); // get template handle from details
		if (NFailed(result) || status != nleesTemplateCreated)
		{
			bRes = false;
			wstring message = msg + L"Extraction of template from image failed for face %u of %u (result: %%s)\r\n";
			message = FormatWString(message.c_str(), nFace, detectedFaces.facesCount);
			if (NFailed(result))
				message = FormatWString(message.c_str(), m_vlUtils.GetErrorString(result).c_str());
			else // if (status == nleesTemplateCreated)
				message = FormatWString(message.c_str(), m_vlUtils.GetExtractionErrorString(status).c_str());
			LogDebugString(message);
		}
		else // extraction successful
		{
			if (ExtractTemplateParams(hTemplate, true, faceTemplate))	// get byte array from handle
			{
				int nIndxFoundCache;
				if (!FindMatchingTemplate(faceCache, faceTemplate, nIndxFoundCache))	// search template in cache
					bRes = false;
				if (m_bMatch && !faceTemplate.strName.length())			// template is still not recognized
				{
					int nIndxFoundGallery;
					if (FindMatchingTemplate(faceGallery, faceTemplate, nIndxFoundGallery))	// search template in database samples
					{
						if (nIndxFoundGallery >=0 && nIndxFoundCache >= 0)
						{
							FaceTemplate& ftCache = faceCache.GetAtPosition(nIndxFoundCache);
							ftCache.strName = faceTemplate.strName;
							if (ftCache.eStatus == TEMPLATE_PROCESSED)		// template is found in cache
								ftCache.eStatus = TEMPLATE_FOUND;			// force to send recognition message
						}
					}
					else
						bRes = false;
				}
			}
			else 
				bRes = false;

			NObjectFree(hTemplate);
		}
	}

	return bRes;
}

bool DetectFacialFeatures::ExtractTemplateParams(HNLTemplate& templ, NBool isProbe, FaceTemplate& faceTemplate)
{
	NResult result;
	NleTemplateSize templSize;
	NSizeType bufferSize;
	const wstring msg = s_Error + L"ExtractTemplateParams failed: ";

	// get the size of the template
	result = NLTemplateGetSize(templ, 0, &bufferSize);
	if (NFailed(result))
	{
		wstring message = msg + FormatWString(L"\tNLTemplateGetSize() failed (result: %s)!\r\n", m_vlUtils.GetErrorString(result).c_str());
		LogDebugString(message);
		return false;
	}

	// set template size
	templSize = (isProbe) ? nletsMedium : nletsLarge;
	result = NObjectSetParameterEx(m_extractor, NLEP_TEMPLATE_SIZE, N_TYPE_INT, &templSize, sizeof(templSize));
	if (NFailed(result))
	{
		wstring message = msg + FormatWString(L"\tNleSetParameter() failed (result: %s)!\r\n", m_vlUtils.GetErrorString(result).c_str());
		LogDebugString(message);
		return false;
	}

	// transform template handle to byte array
	faceTemplate.bufferSize = 0;
	faceTemplate.buffer = new NByte[bufferSize];
	result = NLTemplateSaveToMemory(templ, faceTemplate.buffer, bufferSize, 0, &bufferSize);
	if (NFailed(result))
	{
		delete[] faceTemplate.buffer;
		faceTemplate.buffer = NULL;
		wstring message = msg + FormatWString(L"\tNLTemplateSaveToMemory() failed (result: %s)!\r\n", m_vlUtils.GetErrorString(result).c_str());
		LogDebugString(message);
		return false;
	}
	else
		faceTemplate.bufferSize = bufferSize;

	return true;
}

bool DetectFacialFeatures::FindMatchingTemplate(FaceGallery& faceCollection, FaceTemplate& faceTemplate, int& indexCurrent)

{
	NResult result;
	HNMatchingDetails hMatchingDetails = NULL;
	NInt score;
	const wstring msg = s_Error + L"FindMatchingTemplate failed: ";

	result = NMIdentifyStartEx(m_matcher, faceTemplate.buffer, faceTemplate.bufferSize, &hMatchingDetails);
	if (NFailed(result))
	{
		wstring message = msg + FormatWString(L"NMIdentifyStart() failed (result: %s)!\r\n", m_vlUtils.GetErrorString(result).c_str());
		LogDebugString(message);
		return false;
	}
	
	FaceTemplate fctMatching;
	size_t i, templateCount = faceCollection.GetCount();
	NInt scoreCurrent = 0;
	indexCurrent = -1;
	for (i = 0; i < templateCount; i++)
	{
		FaceTemplate fct = faceCollection.GetTemplate(i);
		if (fct.bufferSize == 0)
			continue;
		result = NMIdentifyNextEx(m_matcher, fct.buffer, fct.bufferSize, hMatchingDetails, &score);
		if (NFailed(result))
		{
			wstring message = msg + FormatWString(L"NMIdentifyNextEx failed for template %i (result: %s)!\r\n", i, m_vlUtils.GetErrorString(result).c_str());
			LogDebugString(message);
			return false;
		}
		else if (score > scoreCurrent) // faceTemplate is found in collection
		{
			indexCurrent = i;
			fctMatching = fct;
		}
	}
	NObjectFree(hMatchingDetails);
	hMatchingDetails = NULL;

	result = NMIdentifyEnd(m_matcher);
	if (NFailed(result))
	{
		wstring message = msg + FormatWString(L"NMIdentifyEnd failed (result: %s)!\r\n", m_vlUtils.GetErrorString(result).c_str());
		LogDebugString(message);
		return false;
	}

	if (indexCurrent >= 0) // faceTemplate is found in collection
	{
		faceTemplate.nId = fctMatching.nId;
		if (fctMatching.strName.length() && !faceTemplate.strName.length())
			faceTemplate.strName = fctMatching.strName;	// assign new Name if still no
	}
	faceCollection.ProcessTemplate(faceTemplate, indexCurrent);
	return true;
}
