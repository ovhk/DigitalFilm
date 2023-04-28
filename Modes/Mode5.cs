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
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Grayscale picture";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Display the selected picture in grayscale.";

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Display duration in ms")]
        public int Duration
        { get; set; } = 5000;

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

            Size sz = new Size(engine.Panel.Width, engine.Panel.Height);

            Bitmap b;

            // this way permit to not lock the file : https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
            using (var bmpTemp = new Bitmap(ImagePath))
            {
                b = GrayScale.MakeGrayscale3(new Bitmap(bmpTemp, sz));

                // invert B&W !!!!
                using (DirectBitmap bbw = new DirectBitmap(sz.Width, sz.Height))
                {
                    for (int x = 0; x < b.Width; x++)
                    {
                        for (int y = 0; y < b.Height; y++)
                        {
                            Color c = b.GetPixel(x, y); ;

                            // invert
                            bbw.SetPixel(x, y, Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B));
                        }
                    }
                    engine.PushImage(bbw.Bitmap, Duration);
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
    }
}
