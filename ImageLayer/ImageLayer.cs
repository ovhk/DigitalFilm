using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageLayer : IDisposable
    {
#if TEST_BUFFERED_FILE
        /// <summary>
        /// 
        /// </summary>
        private Bitmap _bitmap;

        /// <summary>
        /// 
        /// </summary>
        private string _imgPath;
#endif

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public Bitmap Bitmap
        {
#if TEST_BUFFERED_FILE
            get
            {
                if (this._bitmap == null)
                {
                    this.LoadImage();
                }
                return this._bitmap;
            }
#else
            get;
            private set;
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public static Size ThumbnailSize = new Size(128, 128); // max is 256x256 and must match with TileSize

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
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
#if TEST_BUFFERED_FILE
            this._imgPath = ImageLayerFile.GetImagePath(index, expositionDuration);
            bmp.Save(this._imgPath);
#else
            this.Bitmap = bmp;
#endif
            this.Thumbnail = bmp.GetThumbnailImage(ThumbnailSize.Width, ThumbnailSize.Height, callback, new IntPtr()); // 256x256 max
            this.ExpositionDuration = expositionDuration;
            this.Index = index;
#if TEST_BUFFERED_FILE
            bmp.Dispose();
#endif
        }

#if TEST_BUFFERED_FILE
        /// <summary>
        /// 
        /// </summary>
        public void LoadImage()
        {
            if (this._bitmap == null)
            {
                // this way permit to not lock the file : https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
                using (var bmpTemp = new Bitmap(this._imgPath))
                {
                    this._bitmap = new Bitmap(bmpTemp);
                }
            }
        }
#endif

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

#if TEST_BUFFERED_FILE
            System.GC.Collect(); // Needed to free image memory in time
            //System.GC.WaitForPendingFinalizers(); // not needed
#endif
        }
    }
}
