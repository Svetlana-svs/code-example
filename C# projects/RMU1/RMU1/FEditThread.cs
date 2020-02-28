using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace RMU1
{
    partial class FData
    {
        public class FEditThread
        {
            MODE_EDIT ModeEdit;

            private Bitmap bmp = null;

            private Int16 itemId;
            private Int16 itemWidth;
            private Int16 height_frame_bmp;
            private Boolean bUseAdjustment = false;

            public FEditThread(Int16 itemIdP, MODE_EDIT ModeEditP, Int16 itemWidthP, Boolean bUseAdjustmentP, Int16 height_frame_bmpP)
            {
                itemId = itemIdP;
                ModeEdit = ModeEditP;
                itemWidth = itemWidthP;
                bUseAdjustment = bUseAdjustmentP;
                height_frame_bmp = height_frame_bmpP;
            }

            ~FEditThread()
            {
                bmp.Dispose();
            }
            
            public Bitmap FDataEditThread(out HRESULT Res, out Int16 item)
            {
                Res = HRESULT.S_OK;
                item = itemId;
                
                if (ModeEdit.Shift == true)
                {
                    Res = SetShift(itemId);
                    if (Res == HRESULT.S_ERROR)
                    {
                        return bmp;
                    }
                    if ((FData.bmpInverse != null) && (FData.bmpInverse[itemId] != null))
                    {
                        lock (FData.bmpInverse)
                        {
                            FData.bmpInverse[itemId].Dispose();
                            FData.bmpInverse[itemId] = null;
                        }
                    }
                    if ((FData.bmpClone != null) && (bmp != null))
                    {
                        lock (FData.bmpClone)
                        {
                            FData.bmpClone[itemId].Dispose();
                            FData.bmpClone[itemId] = (Bitmap)bmp.Clone();
                        }
                    }
                }

                if (ModeEdit.Inverse == true)
                {
                    Res = SetItemInverse(itemId);
                    if (Res == HRESULT.S_ERROR)
                    {
                        return bmp;
                    }
               }

                if (bmp == null)
                {
                    lock (FData.bmpClone)
                    {
                        bmp = (Bitmap)FData.bmpClone[itemId].Clone();
                    }
                }

                if (ModeEdit.Contrast == true)
                {
                    Res = SetItemContrast(itemId);
                    if (Res == HRESULT.S_ERROR)
                    {
                        return bmp;
                    }
                }

                return bmp;
            }

            private HRESULT SetItemContrast(int item)
            {
                try
                {
                    double contrast = (double)ModeEdit.ContrastValue;
                    int diff = ModeEdit.ShiftValue;
                    byte pixel1 = 0, pixel2 = 0;
                    contrast = (10.0 + contrast) / 10.0;
                    double val_d;
                    contrast *= contrast;
                    int val;

                    BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format48bppRgb);

                    int stride = bmData.Stride;

                    unsafe
                    {
                        byte* p = (byte*)(void*)bmData.Scan0.ToPointer();

                        int nOffset = stride - itemWidth * 6;

                        for (int y = 0; y < bmp.Height; ++y)
                        {
                            for (int x = 0; x < itemWidth; ++x)
                            {
                                val = (p[1] << 8) | p[0];
                                val_d = val;
                                val_d = val_d / 255.0;
                                val_d -= 0.5;
                                val_d *= contrast;
                                val_d += 0.5;
                                val_d *= 255;
                                val = (int)(val_d);

                                if (val < 0) val = 4;
                                if (val > 65535) val = 65535;

                                pixel1 = (byte)(val & 0x00FF);
                                pixel2 = (byte)((val & 0xFF00) >> 8);

                                p[5] = (byte)pixel2;
                                p[4] = (byte)pixel1;
                                p[3] = (byte)pixel2;
                                p[2] = (byte)pixel1;
                                p[1] = (byte)pixel2;
                                p[0] = (byte)pixel1;
                                p += 6;
                            }
                            p += nOffset;
                        }
                    }
                    bmp.UnlockBits(bmData);
                }
                catch (Exception e)
                {
                    if (FData.Result == HRESULT.S_OK)
                    {
                        GC.Collect();
                        MessageBox.Show("Ошибка при изменении контрастности. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return HRESULT.S_ERROR;
                }
                return HRESULT.S_OK;
            }

            private HRESULT SetItemInverse(int item)
            {
                try
                {
                    if (FData.bmpInverse[item] == null)
                    {

                        if (bmp == null)
                        {
                            lock (FData.bmpClone)
                            {
                                bmp = (Bitmap)FData.bmpClone[item].Clone();
                            }
                        }

                        BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                        ImageLockMode.ReadWrite, PixelFormat.Format48bppRgb);
                        int stride = bmData.Stride;
                        System.IntPtr Scan0 = bmData.Scan0;
                        unsafe
                        {
                            Byte* p = (Byte*)(void*)Scan0;
                            int nOffset = stride - bmp.Width * 6;
                            int nWidth = bmp.Width * 3;
                            for (int y = 0; y < bmp.Height; ++y)
                            {
                                for (int x = 0; x < nWidth; ++x)
                                {
                                    p[0] = (Byte)(~p[0]);
                                    p[1] = (Byte)((~p[1]) & 0x1F);
                                    p += 2;
                                }
                                p += nOffset;
                            }
                        }

                        bmp.UnlockBits(bmData);

                        lock (FData.bmpInverse)
                        {
                            FData.bmpInverse[item] = (Bitmap)bmp.Clone(); ;
                        }
                    }
                    else
                    {
                        lock (FData.bmpInverse)
                        {
                            bmp = (Bitmap)FData.bmpInverse[item].Clone();
                        }
                    }
                }
                catch (Exception e)
                {
                    if (FData.Result == HRESULT.S_OK)
                    {
                        MessageBox.Show("Ошибка при изменении инверсии. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return HRESULT.S_ERROR;
                }
                return HRESULT.S_OK;
            }

            private HRESULT SetShift(Int16 item)
            {
                try
                {
                    UInt16[] buf_total = new UInt16[itemWidth * height_frame_bmp];
                    int currentX = item * coefficient * width_frame_bmp;

                    FDataModification dataTransform = new FDataModification();
                    dataTransform.SetMode(MODE.Shift);
                    dataTransform.SetBufTotalToBufForBitmap(ModeEdit.ShiftValue, height_frame_bmp * currentX, height_frame_bmp * itemWidth, 0, ref buf_total);

                    if (bUseAdjustment)
                    {
                        dataTransform.SetAdjustmentParameters(bUseAdjustment, item);
                    }
                    int itemFramesCount = itemWidth / FData.width_frame_bmp;

                    bmp = new Bitmap(itemWidth, height_frame_bmp, PixelFormat.Format48bppRgb); ;
                    dataTransform.SetBitmap(itemFramesCount, ref buf_total, ref bmp);
                }
                catch (Exception e)
                {
                    if (FData.Result == HRESULT.S_OK)
                    {
                        GC.Collect();
                        MessageBox.Show("Ошибка при изменении разрядности. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    return HRESULT.S_ERROR;
                }

                return HRESULT.S_OK;
            }
        }
    }
}