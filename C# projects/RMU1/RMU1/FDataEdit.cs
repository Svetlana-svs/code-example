 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Diagnostics;


namespace RMU1
{
    public partial class FData
    {
        private void FDataEdit()
        {
            try
            {
                if (im != null)
                {
                    Int16[] semaforScreenItemsP;

                    // Array of items that is displaied (maped) on screen (can be one or 2 elements by coefficient > 4)
                    semaforScreenItems = new List<Int16>(DetermineScreenItem());
                    semaforScreenItemsP = semaforScreenItems.ToArray();
                    // Items of the screen are processed on the first

                    Result = HRESULT.S_OK;
                    foreach (Int16 item in semaforScreenItems)
                    {
                        Int16 itemP = item;
                        Int16 itemWidthP = itemWidth;
                        if (bmp[item] != null)
                        {
                            FEditThread threadEditBitmap = new FEditThread(item, getModeEditStruct(), (Int16)bmp[item].Width, stSerOb.UseAdjustment, height_frame_bmp);
                            DelegateEditBitmap delegateEditThread = new DelegateEditBitmap(threadEditBitmap.FDataEditThread);
                            AsyncCallback callbackEditBitmap = new AsyncCallback(EndEditBitmap);
                            IAsyncResult ar = delegateEditThread.BeginInvoke(out Result, out itemP, callbackEditBitmap, delegateEditThread);
                        }
                        else
                        {
                            semaforScreenItems.Remove(item);
                            if ((Result != HRESULT.S_OK) || (semaforScreenItems.Count == 0))
                            {
                                Mode = MODE.NotActive;
                                return;
                            }
                        }
                    }
                    while ((semaforScreenItems.Count != 0) && (Mode != MODE.Scan) && (Result == HRESULT.S_OK))
                    {
                        //                  Thread.Sleep(1000);
                    }
                    Debug.WriteLine("update ");
                    Invalidate();
                    Update();

                    if (Result != HRESULT.S_OK)
                    {
                        Mode = MODE.NotActive;
                        return;
                    }

                    for (Int16 item = 0; item < bmp.Length; item++)
                    {
                        if (Array.IndexOf(semaforScreenItemsP, item) == -1)
                        {
                            Int16 itemWidthP = itemWidth;
                            if (bmp[item] != null)
                            {
                                FEditThread threadEditBitmap = new FEditThread(item, getModeEditStruct(), (Int16)bmp[item].Width, stSerOb.UseAdjustment, height_frame_bmp);
                                DelegateEditBitmap delegateEditThread = new DelegateEditBitmap(threadEditBitmap.FDataEditThread);
                                AsyncCallback callbackEditBitmap = new AsyncCallback(EndEditBitmap);
                                IAsyncResult ar = delegateEditThread.BeginInvoke(out Result, out item, callbackEditBitmap, delegateEditThread);
                            }
                            else
                            {
                                semaforScreenItems.Remove(item);
                                if ((Result != HRESULT.S_OK) && (semaforScreenItems.Count == 0))
                                {
                                    Mode = MODE.NotActive;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {

            }
            Mode = MODE.NotActive;
        }
        
        static private void EndEditBitmap(IAsyncResult ar)
        {
            DelegateEditBitmap dlgt = (DelegateEditBitmap)ar.AsyncState;
            // Complete the call.
            Int16 itemP;
            Bitmap bmpCurrent = dlgt.EndInvoke(out Result, out itemP, ar);
            lock (bmp[itemP])
            {
                if (bmpCurrent != null)
                {
                    bmp[itemP] = (Bitmap)bmpCurrent.Clone();
                    bmpCurrent.Dispose();
                }
            }
            
            Debug.WriteLine("item " + itemP.ToString());

            if (semaforScreenItems.Contains(itemP)) {
                semaforScreenItems.Remove(itemP);
            }
        }

        private Int16[] DetermineScreenItem()
        {
            Int16[] currentFrames;
            Int16 currentFrame;
            int rem;
            int cur = (hScrollBar.Value) * 100 / delta;
            
            currentFrame = (Int16)(Math.DivRem(cur, itemWidth, out rem));

            if ((currentFrame != (bmp.Length - 1)) && (rem > (itemWidth - (rectScreen.Width * 100 / delta))))
            {
                currentFrames = new Int16[2];
                currentFrames[1] = (Int16)(currentFrame + 1);
            }
            else
            {
                currentFrames = new Int16[1];

            }
            currentFrames[0] = currentFrame;

            return currentFrames;
        }
        
        private MODE_EDIT getModeEditStruct()
        {
            MODE_EDIT ModeEdit = new MODE_EDIT();

            ModeEdit.Inverse = chBxInvert.Checked;
            ModeEdit.None = true;
 
            if (Mode == MODE.Shift)
            {
                ModeEdit.Shift = true;
                ModeEdit.ShiftValue = (Int16)tbShift.Value;
            }
            else
            {
                ModeEdit.Shift = false;
                ModeEdit.ShiftValue = (Int16)tbShift.Value;
            }
            
            if (bContrastChange || ((int)nContrast.Value != 0))
            {
                ModeEdit.Contrast = true;
                ModeEdit.ContrastValue = (Int16)nContrast.Value;
            }
            else
            {
                ModeEdit.Contrast = false;
                ModeEdit.ContrastValue = 0;
            }
        
            return ModeEdit;
        }

        private void SetBrightness()
        {
            ImageAttributes iAttr = new ImageAttributes();
            iAttr.SetGamma(tbBrightness.Value / 10.0f);
            imageAttr = iAttr;
            Invalidate();
        }

        #region PanelEdit

            private Boolean bContrastChange = false;
            
            private void nContrast_ValueChanged(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (nContrast.Value != tbContrast.Value)
                {
                    tbContrast.Value = (int)nContrast.Value;
                }
                if (!bContrastScroll)
                {
                    bContrastChange = true;
                    FDataEdit();
                    bContrastChange = false;
                }
            }
            
            private void tbContrast_MouseDown(object sender, MouseEventArgs e)
            {
                bContrastScroll = false;
            }        

            private void tbContrast_MouseUp(object sender, MouseEventArgs e)
            {
                if (nContrast.Value != (int)tbContrast.Value)
                {
                    nContrast.Value = (int)tbContrast.Value;
                }

                Cursor.Current = Cursors.Default;
            }

            private void chBxInvert_Click(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;
                FDataEdit();
                Cursor.Current = Cursors.Default;
            }

            private void nShift_ValueChanged(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;
                int tempShift = tbShift.Value;
        
                if (nShift.Value == 0)
                {
                    if (tempShift == 1)
                    {
                        nShift.Value = (int)-1;
                    }
                    else
                    {
                        nShift.Value = (int)1;
                    }
                    return;
                }

                if (nShift.Value != ((int)tbShift.Value))
                {
                    tbShift.Value = (int)nShift.Value;
                }
                if (!bShiftScroll && (tbShift.Value != 0))
                {
                    Mode = MODE.Shift;
                    FDataEdit();
                }
                if (bmp == null)
                {
                    Mode = MODE.NotActive;
                }
            }

            private void tbShift_MouseDown(object sender, MouseEventArgs e)
            {
                bShiftScroll = true;
            }
           
            private void tbShift_MouseUp(object sender, MouseEventArgs e)
            {
                bShiftScroll = false;
                {
                    if ((int)tbShift.Value == 0)
                    {
                        if (nShift.Value == 1)
                        {
                            nShift.Value = -1;
                        }
                        else
                        {
                            nShift.Value = 1;
                        }
                    }
          
                    if (nShift.Value != ((int)tbShift.Value))
                    {
                        nShift.Value = (int)tbShift.Value;
                    }
                }
                // Safe current state of the scroll Shift to exclude shift = 0
                oldShiftValue = (int)tbShift.Value; 
            }

            private void tbBrightness_Scroll(object sender, EventArgs e)
            {
                if (nBrightness.Value != (int)tbBrightness.Value)
                {
                    nBrightness.Value = (int)tbBrightness.Value;
                }
            }

            private void nBrightness_ValueChanged(object sender, EventArgs e)
            {
                Cursor.Current = Cursors.WaitCursor;
                if (nBrightness.Value != tbBrightness.Value)
                {
                    tbBrightness.Value = ((int)nBrightness.Value < 0) ? (int)nBrightness.Value + 1 : (int)nBrightness.Value;
                }
                SetBrightness();
                Cursor.Current = Cursors.Default;
            }
 
        #endregion PanelEdit
    }  
}