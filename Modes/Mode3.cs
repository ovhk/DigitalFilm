using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Modes
{
    internal class Mode3 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name => "Mode 3";

        public string Description => "This mode draw mosaic to dertermin contrast capacity.";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IMode.Load(string[] imgPaths, int duration)
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            Bitmap b = new Bitmap(engine.Panel.Width, engine.Panel.Height);

            Graphics g = Graphics.FromImage(b);

            // TODO draw MOSAIC

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
