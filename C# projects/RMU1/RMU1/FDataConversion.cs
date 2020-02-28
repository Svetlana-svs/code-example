using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace RMU1
{
    public partial class FData
    {
        // This function is used to transform bite buffer in int by reading data from device
        private HRESULT FDataConvertion_BufferConversion(int indexBegin, ref Byte[] bufP, ref UInt32[] buf_frame_intP)
        {
            try
            {
                buf_frame_intP.Initialize();

                for (int i = 0, j = indexBegin; i < buf_frame_intP.Length; i++, j = ((Mode == MODE.Scan) ? j + 4 : j + 2))
                {
                    buf_frame_intP[i] = (UInt32)(((bufP[j + 1] << 8) | bufP[j]) & 0x3FFF);
                }
           }
            catch(Exception e)
            {
                MessageBox.Show("Error by obtained buffer to integer. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return HRESULT.S_ERROR;
            }
            return HRESULT.S_OK;
        }

        // This function is used to transform bite buffer in int and to bitmap by reading data from file
        private HRESULT FDataConvertion_BitmapConversion(ref Byte[] bufP)
        {
            buf_total_int = new UInt32[height_frame_bmp * width_frame_bmp * framesCount];

            nSumBufTotalInt = 0;
 
            Result = FDataConvertion_BufferConversion(HEADER_SIZE, ref bufP, ref buf_total_int);
            if (Result != HRESULT.S_OK)
            {
                return Result;
            }

            /*    test                     UInt32 temp_data = 0;
                                   temp_data = buf_total_int[i] >> stSerOb.DataShift;
                                   if (temp_data > 0xFF)
                                       buf_total[i] = (byte)(0xFF);
                                   else
                                       buf_total[i] = (byte)temp_data;
                                   if (stSerOb.DataPositive)
                                       buf_total[i] = (byte)(buf_total[i] ^ 0xFF);
                               }
                               buf_total_bitmap = new Byte[buf_total.Length * 3];

                                 if (bit != null) { bit.Dispose(); }
                               bit = new Bitmap(buf_total.Length / num_diods, num_diods, PixelFormat.Format24bppRgb);
                               Cursor.Current = Cursors.WaitCursor;
                             DrawScreen_FromFile_Short(num_diods, ref buf_total, ref buf_total_bitmap, ref bit);
             //                    FWait formWait = new FWait(num_diods, ref buf_total, ref buf_total_bitmap, ref bit);
            //                   formWait.ShowDialog();
            //                    bit.SetPixel(0, bit.Height - 5, Color.Red);
            //                    bit.SetPixel(0, bit.Height - 4, Color.Red);
            //                    bit.SetPixel(0, bit.Height - 3, Color.Red);
            //                    bit.SetPixel(0, bit.Height - 2, Color.Red);
            //                    bit.SetPixel(0, bit.Height - 1, Color.Red);
                               im = bit;
             test
            */
            bmp = null;
            // Number of items in all image
            int rem;
            itemsCount = (Int16)(Math.DivRem(framesCount, coefficient, out rem));
            bmp = new Bitmap[itemsCount + (rem > 0 ? 1 : 0)];

            FDataModification dataModification = new FDataModification();
            dataModification.SetMode(Mode);

            if (bmp != null)
            {
                int itemWidthCurrent = itemWidth;
                itemWidthLast = (Int16)(rem * width_frame_bmp);
                int itemFramesCountCurrent = coefficient;
                try
                {
                    for (int item = 0; item < bmp.Length; item++)
                    {
                        if (item == itemsCount)
                        {
                            itemWidthCurrent = itemWidthLast;
                            itemFramesCountCurrent = rem;
                        }

                        UInt16[] buf_total_item = new UInt16[itemWidthCurrent * height_frame_bmp];
                        int currentX = item * coefficient * width_frame_bmp;

                        Result = dataModification.SetBufTotalToBufForBitmap((int)nShift.Value, height_frame_bmp * currentX, height_frame_bmp * itemWidth, 0, ref buf_total_item);
                        if (Result != HRESULT.S_OK)
                        {
                            MessageBox.Show("Error by set the bitmap buffer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return Result;
                        }

                        bmp[item] = new Bitmap(itemWidthCurrent, height_frame_bmp, PixelFormat.Format48bppRgb);

                        if (stSerOb.UseAdjustment)
                        {
                            dataModification.SetAdjustmentParameters(stSerOb.UseAdjustment, item);
                        }
 
                        // Set frames in bitmap array item
                        Result = dataModification.SetBitmap(itemFramesCountCurrent, ref buf_total_item, ref bmp[item]);
                        if (Result != HRESULT.S_OK)
                        {
                            DisposeBitmapArray(ref bmp);
                            MessageBox.Show("Error by set the bitmap.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return Result;
                        }
                    }
                }
                catch (Exception e)
                {
                    DisposeBitmapArray(ref bmp);
                    MessageBox.Show("Error by set the bitmap.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_ERROR;
                }
            }
            return HRESULT.S_OK;
        }

        private void FDataConvertion_DrawImage(int itemP, int itemWidthP)
        {
            if ((im != null) && (bmp[itemP] != null))
            {
                lock (im)
                {
                    using (Graphics gr = Graphics.FromImage(im))
                    {
                        gr.Clear(Color.White);
                        if ((bmp[itemP].Width - itemWidthP) > im.Width)
                        {
                            // Draw the specified section of the source bitmap to the new one
                            gr.DrawImage(bmp[itemP], 0, 0, new Rectangle(itemWidthP, 0, im.Width, height_frame_bmp), GraphicsUnit.Pixel);

                        }
                        else if ((itemP == (bmp.Length - 1)) && ((bmp[itemP].Width - itemWidthP) < im.Width))
                        {
                            // Draw the specified section of the source bitmap to the new one
                            int width = itemWidthP + ((rectScreen.Width + 10) * delta_default / delta);
                            gr.DrawImage(bmp[itemP], 0, 0, new Rectangle(itemWidthP, 0, width > bmp[itemP].Width ? bmp[itemP].Width : width, height_frame_bmp), GraphicsUnit.Pixel);

                        }
                        else if ((itemP != (bmp.Length - 1)) && ((bmp[itemP].Width - itemWidthP) < im.Width))
                        {
                            // Draw the specified section of the source bitmap to the new one
                            gr.DrawImage(bmp[itemP], 0, 0, new Rectangle(itemWidthP, 0, bmp[itemP].Width - itemWidthP, height_frame_bmp), GraphicsUnit.Pixel);
                            gr.DrawImage(bmp[itemP + 1], bmp[itemP].Width - itemWidthP, 0, new Rectangle(0, 0, im.Width - (bmp[itemP].Width - itemWidthP), height_frame_bmp), GraphicsUnit.Pixel);

                        }
                        gr.Dispose();
                    }
                }
            }
        }
    }
}
