#pragma once

#include <memory>
#include <vector>
#include <string>

#include <NLExtractor.h>

#include <GscMediaPlayer.h>
#include <GscActions.h>

using namespace std;
using namespace GeviScope;

enum template_status {TEMPLATE_UNPROCESSED, TEMPLATE_NEW, TEMPLATE_PROCESSED, TEMPLATE_FOUND};    

struct FaceTemplate
{
	unsigned int nId;
	NleDetectionDetails details;
	NByte *buffer;
	NSizeType bufferSize;
	wstring strName;
	template_status eStatus;

	FaceTemplate();
	FaceTemplate(const FaceTemplate &fct);
	~FaceTemplate();
	void Clear();
	void Copy(const FaceTemplate &fct);
	const FaceTemplate& operator = (const FaceTemplate& fct) { Copy(fct); return *this; }
};

class FaceGallery
{
public:
	FaceGallery();
	~FaceGallery();
	size_t GetCount() { return m_CollectionTemplates.size(); };
	FaceTemplate GetTemplate(size_t i);
	void AddTemplate(FaceTemplate& faceTemplate);
	virtual void ProcessTemplate(FaceTemplate& faceTemplate, int nIndexOfFound) {};

protected:
	std::vector<FaceTemplate> m_CollectionTemplates;
	CRITICAL_SECTION m_cs;
};

class FaceCache : public FaceGallery // synchronization for this is not neccessary because this will not be shared
{
public:
	FaceCache();
	~FaceCache();
	void InitializeFrame();
	void FinalizeFrame(HGscPLC pGscPLC);
	void ProcessTemplate(FaceTemplate& faceTemplate, int nIndexOfFound);
	FaceTemplate& GetAtPosition(size_t nIndex) { return m_CollectionTemplates[nIndex]; }
	void SendCustomAction(HGscPLC pGscPLC, const wchar_t *pMessage);
	void SendCustomAction(HGscPLC pGscPLC, const unsigned int nId, const wchar_t *strName, template_status status);

	unsigned int nChannel;

private:
	unsigned int m_nNextTemplateId;

	unsigned int GetNextTemplateId();
	//void SendAction();
};
