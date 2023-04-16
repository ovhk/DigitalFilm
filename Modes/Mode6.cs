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
using DigitalDarkroom.Controls;
using DigitalDarkroom.Tools;
using DigitalDarkroom.Engine;

namespace DigitalDarkroom.Modes
{
    internal class Mode6 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "GrayToTime picture";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Display the selected picture in 256 B&W pictures with GrayToTime algorithm.";

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Source file to display")]
        [Editor(typeof(ImageFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ImagePath
        { get; set; }

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

#if USE_CACHE
            string md5 = Tools.Checksum.CalculateMD5(ImagePath);

            if (engine.IsInCache(md5) == true)
            {
                engine.LoadCache(md5);
            }
            else
            {
                engine.SetCacheIdentifier(md5);
#endif
                // this way permit to not lock the file : https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
                using (var bmpTemp = new Bitmap(ImagePath))
                {
                    Size sz = new Size(engine.Panel.Width, engine.Panel.Height);
                    Bitmap origin = GrayScale.MakeGrayscale3(new Bitmap(bmpTemp, sz));

                    List<ImageLayer> ils = GetImageLayers(origin, engine.Panel.Width, engine.Panel.Height);

                    foreach (ImageLayer il in ils)
                    {
                        engine.PushImage(il);
                    }
                }
#if USE_CACHE
            }
#endif
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
        public static List<ImageLayer> GetImageLayers(Bitmap bitmap, int width, int height)
        {
            List<ImageLayer> imageLayers = new List<ImageLayer>(); // TODO : move all this into mode 6 but mode 2 also use timings???

            int[] timings = GrayToTime.Timings;

#if TEST_PARALELLE
            //Stopwatch stopwatch = Stopwatch.StartNew();

            Parallel.For(0, timings.Length, i =>
#else
            for (int i = 0; i < timings.Length; i++)
#endif
            {
                using (DirectBitmap b = new DirectBitmap(width, height))
                {
#if TEST_PARALELLE
                    Parallel.For(0, b.Width, x =>
#else
                    for (int x = 0; x < b.Width; x++)
#endif
                    {
#if TEST_PARALELLE
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
#if TEST_PARALELLE
);
#endif
                    }
#if TEST_PARALELLE
);
#endif
                    imageLayers.Add(new ImageLayer(b.Bitmap, timings[i], i));
                }
            }

#if TEST_PARALELLE
            );

        //    stopwatch.Stop();

        ////For: 8727,5777
        //    Log.WriteLine("For : {0}", stopwatch.Elapsed.TotalMilliseconds);
#endif
            return imageLayers;
        }
    }
}
