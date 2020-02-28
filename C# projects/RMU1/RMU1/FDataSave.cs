using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace RMU1
{
    public partial class FData
    {
        #region OpenFile
            /*****************************   Read From File   *****************************/

            private HRESULT ReadFileHeader(ref byte[] bufPar)
            {
                for (int i = 0; i < HEADER_SIZE; i = i + 2)
                {
                    switch (i)
                    {
                        case 0:
                            width_frame_bmp = (Int16)(((bufPar[1] << 8) | bufPar[0]) & 0xFFF);
                            break;
                        case 1:
                            height_frame_bmp = (Int16)(((bufPar[3] << 8) | bufPar[2]) & 0xFFF);
                            break;
                    }
                }
                if ((width_frame_bmp == 0) || (height_frame_bmp == 0))
                {
                    width_frame_bmp = (stSerOb.StreidWidth == 0) ? num_diods : stSerOb.StreidWidth;
                    height_frame_bmp = (stSerOb.StreidHeight == 0) ? total_read : stSerOb.StreidHeight;
                }
                itemWidth = (Int16)(coefficient * width_frame_bmp);
                framesCount = (bufPar.Length - HEADER_SIZE) / (width_frame_bmp * height_frame_bmp * 2);

                return HRESULT.S_OK;
            }

            private HRESULT FDataSave_ReadFile(string fileNameP, ref Byte[] bufP)
            {
                try
                {
                    FileStream fStr = new FileStream(fileNameP, FileMode.Open, System.IO.FileAccess.Read);
                    bufP = new Byte[fStr.Length];
                    fStr.Read(bufP, 0, (int)fStr.Length);
                    fStr.Close();
                    fStr.Dispose();
                }
                catch
                {
                    MessageBox.Show("Error by reading from file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_ERROR;
                }
                return HRESULT.S_OK;
            }

            /*****************************   Open File   *****************************/

            private void ButtonOpen_Click(object sender, EventArgs e)
            {
                Result = OpenFile();
                if (Result == HRESULT.S_ERROR)
                {
                    MessageBox.Show("Error by read the file.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }

            private HRESULT OpenFile()
            {
                try
                {
                    MODE currentMode = Mode;
                    Mode = MODE.Open;
                    String fileName;
                    if (Directory.Exists(stSerOb.CurrentFolderName))
                    {
                        openFile.InitialDirectory = stSerOb.CurrentFolderName;
                    }
                    openFile.Filter = "(*.rmu1)|*.rmu1";
                    openFile.FileName = null;
                    DialogResult dRes = openFile.ShowDialog();
                    if (dRes != DialogResult.OK)
                    {
                        Mode = currentMode;
                        if (dRes == DialogResult.Cancel)
                        {
                            return HRESULT.S_CANCEL;
                        }
                        else
                        {
                            return HRESULT.S_ERROR;
                        }
                    }

                    Cursor.Current = Cursors.WaitCursor;
                    stSerOb.CurrentFolderName = openFile.FileName.Substring(0, openFile.FileName.LastIndexOf("\\") - 1);
                    fileName = openFile.FileName.ToString();
                    Byte[] buf_total_big = null;

                    Result = FDataSave_ReadFile(fileName, ref buf_total_big);
                    if (Result == HRESULT.S_OK)
                    {
                        buf_total_int = null;
                        Mode = MODE.Open;
                        tbShowShift.Value = 1;


                        //test!!!!!                   buf_total = new UInt16[buf_total_big.Length];
                        //                     buf_total_int = new UInt32[buf_total_big.Length];
                        //                    total_read = buf_total_big.Length / num_diods;

                        //                    buf_total_int = new UInt32[buf_total_big.Length];

                        /*test    
                         for (int i = 0, j = 0; i < buf_total_big.Length; i = i + 2, j++)
                         {
                             buf_total_big[i + 1] = 0;
                             if (j == 256) j = 0;
                             buf_total_big[i] = (byte)j;
                         }

                         test */
                        ReadFileHeader(ref buf_total_big);

                        if (arAvgFrame.Count != 0)
                        {
                            arAvgFrame.Clear();
                            nSumBufTotalInt = 0;
                        }
                        // Transformation buffer obtained by reading file and addition to bitmap
                        Result = FDataConvertion_BitmapConversion(ref buf_total_big);
                        if (Result != HRESULT.S_OK)
                        {
                            Mode = currentMode;
                            return Result;
                        }
                    }

                    DrawImage();
                    ClearScreen(2);
                    InizializeEditBitmap();
                    FDataEdit();

                    Mode = currentMode;
                    Invalidate();
                    Update();

                    Cursor.Current = Cursors.Cross;
                }
                catch
                {
                    Mode = MODE.NotActive;
                    return HRESULT.S_ERROR;
                }
                return HRESULT.S_OK;
            }

        #endregion OpenFile

        #region SaveFile

            /*****************************   Save File   *****************************/

            private String strFileExtension = "rmu1";

            private void ButtonSave_Click(object sender, EventArgs e)
            {
                Mode = MODE.Save;
                Result = SaveFile();
                if (Result != HRESULT.S_OK)
                {
                    MessageBox.Show("No image save.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                Mode = MODE.NotActive;
            }

            private HRESULT SaveFile()
            {
                if ((bmp == null) || (im == null))
                {
                    return HRESULT.S_ERROR;
                }
                try
                {
                    FileStream fStr;
                    String strFileName = "";
                    if (Mode == MODE.Save)
                    {
                        if (Directory.Exists(stSerOb.CurrentFolderName))
                        {
                            saveFileDlg.InitialDirectory = stSerOb.CurrentFolderName;
                        }
                        saveFileDlg.Filter = "(*.rmu1)|*.rmu1|(*.rmu)|*.rmu|(*.bmp)|*.bmp|(*.jpeg)|*.jpeg|(*.tif)|*.tif";
                        DialogResult dRes = saveFileDlg.ShowDialog();
                        if (dRes == DialogResult.OK)
                        {
                            strFileName = saveFileDlg.FileName.ToString();
                            if (File.Exists(strFileName))
                            {
                                File.Delete(strFileName);
                            }
                            stSerOb.CurrentFolderName = saveFileDlg.FileName.Substring(0, saveFileDlg.FileName.LastIndexOf("\\") - 1);
                        }
                    }
                    else
                    {
                        strFileName = GetNameAutoSaveFile(strFileExtension);
                    }

                    if (strFileName.Contains(".bmp"))
                    {
                        Result = FDataSave_SaveFile_Full(strFileName, ImageFormat.Bmp);

                        if (Result == HRESULT.S_ERROR)
                        {
                            FDataSave_SaveFile_Frame(strFileName, ImageFormat.Bmp);
                        }
                    }
                    else if (strFileName.Contains(".jpeg"))
                    {
                        Result = FDataSave_SaveFile_Full(strFileName, ImageFormat.Jpeg);

                        if (Result == HRESULT.S_ERROR)
                        {
                            FDataSave_SaveFile_Frame(strFileName, ImageFormat.Jpeg);
                        }
                    }
                    else if (strFileName.Contains(".tif"))
                    {
                        Result = FDataSave_SaveFile_Full(strFileName, ImageFormat.Tiff);

                        if (Result == HRESULT.S_ERROR)
                        {
                            FDataSave_SaveFile_MultiFrameTiff(strFileName);
                        }
                    }
                    else if (strFileName.Contains(".txt"))
                    {
                        File.WriteAllLines(strFileName, commentText);
                    }
                    else if (strFileName.Contains(".rmu1"))
                    {
                        // Streid width and height is wrtote in 4 first bytes 
                        byte[] temp_buf = new byte[buf_total_int.Length * 2 + HEADER_SIZE];
                        WriteFileHeader(ref temp_buf);

                        for (int i = 0, j = HEADER_SIZE; i < buf_total_int.Length; i++, j = j + 2)
                        {
                            temp_buf[j] = (byte)(buf_total_int[i] & 0xFF);
                            temp_buf[j + 1] = (byte)((buf_total_int[i] >> 8) & 0xFF);
                        }
                        fStr = new FileStream(strFileName, FileMode.CreateNew, System.IO.FileAccess.Write);
                        fStr.Write(temp_buf, 0, temp_buf.Length);

                        fStr.Close();
                        fStr.Dispose();
                    }

                    if (Mode != MODE.NotActive)
                    {
                        File.SetAttributes(strFileName, FileAttributes.ReadOnly);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error by save data. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_ERROR;
                }

                return HRESULT.S_OK;
            }

            private HRESULT FDataSave_SaveFile_Full(String strFileName, ImageFormat imageFormat)
            {
                Result = HRESULT.S_OK;
                try
                {
                    int width = ((bmp.Length - 1) * bmp[0].Width) + bmp[bmp.Length - 1].Width;

                    Image imTemp = new Bitmap(width, height_frame_bmp);
                    for (int item = 0; item < bmp.Length; item++)
                    {
                        lock (bmp)
                        {
                            using (Graphics gr = Graphics.FromImage(imTemp))
                            {
                                // Draw the specified section of the source bitmap to the new one
                                gr.DrawImage(bmp[item], item * bmp[0].Width, 0, new Rectangle(0, 0, bmp[item].Width, bmp[item].Height), GraphicsUnit.Pixel);
                                gr.Dispose();
                            }
                        }
                    }

                    imTemp.Save(strFileName, imageFormat);
                    imTemp.Dispose();
                }
                catch (Exception e)
                {
                    Result = HRESULT.S_ERROR;
                }

                return Result;
            }

            private void FDataSave_SaveFile_Frame(String strFileName, ImageFormat imageFormat)
            {
                string strExtention = "." + imageFormat.ToString().ToLower();

                // It is possible exception is ossures as result a memory lack,
                // try save separates bitmaps
                strFileName = strFileName.Replace(strExtention, "_");

                for (int item = 0; item < bmp.Length; item++)
                {
                    if (bmp[item] != null)
                        bmp[item].Save(strFileName + item.ToString() + strExtention, imageFormat);
                }

                Result = HRESULT.S_OK;
            }

            private static void FDataSave_SaveFile_MultiFrameTiff(String strFileName)
            {
                lock (bmp)
                {
                    EncoderParameters ep = new EncoderParameters(1);
                    EncoderParameter epMultiFrame = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                    ep.Param[0] = epMultiFrame;

                    // Get Image Codec Information
                    ImageCodecInfo codecInfo = GetEncoderInfo("image/tiff");

                    bmp[0].Save(strFileName, codecInfo, ep);

                    // Encoding format
                    EncoderParameter epFrameDimensionPage = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                    ep.Param[0] = epFrameDimensionPage;

                    for (int item = 1; item < bmp.Length; item++)
                    {
                        if (bmp[item] != null)
                        {
                            bmp[0].SaveAdd((Image)bmp[item], ep);
                        }
                    }
                }
            }

            private HRESULT WriteFileHeader(ref byte[] bufPar)
            {
                for (int i = 0; i < HEADER_SIZE; i = i + 2)
                {
                    switch (i)
                    {
                        case 0:
                            bufPar[0] = (byte)(width_frame_bmp & 0xFF);
                            bufPar[1] = (byte)((width_frame_bmp >> 8) & 0xFF);
                            break;
                        case 1:
                            bufPar[2] = (byte)(height_frame_bmp & 0xFF);
                            bufPar[3] = (byte)((height_frame_bmp >> 8) & 0xFF);
                            break;
                    }
                }

                return HRESULT.S_OK;
            }

            private String SetNameAutoSaveFolder()
            {
                String autoFolderName = "";
                autoFolderName += ((stSerOb.ScanDataFolderName == "") ? "C:/Rmu1Data" : stSerOb.ScanDataFolderName) + "/" + DateTime.Now.ToString(@"yyyy_MM_dd_HH_mm_ss");
                System.IO.Directory.CreateDirectory(autoFolderName);

                return autoFolderName;
            }

            private String GetNameAutoSaveFile(String strFileExtensionP)
            {
                String autoFileName = "";
                autoFileName = strAutoFolderName + "/rmu1_" + DateTime.Now.ToString(@"yyyy_MM_dd_HH_mm_ss");
                autoFileName += "." + strFileExtensionP;

                return autoFileName;
            }

            private HRESULT FDataSave_AutoSaveData()
            {
                Result = HRESULT.S_OK;
                strAutoFolderName = SetNameAutoSaveFolder();
                strFileExtension = "rmu1";
                Result = SaveFile();
                strFileExtension = "tif";
                Result = SaveFile();
                strFileExtension = "txt";
                Result = SaveFile();

                return Result;
            }

        #endregion SaveFile
    }
}