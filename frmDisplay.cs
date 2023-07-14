using DigitalFilm.Engine;
using DigitalFilm.Panels;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DigitalFilm
{
    /// <summary>
    /// Display Form
    /// </summary>
    public partial class frmDisplay : Form
    {
        /// <summary>
        /// Display Engine
        /// </summary>
        private readonly DisplayEngine engine;

        /// <summary>
        /// FrmDisplay constructor
        /// </summary>
        public frmDisplay()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Needed to eliminate flickering
            engine = DisplayEngine.GetInstance();
            engine.OnNewPanel += Engine_OnNewPanel;
        }

        private readonly Stopwatch _sw = new Stopwatch();

        /// <summary>
        /// DisplayEngine send us a new image to display
        /// </summary>
        /// <param name="bmp"></param>
        private void Engine_OnNewImage(Bitmap bmp)
        {
            this._imageToDisplay = bmp;
            //_sw.Restart();
            SafeUpdate(() => this.Refresh());
        }

        /// <summary>
        /// DisplayEngine send us a new image size to adapt
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void Engine_OnNewPanel(IPanel panel)
        {
            SafeUpdate(() => this.Width = panel.Width);
            SafeUpdate(() => this.Height = panel.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        private Bitmap _imageToDisplay;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            if (this._imageToDisplay != null)
            {
                try
                {
                    // This is faster than use BackgroudImage or a PictureBox, thanks to https://stackoverflow.com/questions/28689358/slow-picture-box
                    e.Graphics.DrawImage(this._imageToDisplay, this.ClientRectangle);
                    //Log.WriteLine("frmFisplay:OnPaint=" + this._sw.ElapsedMilliseconds.ToString());
                }
                catch
                {
                    Log.WriteLine("frmDisplay:Fail to display on image");
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
            if (engine.Panel.IsFullScreen == false)
            {
                dragging = true;
                dragCursorPoint = Cursor.Position;
                dragFormPoint = this.Location;
            }
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
                // Subscribe to NewImage only if Form is Visible
                engine.OnNewImage += Engine_OnNewImage;

                // Adapte to panel size
                if (engine.Panel.IsFullScreen)
                {
                    ExternalPanel ep = engine.Panel as ExternalPanel;

                    Point p = new Point
                    {
                        X = ep.Screen.WorkingArea.Left,
                        Y = ep.Screen.WorkingArea.Top
                    };

                    this.TopMost = true; // TODO : A tester
                    this.Location = p;
                }
                if (engine.Panel is PanelSimulator)
                {
                    Point p = new Point
                    {
                        X = Screen.PrimaryScreen.WorkingArea.Left,
                        Y = Screen.PrimaryScreen.WorkingArea.Top
                    };

                    this.TopMost = false; // TODO : A tester
                    this.Location = p;
                }
            }
            else
            {
                engine.OnNewImage -= Engine_OnNewImage;
            }
        }
    }
}
