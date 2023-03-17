using DigitalDarkroom.Panels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalDarkroom
{
    public enum EngineStatus
    {
        Started, Running, Stopped, Ended
    }

    public delegate void NewImageEvent(Bitmap bmp);
    public delegate void NewPanelSizeEvent(int width, int height);
    public delegate void NewProgessEvent(int imageLayerIndex, TimeSpan timeSpan);

    public class DisplayEngine // TODO : FSM
    {
        #region Singleton

        private static DisplayEngine _instance;

        private static readonly object _lock = new object();

        public static DisplayEngine GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DisplayEngine();
                    }
                }
            }
            return _instance;
        }

        #endregion

        private Thread thread;

        //private System.Timers.Timer timer = new System.Timers.Timer();

        /// <summary>
        /// Display Engine private constructor, use only the GetInstance
        /// </summary>
        private DisplayEngine() 
        {
            //this.timer.Interval = 1000;
            //this.timer.Elapsed += Timer_Elapsed;

            this.EngineStatusNotify += DisplayEngine_EngineStatusNotify;
        }

        /// <summary>
        /// New image event
        /// </summary>
        public event NewImageEvent OnNewImage;

        /// <summary>
        /// Change panel size event
        /// </summary>
        public event NewPanelSizeEvent OnNewPanelSize;


        public event NewProgessEvent OnNewProgress;

        public event EventHandler<EngineStatus> EngineStatusNotify;

        private int _width;
        private int _height;

        /// <summary>
        /// Get panel's width
        /// </summary>
        public int Width { get => this._width; }

        /// <summary>
        /// Get panel's height
        /// </summary>
        public int Height { get => this._height; }

        /// <summary>
        /// Set the size of the panel
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void setSize(int width, int height)
        {
            this._height = height;
            this._width = width;
            this.OnNewPanelSize?.Invoke(width, height);
        }

        //private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    EngineStatusNotify?.Invoke(this, EngineStatus.Running);
        //}

        private void DisplayEngine_EngineStatusNotify(object sender, EngineStatus e)
        {
            switch (e) 
            { 
                case EngineStatus.Stopped:
                case EngineStatus.Ended:
                    this.OnNewImage?.Invoke(null); // Force black screen (because Background controls color are set to black)
                    break; 
            }
        }

        /// <summary>
        /// ImageLayer queue 
        /// </summary>
        private Queue<ImageLayer> layers = new Queue<ImageLayer>();


        /// <summary>
        /// Add an image in the DisplayEngine queue
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="expositionDuration"></param>
        public void PushImage(Bitmap bitmap, int expositionDuration)
        {
            // TODO : put grayslace filter here ?
            layers.Enqueue(new ImageLayer(bitmap, expositionDuration));
        }

        //private static ManualResetEvent oStopEvent = new ManualResetEvent(false);


        private static void ThreadProc(object obj)
        {
            DisplayEngine de = (DisplayEngine)obj;

            Thread.CurrentThread.IsBackground = true;

            // TODO : cummuler toutes les expositionDuration et faire un décompte ?

            int sum = 0;

            foreach (ImageLayer i in de.layers)
            {
                sum += i.GetExpositionDuration();
            }

            de.EngineStatusNotify?.Invoke(de, EngineStatus.Started);
            Stopwatch stopwatch = Stopwatch.StartNew();
            de.OnNewProgress?.Invoke(0, new TimeSpan(0, 0, 0));

            while (de.layers.Count > 0)
            {
                ImageLayer il = de.layers.Dequeue();

                Bitmap b = il.GetBitmap();

                Bitmap bmpToDisplay;

                switch (b.PixelFormat) // TODO : work here !
                {
                    case PixelFormat.Format32bppArgb:
                    case PixelFormat.Format32bppRgb:
                        bmpToDisplay = MakeGrayscale3(b);
                        break;
                    case PixelFormat.Format8bppIndexed:
                        bmpToDisplay = b;
                        bmpToDisplay.Palette = GetGrayScalePalette();
                        break;
                    default:
                        bmpToDisplay = new Bitmap(b); ;
                        break;
                }

                de.OnNewImage?.Invoke(bmpToDisplay);

                int iter = il.GetExpositionDuration() / 1000; // convert in seconds

                for (int i = 0; i < iter; i++)
                {
                    Thread.Sleep(5000);

                    if (de._stop)
                    {
                        break;
                    }

                    de.OnNewProgress?.Invoke(0, stopwatch.Elapsed); // TODO : put ImageLayerIndex
                }
            }

            stopwatch.Stop();

            de.EngineStatusNotify?.Invoke(de, EngineStatus.Ended);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start() 
        {
            _stop = false;

            this.thread = new Thread(ThreadProc);

            //oStopEvent.Reset();
            this.thread.Start((object)this);
        }

        private bool _stop = false; // TODO do an FSM ?

        /// <summary>
        /// 
        /// </summary>
        public void Stop() 
        {
            if (this.thread == null) return;
            if (this.thread.ThreadState == System.Threading.ThreadState.Stopped)
            {
                return;
            }

            //this.OnNewImage?.Invoke(null); // Force black screen (because Background controls color are set to black)

            _stop = true;

            //oStopEvent.Set();
            this.thread.Join();
            this.EngineStatusNotify?.Invoke(this, EngineStatus.Stopped);
        }

        public void Pause() { }
        public void Next() { }
        public void Reset() 
        { 
        }

        /// <summary>
        /// Based on http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
        /// </summary>
        /// <returns></returns>
        static ColorPalette GetGrayScalePalette() // TODO : mode to tools ?
        {
            Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);

            ColorPalette monoPalette = bmp.Palette;

            Color[] entries = monoPalette.Entries;

            for (int i = 0; i < 256; i++)
            {
                entries[i] = Color.FromArgb(i, i, i);
            }

            return monoPalette;
        }

        static Bitmap MakeGrayscale3(Bitmap original) // TODO : mode to tools ?
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
    }
}
