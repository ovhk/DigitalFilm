﻿using System.Drawing;
using System.Windows.Forms;

namespace DigitalFilm.Controls
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
            get => _image;
            set
            {
                this._image = value;
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
                try
                {
                    // This is faster than use BackgroudImage or a PictureBox, thanks to https://stackoverflow.com/questions/28689358/slow-picture-box
                    e.Graphics.DrawImage(this._image, this.ClientRectangle);
                }
                catch
                {
                    Log.WriteLine("MyPictureBox:Fail to display one image");
                }
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
