using DigitalFilm.Engine;
using DigitalFilm.Tools;
using System.ComponentModel;
using System.Drawing;

namespace DigitalFilm.Modes
{
    internal class Mode6 : IMode
    {
        /// <summary>
        /// Name
        /// </summary>
        [Browsable(false)]
        public string Name => "GrayToTime calibration";

        /// <summary>
        /// Description
        /// </summary>
        [Browsable(false)]
        public string Description => "Generate a matrix of 5x4 square with 20 differents exposures from minTime to maxTime. This will permit de mesure the gray level with a colormeter and update the XLS file.";

        /// <summary>
        /// Access to the Engine
        /// </summary>
        private readonly DisplayEngine engine = DisplayEngine.GetInstance();

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Minimum time")]
        public int minTime
        { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Maximum time")]
        public int maxTime
        { get; set; } = 10000;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            int nbRows = 4;
            int nbCols = 5;

            int width = engine.Panel.Width;
            int height = engine.Panel.Height;
            int iWidth = width / nbCols;
            int iHeight = height / nbRows;

            // width / NbInterval not round so we adjust...
            width = iWidth * nbCols;
            height = iHeight * nbRows;

            Bitmap b = new Bitmap(width, height);

            Graphics gfx = Graphics.FromImage(b);

            // First, erase all
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                gfx.FillRectangle(brush, 0, 0, engine.Panel.Width, engine.Panel.Height);
            }

            // TODO : faire un cadrillage de 4 x 5 avec des lignes blanches franches pour délimiter


            int interval = (maxTime - minTime) / (nbRows * nbCols);

            int init = minTime + interval;

            int[,] matrix = new int[nbRows, nbCols];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = init;
                    init += interval;
                }
            }

            for (int r = 0; r < matrix.GetLength(0); r++)
            {
                for (int c = 0; c < matrix.GetLength(1); c++)
                {
                    // First, erase all
                    using (SolidBrush brush = new SolidBrush(Color.Black))
                    {
                        gfx.FillRectangle(brush, 0, 0, engine.Panel.Width, engine.Panel.Height);
                    }

                    using (SolidBrush brush = new SolidBrush(Color.White))
                    {
                        gfx.FillRectangle(brush, c * iWidth, r * iHeight, iWidth, iHeight);
                    }

                    engine.PushImage(new Bitmap(b), matrix[r, c]);
                }
            }

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
