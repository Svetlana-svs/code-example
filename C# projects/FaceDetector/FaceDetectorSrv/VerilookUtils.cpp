#include "stdafx.h"

#include <NCore.h>

// system headers
#ifndef _CRT_SECURE_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
#endif // !defined(_CRT_SECURE_NO_WARNINGS)
#ifndef _CRT_NON_CONFORMING_SWPRINTFS
#define _CRT_NON_CONFORMING_SWPRINTFS
#endif // !defined(_CRT_NON_CONFORMING_SWPRINTFS)

#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#include <tchar.h>
#include <Windows.h>

//#pragma comment( lib, "bioapi20.dll.lib" )
#pragma comment( lib, "NBiometrics.dll.lib" )
//#pragma comment( lib, "NBiometricStandards.dll.lib" )
//#pragma comment( lib, "NBiometricTools.dll.lib" )
//#pragma comment( lib, "NCluster.dll.lib" )
#pragma comment( lib, "NCore.dll.lib" )
//#pragma comment( lib, "NDevices.dll.lib" )
#pragma comment( lib, "NImages.dll.lib" )
//#pragma comment( lib, "NMedia.dll.lib" )
//#pragma comment( lib, "NMediaProc.dll.lib" )
#pragma comment( lib, "NLicensing.dll.lib" )
//#pragma comment( lib, "NSmartCards.dll.lib" )
//#pragma comment( lib, "sqlite3.dll.lib" )

#ifndef N_PRODUCT_HAS_NO_LICENSES
#include <NLicensing.h>
#endif

#include "VerilookUtils.h"

#ifdef _DEBUG

#define INITMSGCHAIN					\
	m_pMsg = m_cMessage;				\
	m_nRestLen = MESSMAXCHAR

#define SHIFTMSGPTR						\
	m_nRestLen -= m_nMslen;				\
	m_pMsg     += m_nMslen

#define LOGERROR(msg)															\
	if (m_nRestLen > 0)															\
	{																			\
		m_nMslen = swprintf(m_pMsg, m_nRestLen, (msg));							\
		SHIFTMSGPTR;															\
	}

#define LOGERROR1(msg, par1)													\
	if (m_nRestLen > 0)															\
	{																			\
		m_nMslen = swprintf(m_pMsg, m_nRestLen, (msg), (par1));					\
		SHIFTMSGPTR;															\
	}

#define LOGERROR2(msg, par1, par2)												\
	if (m_nRestLen > 0)															\
	{																			\
		m_nMslen = swprintf(m_pMsg, m_nRestLen, (msg), (par1), (par2));			\
		SHIFTMSGPTR;															\
	}

