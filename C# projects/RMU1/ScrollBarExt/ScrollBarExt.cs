using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Diagnostics;


namespace ScrollBarExt {

    [Designer(typeof(ScrollbarControlDesigner))]
    public class ScrollBarExt : UserControl {

        protected Color moChannelColor = Color.Empty;
        protected Image moUpArrowImage = null;
        protected Image moDownArrowImage = null;
        protected Image moThumbArrowImage = null;

        protected Image moThumbTopImage = null;
        protected Image moThumbTopSpanImage = null;
        protected Image moThumbBottomImage = null;
        protected Image moThumbBottomSpanImage = null;
        protected Image moThumbMiddleImage = null;

        protected int moLargeChange = 10;
        protected int moSmallChange = 1;
        protected int moMinimum = 0;
        protected int moMaximum = 200;
        protected bool moVScroll = true;
        protected bool moVScrollOld = true;
        protected int moValue = 0;
        private int nClickPoint;

        protected int moThumbTop = 0;

        protected bool moAutoSize = false;

        private bool moArrowDown = false;
        protected bool moThumbDown = false;
        private bool moThumbDragging = false;

        private int nTrackLength = 0;
        private int nThumbLength = 0;
        private float fThumbLength = (float)0;
        private float fSpanLength = (float)0;

        int nPixelRange = 0;
        int nRealRange = 0;

        public new event EventHandler Scroll = null;
        public event EventHandler ValueChanged = null;

