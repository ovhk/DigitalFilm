using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom
{
    /// <summary>
    /// 
    /// </summary>
    internal class ImageLayer : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private Bitmap bmp;

        /// <summary>
        /// 
        /// </summary>
        private Image thumbnail;

        /// <summary>
        /// 
        /// </summary>
        private int expositionDuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="expositionDuration"></param>
        public ImageLayer (Bitmap bmp, int expositionDuration)
        {
            Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

            this.bmp = bmp;
            this.thumbnail = bmp.GetThumbnailImage(100, 100, callback, new IntPtr());
            this.expositionDuration = expositionDuration;
        }

        private bool ThumbnailCallback()
        {
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Image GetThumbnail()
        {
            return this.thumbnail;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Bitmap GetBitmap()
        {
            return this.bmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetExpositionDuration()
        { 
            return this.expositionDuration; 
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.bmp.Dispose();
            this.thumbnail.Dispose();
        }
    }
}
