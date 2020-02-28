using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ipp;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace .Wlds.DataProcessing
{
    class Statistic
    {
        public static unsafe HRESULT SetAvgData(Detector detector)
        {
            if (detector.dataMeasurement.Count == 0)
            {
                return HRESULT.S_CANCEL;
            }
            float itemCount = ((float)detector.dataMeasurement.Count - 1);
            IppStatus status;
            int dataLength = detector.dataMeasurement[0][DATA_TYPE.BASE].Length;
            detector.data[DATA_TYPE.BASE] = new float[dataLength];

            Array.Copy(detector.dataMeasurement[0][DATA_TYPE.BASE], 0, detector.data[DATA_TYPE.BASE], 0, dataLength);
            if (detector.dataMeasurement.Count() == 1)
            {
                return HRESULT.S_OK;
            }

            for (Int16 i = 0; i < detector.dataMeasurement.Count; i++)
            {
                // Average value of the data array elements 
                fixed (float* pSrc = detector.dataMeasurement[i][DATA_TYPE.BASE], pSrcDst = detector.data[DATA_TYPE.BASE])
                {
                    status = ipp.sp.ippsAdd_32f_I(pSrc, pSrcDst, dataLength);
                }
                if (status != IppStatus.ippStsNoErr)
                {
                    string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                    MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_ERROR;
                }
            }

            for (int i = 0; i < dataLength; i++)
            {
                detector.data[DATA_TYPE.BASE][i] = detector.data[DATA_TYPE.BASE][i] / itemCount;
            }

            return HRESULT.S_OK;
        }

        public static unsafe HRESULT MeanData(ref float[] dataBuffer)
        {
            IppStatus status;
            float avg = 0;
            int dataLength = dataBuffer.Length;

            for (Int16 i = 0; i < 2; i++)
            {
                // Average value of the data array elements 
                fixed (float* pSrc = dataBuffer)
                {
                    status = ipp.sp.ippsMean_32f(pSrc, dataLength, &avg, 1);
                }
                if (status != IppStatus.ippStsNoErr)
                {
                    string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                    MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_ERROR;
                }

                // Sum sqr data array elements 
                fixed (float* pSrc = dataBuffer)
                {
                    status = ipp.sp.ippsSubC_32f_I(avg, pSrc, dataLength);
                }
                if (status != IppStatus.ippStsNoErr)
                {
                    string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                    MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_ERROR;
                }
            }

            return HRESULT.S_OK;
        }

        public static unsafe HRESULT geFFTDataOld(Detector detector, DATA_TYPE type)
        {
            if ((detector.data == null) || (detector.data.Count == 0) ||
                (detector.data[DATA_TYPE.BASE] == null) || (detector.data[DATA_TYPE.BASE].Length == 0))
            {
                return HRESULT.S_ERROR;
            }

            IppStatus status;
            Ipp32fc[] dataComplex;
            IppsFFTSpec_R_32f* pFFTSpec; 

            byte sizeSpec = 0, sizeInit = 0, sizeBuf = 0;
            int flag = 1; // IPP_FFT_NODIV_BY_ANY
            int dataLength = detector.data[DATA_TYPE.BASE].Length;
            detector.Order = (Int16)(Math.Log((double)dataLength) / Math.Log(2.0));

            // Initialize FFT

            dataLength = (int)Math.Pow(2.0, detector.Order);
            int dataLengthResult = dataLength - 2;
            float[] dataBuf = new float[dataLength];
            Array.Copy(detector.data[DATA_TYPE.BASE], 0, dataBuf, 0, dataLength);

            status = ipp.sp.ippsFFTGetSize_R_32f(detector.Order, flag, 1, &sizeSpec, &sizeInit, &sizeBuf);
            if (status != IppStatus.ippStsNoErr)
            {
                string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }

            status = ipp.sp.ippsFFTInitAlloc_R_32f(&pFFTSpec, detector.Order, flag, 1);
            if (status != IppStatus.ippStsNoErr)

            {
                string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }

            // Transform FFT of a real signal to packed complex values
            fixed (float* pSrcDst = dataBuf)
            {
                status = ipp.sp.ippsFFTFwd_RToCCS_32f_I(pSrcDst, pFFTSpec, null);
            }
            if (status != IppStatus.ippStsNoErr)
            {
                string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }

            dataComplex = new Ipp32fc[dataLengthResult];
            fixed (float* pSrc = dataBuf)
            {
                fixed (Ipp32fc* pDst = dataComplex)
                {
                    status = ipp.sp.ippsConjCcs_32fc(pSrc, pDst, dataLengthResult);
                }
            }
            if (status != IppStatus.ippStsNoErr)
            {
                string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }

            ipp.sp.ippsFFTFree_R_32f(pFFTSpec);
            detector.data[DATA_TYPE.FFT] = new float[dataLengthResult / 2];
            detector.data[DATA_TYPE.FFT_S] = new float[dataLengthResult / 2];
            for (int i = 0; i < (dataLengthResult / 2); i++)
            {
                if (dataComplex[i].im == 0 || dataComplex[i].re == 0)
                {
                    detector.data[DATA_TYPE.FFT][i] = 0;
                }
                else
                {
                    detector.data[DATA_TYPE.FFT][i] = (float)Math.Sqrt((dataComplex[i].im * dataComplex[i].im + dataComplex[i].re * dataComplex[i].re));
                    detector.data[DATA_TYPE.FFT_S][i] = (float)Math.Log10(detector.data[DATA_TYPE.FFT][i]);
                }
            }
            float val = 20.0F;
            fixed (float* pSrcDst = detector.data[DATA_TYPE.FFT_S])
            {
                status = ipp.sp.ippsMulC_32f_I(val, pSrcDst, detector.data[DATA_TYPE.FFT_S].Length);
            }
            if (status != IppStatus.ippStsNoErr)
            {
                string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }

            return HRESULT.S_OK;
        }


        public static unsafe HRESULT geFFTData(Detector detector, Int16 indexMeasurement)
        {
            if (((indexMeasurement == -1) && ((detector.data == null) || (detector.data.Count == 0) ||
                (detector.data[DATA_TYPE.BASE] == null) || (detector.data[DATA_TYPE.BASE].Length == 0))) ||
                ((indexMeasurement > 0) && ((detector.dataMeasurement == null) || (detector.dataMeasurement.Count == 0) ||
                (detector.dataMeasurement[indexMeasurement][DATA_TYPE.BASE] == null) || (detector.dataMeasurement[indexMeasurement][DATA_TYPE.BASE].Length == 0))))
            {
                return HRESULT.S_ERROR;
            }

            float[] data;

            if (indexMeasurement == -1)
            {
                data = new float[detector.data[DATA_TYPE.BASE].Length];
                Array.Copy(detector.data[DATA_TYPE.BASE], 0, data, 0, detector.data[DATA_TYPE.BASE].Length);
            } 
            else
            {
                data = new float[detector.dataMeasurement[indexMeasurement][DATA_TYPE.BASE].Length];
                Array.Copy(detector.dataMeasurement[indexMeasurement][DATA_TYPE.BASE], 0,
                    data, 0, detector.dataMeasurement[indexMeasurement][DATA_TYPE.BASE].Length);
            }

            float[] dataFFT;
            float[] dataFFT_S;


            IppStatus status;
            Ipp32fc[] dataComplex;
            IppsFFTSpec_R_32f* pFFTSpec;

            byte sizeSpec = 0, sizeInit = 0, sizeBuf = 0;
            int flag = 1; // IPP_FFT_NODIV_BY_ANY
            int dataLength = data.Length;
            detector.Order = (Int16)(Math.Log((double)dataLength) / Math.Log(2.0));

            // Initialize FFT

            dataLength = (int)Math.Pow(2.0, detector.Order);
            int dataLengthResult = dataLength - 2;

            status = ipp.sp.ippsFFTGetSize_R_32f(detector.Order, flag, 1, &sizeSpec, &sizeInit, &sizeBuf);
            if (status != IppStatus.ippStsNoErr)
            {
                string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }

            status = ipp.sp.ippsFFTInitAlloc_R_32f(&pFFTSpec, detector.Order, flag, 1);
            if (status != IppStatus.ippStsNoErr)
            {
                string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }

            // Transform FFT of a real signal to packed complex values
            fixed (float* pSrcDst = data)
            {
                status = ipp.sp.ippsFFTFwd_RToCCS_32f_I(pSrcDst, pFFTSpec, null);
            }
            if (status != IppStatus.ippStsNoErr)
            {
                string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }

            dataComplex = new Ipp32fc[dataLengthResult];
            fixed (float* pSrc = data)
            {
                fixed (Ipp32fc* pDst = dataComplex)
                {
                    status = ipp.sp.ippsConjCcs_32fc(pSrc, pDst, dataLengthResult);
                }
            }
            if (status != IppStatus.ippStsNoErr)
            {
                string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }

            ipp.sp.ippsFFTFree_R_32f(pFFTSpec);

            dataFFT = new float[dataLengthResult / 2];
            dataFFT_S = new float[dataLengthResult / 2];
            for (int i = 0; i < (dataLengthResult / 2); i++)
            {
                if (dataComplex[i].im == 0 || dataComplex[i].re == 0)
                {
                    dataFFT[i] = 0;
                }
                else
                {
                    dataFFT[i] = (float)Math.Sqrt((dataComplex[i].im * dataComplex[i].im + dataComplex[i].re * dataComplex[i].re));
                    dataFFT_S[i] = (float)Math.Log10(dataFFT[i]);
                }
            }
            float val = 20.0F;
            fixed (float* pSrcDst = dataFFT_S)
            {
                status = ipp.sp.ippsMulC_32f_I(val, pSrcDst, dataFFT_S.Length);
            }
            if (status != IppStatus.ippStsNoErr)
            {
                string strStatus = Marshal.PtrToStringAnsi(ipp.sp.ippGetStatusString(status));
                MessageBox.Show("Error: " + strStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }

            if (indexMeasurement == -1)
            {
                detector.data[DATA_TYPE.FFT] = new float[dataFFT.Length];
                Array.Copy(dataFFT, 0, detector.data[DATA_TYPE.FFT], 0, dataFFT.Length);
                detector.data[DATA_TYPE.FFT_S] = new float[dataFFT_S.Length];
                Array.Copy(dataFFT_S, 0, detector.data[DATA_TYPE.FFT_S], 0, dataFFT_S.Length);
            }
            else
            {
                detector.dataMeasurement[indexMeasurement][DATA_TYPE.FFT] = new float[dataFFT.Length];
                Array.Copy(dataFFT, 0, detector.dataMeasurement[indexMeasurement][DATA_TYPE.FFT], 0, dataFFT.Length);
                detector.dataMeasurement[indexMeasurement][DATA_TYPE.FFT_S] = new float[dataFFT_S.Length];
                Array.Copy(dataFFT_S, 0, detector.dataMeasurement[indexMeasurement][DATA_TYPE.FFT_S], 0, dataFFT_S.Length);
            }

            return HRESULT.S_OK;
        }
    }
}
