using DigitalFilm.Controls;
using DigitalFilm.Engine;
using DigitalFilm.Tools;
using System.ComponentModel;
using System.Drawing;

namespace DigitalFilm.Modes
{
    internal class Mode10 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Display serie of masks";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Display a serie of mask.";

        /// <summary>
        /// Mask files to display
        /// </summary>
        [Category("Configuration")]
        [Description("Masks files to display")]
        [Editor(typeof(ImageFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string MaskPath
        { get; set; }

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
            return false; // keep safe during dev...

            string[] files = new string[1]; // MaskPath : is it a list of file or a directory ???

            foreach (var file in files)
            {
                int exposure = 100; // TODO : extract from filename ???

                engine.PushImage((Bitmap) Image.FromFile(file), exposure);
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
            return BitmapTools.GetInvertedBitmap(bitmap);
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
