#include "StdAfx.h"

#include <gscactionstypes.h>

#include "FaceGallery.h"
#include "Helpers.h"

//using namespace GscPLC;

///////////////////////////////////////////////////
// FaceTemplate
///////////////////////////////////////////////////

FaceTemplate::FaceTemplate()
{
	nId = 0;
	buffer = NULL;
	bufferSize = 0;	
	eStatus = TEMPLATE_NEW;
}

FaceTemplate::FaceTemplate(const FaceTemplate &fct)
{
	Copy(fct);
}

FaceTemplate::~FaceTemplate()
{
	Clear();
}

void FaceTemplate::Copy(const FaceTemplate &fct)
{
	nId = fct.nId;
	details = fct.details;
	strName = fct.strName;
	eStatus = fct.eStatus;
	if (fct.bufferSize > 0)
	{
		bufferSize = fct.bufferSize;
		buffer = new NByte[fct.bufferSize];
		memcpy(buffer, fct.buffer, fct.bufferSize);
	}
	else
	{
		bufferSize = 0;
		buffer = NULL;
	}
}

void FaceTemplate::Clear()
{
	if (buffer)
	{
		delete[] buffer;
		buffer = NULL;
		bufferSize = 0;
	}
}

///////////////////////////////////////////////////
// FaceGallery
///////////////////////////////////////////////////

FaceGallery::FaceGallery()
{
	InitializeCriticalSection(&m_cs);
}

FaceGallery::~FaceGallery()
{	
	m_CollectionTemplates.clear();
}

FaceTemplate FaceGallery::GetTemplate(size_t i)
{
	FaceTemplate ft;
	EnterCriticalSection(&m_cs); 
	ft = m_CollectionTemplates[i];
	LeaveCriticalSection(&m_cs); 
	return ft;
}

void FaceGallery::AddTemplate(FaceTemplate& faceTemplate)
{
	EnterCriticalSection(&m_cs); 
	m_CollectionTemplates.push_back(faceTemplate);
	LeaveCriticalSection(&m_cs); 
}

///////////////////////////////////////////////////
// FaceCache
///////////////////////////////////////////////////

FaceCache::FaceCache()
{
	m_nNextTemplateId = 1;
}

FaceCache::~FaceCache()
{	
}

void FaceCache::InitializeFrame()
{
	// mark all templates in gallery as unprocessed
	for (std::vector<FaceTemplate>::iterator p = m_CollectionTemplates.begin(); p != m_CollectionTemplates.end(); p++)
		p->eStatus = TEMPLATE_UNPROCESSED;
}

void FaceCache::FinalizeFrame(HGscPLC pGscPLC)
{
	unsigned int n = 0;
	for (std::vector<FaceTemplate>::iterator p = m_CollectionTemplates.begin(); p != m_CollectionTemplates.end(); p++)
	{
		switch (p->eStatus) 
		{
			case TEMPLATE_UNPROCESSED:
				n++;
			case TEMPLATE_NEW:
			case TEMPLATE_FOUND:
				if (pGscPLC)
					SendCustomAction(pGscPLC, p->nId, p->strName.c_str(), p->eStatus);
				break;
		}
	}
	// remove expired faces from cache
	if (n > 0)
		m_CollectionTemplates.erase(m_CollectionTemplates.begin(), m_CollectionTemplates.begin() + n);
}

void FaceCache::ProcessTemplate(FaceTemplate& faceTemplate, int nIndexOfFound)
{
	if (nIndexOfFound >=0 && (size_t)nIndexOfFound < m_CollectionTemplates.size())
	{
		// replace matching template in cache with recend
		m_CollectionTemplates.erase(m_CollectionTemplates.begin() + nIndexOfFound);
		faceTemplate.eStatus = TEMPLATE_PROCESSED;
	}
	else if (faceTemplate.eStatus == TEMPLATE_NEW)
		faceTemplate.nId = GetNextTemplateId();		// assign subsequient number
	m_CollectionTemplates.push_back(faceTemplate);	// add template to cache. FaceTemplate object is copied into vector
	// fire video alarm Action
	//TPlcRect rect(faceTemplate.details.Face.Rectangle.Y, faceTemplate.details.Face.Rectangle.X, 
	//			  faceTemplate.details.Face.Rectangle.Y + faceTemplate.details.Face.Rectangle.Height,
	//			  faceTemplate.details.Face.Rectangle.X + faceTemplate.details.Face.Rectangle.Width);
	//actionSender.PushSensorVideoAlarmEx(nChannel, vskIPAD, 0, 0, 0, 0, 0, &rect, 0);
}

unsigned int FaceCache::GetNextTemplateId()
{
	if (m_nNextTemplateId >= 1000)
		m_nNextTemplateId = 1;
	return m_nNextTemplateId++;
}

void FaceCache::SendCustomAction(HGscPLC pGscPLC, const wchar_t *pMessage)
{
	if(pGscPLC == NULL)
		return;

	// send a custom action to the GeViScope server

	// create a custom action
	HGscAction CustomAction = GscAct_CreateCustomAction(1, pMessage);
	if (CustomAction != NULL)
	{
		// store the action in a binary buffer
		unsigned int nSize = 0;
		CustomAction->StoreToBinBuffer(NULL, 0, &nSize);
		if (nSize)
		{
			void* pBuffer = new byte[nSize];
			// send the action by sending the binary buffer
			if (CustomAction->StoreToBinBuffer(pBuffer, nSize, NULL))
				pGscPLC->SendAction(nSize, pBuffer);
			delete[] pBuffer;
		}
		delete CustomAction;
	}
}

void FaceCache::SendCustomAction(HGscPLC pGscPLC, const unsigned int nId, const wchar_t *strName, template_status status)
{
	wchar_t buffer[256];
	const wchar_t msg0[] = L"Person %u %s observed area";
	const wchar_t msg1[] = L"Person %u (%s) %s observed area";
	const wchar_t msg3[] = L"Person %u recognized as %s";
	const wchar_t msgEntr[] = L"entered";
	const wchar_t msgLeft[] = L"left";

	switch (status)
	{
	case TEMPLATE_NEW:
		if (wcslen(strName))
			swprintf(buffer, 256, msg1, nId, strName, msgEntr);
		else
			swprintf(buffer, 256, msg0, nId, msgEntr);
		break;
	case TEMPLATE_UNPROCESSED:
		if (wcslen(strName))
			swprintf(buffer, 256, msg1, nId, strName, msgLeft);
		else
			swprintf(buffer, 256, msg0, nId, msgLeft);
		break;
	case TEMPLATE_FOUND:
		swprintf(buffer, 256, msg3, nId, strName);
		break;
	default:
		return;
	}

	SendCustomAction(pGscPLC, buffer);
	LogDebugString(FormatWString(L">>>SUCCESS>>>SendCustomAction: %s\r\n", buffer));
}
