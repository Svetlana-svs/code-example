#pragma once

#ifndef SynchronizationH
#define SynchronizationH

#include <windows.h>

////////////////////////////////////////////////////////////////////////////////////
//  Synchronized
////////////////////////////////////////////////////////////////////////////////////

class SyncInt;

template<class T>
class Synchronized
{
public:
	Synchronized() { InitializeCriticalSection(&m_cs); }
	void set(const T& val);
	const T& operator = (const T& val) { set(val); return m_val; }
	T get();
	operator T() { return get(); }
	virtual void copy(T& valDst, const T& valSrc) { valDst = valSrc; }	// The assign operation of the type T should be thread safe.
																		// Otherwise use the custom copy overlaoding.
protected:
	T m_val;
	CRITICAL_SECTION m_cs;
};

template<class T> 
void Synchronized<T>::set(const T& val)
{
	EnterCriticalSection(&m_cs); 
	copy(m_val, val);
	LeaveCriticalSection(&m_cs); 
}

template<class T> 
T Synchronized<T>::get()
{
	T val;
	EnterCriticalSection(&m_cs); 
	copy(val, m_val);
	LeaveCriticalSection(&m_cs); 
	return val;
}

////////////////////////////////////////////////////////////////////////////////////
//  SyncInt
////////////////////////////////////////////////////////////////////////////////////

class SyncInt : public Synchronized<int>
{
public:
	const int& operator = (const int& val) { set(val); return m_val; }	// can not be inherited
	operator int() { return get(); }									// can not be inherited
	const int& increase(const int& val);
	const int& decrease(const int& val);
	const int& operator += (const int& val) { return increase(val); }
	const int& operator -= (const int& val) { return decrease(val); }
	const int& operator ++ () { return increase(1); }
	const int& operator -- () { return decrease(1); }
};

inline const int& SyncInt::increase(const int& val)
{
	EnterCriticalSection(&m_cs); 
	m_val += val;
	LeaveCriticalSection(&m_cs); 
	return m_val;
}

inline const int& SyncInt::decrease(const int& val)
{
	EnterCriticalSection(&m_cs); 
	m_val -= val;
	LeaveCriticalSection(&m_cs); 
	return m_val;
}


#endif // SynchronizationH