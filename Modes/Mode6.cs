using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DigitalFilm.Controls;
using DigitalFilm.Tools;
using DigitalFilm.Engine;

namespace DigitalFilm.Modes
{
    internal class Mode6 : IMode
    {
        /// <summary>
        /// Name
        /// </summary>
        [Browsable(false)]
        public string Name => "GrayToTime picture";

        /// <summary>
        /// Description
        /// </summary>
        [Browsable(false)]
        public string Description => "Display the selected picture in 256 B&W pictures with GrayToTime algorithm.";

        /// <summary>
        /// Source file to display
        /// </summary>
        [Category("Configuration")]
        [Description("Source file to display")]
        [Editor(typeof(ImageFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ImagePath
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Category("Cache")]
        [Description("Use cache to load faster?")]
        public bool UseCache
        { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Display mode")]
        public SizeMode DisplayMode
        { get; set; } = SizeMode.StretchImage;

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
        /// Access to the Engine
        /// </summary>
        private readonly DisplayEngine engine = DisplayEngine.GetInstance();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ImagePath == null || ImagePath.Length == 0) return false;

            string md5 = Tools.Checksum.CalculateMD5(ImagePath);

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

            // this way permit to not lock the file : https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
            using (var bmpPicture = new Bitmap(ImagePath))
            {
                bmpPicture.RotateFlip(Rotation);

                Size sz = new Size();

                switch (DisplayMode)
                {
                    case SizeMode.CenterImage:
                        // TODO
                        break;

                    case SizeMode.StretchImage:
                    default:
                        sz.Width = engine.Panel.Width;
                        sz.Height = engine.Panel.Height;
                        break;
                }

                Bitmap origin = GrayScale.MakeGrayscale3(new Bitmap(bmpPicture, sz));

                List<ImageLayer> ils = GetImageLayers(origin, engine.Panel.Width, engine.Panel.Height);

                foreach (ImageLayer il in ils)
                {
                    engine.PushImage(il);
                }


            }

            return true;
        }

        /// <summary>
        /// 
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
        public List<ImageLayer> GetImageLayers(Bitmap bitmap, int width, int height)
        {
            List<ImageLayer> imageLayers = new List<ImageLayer>();

            int[] timings = GrayToTime.TimingsPMUTH;

            SolidBrush marginBrush = (MarginColor == MarginColor.Back) ? new SolidBrush(Color.White) : new SolidBrush(Color.Black); // invert color for film

#if TEST_PARALLEL
            //Stopwatch stopwatch = Stopwatch.StartNew();

            Parallel.For(0, timings.Length, i =>
#else
            for (int i = 0; i < timings.Length; i++)
#endif
            {
                using (DirectBitmap b = new DirectBitmap(width, height))
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
                            Color c = bitmap.GetPixel(x, y); ;

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
                    Graphics gfx = Graphics.FromImage(b.Bitmap);

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

                    imageLayers.Add(new ImageLayer(b.Bitmap, timings[i], i));
                }
            }

#if TEST_PARALLEL
            );

        //    stopwatch.Stop();

        ////For: 8727,5777
        //    Log.WriteLine("For : {0}", stopwatch.Elapsed.TotalMilliseconds);
#endif

            marginBrush.Dispose();

            return imageLayers;
        }
    }
}
