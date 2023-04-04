using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
#if TEST_FILE
        private Bitmap _bitmap;
#endif

        /// <summary>
        /// 
        /// </summary>
        public Bitmap Bitmap
        {
#if TEST_FILE
            get
            {
                if (this._bitmap == null)
                {
                    // this way permit to not lock the file : https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
                    using (var bmpTemp = new Bitmap(Index + ".bmp"))
                    {
                        this._bitmap = new Bitmap(bmpTemp);
                    }
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
#if TEST_FILE
            bmp.Save(index + ".bmp");
#else
            this.Bitmap = bmp;
#endif
            this.Thumbnail = bmp.GetThumbnailImage(128, 128, callback, new IntPtr()); // 256x256 max
            this.ExpositionDuration = expositionDuration;
            this.Index = index;
#if TEST_FILE
            bmp.Dispose();
#endif
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
#if TEST_FILE
            System.IO.File.Delete(Index + ".bmp");
#else
#endif
            this.Bitmap.Dispose();
            this.Thumbnail.Dispose();

#if TEST_FILE
            // TODO : OK ceci montre que je n'ai pas de fuite mémoire...
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
#endif
        }
    }
}
