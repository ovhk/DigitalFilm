using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace DigitalDarkroom
{
    public partial class frmDisplay : Form
    {
        public frmDisplay()
        {
            InitializeComponent();
        }

        // TODO : déplacer la fenête sur un autre écran : https://stackoverflow.com/questions/8420203/move-form-onto-specified-screen

        #region TODO REMOVE Display queue management

        Queue<ImageLayer> layers = new Queue<ImageLayer>();

        ManualResetEvent oPausevent = new ManualResetEvent(false);

        public void PushImage(Bitmap bitmap, int expositionDuration)
        {
            layers.Enqueue(new ImageLayer(bitmap, expositionDuration));
        }

        public void Run()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                
                while (layers.Count > 0)
                {
                    ImageLayer il = layers.Dequeue();
                    this.setImage(il.GetBitmap());
                    Thread.Sleep(il.GetExpositionDuration());
                }

            }).Start();
        }

        public void Pause()
        {
            this.oPausevent.Set();
        }
        public void Stop()
        {
            SafeUpdate(() => this.BackgroundImage = null);
            SafeUpdate(() => this.Refresh());
        }

        #endregion

        public void setImage(Bitmap bmp)
        {
            if (bmp == null)
            {
                return;
            }
            
            Bitmap bmpToDisplay;
 
            switch(bmp.PixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppRgb:
                    bmpToDisplay = MakeGrayscale3(bmp);
                    break;
                case PixelFormat.Format8bppIndexed:
                    bmpToDisplay = bmp;
                    bmpToDisplay.Palette = GetGrayScalePalette();
                    break;
                default:
                    bmpToDisplay = new Bitmap(bmp); ;
                    break;
            }

            SafeUpdate(() => this.BackgroundImage = bmpToDisplay);
            SafeUpdate(() => this.Refresh());
        }

        #region TODO REMOVE

        /// <summary>
        /// Based on http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
        /// </summary>
        /// <returns></returns>
        private ColorPalette GetGrayScalePalette()
        {
            Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);

            ColorPalette monoPalette = bmp.Palette;

            Color[] entries = monoPalette.Entries;

            //for (int i = 0; i < 256 - 1; i++)
            for (int i = 0; i < 256; i++)
            {
                entries[i] = Color.FromArgb(i, i, i);
            }

            return monoPalette;
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        #endregion

        public void setSize(int width, int height) 
        {
            SafeUpdate(() => this.Width = width);
            SafeUpdate(() => this.Height = height);
        }

        #region Invoke Management

        private void SafeUpdate(Action action)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        #endregion

        #region Dragging Management

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void frmDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void frmDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void frmDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        #endregion
    }
}
