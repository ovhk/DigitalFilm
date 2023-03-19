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

            Size sz = new Size(engine.Width, engine.Height);

            Image img = Image.FromFile(@"C:\Users\sectronic\source\repos\DigitalDarkroom\img\F1000015.jpg");

            Bitmap b = ImageTools.MakeGrayscale3(new Bitmap(img, sz));

            engine.PushImage(b, duration);

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
    }
}
