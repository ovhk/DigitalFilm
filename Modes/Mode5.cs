using DigitalFilm.Controls;
using DigitalFilm.Engine;
using DigitalFilm.Papers;
using DigitalFilm.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DigitalFilm.Modes
{
    internal class Mode5 : IMode
    {
        /// <summary>
        /// Name
        /// </summary>
        [Browsable(false)]
        public string Name => "Display picture";

        /// <summary>
        /// Description
        /// </summary>
        [Browsable(false)]
        public string Description => "Display the selected picture following parameters.";

        /// <summary>
        /// Display exposure time in ms
        /// </summary>
        [Category("Mode Direct")]
        [Description("Display exposure time in ms")]
        public int ExposureTime
        { get; set; } = 5000;

        [Category("Mode Direct")]
        [Description("Type of paper")]
        [TypeConverter(typeof(PaperConverter))]
        public Paper Paper
        { get; set; }

        /// <summary>
        /// Source file to display
        /// </summary>
        [Category("Configuration")]
        [Description("Source file to display")]
        [Editor(typeof(ImageFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ImagePath
        { get; set; }

        /// <summary>
        /// Display mode
        /// </summary>
        [Category("Configuration")]
        [Description("Size mode")]
        public SizeMode SizeMode
        { get; set; } = SizeMode.CenterImage;

        #region Margins

        /// <summary>
        /// 
        /// </summary>
        [Category("Margin")]
        [Description("Margin color?")]
        public MarginColor MarginColor
        { get; set; } = MarginColor.White; // remember to invert color somewhere

        /// <summary>
        /// 
        /// </summary>
        [Category("Margin")]
        [Description("Top and bottom margin size.")]
        public int MarginTopBottom
        { get; set; } = 10;

        /// <summary>
        /// 
        /// </summary>
        [Category("Margin")]
        [Description("Left and right margin size.")]
        public int MarginLeftRight
        { get; set; } = 10;

        #endregion

        /// <summary>
        /// Rotation
        /// </summary>
        [Category("Configuration")]
        [Description("Rotation")]
        public RotateFlipType Rotation
        { get; set; } = RotateFlipType.RotateNoneFlipNone;

        /// <summary>
        /// 
        /// </summary>
        [Category("Display Mode")]
        [Description("Define algo used to diplay the picture.")]
        public DisplayMode DisplayMode
        { get; set; } = DisplayMode.Direct;

        /// <summary>
        /// 
        /// </summary>
        [Category("Mode GrayToTime")]
        [Description("Use cache to load faster? (only in GrayToTime mode)")]
        public bool UseCache
        { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Category("Mode GrayToTime")]
        [Description("Select GrayToTime curve")]
        public GrayToTimeCurve Curve
        { get; set; } = GrayToTimeCurve.PMuth;

        /// <summary>
        /// 
        /// </summary>
        [Category("Mode Custom GrayToTime")]
        [Description("Select GrayToTime curve")]
        public string Formula
        { get; set; } = "(int)(0.0001 * Math.Pow(x, 4) - 0.0722 * Math.Pow(x, 3) + 17.586 * Math.Pow(x, 2) - 1890.5 * x + 89686.0)";

        /// <summary>
        /// Access to the Engine
        /// </summary>
        private readonly DisplayEngine engine = DisplayEngine.GetInstance();

        /// <summary>
        /// Load function
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ImagePath == null || ImagePath.Length == 0)
            {
                return false;
            }

            if (DisplayMode == DisplayMode.Direct && Paper == null)
            {
                return false;
            }

            string md5 = Tools.Checksum.CalculateMD5(ImagePath);

            md5 += "-" + (Rotation.GetHashCode()
                + 22 * SizeMode.GetHashCode()
                + 333 * MarginColor.GetHashCode()
                + 4444 * MarginTopBottom.GetHashCode()
                + 55555 * MarginLeftRight.GetHashCode()
                ).ToString(); // Just a way to have an unique value with parameters, not perfect but seems enought!

            if (DisplayMode == DisplayMode.GrayToTime)
            {
                // Use Cache ?
                if (this.UseCache)
                {
                    if (engine.Cache.IsInCache(md5) == true)
                    {
                        return engine.Cache.LoadFromCache(md5);
                    }
                    else
                    {
                        engine.Cache.SetCacheIdentifier(md5);
                    }
                }
                else
                {
                    engine.Cache.ClearCacheFromIdentifier(md5);
                }
            }

            // this way permit to not lock the file : https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
            using (Bitmap bmpPicture = new Bitmap(ImagePath))
            {
                Bitmap bmpPanel = new Bitmap(engine.Panel.Width, engine.Panel.Height, PixelFormat.Format24bppRgb);

                Graphics gfx = Graphics.FromImage(bmpPanel);

                bmpPicture.RotateFlip(Rotation);

                // 1. erase all
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    gfx.FillRectangle(brush, 0, 0, engine.Panel.Width, engine.Panel.Height);
                }

                // 2. Draw margins
                SolidBrush marginBrush = (MarginColor == MarginColor.Back) ? new SolidBrush(Color.Black) : new SolidBrush(Color.White); // do not invert color here, done after

                if (MarginLeftRight > 0)
                {
                    gfx.FillRectangle(marginBrush, 0, 0, MarginLeftRight, engine.Panel.Height); // LEFT

                    gfx.FillRectangle(marginBrush, engine.Panel.Width - MarginLeftRight, 0, MarginLeftRight, engine.Panel.Height); // RIGHT
                }

                if (MarginTopBottom > 0)
                {
                    gfx.FillRectangle(marginBrush, 0, 0, engine.Panel.Width, MarginTopBottom); // TOP

                    gfx.FillRectangle(marginBrush, 0, engine.Panel.Height - MarginTopBottom, engine.Panel.Width, MarginTopBottom); // BOTTOM
                }

                marginBrush.Dispose();

                // 3. determine size
                Size sz = new Size();

                switch (SizeMode)
                {
                    case SizeMode.CenterImage:
                        // Ratio is : canavas / original
                        double ratioW = (engine.Panel.Width - (2 * (double)MarginLeftRight)) / bmpPicture.Width;
                        double ratioH = (engine.Panel.Height - (2 * (double)MarginTopBottom)) / bmpPicture.Height;

                        // get the smaller ratio
                        double ratio = (ratioW < ratioH) ? ratioW : ratioH;

                        sz.Width = Convert.ToInt32(ratio * bmpPicture.Width);
                        sz.Height = Convert.ToInt32(ratio * bmpPicture.Height);
                        break;

                    case SizeMode.StretchImage:
                        sz.Width = engine.Panel.Width - (2 * MarginLeftRight);
                        sz.Height = engine.Panel.Height - (2 * MarginTopBottom);
                        break;

                    default:
                        break;
                }

                // 4. convert picture to grayscale
                Bitmap grayscalePicture = BitmapTools.MakeGrayscale3(bmpPicture);

                // 5. size image
                Rectangle imgRect = new Rectangle
                {
                    X = MarginLeftRight,
                    Y = MarginTopBottom,
                    Width = sz.Width, // margin already inside
                    Height = sz.Height
                };

                if (SizeMode == SizeMode.CenterImage)
                {
                    // Add offset to center image
                    imgRect.X += Convert.ToInt32(((engine.Panel.Width - (2 * (double)MarginLeftRight)) / 2) - (sz.Width / 2.0));
                    imgRect.Y += Convert.ToInt32(((engine.Panel.Height - (2 * (double)MarginTopBottom)) / 2) - (sz.Height / 2.0));
                }

                // Check ratio
                //System.Windows.Forms.MessageBox.Show("Ratio=" + (double)imgRect.Width / (double)imgRect.Height); // for debug only

                // 6. draw image
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //bmpPanel.SetResolution(grayscalePicture.HorizontalResolution/2, grayscalePicture.VerticalResolution*2);

                //Log.WriteLine("bmpPicture H={0},V={1}", bmpPicture.HorizontalResolution, bmpPicture.VerticalResolution);
                //Log.WriteLine("grayscalePicture H={0},V={1}", grayscalePicture.HorizontalResolution, grayscalePicture.VerticalResolution);
                //Log.WriteLine("bmpPanel H={0},V={1}", bmpPanel.HorizontalResolution, bmpPanel.VerticalResolution);

                //gfx.DrawImage(imgPhoto, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);

                gfx.DrawImage(grayscalePicture, imgRect);

                //engine.PushImage((Bitmap)bmpPanel.Clone(), ExposureTime); // for debug only

                switch (DisplayMode)
                {
                    case DisplayMode.Direct:
                        {
                            if (this.Paper == null)
                            {
                                return false;
                            }
                            // 7.1. convert image for selected paper
                            Bitmap imageForPaper = BitmapTools.BitmapToPaper(bmpPanel, this.Paper);

                            // 8.1. push image to engine
                            engine.PushImage(imageForPaper, this.ExposureTime);
                        }
                        break;

                    case DisplayMode.DirectPaperGamma:
                        {
                            if (this.Paper == null)
                            {
                                return false;
                            }

                            // 7.2.1 apply the Gamma from the selected paper
                            Bitmap imageGamma = BitmapTools.GetBitmapWithGamma(bmpPanel, this.Paper.Gamma);

                            // 7.2.2 invert black and white
                            Bitmap imageForPaper = BitmapTools.GetInvertedBitmap(imageGamma);

                            // 8.2. push image to engine
                            engine.PushImage(imageForPaper, this.ExposureTime);
                        }
                        break;

                    case DisplayMode.DirectAllGrade:
                        {
                            // 7.3. convert image with all grade of selected paper
                            Bitmap imageForPaper = BitmapTools.BitmapToPapers(bmpPanel);

                            // 8.3. push image to engine
                            engine.PushImage(imageForPaper, this.ExposureTime);
                        }
                        break;

                    case DisplayMode.GrayToTime:
                        {
                            // 7.4. get image layers
                            List<ImageLayer> ils = GetImageLayers(bmpPanel);

                            foreach (ImageLayer il in ils)
                            {
                                // 8.4. push image to engine
                                engine.PushImage(il);
                            }
                        }
                        break;
                }

                gfx.Dispose();
            }

            return true;
        }

        /// <summary>
        /// Get Image preview
        /// </summary>
        /// <param name="bitmap">displayed image</param>
        /// <returns>preview image</returns>
        public Bitmap GetPrewiew(Bitmap bitmap)
        {
            Bitmap imageToDisplay = null;

            switch (this.DisplayMode)
            {
                case Modes.DisplayMode.Direct:
                case Modes.DisplayMode.GrayToTime:
                    imageToDisplay = BitmapTools.BitmapFromPaper(bitmap, this.Paper);
                    break;
                case Modes.DisplayMode.DirectPaperGamma:
                    // Upper we applied Gamma, then invert color, so now invert color and apply invert Gamma 
                    Bitmap invertedImage = BitmapTools.GetInvertedBitmap(bitmap);
                    imageToDisplay = BitmapTools.GetBitmapWithGamma(invertedImage, 1 / this.Paper.Gamma);
                    break;
                case Modes.DisplayMode.DirectAllGrade:
                    imageToDisplay = BitmapTools.BitmapFromPapers(bitmap);
                    break;
            }

            return imageToDisplay;
        }

        /// <summary>
        /// Unload function
        /// </summary>
        /// <returns></returns>
        public bool Unload()
        {
            this.engine.Clear();

            return true;
        }

        /// <summary>
        /// Return the Name parameter
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public List<ImageLayer> GetImageLayers(Bitmap bitmap)
        {
            List<ImageLayer> imageLayers = new List<ImageLayer>();

            int[] timings = null;

            if (this.Curve == GrayToTimeCurve.PMuth)
            {
                timings = GrayToTime.TimingsPMUTH;
            }
            else if (this.Curve == GrayToTimeCurve.Custom)
            {
                // TODO : put 256 in a parameter
                timings = GrayToTime.ComputeTimingsCustom(256, this.Formula);

                // some debug
                int[] timingsPMUTH = GrayToTime.TimingsPMUTH;

                for (int i = 0; i < timingsPMUTH.Length; i++)
                {
                    Log.WriteLine("[" + i + "] PMuth=" + timingsPMUTH[i] + ", Custom=" + timings[i]);
                }
            }

            for (int i = 0; i < timings.Length; i++)
            {
                using (DirectBitmap b = new DirectBitmap(bitmap.Width, bitmap.Height))
                {
                    for (int x = 0; x < b.Width; x++)
                    {
                        for (int y = 0; y < b.Height; y++)
                        {
                            Color c = bitmap.GetPixel(x, y);

                            // we use R but G or B are equal
                            if (c.R < i) // TODO : < or <= ?
                            {
                                b.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                            }
                            else
                            {
                                b.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                            }
                        }
                    }

                    imageLayers.Add(new ImageLayer(b.Bitmap, timings[i], i));
                }
            }

            return imageLayers;
        }
    }
}
