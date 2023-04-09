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
        public string Name => "Grayscale picture";

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

            Bitmap b;

            // this way permit to not lock the file : https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
            using (var bmpTemp = new Bitmap(imgPaths[0]))
            {
                b = GrayScale.MakeGrayscale3(new Bitmap(bmpTemp, sz));
            }

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
