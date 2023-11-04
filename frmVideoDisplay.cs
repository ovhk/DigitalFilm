using DigitalFilm.Engine;
using DigitalFilm.Panels;
using LibVLCSharp.Shared;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DigitalFilm
{
    /// <summary>
    /// Display Form
    /// </summary>
    public partial class frmVideoDisplay : Form
    {
        /// <summary>
        /// Display Engine
        /// </summary>
        private readonly VideoDisplayEngine engine;

        /// <summary>
        /// FrmVideoDisplay constructor
        /// </summary>
        public frmVideoDisplay()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Needed to eliminate flickering
            engine = VideoDisplayEngine.GetInstance();
            
            videoView1.MediaPlayer = engine._mp;
        }

        private readonly Stopwatch _sw = new Stopwatch();


        #region Invoke Management

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        private void SafeUpdate(Action action)
        {
            if (this.InvokeRequired)
            {
                _ = this.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        #endregion

        #region Dragging Management

        /// <summary>
        /// Dragging flag
        /// </summary>
        private bool dragging = false;

        /// <summary>
        /// 
        /// </summary>
        private Point dragCursorPoint;

        /// <summary>
        /// 
        /// </summary>
        private Point dragFormPoint;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Cursor.Current = Cursors.SizeAll;
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            dragging = false;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDisplay_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
            
            }
        }

        private void frmVideoDisplay_Load(object sender, EventArgs e)
        {
            Media media = new Media(engine._libVLC, "C:\\Users\\sectronic\\source\\repos\\DigitalFilm\\bin\\Debug\\cache\\tmp2\\out.avi", FromType.FromPath);
            videoView1.MediaPlayer.Play(media);
        }
    }
}
