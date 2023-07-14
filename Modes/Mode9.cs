using DigitalFilm.Engine;
using DigitalFilm.Tools;
using System.ComponentModel;
using System.Drawing;

namespace DigitalFilm.Modes
{
    internal class Mode9 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Gray palette";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Generate a gray palette";

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Exposure time")]
        public int ExposureTime
        { get; set; } = 30;

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Gamma correction")]
        public double Gamma
        { get; set; } = 1;

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
            int squareW = 16;
            int squareH = 16;

            int width = engine.Panel.Width;
            int height = engine.Panel.Height;
            int iWidth = width / squareW;
            int iHeight = height / squareH;

            // width / squareW not round so we adjust...
            width = iWidth * squareW;
            height = iHeight * squareH;

            using (Bitmap b = new Bitmap(width, height))
            {
                Graphics gfx = Graphics.FromImage(b);

                // First, erase all
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    gfx.FillRectangle(brush, 0, 0, engine.Panel.Width, engine.Panel.Height);
                }

                int color = 0;

                for (int col = 0; col < squareW; col++)
                {
                    for (int row = 0; row < squareH; row++, color++)
                    {
                        // Gamma correction
                        int filteredColor = ColorTools.GetColorWithGamma(color, this.Gamma);

                        Color newColor = ColorTools.GetInvertedColor(filteredColor);

                        using (SolidBrush brush = new SolidBrush(newColor))
                        {
                            Rectangle r = new Rectangle(col * iWidth, row * iHeight, iWidth, iHeight);

                            gfx.FillRectangle(brush, r);

                            SolidBrush brushTxt = new SolidBrush(Color.FromArgb(color, color, color));

                            DrawTools.DrawLargestString(ref gfx, ref brushTxt, color.ToString(), r, 0.3);
                        }
                    }
                }

                engine.PushImage(new Bitmap(b), ExposureTime * 1000);

                gfx.Dispose();
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