        public ScrollBarExt() {

            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            moChannelColor = Color.FromArgb(0, 0, 255);
            UpArrowImage = Resource.uparrow;
            DownArrowImage = Resource.downarrow;

            ThumbBottomImage = Resource.ThumbBottomBlue;
            ThumbBottomSpanImage = Resource.ThumbSpanBottomBlue;
            ThumbTopImage = Resource.ThumbTopBlue;
            ThumbTopSpanImage = Resource.ThumbSpanTopBlue;
            ThumbMiddleImage = Resource.ThumbMiddleBlue;
            SetValues();
            SetImages();
            base.Size = base.MinimumSize;
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("LargeChange")]
        public int LargeChange {
            get { return moLargeChange; }
            set { moLargeChange = value;
            SetValues();
            Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("SmallChange")]
        public int SmallChange {
            get { return moSmallChange; }
            set { moSmallChange = value;
            Invalidate();    
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Minimum")]
        public int Minimum {
            get { return moMinimum; }
            set { moMinimum = value;
            Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Maximum")]
        public int Maximum {
            get { return moMaximum; }
            set { moMaximum = value;
            SetValues();
            Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(true), Category("Behavior"), Description("VScrolling")]
        public bool VScrolling
        {
            get { return moVScroll; }
            set
            {
                moVScroll = value;
                SetImages();
                moVScrollOld = moVScroll;
                if (moVScroll) { this.Value = moValue; }
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(0), Category("Behavior"), Description("Value")]
        public int Value {
            get { return moValue; }
            set {
                moValue = value;
                float fPerc = 0.0f;
                if (nRealRange != 0)
                {
                    fPerc = (float)moValue / (float)nRealRange;
                }

                float fTop = fPerc * nPixelRange;
                   if (fTop < 0)
                {
                    moThumbTop  = 0; ;
                }
                else if (fTop > nPixelRange)
                {
                    moThumbTop  = nPixelRange;
                }
                else
                {
                    moThumbTop = (int)fTop;
                }

                Invalidate();
            }
        }
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(false), DefaultValue(true), Category("Behavior"), Description("Arrow Down")]
        public bool ArrowDown
        {
            get { return this.moArrowDown; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Channel Color")]
        public Color ChannelColor
        {
            get { return moChannelColor; }
            set { moChannelColor = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image UpArrowImage {
            get { return moUpArrowImage; }
            set { moUpArrowImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image DownArrowImage {
            get { return moDownArrowImage; }
            set { moDownArrowImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbTopImage {
            get { return moThumbTopImage; }
            set { moThumbTopImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbTopSpanImage {
            get { return moThumbTopSpanImage; }
            set { moThumbTopSpanImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbBottomImage {
            get { return moThumbBottomImage; }
            set { moThumbBottomImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbBottomSpanImage {
            get { return moThumbBottomSpanImage; }
            set { moThumbBottomSpanImage = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Up Arrow Graphic")]
        public Image ThumbMiddleImage {
            get { return moThumbMiddleImage; }
            set { moThumbMiddleImage = value; }
        }

        protected override void OnPaint(PaintEventArgs e) {

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            if (UpArrowImage != null) {
                e.Graphics.DrawImage(UpArrowImage, new Rectangle(new Point(0, 0), new Size((moVScroll ? this.Size.Width : moUpArrowImage.Width), (moVScroll ? moUpArrowImage.Height : this.Size.Height))));
            }
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            Brush oBrush = new SolidBrush(moChannelColor);
            Brush oWhiteBrush = new SolidBrush(Color.FromArgb(255,255,255));
            
            //draw channel left and right border colors
            e.Graphics.FillRectangle(oWhiteBrush, new Rectangle((moVScroll ? 0 : moUpArrowImage.Width), (moVScroll ? moUpArrowImage.Height : 0), (moVScroll ? 1 : this.Width - moDownArrowImage.Width), (moVScroll ? this.Height - moDownArrowImage.Height : 1)));
            e.Graphics.FillRectangle(oWhiteBrush, new Rectangle((moVScroll ? this.Width - 1 : moUpArrowImage.Width), (moVScroll ? moUpArrowImage.Height : this.Height - 1), (moVScroll ? 1 : this.Width - moDownArrowImage.Width), (moVScroll ? this.Height - moDownArrowImage.Height : 1)));
            
            //draw channel
            e.Graphics.FillRectangle(oBrush, new Rectangle((moVScroll ? 1 : moUpArrowImage.Width), (moVScroll ? moUpArrowImage.Height : 1), this.Size.Width - (moVScroll ? 2 : moDownArrowImage.Width), (this.Height - (moVScroll ? moDownArrowImage.Height : 2))));

            //draw thumb
            int nSpanLength = (int)fSpanLength;

        int nTop = moThumbTop;
            nTop += (moVScroll ? UpArrowImage.Height : UpArrowImage.Width);

            //draw top
            e.Graphics.DrawImage(ThumbTopImage, new Rectangle((moVScroll ? 1 : nTop), (moVScroll ? nTop : 1), (moVScroll ? (this.Size.Width - 2) : moThumbTopImage.Width), (moVScroll ? moThumbTopImage.Height : (this.Height - 2))));
            nTop += (moVScroll ? moThumbTopImage.Height : moThumbTopImage.Width);

            //draw top span
            e.Graphics.DrawImage(ThumbTopSpanImage, new RectangleF((moVScroll ? 1.0f : (float)nTop), (moVScroll ? (float)nTop : 1.0f), (moVScroll ? (float)this.Size.Width - 2.0f : (float)fSpanLength * 2), (moVScroll ? (float)fSpanLength * 2 : (float)(this.Height - 2.0f))));
            nTop += nSpanLength;

            //draw middle
            e.Graphics.DrawImage(ThumbMiddleImage, new Rectangle((moVScroll ? 1 : nTop), (moVScroll ? nTop : 1), (moVScroll ? this.Width - 2 : ThumbMiddleImage.Width), (moVScroll ? ThumbMiddleImage.Height : this.Height - 2)));
            nTop += (moVScroll ? ThumbMiddleImage.Height : ThumbMiddleImage.Width);

            //draw bottom span
            Rectangle rect = new Rectangle((moVScroll ? 1 : nTop), (moVScroll ? nTop : 1), (moVScroll ? this.Size.Width - 2 : nSpanLength * 2), (moVScroll ? nSpanLength * 2 : this.Height - 2));
            e.Graphics.DrawImage(ThumbBottomSpanImage, rect);
            nTop += nSpanLength;

            //draw bottom
            e.Graphics.DrawImage(ThumbBottomImage, new Rectangle((moVScroll ? 1 : nTop), (moVScroll ? nTop : 1), (moVScroll ? this.Width - 2 : nSpanLength), (moVScroll ? nSpanLength : this.Height - 2)));
            
            if (DownArrowImage != null) {
                e.Graphics.DrawImage(DownArrowImage, new Rectangle(new Point((moVScroll ? 0 : this.Width - DownArrowImage.Width), (moVScroll ? (this.Height - DownArrowImage.Height) : 0)), new Size((moVScroll ? this.Width : DownArrowImage.Width), (moVScroll ? DownArrowImage.Height : this.Height))));
            }
            
        }

        public override bool AutoSize {
            get {
                return base.AutoSize;
            }
            set {
                base.AutoSize = value;
                if (base.AutoSize) {
                }
            }
        }

        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // ScrollBarExt
            // 
            this.Name = "ScrollBarExt";
            this.Load += new System.EventHandler(this.ScrollBarExt_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScrollBarExt_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScrollBarExt_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScrollBarExt_MouseUp);
            this.SizeChanged += new System.EventHandler(this.ScrollBarExt_SizeChanged);
            this.ResumeLayout(false);

        }

        private void ScrollBarExt_MouseDown(object sender, MouseEventArgs e) {
            Point ptPoint = this.PointToClient(Cursor.Position); 
                this.moArrowDown = false;
            int nTop = moThumbTop;
            nTop += (moVScroll ? UpArrowImage.Height : UpArrowImage.Width);

  
            Rectangle thumbrect = (moVScroll ? 
                new Rectangle(new Point(1, nTop), new Size(ThumbMiddleImage.Width, nThumbLength)) :
                new Rectangle(new Point(nTop, 1), new Size(nThumbLength, ThumbMiddleImage.Height)));
            if (thumbrect.Contains(ptPoint))
            {
              Debug.WriteLine(thumbrect.ToString());
               
                //hit the thumb
                nClickPoint = ((moVScroll ? ptPoint.Y : ptPoint.X) - nTop);
                //MessageBox.Show(Convert.ToString((ptPoint.Y - nTop)));
                this.moThumbDown = true;
            }

            Rectangle uparrowrect = (moVScroll ?
                new Rectangle(new Point(1, 0), new Size(UpArrowImage.Width, UpArrowImage.Height)) :
                new Rectangle(new Point(1, 0), new Size(UpArrowImage.Height, UpArrowImage.Width)));
            if (uparrowrect.Contains(ptPoint))
            {
                this.moArrowDown = true;
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if ((moThumbTop - SmallChange) < 0)
                            moThumbTop = 0;
                        else
                            moThumbTop -= SmallChange;

                        //figure out value
                        float fPerc = (float)moThumbTop / (float)nPixelRange;
                        float fValue = fPerc * (Maximum - LargeChange);
                        
                        moValue = (int)fValue;
                        Debug.WriteLine(moValue.ToString());

                        if (ValueChanged != null)
                            ValueChanged(this, new EventArgs());

                        if (Scroll != null)
                            Scroll(this, new EventArgs());

                        Invalidate();
                    }
                }
            }

            Rectangle downarrowrect = (moVScroll ?
                new Rectangle(new Point(1, UpArrowImage.Height + nTrackLength), new Size(UpArrowImage.Width, UpArrowImage.Height)) :
                new Rectangle(new Point(UpArrowImage.Width + nTrackLength, 1), new Size(UpArrowImage.Height, UpArrowImage.Width)));
            if (downarrowrect.Contains(ptPoint))
            {
                this.moArrowDown = true;
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        if ((moThumbTop + SmallChange) > nPixelRange)
                            moThumbTop = nPixelRange;
                        else
                            moThumbTop += SmallChange;

                        //figure out value
                        float fPerc = (float)moThumbTop / (float)nPixelRange;
                        float fValue = fPerc * (Maximum);
                       
                        moValue = (int)fValue;
                        Debug.WriteLine(moValue.ToString());

                        if (ValueChanged != null)
                            ValueChanged(this, new EventArgs());

                        if (Scroll != null)
                            Scroll(this, new EventArgs());
                        Invalidate();
                    }
                }
            }
        }

        private void ScrollBarExt_MouseUp(object sender, MouseEventArgs e) {
            this.moThumbDown = false;
            this.moArrowDown = false;
            this.moThumbDragging = false;
        }

        private void MoveThumb(int y) {

            int nSpot = nClickPoint;
            if (moThumbDown && nRealRange > 0) {
                if (nPixelRange > 0) {
                    int nNewThumbTop = y - ((moVScroll ? UpArrowImage.Height : UpArrowImage.Width )+nSpot);
                    
                    if(nNewThumbTop<0)
                    {
                        moThumbTop = nNewThumbTop = 0;
                    }
                    else if(nNewThumbTop > nPixelRange)
                    {
                        moThumbTop = nNewThumbTop = nPixelRange;
                    }
                    else {
                        moThumbTop = y - ((moVScroll ? UpArrowImage.Height : UpArrowImage.Width)  + nSpot);
                    }
                   
                    //figure out value
                    float fPerc = (float)moThumbTop / (float)nPixelRange;
                    float fValue = fPerc * (Maximum);
                    moValue = (int)fValue;
                    Debug.WriteLine("move value " + moValue.ToString());

                    Application.DoEvents();

                    Invalidate();
                }
            }
        }

        private void ScrollBarExt_MouseMove(object sender, MouseEventArgs e) {
            if(moThumbDown == true)
            {
                this.moThumbDragging = true;
            }

            if (this.moThumbDragging) {

                MoveThumb((moVScroll ? e.Y : e.X));
            }

            if(ValueChanged != null)
                ValueChanged(this, new EventArgs());

            if(Scroll != null)
                Scroll(this, new EventArgs());
        }

        private void ScrollBarExt_Load(object sender, EventArgs e)
        {

        }
        private void SetValues()
        {
            nTrackLength = (moVScroll ? (this.Height - (UpArrowImage.Height + DownArrowImage.Height)) : (this.Width - (UpArrowImage.Width + DownArrowImage.Width)));
            fThumbLength = ((float)LargeChange / (float)Maximum) * nTrackLength;
            nThumbLength = (int)fThumbLength;

            if (nThumbLength > nTrackLength)
            {
                nThumbLength = nTrackLength;
                fThumbLength = nTrackLength;
            }
            if (nThumbLength < 56)
            {
                nThumbLength = 56;
                fThumbLength = 56;
            }
            fSpanLength = moVScroll? ((fThumbLength - (ThumbMiddleImage.Height + ThumbTopImage.Height + ThumbBottomImage.Height)) / 2.0f) :
                    ((fThumbLength - (ThumbMiddleImage.Width + ThumbTopImage.Width + ThumbBottomImage.Width)) / 2.0f);
            nPixelRange = nTrackLength - nThumbLength;
            nRealRange = (Maximum - Minimum) - LargeChange;
        }

        private void SetImages()
        {
            UpArrowImage = Resource.uparrow;
            DownArrowImage = Resource.downarrow;

            ThumbBottomImage = Resource.ThumbBottomBlue;
            ThumbBottomSpanImage = Resource.ThumbSpanBottomBlue;
            ThumbTopImage = Resource.ThumbTopBlue;
            ThumbTopSpanImage = Resource.ThumbSpanTopBlue;
            ThumbMiddleImage = Resource.ThumbMiddleBlue;
           
            if (!moVScroll)
            {
              
                 UpArrowImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
                 DownArrowImage.RotateFlip(RotateFlipType.Rotate90FlipXY);

                 ThumbMiddleImage.RotateFlip(RotateFlipType.Rotate90FlipXY);  
               ThumbTopImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
                ThumbBottomImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
                ThumbBottomSpanImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
                ThumbTopSpanImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
                Debug.WriteLine("width v1: " + moVScroll.ToString());
            }
 
            this.MinimumSize = moVScroll ? new Size(moUpArrowImage.Width, moUpArrowImage.Height + moDownArrowImage.Height + nThumbLength) :
            new Size(moUpArrowImage.Width + moDownArrowImage.Width + nThumbLength, moUpArrowImage.Height);

            if (!moVScroll)
            {
                int width = this.Width;
                this.Width = (this.Size.Width > this.Size.Height ? this.Size.Width : (this.Size.Height > moUpArrowImage.Width + moDownArrowImage.Width + nThumbLength) ? this.Size.Height : moUpArrowImage.Width + moDownArrowImage.Width + nThumbLength);
                this.Height = moVScroll ? ((width > moUpArrowImage.Height + moDownArrowImage.Height + moThumbMiddleImage.Height + nThumbLength) ? width : moUpArrowImage.Height + moDownArrowImage.Height + nThumbLength) : moUpArrowImage.Height;
            }
            else
            {
                int width = this.Width;

                this.Width = moVScroll ? moUpArrowImage.Width :
                         ((this.Size.Width > moUpArrowImage.Width + moDownArrowImage.Width + moThumbMiddleImage.Width + nThumbLength) ? this.Size.Width : moUpArrowImage.Width + moDownArrowImage.Width + moThumbMiddleImage.Width + nThumbLength);
                this.Height = moVScroll ? ((this.Size.Height > moUpArrowImage.Height + moDownArrowImage.Height + nThumbLength) ? this.Size.Width : moUpArrowImage.Height + moDownArrowImage.Height + moThumbMiddleImage.Height + nThumbLength) : moUpArrowImage.Height;
            }
            SetValues();
        }

        private void ScrollBarExt_SizeChanged(object sender, EventArgs e)
        {
            SetValues();
        }
    }

    internal class ScrollbarControlDesigner : System.Windows.Forms.Design.ControlDesigner {

        public override SelectionRules SelectionRules {
            get {
                SelectionRules selectionRules = base.SelectionRules;
                PropertyDescriptor propDescriptor = TypeDescriptor.GetProperties(this.Component)["AutoSize"];
                if (propDescriptor != null) {
                    bool autoSize = (bool)propDescriptor.GetValue(this.Component);
                    if (autoSize) {
                        selectionRules = SelectionRules.Visible | SelectionRules.Moveable | SelectionRules.BottomSizeable | SelectionRules.TopSizeable;
                    }
                    else {
                        selectionRules = SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable;
                    }
                }
                return selectionRules;
            }
        } 
    }
}