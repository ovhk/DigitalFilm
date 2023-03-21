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

            int width = engine.Panel.Width;
            int height = engine.Panel.Height;
            
            SolidBrush brushBlack = new SolidBrush(Color.Black);
            SolidBrush brushWhite = new SolidBrush(Color.White);

            g.FillRectangle(brushBlack, 0, 0, width, height / 2);
            g.FillRectangle(brushWhite, 0, height/2, width, height / 2);

            for (int i = 0; i < 10; i++)
            {
                g.FillRectangle(brushWhite, (i + 1) * width / 10, height * 1 / 4, (i + 1) * 1, (i + 1) * 1);
            }

            for (int i = 0; i < 10; i++)
            {
                g.FillRectangle(brushBlack, (i + 1) * width / 10, height * 3 / 4, (i + 1) * 1, (i + 1) * 1);
            }
            
            for (int i = 0; i < width - 3; i += 3)
            {
                if (i % 2 != 0)
                {
                    g.FillRectangle(brushBlack, i, height - 23, 3, 3);
                    g.FillRectangle(brushWhite, i + 1, height - 23 + 1, 1, 1);
                }
                else
                {
                    g.FillRectangle(brushWhite, i, height - 23, 3, 3);
                    g.FillRectangle(brushBlack, i + 1, height - 23 + 1, 1, 1);
                }
            }

            for (int i = 0; i < width - 3; i += 3)
            {
                if (i % 2 == 0)
                {
                    g.FillRectangle(brushBlack, i, height - 20, 3, 3);
                    g.FillRectangle(brushWhite, i + 1, height - 20 + 1, 1, 1);
                }
                else
                {
                    g.FillRectangle(brushWhite, i, height - 20, 3, 3);
                    g.FillRectangle(brushBlack, i + 1, height - 20 + 1, 1, 1);
                }
            }

            for (int i = 0; i < width - 3; i += 3)
            {
                if (i % 2 != 0)
                {
                    g.FillRectangle(brushBlack, i, height - 17, 3, 3);
                    g.FillRectangle(brushWhite, i + 1, height - 17 + 1, 1, 1);
                }
                else
                {
                    g.FillRectangle(brushWhite, i, height - 17, 3, 3);
                    g.FillRectangle(brushBlack, i + 1, height - 17 + 1, 1, 1);
                }
            }

            // TODO : for test only
            b.Save(@"C:\Users\sectronic\Desktop\mode3.bmp");

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
