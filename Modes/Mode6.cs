using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalDarkroom.Tools;

namespace DigitalDarkroom.Modes
{
    internal class Mode6 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name => "Mode 6";

        public string Description => "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Load(string[] imgPaths, int duration)
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            // TODO execute in task
            // TODO use path

            computeTimings();

            Size sz = new Size(engine.Width, engine.Height);

            Image img = Image.FromFile(@"C:\Users\sectronic\source\repos\DigitalDarkroom\img\F1000015.jpg");

            //Bitmap origin = new Bitmap(img, sz);
            Bitmap origin = ImageTools.MakeGrayscale3(new Bitmap(img, sz));

            for (int i = 0; i < stepsImages.Length; i++)
            {
                DirectBitmap b = new DirectBitmap(engine.Width, engine.Height);

                for (int x = 0; x < b.Width; x++)
                {
                    for (int y = 0; y < b.Height; y++)
                    {
                        Color c = origin.GetPixel(x, y);
                        if (c.R < i)
                        {
                            b.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            b.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        }
                    }
                }

                engine.PushImage(b.Bitmap, timings[i]);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Unload()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();
            engine.Clear();

            return true;
        }

        private int[] timings = new int[256];
        private Bitmap[] stepsImages = new Bitmap[256];
        //private Bitmap stopImage;

        // This is Pierre MUTH algo : https://pierremuth.wordpress.com/2020/04/18/digital-picture-to-analog-darkroom-print/ 

        private void computeTimings()
        {
            int cumulatedTimeMs = 0;
            for (int i = 5; i < 256; i++)
            {
                cumulatedTimeMs = (int)((-17.06 * Math.Log((double)i) + 96.417) * 1000);
                //System.out.println("for gray " + i + ", exposure " + cumulatedTimeMs);
                timings[i] = cumulatedTimeMs;
            }

            for (int i = 5; i < timings.Length - 1; i++)
            {
                timings[i] = timings[i] - timings[i + 1];
                if (timings[i] < 80) timings[i] = 80;
                //System.out.println("for gray " + i + ", time " + timings[i]);
            }

            for (int i = 0; i < 5; i++)
            {
                timings[i] = timings[5] + 100;
            }

            timings[255] = 800;

        }
    }
}
