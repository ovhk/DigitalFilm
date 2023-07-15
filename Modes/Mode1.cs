using DigitalFilm.Engine;
using DigitalFilm.Tools;
using System.ComponentModel;
using System.Drawing;

namespace DigitalFilm.Modes
{
    internal class Mode1 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Black & White";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Just a black & White zone to check if the diplay panel is able to block light.";

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Display exposure time in second")]
        public int ExposureTime
        { get; set; } = 10;

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

                using (SolidBrush brush = new SolidBrush(Color.White)) // will be black on the paper
                {
                    gfx.FillRectangle(brush, 0, 0, engine.Panel.Width / 2, engine.Panel.Height);
                }

                using (SolidBrush brush = new SolidBrush(Color.Black)) // will be white on the paper
                {
                    gfx.FillRectangle(brush, engine.Panel.Width / 2, 0, engine.Panel.Width / 2, engine.Panel.Height);
                }
            }

            engine.PushImage(b, ExposureTime * 1000);

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
        bool IMode.Unload()
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
