using DigitalFilm.Engine;
using System.ComponentModel;
using System.Drawing;

namespace DigitalFilm.Modes
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
        [Description("Exposure time in second")]
        public int ExposureTime
        { get; set; } = 30;

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Number of square to display")]
        public int NbSquare
        { get; set; } = 20;

        /// <summary>
        /// Access to the Engine
        /// </summary>
        private readonly DisplayEngine engine = DisplayEngine.GetInstance();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IMode.Load()
        {
            Bitmap b = new Bitmap(engine.Panel.Width, engine.Panel.Height);

            using (Graphics gfx = Graphics.FromImage(b))
            {
                // First, erase all
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    gfx.FillRectangle(brush, 0, 0, engine.Panel.Width, engine.Panel.Height);
                }

                int width = engine.Panel.Width;
                int height = engine.Panel.Height;

                SolidBrush brushBlack = new SolidBrush(Color.Black);
                SolidBrush brushWhite = new SolidBrush(Color.White);

                gfx.FillRectangle(brushBlack, 0, 0, width, height / 4);
                gfx.FillRectangle(brushWhite, 0, height / 4, width, height / 4);

                int iter = NbSquare;

                for (int i = 0; i < iter; i++)
                {
                    gfx.FillRectangle(brushWhite, (i + 1) * width / iter, height * 1 / 8, (i + 1) * 1, (i + 1) * 1);
                }

                for (int i = 0; i < iter; i++)
                {
                    gfx.FillRectangle(brushBlack, (i + 1) * width / iter, height * 3 / 8, (i + 1) * 1, (i + 1) * 1);
                }

                for (int a = 0; a < 80 * 3; a += 3)
                {
                    for (int i = 0; i < width - 3; i += 3)
                    {
                        if (i % 2 == 0)
                        {
                            gfx.FillRectangle(brushBlack, i, height - a, 3, 3);
                            gfx.FillRectangle(brushWhite, i + 1, height - a + 1, 1, 1);
                        }
                        else
                        {
                            gfx.FillRectangle(brushWhite, i, height - a, 3, 3);
                            gfx.FillRectangle(brushBlack, i + 1, height - a + 1, 1, 1);
                        }
                    }
                }

                engine.PushImage(b, ExposureTime * 1000);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Unload()
        {
            this.engine.Clear();

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
