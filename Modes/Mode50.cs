using DigitalFilm.Controls;
using DigitalFilm.Engine;
using DigitalFilm.Papers;
using DigitalFilm.Tools;
using DigitalFilm.FFmpegWrapper;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace DigitalFilm.Modes
{
    internal class Mode50 : IMode
    {
        /// <summary>
        /// Name
        /// </summary>
        [Browsable(false)]
        public string Name => "Display picture (RAW)";

        /// <summary>
        /// Description
        /// </summary>
        [Browsable(false)]
        public string Description => "Display the selected RAW picture following parameters.";

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
        [Description("Margin size.")]
        public int MarginWidth
        { get; set; } = 10;

        #endregion

        /// <summary>
        /// Rotation
        /// </summary>
        [Category("Configuration")]
        [Description("Rotation")]
        public double Rotation
        { get; set; } = 0.0;

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
        [Description("Maximum number of mask to generate")]
        public int MaxLayers
        { get; set; } = 512;

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
        [Category("Mode GrayToTime")]
        [Description("Select GrayToTime curve")]
        public string Formula
        { get; set; } = "(int)(0.0001 * Math.Pow(x, 4) - 0.0722 * Math.Pow(x, 3) + 17.586 * Math.Pow(x, 2) - 1890.5 * x + 89686.0)";

        /// <summary>
        /// Access to the Engine
        /// </summary>
        private readonly DisplayEngine engine = DisplayEngine.GetInstance();

        /// <summary>
        /// Constructor
        /// </summary>
        public Mode50()
        {
            Paper = PapersManager.Papers[2];
        }

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

            if (Paper == null)
            {
                return false;
            }

            string md5 = Tools.Checksum.CalculateMD5(ImagePath);

            md5 += "-" + (Rotation.GetHashCode()
                + 22 * MarginColor.GetHashCode()
                + 333 * MarginWidth.GetHashCode()
                + 4444 * MaxLayers.GetHashCode()
                ).ToString(); // Just a way to have an unique id value with parameters, not perfect but seems enought!

            if (DisplayMode == DisplayMode.GrayToTime)
            {
                if (this.UseCache) // Use Cache ?
                {
                    if (engine.Cache.IsInCache(md5) == true) // If it's in cache
                    {
                        return engine.Cache.LoadFromCache(md5); // load from cache
                    }
                    else
                    {
                        engine.Cache.SetCacheIdentifier(md5); // else define a cache identifier
                    }
                }
                else
                {
                    engine.Cache.ClearCacheFromIdentifier(md5); // if we're not using the cache, then clear old cache
                }
            }

            // this way permit to not lock the file : https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
            using (MagickImage magickImage = new MagickImage(ImagePath))
            {
                using (MagickImage magickPanel = new MagickImage(MagickColors.White, engine.Panel.Width, engine.Panel.Height))
                {
                    System.Windows.Forms.MessageBox.Show("Original TotalColors = " + magickImage.TotalColors);
                    
                    // rotate
                    magickImage.Rotate(Rotation);

                    // 1. erase all
                    // done at the constructor
                    magickPanel.Grayscale();
                    
                    // 2. convert picture to grayscale if needed
                    if (magickImage.ColorSpace != ColorSpace.Gray)
                    {
                        magickImage.Grayscale(PixelIntensityMethod.Average); // TODO : parameter for PixelIntensityMethod?
                    }

                    // 3. Draw margins
                    MagickColor mgkMarginColor = (MarginColor == MarginColor.Back) ? MagickColors.Black : MagickColors.White;
                    
                    magickPanel.Draw(new Drawables().FillColor(mgkMarginColor).Rectangle(0, 0, engine.Panel.Width, engine.Panel.Height));

                    // 4. determine size ratio
                    // Ratio is : canavas / original
                    double ratioW = (engine.Panel.Width - (2.0 * (double)MarginWidth)) / magickImage.Width;
                    double ratioH = (engine.Panel.Height - (2.0 * (double)MarginWidth)) / magickImage.Height;

                    // get the smaller ratio
                    double ratio = (ratioW < ratioH) ? ratioW : ratioH;

                    // 5. resize image
                    Rectangle imgRect = new Rectangle
                    {
                        X = MarginWidth,
                        Y = MarginWidth,
                        Width = Convert.ToInt32(ratio * magickImage.Width), // margin already inside
                        Height = Convert.ToInt32(ratio * magickImage.Height)
                    };

                    // Add offset to center image
                    imgRect.X += Convert.ToInt32(((engine.Panel.Width - (2.0 * (double)MarginWidth)) / 2.0) - (imgRect.Width / 2.0));
                    imgRect.Y += Convert.ToInt32(((engine.Panel.Height - (2.0 * (double)MarginWidth)) / 2.0) - (imgRect.Height / 2.0));

                    MagickGeometry magickGeometry = new MagickGeometry
                    {
                        Width = imgRect.Width,
                        Height = imgRect.Height,
                        IgnoreAspectRatio = false, // do not stretch image
                    };

                    //magickImage.Resize(magickGeometry); // explode the number of grey levels
                    //magickImage.AdaptiveResize(magickGeometry); // explode the number of grey levels
                    magickImage.InterpolativeResize(magickGeometry, PixelInterpolateMethod.Nearest); // not explode to much the number of grey levels, but... 

                    // Debug only: Check ratio
                    //System.Windows.Forms.MessageBox.Show("Ratio=" + (double)imgRect.Width / (double)imgRect.Height);

                    // 6. Draw image
                    magickPanel.Composite(magickImage, imgRect.X, imgRect.Y, CompositeOperator.Over);

                    // 8. Reduce number of colors
                    // https://www.imagemagick.org/discourse-server/viewtopic.php?t=12599
                    // http://www.imagemagick.org/Usage/quantize/#extract

                    //  ???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????,

                    QuantizeSettings quantizeSettings = new QuantizeSettings
                    {
                        Colors = 2048,
                        DitherMethod = DitherMethod.No,
                    };

                    if (magickImage.Format != MagickFormat.Dng) // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! WHY ???????????????????????????
                    {
                        magickPanel.Quantize(quantizeSettings); // HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    }

                    ///////// on a beau avoir plein de couleurs... au delas de 255, c'est vide...

                    System.Windows.Forms.MessageBox.Show("magickPanel.TotalColors = " + magickPanel.TotalColors);

                    //  ???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????,

                    //engine.PushImage((Bitmap)magickPanel.ToBitmap().Clone(), ExposureTime); // for debug only

                    switch (DisplayMode)
                    {
                        case DisplayMode.Direct:
                            {
                                // 7.1. convert image for selected paper
                                Bitmap imageForPaper = BitmapTools.BitmapToPaper(magickPanel.ToBitmap(), this.Paper);

                                // 8.1. push image to engine
                                engine.PushImage(imageForPaper, this.ExposureTime);
                            }
                            break;

                        case DisplayMode.DirectPaperGamma:
                            {
                                // 7.2.1 apply the Gamma from the selected paper
                                magickPanel.GammaCorrect(1.0 / this.Paper.Gamma); // invert because film and paper are opposite

                                // 7.2.2 invert black and white
                                magickPanel.ColorType = ColorType.Grayscale; // Negate() work only with this...
                                magickPanel.Negate(); // magickPanel.NegateGrayscale(); ?

                                // 8.2. push image to engine
                                engine.PushImage(magickPanel.ToBitmap(), this.ExposureTime);
                            }
                            break;

                        case DisplayMode.DirectAllGrade:
                            {
                                // 7.3. convert image with all grade of selected paper
                                Bitmap imageForPaper = BitmapTools.BitmapToPapers(magickPanel.ToBitmap());

                                // 8.3. push image to engine
                                engine.PushImage(imageForPaper, this.ExposureTime);
                            }
                            break;

                        case DisplayMode.GrayToTime:
                            {
                                // 7.4. get image layers
                                List<ImageLayer> ils = GrayToTime.GetImageLayers(magickPanel, this.Curve, this.Formula, this.MaxLayers);

                                if (ils == null)
                                {
                                    return false;
                                }

                                // TODO TEST FFMPEG

                                // TODO : here select engine : ImageEngine / VideoEngine

                                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\cache\tmp";

                                FFconcat.Generate(path, ils);
                                FFmpeg.Execute(path);
                                
                                // TODO END TEST FFMPEG

                                foreach (ImageLayer il in ils)
                                {
                                    // 8.4. push image to engine
                                    engine.PushImage(il);
                                }
                            }
                            break;
                    }
                }
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
                case Modes.DisplayMode.GrayToTime:
                    imageToDisplay = BitmapTools.GetInvertedBitmap(bitmap);
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
    }
}
