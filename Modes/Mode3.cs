using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Browsable(false)]
        public string Name => "Test contrast";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "This mode draw mosaic to dertermine contrast capacity.";

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Display duration in second")]
        public int Duration
        { get; set; } = 30;

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Number of square to display")]
        public int NbSquare
        { get; set; } = 20;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IMode.Load()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            Bitmap b = new Bitmap(engine.Panel.Width, engine.Panel.Height);

            Graphics g = Graphics.FromImage(b);

            int width = engine.Panel.Width;
            int height = engine.Panel.Height;
            
            SolidBrush brushBlack = new SolidBrush(Color.Black);
            SolidBrush brushWhite = new SolidBrush(Color.White);

            g.FillRectangle(brushBlack, 0, 0, width, height / 4);
            g.FillRectangle(brushWhite, 0, height/4, width, height / 4);

            int iter = NbSquare;

            for (int i = 0; i < iter; i++)
            {
                g.FillRectangle(brushWhite, (i + 1) * width / iter, height * 1 / 8, (i + 1) * 1, (i + 1) * 1);
            }

            for (int i = 0; i < iter; i++)
            {
                g.FillRectangle(brushBlack, (i + 1) * width / iter, height * 3 / 8, (i + 1) * 1, (i + 1) * 1);
            }

            for (int a = 0; a < 80 * 3; a += 3)
            {
                for (int i = 0; i < width - 3; i += 3)
                {
                    if (i % 2 == 0)
                    {
                        g.FillRectangle(brushBlack, i, height - a, 3, 3);
                        g.FillRectangle(brushWhite, i + 1, height - a + 1, 1, 1);
                    }
                    else
                    {
                        g.FillRectangle(brushWhite, i, height - a, 3, 3);
                        g.FillRectangle(brushBlack, i + 1, height - a + 1, 1, 1);
                    }
                }
            }

            engine.PushImage(b, Duration * 1000);

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