#define MSGALLOC_(msg,lng)														\
	(msg) = new NChar[(lng) + 1];												\
	if ((msg) == NULL)															\
	{																			\
		LOGERROR(L"ERROR: not enough memory ...");								\
		return

#define MSGALLOC(msg, lng)			MSGALLOC_((msg),(lng)) ;}
#define MSGALLOCRET(msg, lng, ret)	MSGALLOC_((msg),(lng)) (ret);}

#else  // !_DEBUG

#define INITMSGCHAIN ;
#define LOGERROR(msg) ;
#define LOGERROR1(msg, par1) ;
#define LOGERROR2(msg, par1, par2) ;

#endif //  _DEBUG

VerilookUtils::VerilookUtils()
{
	m_entries = NULL;
	INITMSGCHAIN;
}

VerilookUtils::~VerilookUtils()
{
	delete[] m_entries;
}

wstring VerilookUtils::GetLastErrorStr()
{
	wstring str(m_cMessage);
	INITMSGCHAIN;
	return str;
}

NResult VerilookUtils::obtainLicense(const NChar ** licenseList, NInt count)
{
#ifndef N_PRODUCT_HAS_NO_LICENSES
	NResult result;
	NBool available;
	NInt i, j;

	INITMSGCHAIN;
	NChar * address, * port, * license;

	if (m_entries == NULL)
	{
		m_entries = CfgLoad();
		if (m_entries == NULL)
		{
			LOGERROR(L"failed to load license configuration file!");
			return N_E_FAILED;
		}
	}

	// Map licenses back
	for (i = 0; i < count; i++)
	{
		license = CfgGetValue(m_entries, licenseList[i]);
		if (license == NULL)
		{
			licenseList[i] = N_T("");
		}
		else
		{
			licenseList[i] = license;
		}
	}

	// Remove duplicates from the array
	for (i = 0; i < count - 1; i++)
	{
		if (_tcscmp(licenseList[i], N_T("")) == 0)
		{
			continue;
		}

		for (j = i + 1; j < count; j++)
		{
			if (_tcscmp(licenseList[i], licenseList[j]) == 0)
			{
				licenseList[j] = N_T("");
			}
		}
	}

	address = CfgGetValue(m_entries, N_T("Address"));
	port = CfgGetValue(m_entries, N_T("Port"));

	for (i = 0; i < count; i++)
	{
		if (_tcscmp(licenseList[i], N_T("")) == 0)
		{
			continue;
		}

		result = NLicenseObtain(address, port, licenseList[i], &available);
		if (NFailed(result))
		{
			printErrorMsg(N_T("NLicenseObtain() failed, result = %d"), result);
			return result;
		}
		LOGERROR2(L"license for %s %s!", licenseList[i], available ? L"obtained" : L"not available");
		if (!available)
		{
			// If not available
			return N_E_FAILED;
		}
	}
#else
	N_UNREFERENCED_PARAMETER(licenseList);
	N_UNREFERENCED_PARAMETER(count);
#endif // !N_PRODUCT_HAS_NO_LICENSES

	return N_OK;
}

NResult VerilookUtils::releaseLicense(const NChar** licenseList, NInt count)
{
#ifndef N_PRODUCT_HAS_NO_LICENSES
	NResult result;
	NInt i;
	INITMSGCHAIN;

	for (i = 0; i < count; i++)
	{
		if (_tcscmp(licenseList[i], N_T("")) == 0)
		{
			continue;
		}

		result = NLicenseRelease(licenseList[i]);
		if (NFailed(result))
		{
			printErrorMsg(N_T("NLicenseRelease() failed, result = %d"), result);
			return result;
		}
		LOGERROR1(L"license for %s released", licenseList[i]);
	}
#else
	N_UNREFERENCED_PARAMETER(licenseList);
	N_UNREFERENCED_PARAMETER(count);
#endif // !N_PRODUCT_HAS_NO_LICENSES

	return N_OK;
}

NChar* VerilookUtils::stripWhitespaces(const NChar * str)
{
	const NChar *end;
	NSizeType out_size;
	NChar * out_str = NULL;
	NSizeType len = _tcslen(str);

	// Trim leading space
	while(isspace(*str)) str++;
	// Trim trailing space
	end = str + _tcslen(str) - 1;
	while(end > str && (isspace(*end) || *end == '\r' || *end == '\r\n')) end--;
	end++;

	out_size = (NSizeType)(end - str) < len ? (end - str) : len;

	out_str = new NChar[out_size + 100];

	_tcscpy(out_str, str);
	out_str[out_size] = N_T('\0');

	return out_str;
}

void VerilookUtils::CfgParseLine(const NChar* line, CfgEntry * pEntry)
{
	int len = 0;
	NChar name[512] = {N_T('\0'), 0};
	NChar value[512] = {N_T('\0'), 0};
	NChar * name_stripped = NULL;
	NChar * value_stripped = NULL;

	_tcscpy(name, line);

	while (*line)
	{

		if ((N_T('=') == *line))
		{
			name[len] = 0;
			name_stripped = stripWhitespaces(name);

			if (*line)
			{
				_tcscpy(value, line + 1);
				value_stripped = stripWhitespaces(value);
			}

			pEntry->Name = name_stripped;
			pEntry->Value = value_stripped;

			return;
		}
		else
		{
			line++;
			len++;
		}
	}
}

CfgEntry* VerilookUtils::CfgLoad()
{
	CfgEntry * result, *current = NULL;
	FILE *file = NULL;
	NChar line[1024 + 2];
	NChar *path = getCfgPath();
	int lines = 0;

	if (path == NULL)
		return NULL;
	file = _tfopen(path, N_T("r"));
	if (file == NULL)
		return NULL;

	// count lines
	while (_fgetts(line, (sizeof(line) / sizeof(line[0]) - 1), file) != NULL)
	{
		lines++;
	}
	lines ++;

	result = new CfgEntry[lines];
	current = result;

	fseek(file, 0, SEEK_SET);

	while (_fgetts(line, sizeof(line) / sizeof(line[0]), file) != NULL)
	{
		CfgParseLine(line, current);
		if (current->Name)
		{
			current ++;
		}
	}

	fclose(file);

	return result;
}

NChar* VerilookUtils::CfgGetValue(CfgEntry * entries, const NChar * valueName)
{
	CfgEntry * currentElement;

	for (currentElement = entries; currentElement->Name != NULL; currentElement++)
	{
		if (_tcscmp(currentElement->Name, valueName) == 0)
		{
			return currentElement->Value;
		}
	}

	return NULL;
}

NResult VerilookUtils::decodeTime(NLong dateTime, time_t *time, NInt *miliseconds)
{
	NLong UnixEpoch = 621355968000000000LL;
	NLong TicksPerSecond = 10000000LL;
	NLong TicksPerMillisecond = 10000LL;
	NLong value1, value2;

	dateTime -= UnixEpoch;
	value1 = dateTime / TicksPerSecond;
	value2 = (dateTime - value1 * TicksPerSecond) / TicksPerMillisecond; 
	if(dateTime < 0 || value1 > (NLong)(((NULong)((time_t)-1)) / 2))
		return N_E_OVERFLOW;
	if(dateTime - value1 * TicksPerSecond - value2 * TicksPerMillisecond >= (TicksPerMillisecond / 2))
	{
		if (++value2 == 1000)
		{
			value2 = 0;
			if (value1 == (NLong)(((NULong)((time_t)-1)) / 2)) return N_E_OVERFLOW;
			value1++;
		}
	}
	*time = (time_t) value1;
	*miliseconds = (NInt)value2;

	return N_OK;
}

NChar* VerilookUtils::getCfgPath()
{
	NChar szDrive[_MAX_DRIVE];
	NChar szDir[_MAX_DIR];
	static NChar szPath[_MAX_PATH];

	if (!GetModuleFileName(NULL, szPath, _MAX_PATH))
	{
		return NULL;
	}

	if (_tsplitpath_s(szPath, szDrive, _MAX_DRIVE, szDir, _MAX_DIR, NULL, 0, NULL, 0 ))
	{
		return NULL;
	}

	if (_tmakepath_s(szPath, _MAX_PATH, szDrive, szDir, N_T("NLicenses"),N_T("cfg")))
	{
		return NULL;
	}

	return szPath;
}

void VerilookUtils::printError(NResult result)
{
#ifdef _DEBUG
	HNError error = NErrorGetLast();
	if (error != NULL)
	{
		printErrorInformation(error);
	}
	else
	{
		NChar* message = getDefaultErrorMessage(result);
		LOGERROR1(L"error code: %d", result);
		if (message)
		{
			LOGERROR1(L"error description: %s", message);
			delete[] message;
		}
	}
#else // !_DEBUG
	N_UNREFERENCED_PARAMETER(result);
#endif // _DEBUG
}

void VerilookUtils::printErrorMsg(NChar *errorMessage, NResult result)
{
#ifdef _DEBUG
	INITMSGCHAIN;
	LOGERROR1(errorMessage, result);
	LOGERROR(L"\r\n");
	printError(result);
#else // !_DEBUG
	N_UNREFERENCED_PARAMETER(errorMessage);
	N_UNREFERENCED_PARAMETER(result);
#endif // _DEBUG
}

#ifdef _DEBUG

void VerilookUtils::printCallStack(HNError error)
{
	NInt i, count, length;
	NChar *message = NULL;

	if (NSucceeded(NErrorGetCallStackCount(error, &count)))
	{
		for (i = 0; i != count; i++)
		{
			length = NErrorGetCallStackFunctionEx(error, i, NULL, N_INT_MAX);
			if (NFailed(length))
			{
				return;
			}
			MSGALLOC(message, length);
			if (NFailed(NErrorGetCallStackFunctionEx(error, i, message, N_INT_MAX)))
			{
				delete[] message;
				return;
			}
			LOGERROR1(L"    at %s", message);
			delete[] message;
			length = NErrorGetCallStackFileEx(error, i, NULL, N_INT_MAX);
			if (NFailed(length))
				return;
			if (length > 0)
			{
				message = new NChar[length + 1];
				if (NFailed(NErrorGetCallStackFileEx(error, i, message, N_INT_MAX)))
				{
					delete[] message;
					return;
				}
				if (NFailed(NErrorGetCallStackLineEx(error, i, &length)))
				{
					delete[] message;
					return;
				}
				LOGERROR2(L" in %s:line %d\r\n", message, length);
				delete[] message;
			}
			else
			{
				LOGERROR(L"\r\n");
			}
		}
	}
}

NChar * VerilookUtils::getErrorMessage(HNError error)
{
	NInt length;
	NChar *message = NULL;
	length = NErrorGetMessageEx(error, NULL, N_INT_MAX);
	if (NFailed(length))
	{
		return NULL;
	}
	MSGALLOCRET(message, length, NULL);
	if (NFailed(NErrorGetMessageEx(error, message, N_INT_MAX)))
	{
		delete[] message;
		return NULL;
	}

	return message;
}

NChar * VerilookUtils::getErrorParamMessage(HNError error)
{
	NInt length;
	NChar *message = NULL;
	length = NErrorGetParamEx(error, NULL, N_INT_MAX);
	if (NFailed(length))
	{
		return NULL;
	}
	MSGALLOCRET(message, length, NULL);
	if (NFailed(NErrorGetParamEx(error, message, N_INT_MAX)))
	{
		delete[] message;
		return NULL;
	}

	return message;
}

NChar * VerilookUtils::getErrorExternalCallStack(HNError error)
{
	NInt length;
	NChar *message = NULL;
	length = NErrorGetExternalCallStackEx(error, NULL, N_INT_MAX);
	if (NFailed(length))
	{
		return NULL;
	}
	MSGALLOCRET(message, length, NULL);
	if (NFailed(NErrorGetExternalCallStackEx(error, message, N_INT_MAX)))
	{
		delete[] message;
		return NULL;
	}

	return message;
}

NChar * VerilookUtils::getDefaultErrorMessage(NResult result)
{
	NInt length;
	NChar *message = NULL;
	length = NErrorGetDefaultMessageEx(result, NULL, N_INT_MAX);
	if (NFailed(length))
		return NULL;

	MSGALLOCRET(message, length, NULL);
	if (NFailed(NErrorGetDefaultMessageEx(result, message, N_INT_MAX)))
	{
		delete[] message;
		return NULL;
	}
	return message;
}

void VerilookUtils::printErrorInformation(HNError error)
{
	NResult code;
	NChar* message;
	HNError innerError;
	NInt externalError = 0;

	if (NSucceeded(NErrorGetCodeEx(error, &code)))
	{
		LOGERROR1(L"error code: %d\r\n", code);
	}
	
	message = getErrorMessage(error);
	if (message != NULL)
	{
		LOGERROR1(L"error description: %s\r\n", message);
		delete[] message;
	}

	message = getErrorParamMessage(error);
	if (message != NULL)
	{
		LOGERROR1(L"param: %s\r\n", message);
		delete[] message;
	}

	 if (NSucceeded(NErrorGetExternalErrorEx(error, &externalError)))
	{
		LOGERROR1(L"external error: %d\r\n", externalError);
	}

	 if (NSucceeded(NErrorGetInnerErrorEx(error, &innerError)))
	{
		LOGERROR(L" ---> \r\n");
		printErrorInformation(innerError); // RECURSION
		LOGERROR(L"\r\n   --- End of inner exception stack trace ---\r\n");
	}

	message = getErrorExternalCallStack(error);
	if (message != NULL)
	{
		LOGERROR1(L"%s\r\n", message);
		delete[] message;
	}
	printCallStack(error);
}

#endif // _DEBUG

wstring VerilookUtils::GetErrorString(NResult result)
{
	switch (result)
	{
	case N_OK:
		return L"Ok";
	case N_E_FAILED:
		return L"common failure";
	case N_E_CORE:
		return L"core failure";
	case N_E_ABANDONED_MUTEX:
		return L"abandoned mutex";
	case N_E_ARGUMENT:
		return L"argument failure";
	case N_E_ARGUMENT_NULL:
		return L"argument is null";
	case N_E_ARGUMENT_OUT_OF_RANGE:
		return L"argument is out of range";
	case N_E_INVALID_ENUM_ARGUMENT:
		return L"argument of invalid enum";
	case N_E_ARITHMETIC:
		return L"arithmetic failure";
	case N_E_OVERFLOW:
		return L"overflow";
	case N_E_BAD_IMAGE_FORMAT:
		return L"bad image format";
	case N_E_DLL_NOT_FOUND:
		return L"dll not found";
	case N_E_ENTRY_POINT_NOT_FOUND:
		return L"entry point not found";
	case N_E_FORMAT:
		return L"format failure";
	case N_E_FILE_FORMAT:
		return L"file format failure";
	case N_E_INDEX_OUT_OF_RANGE:
		return L"index out of range";
	case N_E_INVALID_CAST:
		return L"invalid cast";
	case N_E_INVALID_OPERATION:
		return L"invalid operation";
	case N_E_IO:
		return L"IO failure";
	case N_E_DIRECTORY_NOT_FOUND:
		return L"directory not found";
	case N_E_DRIVE_NOT_FOUND:
		return L"drive not found";
	case N_E_END_OF_STREAM:
		return L"end of stream";
	case N_E_FILE_NOT_FOUND:
		return L"file not found";
	case N_E_FILE_LOAD:
		return L"file loading failure";
	case N_E_PATH_TOO_LONG:
		return L"path too long";
	case N_E_NOT_IMPLEMENTED:
		return L"not implemented";
	case N_E_NOT_SUPPORTED:
		return L"not supported";
	case N_E_NULL_REFERENCE:
		return L"null reference";
	case N_E_OUT_OF_MEMORY:
		return L"out of memory";
	case N_E_SECURITY:
		return L"security violation";
	case N_E_TIMEOUT:
		return L"timeout expired";
	case N_E_EXTERNAL:
		return L"external error";
	case N_E_CLR:
		return L"CLR error";
	case N_E_COM:
		return L"COM error";
	case N_E_CPP:
		return L"CPP error";
	case N_E_JVM:
		return L"JVM error";
	case N_E_MAC:
		return L"MAC error";
	case N_E_SYS:
		return L"SYS error";
	case N_E_WIN32:
		return L"WIN32 error";
	case N_E_PARAMETER:
		return L"parameter failure";
	case N_E_PARAMETER_READ_ONLY:
		return L"parameter read-only failure";
	case N_E_NOT_ACTIVATED:
		return L"not activated";
	default:
		return L"";
	}
}

wstring VerilookUtils::GetExtractionErrorString(NleExtractionStatus status)
{
	switch (status)
	{
	case nleesEyesNotDetected:
		return L"eyes not detected";
	case nleesFaceNotDetected:
		return L"face not detected";
	case nleesFaceTooCloseToImageBorder:
		return L"face too close to image border";
	case nleesLivenessCheckFailed:
		return L"liveness check failed";
	case nleesQualityCheckExposureFailed:
		return L"quality exposure check failed";
	case nleesQualityCheckGrayscaleDensityFailed:
		return L"quality grayscale density failed";
	case nleesQualityCheckSharpnessFailed:
		return L"quality sharpness failed";
	default:
		return L"";
	}
}
