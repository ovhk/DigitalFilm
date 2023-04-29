using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalDarkroom.Controls;
using DigitalDarkroom.Tools;
using System.ComponentModel;
using DigitalDarkroom.Engine;

namespace DigitalDarkroom.Modes
{
    internal class Mode5 : IMode
    {
        /// <summary>
        /// Name
        /// </summary>
        [Browsable(false)]
        public string Name => "Grayscale picture";

        /// <summary>
        /// Description
        /// </summary>
        [Browsable(false)]
        public string Description => "Display the selected picture in grayscale.";

        /// <summary>
        /// Display duration in ms
        /// </summary>
        [Category("Configuration")]
        [Description("Display duration in ms")]
        public int Duration
        { get; set; } = 5000;

        /// <summary>
        /// Source file to display
        /// </summary>
        [Category("Configuration")]
        [Description("Source file to display")]
        [Editor(typeof(ImageFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ImagePath
        { get; set; }

        /// <summary>
        /// Display mode
        /// </summary>
        [Category("Configuration")]
        [Description("Size mode")]
        public SizeMode SizeMode
        { get; set; } = SizeMode.CenterImage;

        #region Margins

        /// <summary>
        /// 
        /// </summary>
        [Category("Margin")]
        [Description("Margin color?")]
        public MarginColor MarginColor
        { get; set; } = MarginColor.White; // remember to invert color somewhere

        /// <summary>
        /// 
        /// </summary>
        [Category("Margin")]
        [Description("Top and bottom margin size.")]
        public int MarginTopBottom
        { get; set; } = 10;

        /// <summary>
        /// 
        /// </summary>
        [Category("Margin")]
        [Description("Left and right margin size.")]
        public int MarginLeftRight
        { get; set; } = 10;

        #endregion

        /// <summary>
        /// Rotation
        /// </summary>
        [Category("Configuration")]
        [Description("Rotation")]
        public RotateFlipType Rotation
        { get; set; } = RotateFlipType.RotateNoneFlipNone;

        /// <summary>
        /// Access to the Engine
        /// </summary>
        private readonly DisplayEngine engine = DisplayEngine.GetInstance();

        /// <summary>
        /// Load function
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ImagePath == null || ImagePath.Length == 0) return false;

            // this way permit to not lock the file : https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
            using (var bmpPicture = new Bitmap(ImagePath))
            {
                Bitmap bmpPanel = new Bitmap(engine.Panel.Width, engine.Panel.Height);

                Graphics gfx = Graphics.FromImage(bmpPanel);

                bmpPicture.RotateFlip(Rotation);

                // 1. erase all
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    gfx.FillRectangle(brush, 0, 0, engine.Panel.Width, engine.Panel.Height);
                }

                // 2. Draw margins
                SolidBrush marginBrush = (MarginColor == MarginColor.Back) ? new SolidBrush(Color.Black) : new SolidBrush(Color.White); // do not invert color here, done after

                if (MarginLeftRight > 0)
                {
                    gfx.FillRectangle(marginBrush, 0, 0, MarginLeftRight, engine.Panel.Height); // LEFT

                    gfx.FillRectangle(marginBrush, engine.Panel.Width - MarginLeftRight, 0, MarginLeftRight, engine.Panel.Height); // RIGHT
                }

                if (MarginTopBottom > 0)
                {
                    gfx.FillRectangle(marginBrush, 0, 0, engine.Panel.Width, MarginTopBottom); // TOP

                    gfx.FillRectangle(marginBrush, 0, engine.Panel.Height - MarginTopBottom, engine.Panel.Width, MarginTopBottom); // BOTTOM
                }

                marginBrush.Dispose();

                // 3. determine size
                Size sz = new Size();

                switch (SizeMode)
                {
                    case SizeMode.CenterImage:
                        // Ratio is : canavas / original
                        double ratioW = ((double)engine.Panel.Width - (2 * (double)MarginLeftRight)) / ((double)bmpPicture.Width);
                        double ratioH = ((double)engine.Panel.Height - (2 * (double)MarginTopBottom)) / ((double)bmpPicture.Height);

                        // get the smaller ratio
                        double ratio = (ratioW < ratioH) ? ratioW : ratioH;

                        sz.Width  = Convert.ToInt32(ratio * bmpPicture.Width);
                        sz.Height = Convert.ToInt32(ratio * bmpPicture.Height);
                        break;

                    case SizeMode.StretchImage:
                    default:
                        sz.Width = engine.Panel.Width;
                        sz.Height = engine.Panel.Height;
                        break;
                }

                // 4. convert picture to grayscale
                Bitmap grayscalePicture = GrayScale.MakeGrayscale3(bmpPicture);

                // 5. draw image
                Rectangle imgRect = new Rectangle
                {
                    X = MarginLeftRight,
                    Y = MarginTopBottom,
                    Width = sz.Width, // margin already inside
                    Height = sz.Height
                };

                if (SizeMode == SizeMode.CenterImage)
                {
                    // Add offset to center image
                    imgRect.X += Convert.ToInt32(((double)engine.Panel.Width - (2 * (double)MarginLeftRight)) / 2 - (double)sz.Width / 2.0);
                    imgRect.Y += Convert.ToInt32(((double)engine.Panel.Height - (2 * (double)MarginTopBottom)) / 2 - (double)sz.Height / 2.0);
                }

                gfx.DrawImage(grayscalePicture, imgRect);

                //System.Windows.Forms.MessageBox.Show("Ratio=" + (double)imgRect.Width / (double)imgRect.Height);

                // 6. invert image and push image to engine
                Bitmap invertedImage = BitmapTools.GetInvertedBitmap(bmpPanel);
                
                engine.PushImage(invertedImage, Duration);

                gfx.Dispose();
            }

            return true;
        }

        /// <summary>
        /// Unload function
        /// </summary>
        /// <returns></returns>
        public bool Unload()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();
            engine.Clear();

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
