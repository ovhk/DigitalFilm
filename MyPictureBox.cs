using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalDarkroom
{
    public partial class MyPictureBox : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public MyPictureBox()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Needed to eliminate flickering
        }

        /// <summary>
        /// 
        /// </summary>
        public Bitmap Image
        {
            get
            {
                return this._image;
            }
            set
            {
                if (value != null)
                {
                    // Need to clone before engine dispose
                    this._image = (Bitmap)value.Clone();
                }
                else { this._image = null; }
                this.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Bitmap _image;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this._image != null)
            {
                 // This is faster than use BackgroudImage or a PictureBox, thanks to https://stackoverflow.com/questions/28689358/slow-picture-box
                 e.Graphics.DrawImage(this._image, this.ClientRectangle);
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            }
        }
    }
}
