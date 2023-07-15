using DigitalFilm.Engine;
using DigitalFilm.Tools;
using System.ComponentModel;
using System.Drawing;

namespace DigitalFilm.Modes
{
    internal class Mode2 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Find exposure time for a gray"; // TODO : utile ? C'est compliqué de comparer les gris visuellement...

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Find the exposure time for a specific gray in parameter.";

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Gray value from 0 to 255")]
        public int GrayValue
        { get; set; } = 100;

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Number of interval to display")]
        public int NbInterval
        { get; set; } = 30;

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Display exposure time in ms")]
        public int IntervalExposureTime
        { get; set; } = 2000;

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Initial exposure time in ms")]
        public int InitialExposureTime
        { get; set; } = 0;

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
            int iWidth = width / NbInterval;

            // width / NbInterval not round so we adjust...
            width = iWidth * NbInterval;

            Bitmap b = new Bitmap(width, height);

            Graphics gfx = Graphics.FromImage(b);

            // First, erase all
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                gfx.FillRectangle(brush, 0, 0, engine.Panel.Width, engine.Panel.Height);
            }

            // Generate gray levels
            using (SolidBrush brush = new SolidBrush(ColorTools.GetInvertedColor(GrayValue)))
            {
                gfx.FillRectangle(brush, 0, 0, width, height / 2);
            }

            SolidBrush brushBlack = new SolidBrush(Color.Black);
            SolidBrush brushWhite = new SolidBrush(Color.White);

            SolidBrush brushTxt = (GrayValue < 256 / 2) ? brushBlack : brushWhite;

            DrawTools.DrawLargestString(ref gfx, ref brushTxt, "GRAY : " + GrayValue, new Rectangle(0, 0, width, height / 2));

            if (InitialExposureTime > 0)
            {
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    gfx.FillRectangle(brush, 0, height / 2, width, height / 2);
                }

                engine.PushImage(new Bitmap(b), InitialExposureTime);

                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    gfx.FillRectangle(brush, 0, height / 2, width, height / 2);
                }
            }

            for (int i = 0; i < NbInterval; i++)
            {
                for (int j = 0; j < NbInterval; j++)
                {
                    SolidBrush brush = (j > i) ? brushBlack : brushWhite;
                    brushTxt = (j > i) ? brushWhite : brushBlack;

                    gfx.FillRectangle(brush, j * iWidth, height / 2, iWidth, height / 2);

                    // TODO formule compliqué sans doute pour rien...
                    string str = (InitialExposureTime + NbInterval * IntervalExposureTime - j * IntervalExposureTime).ToString();

                    DrawTools.DrawLargestString(ref gfx, ref brushTxt, str, new Rectangle(j * iWidth, height / 2, iWidth, height / 2));
                }
                engine.PushImage(new Bitmap(b), IntervalExposureTime);
            }

            gfx.Dispose();

            return true;
        }

        /// <summary>
        /// Get Image preview
        /// </summary>
        /// <param name="bitmap">displayed image</param>
        /// <returns>preview image</returns>
        public Bitmap GetPrewiew(Bitmap bitmap)
        {
            // Try to display with a default correction : Gamma = 1.4 (standard for paper)

            // invert pixel
            Bitmap invertedImage = BitmapTools.GetInvertedBitmap(bitmap);

            // Apply paper gamma
            return BitmapTools.GetBitmapWithGamma(invertedImage, 1.4);
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
