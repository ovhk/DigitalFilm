using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Modes
{
    internal class Mode2 : IMode // TODO : needed anymore? We have mode 4!
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Gray vs B&W 500ms";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Generate gray scale and linear B&W scale with fixed 500ms interval.";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IMode.Load()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            int width = engine.Panel.Width;
            int height = engine.Panel.Height;
            int iWidth = (width / engine.Panel.NumberOfColors);

            // width / engine.Panel.NumberOfColors not round so we adjust...
            width = iWidth * engine.Panel.NumberOfColors;

            Bitmap b = new Bitmap(width, height);

            using (Graphics gfx = Graphics.FromImage(b))
            {
                // Generate gray levels
                for (int i = 0; i < engine.Panel.NumberOfColors; i++)
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(engine.Panel.NumberOfColors - 1 - i, engine.Panel.NumberOfColors - 1 - i, engine.Panel.NumberOfColors - 1 - i)))
                    {
                        gfx.FillRectangle(brush, i * iWidth, 0, iWidth, height / 2);
                    }
                }

                SolidBrush brushBlack = new SolidBrush(Color.Black);
                SolidBrush brushWhite = new SolidBrush(Color.White);

                int size = 40;

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        SolidBrush brush = (j > i) ? brushBlack : brushWhite;
                        SolidBrush brushTxt = (j > i) ? brushWhite : brushBlack;

                        gfx.FillRectangle(brush, j * (width / size), height / 2, width / size, height / 2);

                        string str = (j + 1).ToString();

                        // TODO adjust font following space available in (width / size)

                        SizeF stringSize = new SizeF();
                        stringSize = gfx.MeasureString(str, SystemFonts.DefaultFont);
                        int offset = size / 2 - (int)stringSize.Width / 2;

                        gfx.DrawString(str, SystemFonts.DefaultFont, brushTxt, j * (width / size) + offset, height / 2 + 10);
                    }
                    engine.PushImage(new Bitmap(b), 500);
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
