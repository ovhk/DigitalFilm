using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageLayer : IDisposable
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
        private int index;

        public int Index 
        { 
            get { return index; } 
            set { index = value; }
        }

        [CategoryAttribute("Duration"),
        DescriptionAttribute("Exposition duration")]
        public int ExpositionDuration { get { return expositionDuration; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="expositionDuration"></param>
        /// <param name="index">index of the bitmap to use in ListView</param>
        public ImageLayer (Bitmap bmp, int expositionDuration, int index)
        {
            Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

            this.bmp = bmp;
            this.thumbnail = bmp.GetThumbnailImage(128, 128, callback, new IntPtr()); // 256x256 max
            this.expositionDuration = expositionDuration;
            
            this.index = index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="expositionDuration"></param>
        public ImageLayer(Bitmap bmp, int expositionDuration)
        {
            Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

            this.bmp = bmp;
            this.thumbnail = bmp.GetThumbnailImage(128, 128, callback, new IntPtr()); // 256x256 max
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
