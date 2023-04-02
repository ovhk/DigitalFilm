using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalDarkroom.Tools;

namespace DigitalDarkroom.Modes
{
    internal class Mode5 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name => "Mode 5";

        public string Description => "Display the selected picture in grayscale.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public bool Load(string[] imgPaths, int duration)
        {
            if (imgPaths == null || imgPaths.Length == 0) return false;

            DisplayEngine engine = DisplayEngine.GetInstance();

            Size sz = new Size(engine.Panel.Width, engine.Panel.Height);

            //@"C:\Users\sectronic\source\repos\DigitalDarkroom\img\F1000015.jpg"

            Image img = Image.FromFile(imgPaths[0]);

            Bitmap b = GrayScale.MakeGrayscale3(new Bitmap(img, sz));

            engine.PushImage(b, duration);

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
    }
}
