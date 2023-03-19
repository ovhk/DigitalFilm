using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Modes
{
    internal class Mode4 : IMode
    {
        public string Name => "Mode 4";

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

            Bitmap b = new Bitmap(engine.Width, engine.Height);

            int width = engine.Width;
            int height = engine.Height;

            using (Graphics gfx = Graphics.FromImage(b))
            {
                for (int i = 0; i < engine.NumberOfColors; i++)
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(engine.NumberOfColors - 1 - i, engine.NumberOfColors - 1 - i, engine.NumberOfColors - 1 - i)))
                    {
                        gfx.FillRectangle(brush, i * (width / engine.NumberOfColors), 0, width / engine.NumberOfColors, height / 2);
                    }
                }

                SolidBrush brushBlack = new SolidBrush(Color.Black);
                SolidBrush brushWhite = new SolidBrush(Color.White);

                int size = engine.NumberOfColors;

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        SolidBrush brush = (j > i) ? brushBlack : brushWhite;

                        gfx.FillRectangle(brush, j * (width / size), height / 2, width / size, height / 2);
                    }
                    engine.PushImage(new Bitmap(b), (duration / 256));
                }
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
    }
}
