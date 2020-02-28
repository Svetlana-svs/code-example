// RE_DLL_CORR.cpp : Defines the initialization routines for the DLL.
//

#include "stdafx.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// mdc.cpp : Defines the initialization routines for the DLL.
//


#include "FTD2XX.h"



//#include "resource.h"
#define ON 4
#define START 3
#define OFF 0xFD 
#define PACKET_ITEM 5
#define SIZE  1112000/2
#define CONTINUES 1
#define FIFO_SIZE 5
#define PAGE  0x4000
#define FRAME 0x8000

#define CONTROL 0x80
#define FREQ 0x81
#define SM_EX 0x8E
 
#define FB 50000000
#define FOKUS 2350

#define STEP_H 9
#define STEP_L 11
#define FSTART_H 1
#define FSTART_L 3
#define FORS_H 5
#define FORS_L 7
#define STEP_UPDW_H 13
#define STEP_UPDW_L 15
#define RESET_H 17
#define RESET_L 19
#define WIDTH 100
#define DY 511
#define ZERO_LEVEL 2000
#define KU_NDT 0x8D

extern "C"
{
DWORD EventDWord; 
DWORD TxBytes; 
DWORD RxBytes; 
DWORD BytesReceived,test; 

DWORD Flags, ID,  Type, LocId;
char SerialNumber[16],Description[64];

BYTE RxBuffer[4096*16],TxBuffer[4096*16], buf_end[512];
static unsigned int dev_open=0,start=0,fr=0xE0,continues=0,cs=0,total_num=0,k_tech=0,oprn_dev=0,corr=0;
static BYTE reg=0,tech_ch=0;
DWORD numDevs;
int ftStatus;
unsigned long  BytesWritten;
 
static FT_HANDLE ftHandle;
static int open_dev=0,sum_frame1=0,sum_frame=0,count_frame=0;
//static  char name[]="AFIFO.RBF";

BYTE buf_test[SIZE*3];
static unsigned int w_test[SIZE];
static unsigned int dark_fon[511*1024],x_no_cond[1024],y_no_cond[1024];
static unsigned int temp[511*1024],rez[511*1024];

void medianfilter(unsigned int* signal, unsigned* result, int N);
void _medianfilter(const unsigned int * signal,unsigned int* result, int N);
int median_filter(unsigned int *dst,unsigned int *src);
int median_filter_Y(unsigned int *dst,unsigned int *src,int y,int delta);
int median_filter_X(unsigned int *dst,unsigned int *src,int x,int delta);
void f_test_cond();

__declspec(dllexport) int f4(int fstart,int fors, int steps, int direction,int tr,int mst);
__declspec(dllexport) int f_make_corr()
{
 /*   corr=1;
	CStdioFile file_corr;
	CFileException e;
	if(!file_corr.Open(_T("corr.dat"),CFile::modeRead|CFile::typeBinary,&e))
	{
   	    return -1;
	}
	else
	{
      file_corr.Read(corr_buf,511*1024*4);
      file_corr.Close();
	  return 0;
	}*/
	return 0;
}
__declspec(dllexport) int   f_save_corr(unsigned int *corr_buf)
{
	CStdioFile file_corr;
	CFileException e;
	if(!file_corr.Open(_T("corr.dat"),CFile::modeCreate|CFile::modeWrite|CFile::typeBinary,&e))
	{
   	    return -1;
	}
	else
	{
      file_corr.Write(corr_buf,511*1024*4);
      file_corr.Close();
	  return 0;
	}
}
__declspec(dllexport) int   f_load_corr(unsigned int *corr_buf)
{
/*	CStdioFile file_corr;
	CFileException e;
	if(!file_corr.Open(_T("corr.dat"),CFile::modeRead|CFile::typeBinary,&e))
	{
   	    return -1;
	}
	else
	{
      file_corr.Read(corr_buf,511*1024*4);
      file_corr.Close();
	  return 0;
	}*/
	return 0;
}
__declspec(dllexport) int   f_write_corr(int base)
{
	int sum,x,n;
	for(x=0,n=1,sum=0; x<1024*511; x++) { if(w_test[x]>base) { sum+=w_test[x]; n++; } } sum/=n;
	if(sum==0) sum=1000;
	for(x=0; x<1024*511; x++) { if(w_test[x]>base) w_test[x]=sum*1000/w_test[x]; else w_test[x]=1000; }
	CStdioFile file_fon;
	CFileException e;
	if(!file_fon.Open(_T("corr.dat"),CFile::modeCreate|CFile::modeWrite|CFile::typeBinary,&e))
	{
   	    return -1;
	}
	else
	{
      file_fon.Write(w_test,511*1024*4);
      file_fon.Close();
	  return 0;
	}
}
__declspec(dllexport) int   f1()
{
	
 /*	CStdioFile file_fon;
	CFileException e;
	if(!file_fon.Open(_T("fon.dat"),CFile::modeRead|CFile::typeBinary,&e))
	   	    return -1;
	else { file_fon.Read(fon_buf,511*1024*4);
	file_fon.Close(); }
*/
  /* 	CStdioFile file_aver,file_unlin;
	CFileException e;
    if(!file_aver.Open(_T("aver.dat"),CFile::modeRead|CFile::typeBinary,&e))
	{
   	    return -1;
	}
   if(!file_unlin.Open(_T("unlin.dat"),CFile::modeRead|CFile::typeBinary,&e))
	{
   	    return -1;
	}
   int buf[1];
   file_aver.Read(buf,4);
   file_unlin.Read(unlin_buf,511*1024*4);
   unlin_aver=buf[0];
   sum_frame1=0; count_frame=0;*/
  /* int x,y,dx,dy;
   double c;
    for(x=0,dx=-512; x<1024; x++,dx++) for(y=0,dy=-256; y<511; y++,dy++)
	{  c=(FOKUS*FOKUS+dx*dx+dy*dy);
	   c/=FOKUS*FOKUS;
	   math_corr[x*511+y]=c*1000;
	}
 
	ftStatus = FT_OpenEx("RMU2_150-300 A",FT_OPEN_BY_DESCRIPTION,&ftHandle);
	if(ftStatus!=FT_OK) return -1;
	ftStatus = FT_SetBitMode(ftHandle, 0xff,0);
	if(ftStatus!=FT_OK) return -1;
	Sleep(100);
	ftStatus = FT_SetUSBParameters(ftHandle,4096*1,1024);
	if(ftStatus!=FT_OK) return -1;
	ftStatus = FT_SetTimeouts(ftHandle,1,1);
	if(ftStatus!=FT_OK) return -1; 
	dev_open=1;
	*/
   return 0;
 }
__declspec(dllexport) int Close_dev()
{
	FT_Close(ftHandle); open_dev=0;
	return 0;
}
__declspec(dllexport) int f_set_ampf(int date)
{
  	ftStatus = FT_OpenEx("RMU2_150-300 A",FT_OPEN_BY_DESCRIPTION,&ftHandle);
	if(ftStatus!=FT_OK) return -1;
	ftStatus = FT_SetBitMode(ftHandle, 0xff,0);
	if(ftStatus!=FT_OK) return -1;
//	Sleep(100);
	ftStatus = FT_SetUSBParameters(ftHandle,4096*16,1024);
	if(ftStatus!=FT_OK) return -1;
	ftStatus = FT_SetTimeouts(ftHandle,1,1);
	if(ftStatus!=FT_OK) return -1; dev_open=1; 
    TxBuffer[0]=KU_NDT;
	TxBuffer[1]=date;
	ftStatus = FT_Write(ftHandle, TxBuffer,2, &BytesWritten); 	  if (ftStatus != FT_OK)  return -1;
	FT_Close(ftHandle); dev_open=0;
}
__declspec(dllexport) int f_clr_step()
{
  	ftStatus = FT_OpenEx("RMU2_150-300 A",FT_OPEN_BY_DESCRIPTION,&ftHandle);
	if(ftStatus!=FT_OK) return -1;
	ftStatus = FT_SetBitMode(ftHandle, 0xff,0);
	if(ftStatus!=FT_OK) return -1;
//	Sleep(100);
	ftStatus = FT_SetUSBParameters(ftHandle,4096*16,1024);
	if(ftStatus!=FT_OK) return -1;
	ftStatus = FT_SetTimeouts(ftHandle,1,1);
	if(ftStatus!=FT_OK) return -1; dev_open=1; 
    TxBuffer[0]=SM_EX;
	TxBuffer[1]=0xf;
	ftStatus = FT_Write(ftHandle, TxBuffer,2, &BytesWritten); 	  
	if (ftStatus != FT_OK)  return -1;
	FT_Close(ftHandle); 
	dev_open=0;
}
__declspec(dllexport) int f2(int mode,int freq,unsigned int *fr_pg)
{
  int flag=0;
  int sum;
 
 
 
 // if(dev_open==0)
  {
   	ftStatus = FT_OpenEx("RMU2_150-300 A",FT_OPEN_BY_DESCRIPTION,&ftHandle);
	if(ftStatus!=FT_OK) return -1;
	ftStatus = FT_SetBitMode(ftHandle, 0xff,0);
	if(ftStatus!=FT_OK) return -1;
//	Sleep(100);
	ftStatus = FT_SetUSBParameters(ftHandle,4096*16,1024);
	if(ftStatus!=FT_OK) return -1;
	ftStatus = FT_SetTimeouts(ftHandle,1,1);
	if(ftStatus!=FT_OK) return -1; dev_open=1; }
/*    TxBuffer[0]=KU_NDT;
	TxBuffer[1]=3;
	ftStatus = FT_Write(ftHandle, TxBuffer,2, &BytesWritten); 	  if (ftStatus != FT_OK)  return -1;*/
/*  if(mode!=CONTINUES) 
  { 
 
	TxBuffer[0]=CONTROL;
    TxBuffer[1]=0;
	ftStatus = FT_Write(ftHandle, TxBuffer,2, &BytesWritten);
	if (ftStatus != FT_OK) { flag=-1; return flag; }

	RxBytes=200;
//	for(; RxBytes>0; )
	for(BytesReceived=1; BytesReceived>0;)
	{ 
	  ftStatus=FT_Read(ftHandle,RxBuffer,RxBytes,&BytesReceived);
	  if (ftStatus != FT_OK) { flag=-1; return flag; }
//	  FT_GetStatus(ftHandle,&RxBytes,&TxBytes,&EventDWord);
//	  if (ftStatus != FT_OK) { flag=-1; return flag; }
	}
//	}
		TxBuffer[0]=CONTROL;
		TxBuffer[1]=3;
	ftStatus = FT_Write(ftHandle, TxBuffer,2, &BytesWritten); 	  if (ftStatus != FT_OK) { flag=-1; return flag; }
  }
  else*/
  {
    TxBuffer[0]=CONTROL;
	TxBuffer[1]=8;
	ftStatus = FT_Write(ftHandle, TxBuffer,2, &BytesWritten); 	  if (ftStatus != FT_OK) { flag=-1; return flag; }
	//ftStatus= FT_GetStatus(ftHandle,&RxBytes,&TxBytes,&EventDWord); if (ftStatus != FT_OK) { flag=-1; return flag; }
	RxBytes=100;
//	for(; RxBytes>0; )
	for(BytesReceived=1; BytesReceived>0;)
	{ 
	  ftStatus=FT_Read(ftHandle,RxBuffer,RxBytes,&BytesReceived);
	  if (ftStatus != FT_OK) { flag=-1; return flag; }
//	  FT_GetStatus(ftHandle,&RxBytes,&TxBytes,&EventDWord);
//	  if (ftStatus != FT_OK) { flag=-1; return flag; }
	}
	TxBuffer[0]=CONTROL;
	TxBuffer[1]=8|4|3;
	ftStatus = FT_Write(ftHandle, TxBuffer,2, &BytesWritten); 	  if (ftStatus != FT_OK) { flag=-1; return flag; }
  }
  long i,j,c;
  int pg;
  for(i=0,c=0; i<SIZE*3; )
  {
    RxBytes=100;
	ftStatus = FT_Read(ftHandle,RxBuffer,RxBytes,&BytesReceived); 	  if (ftStatus != FT_OK) { flag=-1; return flag; }
	for(int j=0; (j<BytesReceived)&&((i+j)<SIZE*3); j++) buf_test[i+j]=RxBuffer[j];
	i+=BytesReceived;
  }
 
//  if(buf_test[FIFO_SIZE+1]&0x80) i=FIFO_SIZE+1; else i=FIFO_SIZE+0;
  for(i=5; ((buf_test[i]&0x80)==0)&&(i<SIZE); i++) ; i++;
 for(; ((buf_test[i]&0x80)==0)&&(i<SIZE); i++) ;
  for(j=0; i<(SIZE*3-FIFO_SIZE-3); i+=3,j++) 
	   { 
 /*   w_test[j]=((buf_test[i]&0xF)<<3)|((buf_test[i]&0x60)<<1);
    w_test[j]=(w_test[j]<<8)|(buf_test[i+1]<<2); if(buf_test[i]&0x10) w_test[j]|=8; else w_test[j]&=0xFFF7;
    w_test[j]|=(buf_test[i+2]&3);*/
	  w_test[j]=(((buf_test[i]&0x60))<<9)|(buf_test[i+1]<<7)|buf_test[i+2];

  }

//  	BYTE b1[100],b2[100],b3[100];
//	for(i=0,j=0; i<300; i+=3,j++) { b1[j]=buf_test[i]; b2[j]= buf_test[i+1]; b3[j]= buf_test[i+2]; }
 //  for(i=5; (i<SIZE)&&(w_test[i]&FRAME==0); i++) ; i++;
 //  for(; (i<SIZE)&&(w_test[i]&FRAME==0); i++) ;   i++;
 //  for(; (i<SIZE)&&(w_test[i]&FRAME==0); i++) ;
	for(i=0; (i<SIZE)&&(w_test[i]&FRAME==0); i++) ;
   for(j=0,pg=0; i<SIZE; i++) 
  {
    if((w_test[i]&PAGE)&&(pg==0)) pg=516;
    if(pg>0) pg--; 
    if((pg<=511)&&(pg>0)) { fr_pg[j]=w_test[i]&0x3FFF;    j++; }
	//if((pg<=511)&&(pg>0)) { fr_pg[j]=(w_test[i]&0x3FFF)|0xFFFFC000; fr_pg[j]=~fr_pg[j]; fr_pg[j]+=1;   j++; }
    pg=pg;
  }
  int y,x,t,g,v,n,m,k;
 
 
  for(x=0,t=0,g=0; x<1024; x++) for(y=0; y<511; y++,g++,t++) 
	  w_test[t]=fr_pg[g];

    for(x=0,g=0; x<1024; x++)
	{
	  for(y=0; y<208; y++,g++  )
	  { 
		rez[g]=temp[g]=fr_pg[x*511+y];
	    //if(fr_pg[x*511+y]>0x2000) rez[g]=temp[g]=fr_pg[x*511+y]-0x2000; else  rez[g]=temp[g]=0; 
      }
	  for(y=210; y<511; y++,g++  )
	  { 
	    rez[g]=temp[g]=fr_pg[x*511+y]; 
	   // if(fr_pg[x*511+y]>0x2000) rez[g]=temp[g]=fr_pg[x*511+y]-0x2000; else  rez[g]=temp[g]=0; 
      }
	  g++;
	  rez[g]=temp[g]=rez[g-1]=0;
	  g++;
	  rez[g]=temp[g]=rez[g-1]=0;
	}
 f_test_cond();

 for(int i=5; i<1024-5; i++) if(x_no_cond[i]) for(int k=0; k<3; k++)median_filter_X(rez,temp,x_no_cond[i],3);

 for(int i=5; i<511-5; i++) if(y_no_cond[i]) for(int k=0; k<3; k++)median_filter_Y(rez,temp,y_no_cond[i],3);


 for(int k=0; k<3; k++) median_filter_Y(rez,temp,256,7);
 //for(int k=0; k<3; k++) median_filter_Y(rez,temp,207,7);

//	  medianfilter(temp,rez,511*1024);
      sum=0;
      for(x=0,t=0,g=0; x<1024; x++) for(y=0; y<511; y++,g++,t++)   { w_test[g]=fr_pg[g]=rez[g];  }///4;
	 

	  if(mode==1)
	  {
		 for(x=0,g=0; x<1024; x++) for(y=0; y<511; y++,g++)  dark_fon[g]=fr_pg[g];
		/* sum=0;
		  for(x=0,g=0; x<1024; x++) for(y=0; y<511; y++,g++) sum+=fr_pg[g]; sum=(sum/511*1024)+1;
		  for(x=0,g=0; x<1024; x++) for(y=0; y<511; y++,g++)  corr_buf[g]=(fr_pg[g]*1000/sum)+1;*/
		  //for(x=0,g=0; x<1024; x++) for(y=0; y<511; y++,g++)  corr_buf[g]=fr_pg[g];
	  }
	  else 
	  {
	//	  for(x=0,g=0; x<1024; x++) for(y=0; y<511; y++,g++) dark_fon[g]=0;
		  for(x=0,g=0; x<1024; x++) for(y=0; y<511; y++,g++)// fr_pg[g]=fr_pg[g]-dark_fon[g];
	      {  if(fr_pg[g]>dark_fon[g])  fr_pg[g]=fr_pg[g]-dark_fon[g]; else fr_pg[g]=0;
	         sum+=fr_pg[g];
		  }
	    sum/=511*1024;
	  }

		  //for(x=0,g=0; x<1024; x++) for(y=0; y<511; y++,g++) fr_pg[g]=fr_pg[g]*1000/corr_buf[g];
		// for(x=0,g=0; x<1024; x++) for(y=0; y<511; y++,g++) if(fr_pg[g]>corr_buf[g]) fr_pg[g]=fr_pg[g]-corr_buf[g]; else fr_pg[g]=0;
	/* for(x=0,t=0,g=0; x<1024; x++) for(y=0; y<511; y++,g++,t++) 
	 { if(mode==1)
		  dark_fon[g]=fr_pg[g];
	  else if(fr_pg[g]>dark_fon[g]) fr_pg[g]-=dark_fon[g]; else  fr_pg[g]=0;
	 }*/
  

  
   //for(x=0; x<1024*511; x++) 
	//{ if(unlin_buf[x]==0) unlin_buf[x]=unlin_aver; fr_pg[x]=fr_pg[x]*unlin_aver/unlin_buf[x]; }

 FT_Close(ftHandle);
  flag=0; return sum;
}
__declspec(dllexport) int f3(int fstart,int fors, int steps, int direction)
{
//  FT_HANDLE ftHandle; 
  int ftStatus;
  DWORD EventDWord;
  unsigned long  BytesWritten;
  DWORD TxBytes; 
  DWORD RxBytes; 
  DWORD BytesReceived,test,sh; 
  BYTE RxBuffer[100],TxBuffer[100];
 // steps-=70;

  if(steps>1)
  {

 
  
   	ftStatus = FT_OpenEx("RMU2_150-300 A",FT_OPEN_BY_DESCRIPTION,&ftHandle);
	if(ftStatus!=FT_OK) return -1;
	ftStatus = FT_SetBitMode(ftHandle, 0xff,0);
	if(ftStatus!=FT_OK) return -1;
	Sleep(100);
	ftStatus = FT_SetUSBParameters(ftHandle,4096*1,1024);
	if(ftStatus!=FT_OK) return -1;
	ftStatus = FT_SetTimeouts(ftHandle,1,1);
	if(ftStatus!=FT_OK) return -1; dev_open=1;
  
 

	ftStatus = FT_SetBitMode(ftHandle, 0xFF,0);
  	Sleep(100);

  	fstart=FB/(128*fstart);
	fors=FB/(128*fors);
    TxBuffer[0]=SM_EX;
	TxBuffer[1]=0x60;//0x61;
	//direction=2;//080417
	ftStatus = FT_Write(ftHandle, TxBuffer,2, &BytesWritten); 	  if (ftStatus != FT_OK)  return -1;

	BYTE T_load[]={0x82,0x06f,0x83,0x7f,0x84,0x1f,0x85,0x7f,0x86,0x7f,0x87,0x7,0x88,0x00,0x89,0x00,0x8A,0x00,0x8B,0x00,0x8C,0x5};//,0x8C,0x1};
    T_load[sizeof(T_load)-1]=5|direction;
	if(steps>16383/2) steps=16383/2;
	steps*=2;
	T_load[STEP_H]=(steps/128)&0x7F;
	T_load[STEP_L]=(steps%128)&0x7F;
	T_load[FSTART_H]=(fstart/128)&0x7F;
	T_load[FSTART_L]=(fstart%128)&0x7F;
	T_load[FORS_H]=(fors/128)&0x7F;
	T_load[FORS_L]=(fors%128)&0x7F;
    T_load[STEP_UPDW_H]=((steps/4)/128)&0x7F;
	T_load[STEP_UPDW_L]=((steps/4)%128)&0x7F;
	T_load[RESET_H]=0x55;
	T_load[RESET_L]=0x55;
	int fstart1,fors1,step,step_updw;
	fstart1=T_load[FSTART_H]*128+T_load[FSTART_L];
	fors1=T_load[FORS_H]*128+T_load[FORS_L];
	step=T_load[STEP_H]*128+T_load[STEP_L];
	step_updw=T_load[STEP_UPDW_H]*128+T_load[STEP_UPDW_L];
//	delta=T_load[DELTA_H]*128+T_load[DELTA_L];
	int size=sizeof(T_load);
	ftStatus = FT_Write(ftHandle, T_load,size, &BytesWritten);		
	if(ftStatus !=FT_OK) return -1;
	Sleep(100);
	BYTE T_load2[]={0x8C,0x1};
    T_load2[1]|=direction;
	ftStatus = FT_Write(ftHandle, T_load2,2, &BytesWritten);		
	if(ftStatus !=FT_OK) return -1;
	//if(direction==2) 


 //   Sleep(1000);
	FT_Close(ftHandle); dev_open=0;
	return 0;
  }
 else {  return 0; }


}
__declspec(dllexport) int f4(int fstart,int fors, int steps, int direction,int tr,int mst)
{
//  FT_HANDLE ftHandle; 
  int ftStatus;
  DWORD EventDWord;
  unsigned long  BytesWritten;
  DWORD TxBytes; 
  DWORD RxBytes; 
  DWORD BytesReceived,test,sh; 
  BYTE RxBuffer[100],TxBuffer[100];


  //fstart=fors=1000;  steps=200; direction=1;
  if(direction==2) {   sum_frame1=0; count_frame=0; }
 // if(dev_open==0)
  { 
   	ftStatus = FT_OpenEx("RMU2_150-300 A",FT_OPEN_BY_DESCRIPTION,&ftHandle);
	if(ftStatus!=FT_OK) return -1;
	ftStatus = FT_SetBitMode(ftHandle, 0xff,0);
	if(ftStatus!=FT_OK) return -1;
	Sleep(100);
	ftStatus = FT_SetUSBParameters(ftHandle,4096*1,1024);
	if(ftStatus!=FT_OK) return -1;
	ftStatus = FT_SetTimeouts(ftHandle,1,1);
	if(ftStatus!=FT_OK) return -1; dev_open=1;
  } 
 
//  ftStatus = FT_OpenEx("RMU2_150-300 A",FT_OPEN_BY_DESCRIPTION,&ftHandle);
//  if(ftStatus ==FT_OK)
  {
	ftStatus = FT_SetBitMode(ftHandle, 0xFF,0);

	Sleep(100);
  	fstart=FB/(128*fstart);
	fors=FB/(128*fors);
    TxBuffer[0]=SM_EX;
	mst=mst<<2;
	TxBuffer[1]=0x60|tr|mst;//0x41;
	ftStatus = FT_Write(ftHandle, TxBuffer,2, &BytesWritten); 	  if (ftStatus != FT_OK)  return -1;
	//fors=GetDlgItemInt(IDC_FORS)-GetDlgItemInt(IDC_FSTART);
	//if(fors>0) {  fors=FB/(16*GetDlgItemInt(IDC_FORS));  fors=fstart-fors; else fors=0; }
	//else fors=0;
	BYTE T_load[]={0x82,0x06f,0x83,0x7f,0x84,0x1f,0x85,0x7f,0x86,0x7f,0x87,0x7,0x88,0x00,0x89,0x00,0x8A,0x00,0x8B,0x00,0x8C,0x5};//,0x8C,0x1};
    T_load[sizeof(T_load)-1]=5|direction;
	if(steps>16383/2) steps=16383/2;
	steps*=2;
	T_load[STEP_H]=(steps/128)&0x7F;
	T_load[STEP_L]=(steps%128)&0x7F;
	T_load[FSTART_H]=(fstart/128)&0x7F;
	T_load[FSTART_L]=(fstart%128)&0x7F;
	T_load[FORS_H]=(fors/128)&0x7F;
	T_load[FORS_L]=(fors%128)&0x7F;
    T_load[STEP_UPDW_H]=((steps/4)/128)&0x7F;
	T_load[STEP_UPDW_L]=((steps/4)%128)&0x7F;
	T_load[RESET_H]=0x55;
	T_load[RESET_L]=0x55;
	int fstart1,fors1,step,step_updw;
	fstart1=T_load[FSTART_H]*128+T_load[FSTART_L];
	fors1=T_load[FORS_H]*128+T_load[FORS_L];
	step=T_load[STEP_H]*128+T_load[STEP_L];
	step_updw=T_load[STEP_UPDW_H]*128+T_load[STEP_UPDW_L];
//	delta=T_load[DELTA_H]*128+T_load[DELTA_L];
	int size=sizeof(T_load);
	ftStatus = FT_Write(ftHandle, T_load,size, &BytesWritten);		
	if(ftStatus !=FT_OK) return -1;
	Sleep(100);
	BYTE T_load2[]={0x8C,0x1};
    T_load2[1]|=direction;
	ftStatus = FT_Write(ftHandle, T_load2,2, &BytesWritten);		
	if(ftStatus !=FT_OK) return -1;
	//if(direction==2) 


	{ Sleep(1000); FT_Close(ftHandle); dev_open=0; }
	return 0;
  }
/*  else
  {
    return -1;
  }	*/

}
//   1D MEDIAN FILTER implementation
//     signal - input signal
//     result - output signal
//     N      - length of the signal
void _medianfilter(const unsigned int* signal, unsigned int* result, int N)
{
   //   Move window through all ints of the signal
   for (int i = 2; i < N - 2; ++i)
   {
      //   Pick up window elements
      unsigned int window[5];
      for (int j = 0; j < 5; ++j)
         window[j] = signal[i - 2 + j];
      //   Order elements (only half of them)
      for (int j = 0; j < 3; ++j)
      {
         //   Find position of minimum element
         int min = j;
         for (int k = j + 1; k < 5; ++k)
            if (window[k] < window[min])
               min = k;
         //   Put found minimum element in its place
         const unsigned int temp = window[j];
         window[j] = window[min];
         window[min] = temp;
      }
      //   Get result - the middle element
      result[i - 2] = window[2];
   }
}

//   1D MEDIAN FILTER wrapper
//     signal - input signal
//     result - output signal
//     N      - length of the signal

void medianfilter(unsigned int* signal, unsigned int* result, int N)
{
   //   Check arguments
   if (!signal || N < 1)
      return;
   //   Treat special case N = 1
   if (N == 1)
   {
      if (result)
         result[0] = signal[0];
      return;
   }
   //   Allocate memory for signal extension
   unsigned int* extension = new unsigned int[N + 4];
   //   Check memory allocation
   if (!extension)
      return;
   //   Create signal extension
   memcpy(extension + 2, signal, N * sizeof(unsigned int ));
   for (int i = 0; i < 2; ++i)
   {
      extension[i] = signal[1 - i];
      extension[N + 2 + i] = signal[N - 1 - i];
   }
   //   Call median filter implementation
   _medianfilter(extension, result ? result : signal, N + 4);
   //   Free memory
   delete[] extension;
}

void insertionSort(unsigned int window[]) 
{ 
    unsigned int temp;
	int  i,j; 
    for(i = 0; i < 9; i++){ 
        temp = window[i]; 
        for(j = i-1; j >= 0 && temp < window[j]; j--){ 
            window[j+1] = window[j]; 
        } 
        window[j+1] = temp; 
    } 
} 

int median_filter(unsigned int *dst,unsigned int *src) 
{ 
  /*    Mat src, dst; 
  
      // Load an image 
      src = imread("book.png", CV_LOAD_IMAGE_GRAYSCALE); 
  
      if( !src.data ) 
      { return -1; } 
  
      //create a sliding window of size 9 */
      unsigned int window[9];//, *psrc, *pdst, *w;

  
     //   psrc=src+512;
	//	pdst=dst+512;
		
        for(int x = 0; x < 1024; x++) 
            for(int y = 0; y < 511; y++) 
                dst[x*511+y] = 0; 
    

        for(int x = 1; x < 1024 - 1; x++){ 
            for(int y = 1; y < 511 - 1; y++) { 
  
                // Pick up window element 
				//w=window;
                window[0] = src[y - 1+(x - 1)*511];// *w = *(src -512);  w++;//
				window[1] = src[y + (x - 1)*511]; //*w=  *(src-511);   w++;//
                window[2] = src[y + 1 + (x - 1)*511]; //*w=  *(src -510);  w++;//
                window[3] = src[y - 1 + x*511]; //*w=  *(src -1);  w++;//
                window[4] = src[y + x*511]; //*w=  *(src);  w++;//
                window[5] = src[y + 1 + x*511]; //*w=  *(src +1);  w++;//
                window[6] = src[y - 1 + (x + 1)*511]; //*w=  *(src +510);  w++;//
                window[7] = src[y + (x + 1)*511]; //*w=  *(src +511);  w++;//
                window[8] = src[y + 1 + (x + 1)*511];// *w=  *(src +512);  w++;//

	 
                // sort the window to find median 
                insertionSort(window); 
  
                // assign the median to centered element of the matrix 
                dst[y+x*511] = window[4];
				
            }// psrc++;
        } 
  
   
  
    return 0; 
}
int median_filter_Y(unsigned int *dst,unsigned int *src,int y_p,int delta) 
{ 
 
      unsigned int window[9];
      for(int x = 0; x < 1024; x++)    for(int y = y_p-delta; y < y_p+delta; y++)  dst[x*511+y] = 0;

      for(int x = 1; x < 1024 - 1; x++)
      for(int y = y_p-delta; y < y_p+delta; y++)
	  { 
                window[0] = src[y - 1+(x - 1)*511];// *w = *(src -512);  w++;//
				window[1] = src[y + (x - 1)*511]; //*w=  *(src-511);   w++;//
                window[2] = src[y + 1 + (x - 1)*511]; //*w=  *(src -510);  w++;//
                window[3] = src[y - 1 + x*511]; //*w=  *(src -1);  w++;//
                window[4] = src[y + x*511]; //*w=  *(src);  w++;//
                window[5] = src[y + 1 + x*511]; //*w=  *(src +1);  w++;//
                window[6] = src[y - 1 + (x + 1)*511]; //*w=  *(src +510);  w++;//
                window[7] = src[y + (x + 1)*511]; //*w=  *(src +511);  w++;//
                window[8] = src[y + 1 + (x + 1)*511];// *w=  *(src +512);  w++;//
                insertionSort(window); 
                dst[y+x*511] = window[4];
        } 
     return 0; 
}
int median_filter_X(unsigned int *dst,unsigned int *src,int x_p,int delta) 
{ 
 
      unsigned int window[9];
	  for(int x = x_p-delta; x < x_p+delta; x++)
      for(int y = 0; y < 511; y++)
        dst[x*511+y] = 0; 
        for(int x = x_p-delta; x < x_p+delta; x++)
        for(int y = 0; y < 511; y++) 
	    { 
                window[0] = src[y - 1+(x - 1)*511];// *w = *(src -512);  w++;//
				window[1] = src[y + (x - 1)*511]; //*w=  *(src-511);   w++;//
                window[2] = src[y + 1 + (x - 1)*511]; //*w=  *(src -510);  w++;//
                window[3] = src[y - 1 + x*511]; //*w=  *(src -1);  w++;//
                window[4] = src[y + x*511]; //*w=  *(src);  w++;//
                window[5] = src[y + 1 + x*511]; //*w=  *(src +1);  w++;//
                window[6] = src[y - 1 + (x + 1)*511]; //*w=  *(src +510);  w++;//
                window[7] = src[y + (x + 1)*511]; //*w=  *(src +511);  w++;//
                window[8] = src[y + 1 + (x + 1)*511];// *w=  *(src +512);  w++;//
                insertionSort(window); 
                dst[y+x*511] = window[4];
        } 
	  for(int x = x_p-delta; x < x_p+delta; x++)
      for(int y = 0; y < 511; y++)
        src[x*511+y]=dst[x*511+y]; 
     return 0; 
}
void f_test_cond()
{
	int test_x[1024],test_y[1000],test=0;
	  for(int i=0; i<1024; i++) x_no_cond[i]=y_no_cond[i]=0;
	  for(int x = 0,i=0; x < 1024 ; x++)  { 
	  if((temp[x*511+100]<ZERO_LEVEL)&&(temp[x*511+200]<ZERO_LEVEL)&&(temp[x*511+300]<ZERO_LEVEL)) { x_no_cond[i]=x; i++; } }
	  for(int y = 0,i=0; y < 511; y++)
	  { if((temp[100*511+y]<ZERO_LEVEL)&& (temp[200*511+y]<ZERO_LEVEL)&&(temp[300*511+y]<ZERO_LEVEL)) 	  { y_no_cond[i]=y; i++; } 
	  }
	
}
}//extern C
