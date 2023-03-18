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
    /// <summary>
    /// Display Form
    /// </summary>
    public partial class frmDisplay : Form
    {
        /// <summary>
        /// Display Engine
        /// </summary>
        private DisplayEngine engine;

        public frmDisplay()
        {
            InitializeComponent();
            engine = DisplayEngine.GetInstance();
            engine.OnNewImage += Engine_OnNewImage;
            engine.OnNewPanelSize += Engine_OnNewPanelSize;
        }

        // TODO : déplacer la fenête sur un autre écran : https://stackoverflow.com/questions/8420203/move-form-onto-specified-screen

        /// <summary>
        /// DisplayEngine send us a new image to display
        /// </summary>
        /// <param name="bmp"></param>
        private void Engine_OnNewImage(Bitmap bmp)
        {
            Image old = this.BackgroundImage;
            SafeUpdate(() => this.BackgroundImage = bmp); // TODO c'est long à afficher !!!!
            //SafeUpdate(() => this.Refresh());
            if (old != null) old.Dispose();
        }

        /// <summary>
        /// DisplayEngine send us a new image size to adapt
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void Engine_OnNewPanelSize(int width, int height)
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

        /// <summary>
        /// Dragging flag
        /// </summary>
        private bool dragging = false;

        private Point dragCursorPoint;
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
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
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
    }
}
