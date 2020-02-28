using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace RMU1
{
    public partial class FData
    {
        #region Scan

            private void ButtonBeginEnd_Click(object sender, EventArgs e)
            {
                ButtonScan_Click(this.ButtonBeginEnd);
            }

            private void ButtonScan_Click(ToolStripButton button)
            {
                if ((Mode == MODE.Cancel) && !backgroundWorker.IsBusy)
                {
                    Mode = MODE.NotActive;
                }
                switch (Mode)
                {
                    case MODE.NotActive:
                    case MODE.Edit:
                    {
                        if (pictureBoxConnection.BackColor == Color.Red)
                        {
                            return;
                        }

                        if (!backgroundWorker.CancellationPending)
                        {
                            this.backgroundWorker.CancelAsync();
                            Thread.Sleep(1000);
                            Cursor.Current = Cursors.WaitCursor;
                            return;
                        }

                        if (backgroundWorker.IsBusy)
                        {
                            MessageBox.Show("Выполняется возврат кареток устройства.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            Result = HRESULT.S_ERROR;
                        }
                        else
                        {
                            button.Image = (Bitmap)global::RMU1.Properties.Resources.imgButtonEnd.Clone();
                            button.Text = "   Стоп";
                            Launch dlgLaunch = new Launch();
                            dlgLaunch.diameter = stSerOb.PipeDiameter;
                            dlgLaunch.wall = stSerOb.PipeWall;
                            dlgLaunch.volltage = stSerOb.VolltageSource;
                            dlgLaunch.current = stSerOb.CurrentSource;
                            dlgLaunch.overlap = stSerOb.OverlapZone;
                            dlgLaunch.crop = stSerOb.CropZone;
                            dlgLaunch.expositionTime = stSerOb.ExpositionTimeFrame;
                            dlgLaunch.targetTime = stSerOb.TargetTimeFrame;
                            dlgLaunch.framesCount = framesCount;
                            dlgLaunch.reverse = stSerOb.Reverse;
                            
                            if (dlgLaunch.ShowDialog() == DialogResult.OK)
                            {
                                framesCount = dlgLaunch.framesCount;
                                stSerOb.PipeDiameter = dlgLaunch.diameter;
                                stSerOb.PipeWall = dlgLaunch.wall;
                                stSerOb.VolltageSource = dlgLaunch.volltage;
                                stSerOb.CurrentSource = dlgLaunch.current;
                                stSerOb.OverlapZone = dlgLaunch.overlap;
                                stSerOb.CropZone = dlgLaunch.crop;
                                width_frame_bmp = (Int16)((stSerOb.CropZone != 0) ? (stSerOb.StreidWidth - stSerOb.CropZone * 2) : stSerOb.StreidWidth);
                                stSerOb.ExpositionTimeFrame = dlgLaunch.expositionTime;
                                stSerOb.TargetTimeFrame = dlgLaunch.targetTime;
                                stSerOb.Reverse = dlgLaunch.reverse;
                                commentText = dlgLaunch.comment;
                            }
                            else
                            {
                                Result = HRESULT.S_CANCEL;
                            }
                        }
                        itemWidth = (Int16)(coefficient * width_frame_bmp);

                        if (Result == HRESULT.S_OK)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            Mode = MODE.Scan;
                            ClearScreen(1);
                            lock (progressBar)
                            {
                                progressBar.Maximum = framesCount;
                                progressBar.Value = 0;

                            }
                            this.backgroundWorker.RunWorkerAsync();
                        }
                        else
                        {
                            Mode = MODE.NotActive;
                            button.Image = (Bitmap)global::RMU1.Properties.Resources.imgButtonBegin.Clone();
                            button.Text = "   Пуск";
                            Result = HRESULT.S_OK;
                        }
                    }
                    break;
                    case MODE.Scan:
                    case MODE.Cancel:
                    {
                        // Cancel Mode or error Result
                        if ((!backgroundWorker.CancellationPending && backgroundWorker.IsBusy) || (Result == HRESULT.S_ERROR))
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            Mode = MODE.Cancel;
                            StringBuilder serverUri = new StringBuilder(stSerOb.ServerUri);

                            try
                            {
                                delegateStopDevice.Invoke(serverUri);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Error by array initialization or XML-RPC initialization RMU1 device on server. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                Result = HRESULT.S_ERROR;
                            }

                            this.backgroundWorker.CancelAsync();
                            Thread.Sleep(100);
                        }

                        button.Image = (Bitmap)global::RMU1.Properties.Resources.imgButtonBegin.Clone();
                        button.Text = "   Пуск";

                        if (Mode != MODE.Cancel)
                        {
                            // Success Result after scanning
                            if (!backgroundWorker.CancellationPending)
                            {
                                this.backgroundWorker.CancelAsync();
                            }
                            Mode = MODE.NotActive;
                        }
                        else
                        {
                            ReviewBitmapArray();
                        }

                        if (stSerOb.UseAdjustment)
                        {
                            UInt16[] buf_empty = new UInt16[0];
                            Adjustment(ref buf_empty);
                        }

                        ClearScreen(1);
                        InizializeEditBitmap();
                        Mode = MODE.Shift;
                        FDataEdit();
                    }
                    break;
                }
            }

            private HRESULT Scan()
            {
                StringBuilder prefixScanFileName = new StringBuilder(stSerOb.ServerScanDataFolderName + "\\rmu1_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                StringBuilder serverUri = new StringBuilder(stSerOb.ServerUri);

                IAsyncResult initDeviceRes = null;
                ScanResultError = 0;
                lock (progressBar)
                {
                    progressBar.Value = 0;
                }
                nSumBufTotalInt = 0;
                UInt16[] buf_frame = null;
                UInt32[] buf_frame_int = null;
                //Int32 overlap = (290 * (45 - stSerOb.OverlapZone)) / 45;//110417
                Int32 overlap = 390  - stSerOb.OverlapZone;//step motor steps 370 - 720
                try
                {
                    // XML-RPC initialization RMU1 device on server
                    initDeviceRes = delegateInitDevice.BeginInvoke(serverUri, framesCount, stSerOb.VolltageSource, stSerOb.CurrentSource, stSerOb.ExpositionTimeFrame, stSerOb.TargetTimeFrame, overlap, Convert.ToInt32(stSerOb.Reverse), prefixScanFileName, callbackInitDevice, null);

                    buf_frame = new UInt16[width_frame_bmp * height_frame_bmp];
                    buf_frame_int = new UInt32[(width_frame_bmp + stSerOb.CropZone * 2) * height_frame_bmp];
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error by array initialization or XML-RPC initialization RMU1 device on server. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Result = HRESULT.S_ERROR;
                }

                if (arAvgFrame.Count != 0)
                {
                    arAvgFrame.Clear();
                }

                current_frame = 0;
                int rem;
                Int16 itemsCount = (Int16)Math.DivRem(framesCount, coefficient, out rem);
                if (rem == 0)
                {
                    rem = coefficient;
                }
                itemWidthLast = (Int16)(rem * width_frame_bmp);

                if (im != null)
                {
                    lock (im)
                    {
                        im.Dispose();
                        im = null;
                    }
                }
                DisposeBitmapArray(ref bmp);

                if (buf_total_int != null)
                {
                    buf_total_int = null;
                }

                // Set timeout in the case of connection check call for prevent server fail  
                if (connectionCheckProcessing)
                {
                    Debug.WriteLine("connection device end wait");
                    Thread.Sleep(timerConnection.Interval / 2);
                }

                stateTimer.Elapsed += OnTimeoutExceptionEvent;

                // Цикл покадрового считывания
                while ((ScanResultError == 0) && (current_frame < framesCount) && (Result == HRESULT.S_OK) && !backgroundWorker.CancellationPending)
                {
                    Boolean bIsDownload = false;
                    Byte[] bufP = null;
                    stateTimer.Enabled = true;

                    // Download the file with frame data from server
                    while ((ScanResultError == 0) && !backgroundWorker.CancellationPending && !bIsDownload)
                    {
                        //                 try
                        {
                            String scanFileName = stSerOb.ServerPath + "\\" + prefixScanFileName + "_" + current_frame.ToString();
                            if (File.Exists(@scanFileName))
                            {
                                FDataSave_ReadFile(scanFileName, ref bufP);
                                stateTimer.Stop();
                                bIsDownload = true;
                            }
                            else
                            {
                                Thread.Sleep(100);
                            }
                        }
                        //                 catch
                        {
                            // Timeout if the file is not ready (until frame is not scanning or error is occured)
                            //                     Thread.Sleep(2000);
                        }
                    } // while

                    if ((bufP != null) && (ScanResultError == 0) && !backgroundWorker.CancellationPending)
                    {
                        Result = FDataConvertion_BufferConversion(0, ref bufP, ref buf_frame_int);

                        if (Result != HRESULT.S_OK)
                        {
                            break;
                        }
                        buf_frame.Initialize();
                        // Заполнение кадра
                        // Create the buffer more size 
                        Result = TotalBufCreate();
                        if (Result != HRESULT.S_OK)
                        {
                            break;
                        }
                        try
                        {
                            // Добавление кадра в общий буфер
                            if (buf_total_int.Length > 0)
                            {
                                for (int j = 0; j < height_frame_bmp; j++)
                                {
                                    Array.Copy(buf_frame_int, ((width_frame_bmp + stSerOb.CropZone + stSerOb.CropZone) * j) + stSerOb.CropZone, buf_total_int, width_frame_bmp * height_frame_bmp * current_frame + width_frame_bmp * j, width_frame_bmp);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Error by copy array. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            Result = HRESULT.S_ERROR;
                        }
                        if (Result != HRESULT.S_OK)
                        {
                            break;
                        }

                        if (stSerOb.UseAdjustment)
                        {
                            arAvgFrame.Add(new long[2]);
                        }

                        if (bmp == null)
                        {
                            int r;
                            int itemsBmpCount = (Int16)(Math.DivRem(framesCount, coefficient, out r) + (r > 0 ? 1 : 0));
                            bmp = new Bitmap[itemsBmpCount];
                        }

                        int item = Math.DivRem(current_frame, coefficient, out rem);

                        FDataModification dataTransform = new FDataModification();
                        dataTransform.SetMode(Mode);
                        dataTransform.SetAdjustmentParameters(stSerOb.UseAdjustment, item);

                        // Преобразование кадра в 16-битовое число
                        Result = dataTransform.SetBufTotalToBufForBitmap((int)nShift.Value, width_frame_bmp * height_frame_bmp * current_frame, width_frame_bmp * height_frame_bmp, current_frame, ref buf_frame);
                        if (Result != HRESULT.S_OK)
                        {
                            break;
                        }
                        
                        // Установка ширины и создание текущей bitmap
                        Int16 itemWidthCurrent = itemWidth;
                        if (rem == 0)
                        {
                            if (item == (bmp.Length - 1))
                            {
                                itemWidthCurrent = itemWidthLast;
                            }

                            bmp[item] = new Bitmap(itemWidthCurrent, height_frame_bmp);
                        }

                        // Добавление кадра в bitmap
                        Result = dataTransform.SetBitmap(current_frame, ref buf_frame, ref bmp[item]);

                        if (Result != HRESULT.S_OK)
                        {
                            break;
                        }

                        // Отображение bitmap с новым кадром на image
                        FDataScan_DrawImage(ref item, ref itemWidthCurrent);

                        // Отображение image на экран
                        SetRectShowSize();
                        lock (progressBar)
                        {
                            progressBar.Increment(1);
                        }

                        current_frame++;
                    } // if buf != null
                } // while

                stateTimer.Elapsed -= OnTimeoutExceptionEvent;
                if ((ScanResultError == 0) && (Result == HRESULT.S_OK))
                {
                    if (!backgroundWorker.CancellationPending)
                    {
                        FDataSave_AutoSaveData();
                        for (int i = 0; i < framesCount; i++)
                        {
                            String scanFileName = stSerOb.ServerPath + prefixScanFileName + "_" + i.ToString();
                            if (File.Exists(scanFileName))
                            {
                                File.Delete(scanFileName);
                            }
                        }
                    }
                }

                return Result;
            }

            // Draw current Bitmap with new frame on Image
            private void FDataScan_DrawImage(ref int item, ref Int16 itemWidthCurrent)
            {
                int rem;
                lock (bmp)
                {
                    if (im == null)
                    {
                        Bitmap bmpTemp = new Bitmap((rectScreen.Width * delta_default) / delta, height_frame_bmp);
                        im = (Image)bmpTemp.Clone();
                    }
                    Math.DivRem(current_frame + 1, coefficient, out rem);
                    if (rem == 0)
                    {
                        rem = coefficient;
                    }
                    itemWidthCurrent = (Int16)(rem * width_frame_bmp);
                    lock (im)
                    {
                        using (Graphics gr = Graphics.FromImage(im))
                        {
                            gr.Clear(Color.White);
                            if ((item == 0) && (itemWidthCurrent < im.Width))
                            {
                                // Draw the specified section of the source bitmap to the new one
                                gr.DrawImage(bmp[item], 0, 0, new Rectangle(0, 0, itemWidthCurrent, height_frame_bmp), GraphicsUnit.Pixel);

                            }
                            if ((item != 0) && (itemWidthCurrent < im.Width))
                            {
                                // Draw the specified section of the source bitmap to the new one
                                gr.DrawImage(bmp[item], im.Width - itemWidthCurrent, 0, new Rectangle(0, 0, itemWidthCurrent, height_frame_bmp), GraphicsUnit.Pixel);
                                // Draw the specified section of the source bitmap to the new one
                                gr.DrawImage(bmp[item - 1], 0, 0, new Rectangle(bmp[item - 1].Width - (im.Width - itemWidthCurrent), 0, (im.Width - itemWidthCurrent), height_frame_bmp), GraphicsUnit.Pixel);

                            }

                            if (itemWidthCurrent > im.Width)
                            {
                                // Draw the specified section of the source bitmap to the new one
                                gr.DrawImage(bmp[item], 0, 0, new Rectangle(itemWidthCurrent - im.Width, 0, im.Width, height_frame_bmp), GraphicsUnit.Pixel);
                            }

                            // Clean up
                            gr.Dispose();
                        }
                    }
                }
            }

            // Total array dynamic create
            private HRESULT TotalBufCreate()
            {
                try
                {
                    int length_buf_total = (buf_total_int == null) ? 0 : buf_total_int.Length;
                    UInt32[] buf_total_int_temp = null;
                    if (length_buf_total != 0)
                    {
                        buf_total_int_temp = new UInt32[length_buf_total];
                        Array.Copy(buf_total_int, 0, buf_total_int_temp, 0, length_buf_total);
                    }
                    buf_total_int = new UInt32[length_buf_total + width_frame_bmp * height_frame_bmp];
                    if (length_buf_total != 0)
                    {
                        Array.Copy(buf_total_int_temp, 0, buf_total_int, 0, length_buf_total);
                    }
                    buf_total_int_temp = null;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error by recreate data array. " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return HRESULT.S_ERROR;
                }
                return HRESULT.S_OK;
            }

            private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
            {
                Scan();
            }

            private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                if ((Mode != MODE.Cancel) && (Mode != MODE.Edit))
                {
                    ButtonScan_Click(this.ButtonBeginEnd);
                }
                else
                {
                    Mode = MODE.NotActive;
                }
            }

            // This function is called after fulfillment of the scan device         
            static private void InitDeviceResult(IAsyncResult iftAr)
            {
                try
                {
                    System.Runtime.Remoting.Messaging.AsyncResult ar = (System.Runtime.Remoting.Messaging.AsyncResult)iftAr;
                    Debug.WriteLine("scanning device begin");
                    DelegateInitDevice bp = (DelegateInitDevice)ar.AsyncDelegate;
                    ScanResultError = bp.EndInvoke(iftAr);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("scanning device error " + e.Message);
                    ScanResultError = 1;
                    Mode = MODE.NotActive;
                }
                if (ScanResultError != 0)
                {
                    stateTimer.Enabled = false;
                    MessageBox.Show("Ошибка при обращении к устройству. Для выяснения деталей обратитесь к log файлу. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                Debug.WriteLine("scanning device end");
            }

            // This function is called by timeout of the file reading by device scanning (Active mode)          
            static private void OnTimeoutExceptionEvent(Object source, System.Timers.ElapsedEventArgs e)
            {
                stateTimer.Enabled = false;
                ScanResultError = 1;
                Result = HRESULT.S_ERROR;
                MessageBox.Show("Ошибка таймаута. Недоступны данные со сканирующего устройства.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        #endregion Scan
    }
}
