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
        public Bitmap Bitmap
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Image Thumbnail
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Duration"),
        DescriptionAttribute("Exposition duration")]
        public int ExpositionDuration
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        [CategoryAttribute("Index"),
        DescriptionAttribute("Display Index")]
        public int Index 
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="expositionDuration"></param>
        /// <param name="index">index of the bitmap to use in ListView</param>
        public ImageLayer (Bitmap bmp, int expositionDuration, int index)
        {
            Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

            this.Bitmap = bmp;
            this.Thumbnail = bmp.GetThumbnailImage(128, 128, callback, new IntPtr()); // 256x256 max
            this.ExpositionDuration = expositionDuration;
            this.Index = index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="expositionDuration"></param>
        public ImageLayer(Bitmap bmp, int expositionDuration)
        {
            Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

            this.Bitmap = bmp;
            this.Thumbnail = bmp.GetThumbnailImage(128, 128, callback, new IntPtr()); // 256x256 max
            this.ExpositionDuration = expositionDuration;
        }

        /// <summary>
        /// This is for GetThumbnailImageAbort call
        /// </summary>
        /// <returns></returns>
        private bool ThumbnailCallback()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Bitmap.Dispose();
            this.Thumbnail.Dispose();
        }
    }
}
