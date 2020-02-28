using System;
using System.Runtime.InteropServices;
using System.Text;


namespace ipp 
{
    public enum IppStatus {
        ippStsNoErr = 0,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Ipp32fc
    {
        public float re;
        public float im;

        public Ipp32fc(float re, float im)
        {
            this.re = re;
            this.im = im;
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct IppsFFTSpec_R_32f { }; 
    
    unsafe public class sp
    {
        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsFIRMRInitAlloc_32f(ref IntPtr pState, float* pTaps, int tapsLen, int upFactor, int upPhase,
                int downFactor, int downPhase, float* pDlyLine);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsFIR_32f(float* pSrc, float* pDst, int numIters, IntPtr pState);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsSqr_32f_I(float* pSrcDst, int len);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsSqrt_32f_I(float* pSrcDst, int len);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsAbs_32f(float* pSrc, float* pDst, int len);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsAbs_32f_I(float* pSrcDst, int len);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsSum_32f(float* pSrc, int len, float* pSum, int hint);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsMean_32f(float* pSrc, int len, float* pMean, int hint);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsStdDev_32f(float* pSrc, int len, float* pMean, int hint);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsMax_32f(float* pSrc, int len, float* pMax);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsFIRFree_32f(IntPtr pState);

        [DllImport("ippcore-7.1.dll")]
        public static extern IntPtr ippGetStatusString(IppStatus StsCode);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsFFTGetSize_R_32f(int order, int flag, int hint, byte*
        pSpecSize, byte* pSpecBufferSize, byte* pBufferSize);

 //       [DllImport("ippsp8-7.1.dll")]
//        public static extern IppStatus ippsFFTInit_R_32f(IppsFFTSpec_R_32f** pFFTSpec, int order, int flag, int hint, byte* pSpec, byte* pSpecBuffer);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsFFTInitAlloc_R_32f(IppsFFTSpec_R_32f** pFFTSpec, int order, int flag, int hint);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsFFTInitAlloc_C_32fc(ref IntPtr pFFTSpec, int order, int flag, int hint);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsFFTFree_R_32f(IppsFFTSpec_R_32f* pFFTSpec);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsFFTFree_C_32fc(IntPtr pFFTSpec);
        
        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsFFTGetBufSize_R_32f(IntPtr pFFTSpec, int* pSize);
 
        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsFFTFwd_RToCCS_32f_I(float* pSrcDst, IppsFFTSpec_R_32f* pFFTSpec, byte* pBuffer);
        
        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsFFTInv_CToC_32fc_I(Ipp32fc* pSrcDst, IntPtr pFFTSpec, byte* pBuffer);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsMul_32fc_I(Ipp32fc* pSrc1, Ipp32fc* pSrcDst, int len);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsMulC_32f_I(float val, float* pSrcDst, int len);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsSubC_32f_I(float val, float* pSrcDst, int len);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsConjCcs_32fc(float* pSrc, Ipp32fc* pDst, int len);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsFFTFwd_RToPerm_32f(float* pSrc, float* pDst, IntPtr pFFTSpec, byte* pBuffer);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsConjPerm_32fc(float* pSrc, float* pDst, int len);

        [DllImport("ippcore-7.1.dll")]
        public static extern IppStatus ippsMulPackConj_32f_I(float* pSrc, float* pSrcDst, int len);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsAdd_32f_I(float* pSrc, float* pSrcDst, int len);

        [DllImport("ippsp8-7.1.dll")]
        public static extern IppStatus ippsDivC_64f_I(float val, float* pDst, int len);
   }
} 
