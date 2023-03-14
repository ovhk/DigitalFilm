using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom
{
    internal class ImageLayer
    {
        Bitmap bmp;
        int expositionDuration;

        public ImageLayer (Bitmap bmp, int expositionDuration)
        {
            this.bmp = bmp;
            this.expositionDuration = expositionDuration;
        }

        public Bitmap GetBitmap()
        {
            return bmp;
        }

        public int GetExpositionDuration()
        { 
            return expositionDuration; 
        }

    }
}
