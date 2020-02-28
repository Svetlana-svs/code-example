#pragma once

#include <string>
#include <NTypes.h>
#include <NCore.h>
#include <NMatcher.h>
#include <NLExtractor.h>

using namespace std;

typedef struct CfgEntry_
{
	NChar *Name;
	NChar *Value;
} CfgEntry;

#define MESSMAXCHAR 16383

class VerilookUtils
{
public:
	VerilookUtils();
	~VerilookUtils();

	NResult obtainLicense(const NChar** licenseList, NInt count);
	NResult releaseLicense(const NChar** licenseList, NInt count);
	NChar* CfgGetValue(CfgEntry * entries, const NChar * valueName);
	wstring GetLastErrorStr();

	void printErrorMsg(NChar *errorMessage, NResult result);
	void printError(NResult result);
	wstring GetErrorString(NResult result);
	wstring GetExtractionErrorString(NleExtractionStatus status);

private:
	CfgEntry * m_entries;
	NChar m_cMessage[MESSMAXCHAR+1];
	NChar* m_pMsg;
	NInt m_nRestLen;
	NInt m_nMslen;

	NChar* getCfgPath();
	NChar* stripWhitespaces(const NChar * str);
	void CfgParseLine(const NChar* line, CfgEntry * pEntry);
	CfgEntry* CfgLoad();
	NResult decodeTime(NLong dateTime, time_t *time, NInt *miliseconds);

#ifdef _DEBUG
	void printCallStack(HNError error);
	NChar * getErrorMessage(HNError error);
	NChar * getErrorParamMessage(HNError error);
	NChar * getErrorExternalCallStack(HNError error);
	NChar * getDefaultErrorMessage(NResult result);
	void printErrorInformation(HNError error);
#endif // _DEBUG

};





