using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace DigitalFilm.Engine
{

    /// <summary>
    /// 
    /// </summary>
    public class ImageLayer : IDisposable
    {
        /// <summary>
        /// the path of the image
        /// </summary>
        public string ImagePath
        {
            get;
            private set;
        }
        /// <summary>
        ///  The image
        /// </summary>
        private Bitmap _bitmap;

        /// <summary>
        /// The image getter
        /// </summary>
        [Browsable(false)]
        public Bitmap Bitmap
        {
            get
            {
                if (this._bitmap == null)
                {
                    this.LoadImage();
                }
                return this._bitmap;
            }
        }

        /// <summary>
        /// Default thumbnail size
        /// </summary>
        [Browsable(false)]
        public static Size ThumbnailSize = new Size(128, 128); // max is 256x256 and must match with TileSize

        /// <summary>
        /// Thumbnail image
        /// </summary>
        [Browsable(false)]
        public Image Thumbnail
        {
            get;
            private set;
        }

        /// <summary>
        /// Exposure time
        /// </summary>
        [CategoryAttribute("ExposureTime"),
        DescriptionAttribute("Exposure time in ms")]
        public int ExposureTime
        {
            get;
            set; // Not private to able to filter it if needed following panel specs.
        }

        /// <summary>
        /// Index
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
        /// <param name="exposureTime"></param>
        /// <param name="index">index of the bitmap to use in ListView</param>
        public ImageLayer(Bitmap bmp, int exposureTime, int index)
        {
            Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

            this.ImagePath = DisplayEngine.GetInstance().Cache.GetTmpCachePath(index, exposureTime);
            bmp.Save(this.ImagePath);

            this.Thumbnail = bmp.GetThumbnailImage(ThumbnailSize.Width, ThumbnailSize.Height, callback, new IntPtr()); // 256x256 max
            this.ExposureTime = exposureTime;
            this.Index = index;
            bmp.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheImgPath"></param>
        public ImageLayer(string cacheImgPath)
        {
            Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(ThumbnailCallback);

            this.ImagePath = cacheImgPath;

            FileInfo file = new FileInfo(this.ImagePath);


            _ = DisplayEngine.GetInstance().Cache.GetIndexAndExposureTime(file.Name, out int index, out int exposureTime);

            using (Bitmap bmp = new Bitmap(this.ImagePath))
            {
                this.Thumbnail = bmp.GetThumbnailImage(ThumbnailSize.Width, ThumbnailSize.Height, callback, new IntPtr()); // 256x256 max
                this.ExposureTime = exposureTime;
                this.Index = index;
            }
        }

        /// <summary>
        /// Load Image in memory
        /// </summary>
        public void LoadImage()
        {
            if (this._bitmap == null)
            {
                // this way permit to not lock the file : https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
                using (Bitmap bmpTemp = new Bitmap(this.ImagePath))
                {
                    this._bitmap = new Bitmap(bmpTemp);
                }
            }
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
        /// Free memory
        /// </summary>
        public void Dispose()
        {
            this._bitmap?.Dispose();
            this.Thumbnail?.Dispose();
        }
    }
}
