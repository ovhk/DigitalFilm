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
using DigitalDarkroom.Panels;

namespace DigitalDarkroom
{
    /// <summary>
    /// Display Form
    /// </summary>
    public partial class frmDisplay : Form
    {
        /// <summary>
        /// Display Engine
        /// </summary>
        private DisplayEngine engine;

        /// <summary>
        /// FrmDisplay constructor
        /// </summary>
        public frmDisplay()
        {
            InitializeComponent();
            this.DoubleBuffered = true; // Needed to eliminate flickering
            engine = DisplayEngine.GetInstance();
            engine.OnNewImage += Engine_OnNewImage; // TODO : uniquement au Show ? et on se désabonne au close ?
            engine.OnNewPanel += Engine_OnNewPanel;
        }

        /// <summary>
        /// DisplayEngine send us a new image to display
        /// </summary>
        /// <param name="bmp"></param>
        private void Engine_OnNewImage(Bitmap bmp)
        {
            this.imageToDisplay = bmp;
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
        Bitmap imageToDisplay;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this.imageToDisplay != null)
            {
                // This is faster than use BackgroudImage or a PictureBox, thanks to https://stackoverflow.com/questions/28689358/slow-picture-box
                e.Graphics.DrawImage(this.imageToDisplay, 0, 0);
            }
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

        private void frmDisplay_Activated(object sender, EventArgs e)
        {
            if (engine.Panel.IsFullScreen)
            {
                ExternalPanel ep = engine.Panel as ExternalPanel; // TODO : à revoir

                Point p = new Point
                {
                    X = ep.Screen.WorkingArea.Left,
                    Y = ep.Screen.WorkingArea.Top
                };

                this.Location = p;
            }
            if (engine.Panel is PanelSimulator)
            {
                Point p = new Point
                {
                    X = Screen.PrimaryScreen.WorkingArea.Left,
                    Y = Screen.PrimaryScreen.WorkingArea.Top
                };

                this.Location = p;
            }
        }
    }
}
