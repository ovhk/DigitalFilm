using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Modes
{
    internal class Mode1 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name => "Black && White";

        public string Description => "Just a black & White zone to check if the diplay panel is able to block light.";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IMode.Load(string[] imgPaths, int duration)
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            Bitmap b = new Bitmap(engine.Panel.Width, engine.Panel.Height);

            using (Graphics gfx = Graphics.FromImage(b))
            {

                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    gfx.FillRectangle(brush, 0, 0, engine.Panel.Width / 2, engine.Panel.Height);
                }

                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    gfx.FillRectangle(brush, engine.Panel.Width / 2, 0, engine.Panel.Width / 2, engine.Panel.Height);
                }
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
