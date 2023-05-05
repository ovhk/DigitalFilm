using DigitalFilm.Engine;
using DigitalFilm.Tools;
using System.ComponentModel;
using System.Drawing;

namespace DigitalFilm.Modes
{
    internal class Mode8 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Test Band";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Generate a test band following parameters";

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Number of interval")]
        public int NbInterval
        { get; set; } = 10;

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Interval exposure time in second")]
        public int IntervalExposureTime
        { get; set; } = 2;

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
            int width = engine.Panel.Width;
            int height = engine.Panel.Height;
            int iWidth = (width / NbInterval);

            // width / NbInterval not round so we adjust...
            width = iWidth * NbInterval;

            Bitmap b = new Bitmap(width, height);

            Graphics gfx = Graphics.FromImage(b);

            // First, erase all
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                gfx.FillRectangle(brush, 0, 0, engine.Panel.Width, engine.Panel.Height);
            }

            SolidBrush brushBlack = new SolidBrush(Color.Black);
            SolidBrush brushWhite = new SolidBrush(Color.White);

            for (int i = 0; i < NbInterval; i++)
            {
                for (int j = 0; j < NbInterval; j++)
                {
                    SolidBrush brush = (j > i) ? brushBlack : brushWhite;
                    SolidBrush brushTxt = (j > i) ? brushWhite : brushBlack;

                    gfx.FillRectangle(brush, j * iWidth, 0, iWidth, height);

                    // TODO formule compliqué sans doute pour rien...
                    string str = (NbInterval * IntervalExposureTime - (j) * IntervalExposureTime).ToString();

                    DrawTools.DrawLargestString(ref gfx, ref brushTxt, str, new Rectangle(j * iWidth, height / 2, iWidth, height / 2));
                }
                engine.PushImage(new Bitmap(b), IntervalExposureTime * 1000);
            }

            gfx.Dispose();

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
