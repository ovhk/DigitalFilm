using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalDarkroom.Controls;
using DigitalDarkroom.Tools;
using System.ComponentModel;
using DigitalDarkroom.Engine;

namespace DigitalDarkroom.Modes
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
        /// Display duration in ms
        /// </summary>
        [Category("Mode Direct")]
        [Description("Display duration in ms")]
        public int Duration
        { get; set; } = 5000;

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
        { get; set; } = DisplayMode.GrayToTime;

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
        /// Access to the Engine
        /// </summary>
        private readonly DisplayEngine engine = DisplayEngine.GetInstance();

        /// <summary>
        /// Load function
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ImagePath == null || ImagePath.Length == 0) return false;

            string md5 = Tools.Checksum.CalculateMD5(ImagePath); // TODO : integrate config to md5

            // TODO manque dans le cache les infos suivantes :
            // Rotation
            // SizeMode
            // MarginColor
            // MarginTopBottom
            // MarginLeftRight

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
            using (var bmpPicture = new Bitmap(ImagePath))
            {
                Bitmap bmpPanel = new Bitmap(engine.Panel.Width, engine.Panel.Height);

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
                        double ratioW = ((double)engine.Panel.Width - (2 * (double)MarginLeftRight)) / ((double)bmpPicture.Width);
                        double ratioH = ((double)engine.Panel.Height - (2 * (double)MarginTopBottom)) / ((double)bmpPicture.Height);

                        // get the smaller ratio
                        double ratio = (ratioW < ratioH) ? ratioW : ratioH;

                        sz.Width  = Convert.ToInt32(ratio * bmpPicture.Width);
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
                Bitmap grayscalePicture = GrayScale.MakeGrayscale3(bmpPicture);

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
                    imgRect.X += Convert.ToInt32(((double)engine.Panel.Width - (2 * (double)MarginLeftRight)) / 2 - (double)sz.Width / 2.0);
                    imgRect.Y += Convert.ToInt32(((double)engine.Panel.Height - (2 * (double)MarginTopBottom)) / 2 - (double)sz.Height / 2.0);
                }

                //System.Windows.Forms.MessageBox.Show("Ratio=" + (double)imgRect.Width / (double)imgRect.Height);

                // 6. draw image
                gfx.DrawImage(grayscalePicture, imgRect);

                switch (DisplayMode)
                {
                    case DisplayMode.Direct:
                        // 7.1. invert image
                        Bitmap invertedImage = BitmapTools.GetInvertedBitmap(bmpPanel);

                        // TODO : apply here Film filter (standard gamma for film is 0.7)
                        Bitmap gammaImage = BitmapTools.GetBitmapWithGamma(invertedImage, 0.7);

                        Bitmap gradeImage = gammaImage;

                        // 8.1. push image to engine
                        engine.PushImage(gradeImage, Duration);
                        break;

                    case DisplayMode.GrayToTime:
                        // 7.2. get image layers
                        List<ImageLayer> ils = GetImageLayers(bmpPanel);

                        foreach (ImageLayer il in ils)
                        {
                            // 8.2. push image to engine
                            engine.PushImage(il);
                        }
                        break;
                }

                gfx.Dispose();
            }

            return true;
        }

        /// <summary>
        /// Unload function
        /// </summary>
        /// <returns></returns>
        public bool Unload()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();
            engine.Clear();

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

            int[] timings = (Curve == GrayToTimeCurve.PMuth) ? GrayToTime.TimingsPMUTH : GrayToTime.TimingsOVH;

#if TEST_PARALLEL2
            //Stopwatch stopwatch = Stopwatch.StartNew();

            Parallel.For(0, timings.Length, i =>
            { 
                Bitmap bitmap2 = (Bitmap) bitmap.Clone();
#else
            for (int i = 0; i < timings.Length; i++)
            {
#endif
                using (DirectBitmap b = new DirectBitmap(bitmap.Width, bitmap.Height))
                {
#if TEST_PARALLEL
                    Parallel.For(0, b.Width, x =>
#else
                    for (int x = 0; x < b.Width; x++)
#endif
                    {
#if TEST_PARALLEL
                        Parallel.For(0, b.Height, y =>
#else
                        for (int y = 0; y < b.Height; y++)
#endif
                        {
#if TEST_PARALLEL2
                            Color c = bitmap2.GetPixel(x, y);
#else
                            Color c = bitmap.GetPixel(x, y);
#endif
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
#if TEST_PARALLEL
);
#endif
                    }
#if TEST_PARALLEL
);
#endif
                    imageLayers.Add(new ImageLayer(b.Bitmap, timings[i], i));
                }
            }

#if TEST_PARALLEL2
            );

        //    stopwatch.Stop();

        //    Log.WriteLine("For : {0}", stopwatch.Elapsed.TotalMilliseconds);
#endif

            return imageLayers;
        }
    }
}
