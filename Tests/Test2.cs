using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Tests
{
    internal class Test2
    {
        public static void Load()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();
            GenerateMasquesTemps(engine.Width, engine.Height);
        }

        private static void GenerateMasquesTemps(int width, int height)
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            Bitmap b = new Bitmap(width, height);

            using (Graphics gfx = Graphics.FromImage(b))
            {
                for (int i = 0; i < 256; i++)
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(i, i, i)))
                    {
                        gfx.FillRectangle(brush, i * (width / 256), 0, width / 256, height / 2);
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

                        SizeF stringSize = new SizeF();
                        stringSize = gfx.MeasureString(str, SystemFonts.DefaultFont);
                        int offset = size / 2 - (int)stringSize.Width / 2;

                        gfx.DrawString(str, SystemFonts.DefaultFont, brushTxt, j * (width / size) + offset, height / 2 + 10);
                    }
                    engine.PushImage(new Bitmap(b), 500);
                }
            }
        }

    }
}
