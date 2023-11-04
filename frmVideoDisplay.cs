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
        public LibVLC _libVLC;
        public MediaPlayer _mp;

        /// <summary>
        /// Display Engine
        /// </summary>
        private readonly DisplayEngine engine;

        /// <summary>
        /// FrmVideoDisplay constructor
        /// </summary>
        public frmVideoDisplay()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Needed to eliminate flickering
            _libVLC = new LibVLC();
            _mp = new MediaPlayer(_libVLC);
            videoView1.MediaPlayer = _mp;


            engine = DisplayEngine.GetInstance();
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
                // Adapte to panel size
                if (engine.Panel is PanelSimulator) // TODO simplify this ! Put Screen in IPanel?
                {
                    Point p = new Point
                    {
                        X = Screen.PrimaryScreen.WorkingArea.Left,
                        Y = Screen.PrimaryScreen.WorkingArea.Top
                    };

                    this.TopMost = false;
                    this.Location = p;
                }
                else if (engine.Panel is ExternalPanel)
                {
                    ExternalPanel ep = engine.Panel as ExternalPanel;

                    Point p = new Point
                    {
                        X = ep.Screen.WorkingArea.Left,
                        Y = ep.Screen.WorkingArea.Top
                    };

                    this.TopMost = true;
                    this.Location = p;
                }
                else if (engine.Panel is Wisecoco8k103Panel)
                {
                    Wisecoco8k103Panel ep = engine.Panel as Wisecoco8k103Panel;

                    Point p = new Point
                    {
                        X = ep.Screen.WorkingArea.Left,
                        Y = ep.Screen.WorkingArea.Top
                    };

                    this.TopMost = true;
                    this.Location = p;
                }
            }
        }

        private void frmVideoDisplay_Load(object sender, EventArgs e)
        {
            Media media = new Media(_libVLC, "C:\\Users\\sectronic\\source\\repos\\DigitalFilm\\bin\\Debug\\cache\\tmp2\\out.avi", FromType.FromPath);
            videoView1.MediaPlayer.Play(media);
        }
    }
}
