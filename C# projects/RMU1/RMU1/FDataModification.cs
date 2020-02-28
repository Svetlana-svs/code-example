using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RMU1 {
    
    public class FDataModification
    {
        
        private HRESULT Result;

        private int itemId;
        private MODE Mode = MODE.NotActive;

        private Boolean bUseAdjustment = false;
        public FDataModification()
        {
//           Debug.WriteLine("FDataTransform construct");
        }
         ~FDataModification()
        {
//           Debug.WriteLine("FDataTransform destruct");
        }
        // Выделение младших 12 битов из 32-разрядного числа и занесение этого значения в 16-разрядный массив для создания bitmap
        public HRESULT SetBufTotalToBufForBitmap(int shift, int index_begin, int count, int current_index, ref UInt16[] buf_frameFunPar)
        {
            // Alexander changes calibration pixels
            int k=10,s=0;
            if (shift < 0) s = shift * (-1); else s = shift;
            switch (s)
            {
                case 1: k = 100; break;
                case 2: k = 105; break;
                case 3: k = 110; break;
                case 4: k = 115; break;
                case 5: k = 120; break;
                case 6: k = 130; break;
                case 7: k = 140; break;
                case 8: k = 150; break;
                case 9: k = 160; break;
                case 10: k = 170; break;
                case 11: k = 200; break;
                case 12: k = 250; break;
                case 13: k =350; break;
                case 14: k = 400; break;
                case 15: k = 500; break;
            }
            //k = 150 + s * 5;
            if (shift < 0) k *= -1;
            try
            {
                uint[]  stat_date = new uint[16384];
                uint[] stat = new uint[511*1024];
                uint t = 0,m,min=0,max=0,sum=0;
                for (int i = 0; i < 16384; i++) stat_date[i] = 0;
                for (int j = 0; j < 511 * 1024; j++) stat[j] = FData.buf_total_int[j];

                for (int i = 511 * 1024 * 0, j = 0; j < 511 * 1024; j++) { stat_date[FData.buf_total_int[i + j]] += 1; sum += FData.buf_total_int[i + j]; }//511 * 1024*2
                sum /= 511 * 1024;
                int test;
                for (m=0, min = 30;  (min < 4000)&&(m <24000) ; min++) m+=stat_date[min];
                for (m=0,max = 8000; (max >100) && (m <4000); max--) m += stat_date[max];
                for (int i = index_begin, j = 0; j < count; j++, i++)//count
                {
                    if (i < FData.buf_total_int.Length)
                    {
                        if (k < 0)
                        {
                              FData.buf_total_int[i] -= 200;
                            buf_frameFunPar[j] = (UInt16)((FData.buf_total_int[i] == 0 ? 4 : FData.buf_total_int[i]) *100/ (-k));
                        }
                        else
                        {
                            test = (int)FData.buf_total_int[i];

                            min = 0; max = 16000;
                        //  min = 1000; max = 8000;

                            if (test >= max) buf_frameFunPar[j] = (UInt16)(max - min);//(UInt16)(1000 * k / 10);
                            if (test > min) test = (UInt16)(test - min + 4) * k / 100; else test = 4;
                            if (test > 8192) buf_frameFunPar[j] = (UInt16)8192;
                            else
                                if (test < min) buf_frameFunPar[j] = 4; else buf_frameFunPar[j] = (UInt16)((test - min + 4) * k / 100);
                            }
                            if (buf_frameFunPar[j] >= 8192) buf_frameFunPar[j] = 8191; 
                        }

                        if (bUseAdjustment && (Mode == MODE.Scan))
                        {
                            FData.arAvgFrame[current_index][0] += (UInt32)buf_frameFunPar[j];
                        }
                    }
               }
            // Alexander changes
                // Накопление общей суммы
                if (bUseAdjustment && (Mode == MODE.Scan))
                {
                    FData.nSumBufTotalInt += FData.arAvgFrame[current_index][0];
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error by transformation array. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }
            return HRESULT.S_OK;
        }
 
        public HRESULT SetBitmap(int current_index, ref UInt16[] buf_frameFunPar, ref Bitmap bit)
        {
            Result = HRESULT.S_OK;
            Result = SetFrameToBitmap(current_index, ref buf_frameFunPar, ref bit);

            return Result;
        }
        
        private HRESULT SetFrameToBitmap(int current_index, ref UInt16[] buf_frameFunPar, ref Bitmap bit)
        {
            BitmapData bmpData = null;
            Bitmap bitTemp = null;
            Result = HRESULT.S_OK;

            // Заполнение кадра размером (num_diods x total_read), начиная с current_index
            // Данные в массиве: столбец размером num_diods за столбцом. Кол-во столбцов total_read
            // Данные в массиве для создания bitmap в обратном порядке: столбец(высота bitmap) total_read, кол-во столбцов(ширина bitmap) num_diods
            long index_source_current = 0;
            int j = 0;
            int streid_bmp_current = 0;
            int width = 0;
            Byte[] buf_bitmap_new;
            Rectangle rectBitmap;

            // Set index range in accordance with Mode type
            int k_start = 0;
            int k_end = 0;

            try
            {
                switch (Mode)
                {
                    case MODE.Open:
                    case MODE.Shift:
                        rectBitmap = new Rectangle(0, 0, bit.Width, bit.Height);
                        bmpData = bit.LockBits(rectBitmap, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                                PixelFormat.Format48bppRgb);
                        width = bit.Width;

                        // Read from total array. Write to item array from item bitmap.
                        k_start = 0;
                        k_end = current_index;
                    break;
                    case MODE.Scan:
                        // Create new temp Bitmap with size of the one frame
                        rectBitmap = new Rectangle(0, 0, FData.width_frame_bmp, FData.height_frame_bmp);
                        bitTemp = new Bitmap(FData.width_frame_bmp, FData.height_frame_bmp, PixelFormat.Format48bppRgb);
                        bmpData = bitTemp.LockBits(rectBitmap, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                                PixelFormat.Format48bppRgb);
                        streid_bmp_current = bitTemp.Width * 6;
                        width = bitTemp.Width;

                        // Read from buffer array. Size of buffer array is frame size.
                        k_start = current_index;
                        k_end = current_index + 1;
                    break;
                }

                int streid = bmpData.Stride;
                int offset_streid = streid - width * 6;
                GC.Collect();
                //6 inside 3 because Format48bppRgb
                buf_bitmap_new = new Byte[streid * FData.height_frame_bmp];
                Byte byte_1 = 0, byte_2 = 0;

                // Variables to exclude multiply execution of operations
                int index_dest_const = FData.height_frame_bmp * FData.width_frame_bmp;
                long index_source_const = width * 6;
                long index_dest_streid_current = 0;
                long index_dest_current = 0;
                long index_source_streid_current;

                for (int k = k_start; k < k_end; k++)
                {
                    streid_bmp_current = FData.width_frame_bmp * 6 * ((Mode == MODE.Scan) ? current_index : k);
                    // Занесение в массив начинается с индекса последнего кадра
                    index_source_current = (Mode == MODE.Scan) ? 0 : streid_bmp_current;
                    index_source_streid_current = (Mode == MODE.Scan) ? 0 : streid_bmp_current;
                    index_dest_current = (Mode == MODE.Scan) ? index_dest_streid_current : index_dest_const * k;

                    for (j = 0; j < FData.height_frame_bmp; j++)
                    {
                        for (int i = 0; i < FData.width_frame_bmp; i++)
                        {
                            if ((index_source_current < buf_bitmap_new.Length) && (index_dest_current < buf_frameFunPar.Length))
                            {
                                if (bUseAdjustment && (Mode != MODE.Scan))
                                {
                                    buf_frameFunPar[index_dest_current + i] = (ushort)((buf_frameFunPar[index_dest_current + i] * FData.nAvgBufTotalInt) / FData.arAvgFrame[(itemId * FData.coefficient) + k][1]);
                                }
                                // Old byte
                                byte_2 = (Byte)(buf_frameFunPar[index_dest_current + i] >> 8);
                                // Younger byte
                                byte_1 = (Byte)(buf_frameFunPar[index_dest_current + i] & 0xFF);

                                buf_bitmap_new[index_source_current] = byte_1;
                                buf_bitmap_new[index_source_current + 1] = byte_2;
                                buf_bitmap_new[index_source_current + 2] = byte_1;
                                buf_bitmap_new[index_source_current + 3] = byte_2;
                                buf_bitmap_new[index_source_current + 4] = byte_1;
                                buf_bitmap_new[index_source_current + 5] = byte_2;
                            }
                            index_source_current = index_source_current + 6;
                        }
                        index_dest_current = index_dest_current + FData.width_frame_bmp;
                        index_source_streid_current = index_source_streid_current + index_source_const + ((Mode == MODE.Edit) ? 0 : offset_streid);
                        index_source_current = index_source_streid_current;
                    }
                    index_dest_streid_current = index_dest_streid_current + index_dest_const;
                }

                if (((Mode == MODE.Shift) || (Mode == MODE.Open)) && (buf_bitmap_new != null))
                {
                    Marshal.Copy(buf_bitmap_new, 0, bmpData.Scan0, streid * FData.height_frame_bmp);
                }
                else
                {
                    Marshal.Copy(buf_bitmap_new, 0, bmpData.Scan0, streid * FData.height_frame_bmp); //buf_bitmap_new.Length);
                }
            }
            catch (ArgumentNullException e)
            {
                MessageBox.Show("Error by transformation data array to bitmap array. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Result = HRESULT.S_ERROR;
            }
            catch (ArgumentOutOfRangeException e)
            {
                MessageBox.Show("Error by transformation data array to bitmap array. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Result = HRESULT.S_ERROR;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error by transformation data array to bitmap array. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Result = HRESULT.S_ERROR;
            }
            finally
            {
                if ((Mode == MODE.Shift) || (Mode == MODE.Open))
                {
                    bit.UnlockBits(bmpData);
                }
                else
                {
                    bitTemp.UnlockBits(bmpData);
                    if (Result != HRESULT.S_ERROR) {
                        int currentBmpIndex;
                        Math.DivRem(current_index, FData.coefficient, out currentBmpIndex);
                        using (Graphics g = Graphics.FromImage(bit))
                        {
                            // Draw the specified section of the source bitmap to the new one
                            g.DrawImage(bitTemp, new Rectangle(currentBmpIndex * bitTemp.Width, 0, bitTemp.Width, bitTemp.Height), new Rectangle(0, 0, bitTemp.Width, bitTemp.Height), GraphicsUnit.Pixel);

                            // Clean up
                            g.Dispose();
                        }
                    }
                    bitTemp.Dispose();
                }

                buf_bitmap_new = null;
                bmpData = null;
            }

            return Result;
        }
 
        public void SetMode(MODE ModeP)
        {
            Mode = ModeP;
        }

        public void SetAdjustmentParameters(Boolean bUseAdjustmentP, int itemP)
        {
            bUseAdjustment = bUseAdjustmentP;
            itemId = itemP;
        }
    }
}
