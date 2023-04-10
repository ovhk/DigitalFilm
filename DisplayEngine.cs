using DigitalDarkroom.Panels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalDarkroom
{
    /// <summary>
    /// 
    /// </summary>
    public enum EngineStatus
    {
        Started, Running, Stopped, Ended // TODO determine engine status
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bmp"></param>
    public delegate void NewImageEvent(Bitmap bmp);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="panel"></param>
    public delegate void NewPanelEvent(IPanel panel);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="imageLayerIndex"></param>
    /// <param name="elapseTime"></param>
    /// <param name="totalDuration"></param>
    public delegate void NewProgessEvent(int imageLayerIndex, TimeSpan elapseTime, TimeSpan totalDuration);

    /// <summary>
    /// This is this Display engine. It manage the scheduling of displaying the list of image
    /// </summary>
    public class DisplayEngine
    {
        #region Singleton

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static DisplayEngine _instance;

        /// <summary>
        /// Lock object
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// Get the instance of the singleton
        /// </summary>
        /// <returns></returns>
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

        #region Threads

        /// <summary>
        /// 
        /// </summary>
        private Thread _threadDisplay;

#if TEST_BUFFERED_FILE
        /// <summary>
        /// 
        /// </summary>
        private Thread _threadFiles;
#endif

        #endregion

        /// <summary>
        /// Display Engine private constructor, use only the GetInstance
        /// </summary>
        private DisplayEngine()
        {
            this.EngineStatusNotify += DisplayEngine_EngineStatusNotify;
        }

        #region Events

        /// <summary>
        /// New image event
        /// </summary>
        public event NewImageEvent OnNewImage;

        /// <summary>
        /// Change panel size event
        /// </summary>
        public event NewPanelEvent OnNewPanel;

        /// <summary>
        /// Progress event
        /// </summary>
        public event NewProgessEvent OnNewProgress;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EngineStatus> EngineStatusNotify;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        private IPanel _panel;

        /// <summary>
        /// 
        /// </summary>
        public IPanel Panel
        {
            get => this._panel;
            set
            {
                this._panel = value;
                this.OnNewPanel?.Invoke(this._panel);
            }
        }

        private EngineStatus _status;

        public EngineStatus Status => this._status;

        /// <summary>
        /// Engine self notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayEngine_EngineStatusNotify(object sender, EngineStatus e)
        {
            this._status = e;
            switch (e) 
            { 
                case EngineStatus.Stopped:
                case EngineStatus.Ended:
#if TEST_BUFFERED_FILE
                    this._fileLayers.Clear();
#endif
                    this._imglayers.Clear();
                    this.OnNewImage?.Invoke(null); // Force black screen (because Background controls color are set to black)
                    break; 
            }
        }

        /// <summary>
        /// ImageLayer queue 
        /// </summary>
        private readonly Queue<ImageLayer> _imglayers = new Queue<ImageLayer>();

        /// <summary>
        /// Thread stop flag
        /// </summary>
        private bool _stop = false;

        /// <summary>
        /// Thread Engine
        /// </summary>
        /// <param name="obj"></param>
        private static void ThreadProcDisplay(object obj)
        {
            Thread.CurrentThread.IsBackground = true;

            DisplayEngine de = (DisplayEngine)obj;

            de.EngineStatusNotify?.Invoke(de, EngineStatus.Running);

            TimeSpan tsTotalDuration = TimeSpan.Zero;
            int iNotificationInterval = 0;

            foreach (ImageLayer i in de._imglayers)
            {
                tsTotalDuration += TimeSpan.FromMilliseconds(i.ExpositionDuration);
            }

            Stopwatch stopwatch = Stopwatch.StartNew();

            de.OnNewProgress?.Invoke(0, TimeSpan.Zero, tsTotalDuration);

#if TEST_BUFFERED_FILE
            // We"re waiing to start
            _evStartDisplay.WaitOne();

            while (!de._stop)
#else
            while (de.layers.Count > 0)
#endif
            {
                // Please do as fast as possible here !
                DateTime dtStart = DateTime.Now;

#if TEST_BUFFERED_FILE

                if (de._fileLayers.Count == 0) 
                {
                    Thread.Yield();
                    continue;
                }

                ImageLayer il = de._fileLayers.Dequeue();
                _semaphoreFiles.Release();
#else
                ImageLayer il = de.layers.Dequeue();
#endif

                if (de._stop)
                {
                    break;
                }

                // Subscribers need to Clone the object to keep it
                de.OnNewImage?.Invoke(il.Bitmap);

                int duration = il.ExpositionDuration;

                iNotificationInterval += duration;

                if (de.SleepMsWithBreak(duration, ref de._stop) == false)
                {
                    break;
                }

                if (iNotificationInterval >= 500) // Do not send progress event too much...
                {
                    iNotificationInterval = 0;
                    de.OnNewProgress?.Invoke(il.Index, stopwatch.Elapsed, tsTotalDuration);
                }

                double mesured = (DateTime.Now - dtStart).TotalMilliseconds;
#if TEST_BUFFERED_FILE
                Log.WriteLine("Step Count={0}, {1}ms, measured: {2}ms, delta: {3}ms", il.Index, il.ExpositionDuration, mesured, string.Format("{0:N1}", (mesured - duration)));
                il.Dispose(); // Nedded to free image memory
#else
                Log.WriteLine("Step Count={0}, {1}ms, measured: {2}ms, delta: {3}ms", de.layers.Count, duration, mesured, string.Format("{0:N1}", (mesured - duration)));
#endif
            }

            stopwatch.Stop();

            // Last black image
            de.OnNewImage?.Invoke(null);

            de.EngineStatusNotify?.Invoke(de, EngineStatus.Ended);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        private bool SleepMsWithBreak(int ms, ref bool stop)
        {
            ms += Properties.Settings.Default.IntervalOffset;

            if (ms <= 0)
            {
                return true;
            }

            // if duration is small, we don't cut it in seconds
            if (ms <= 1000)
            {
                Thread.Sleep(ms);
            }
            else
            {
                int iter = ms / 1000; // convert in number of secondes

                for (int i = 0; i < iter; i++)
                {
                    Thread.Sleep(1000);

                    if (stop)
                    {
                        return false; 
                    }
                }

                int wait = ms - (iter * 1000);

                if (wait >= 1)
                {
                    Thread.Sleep(wait);
                }
            }

            if (stop)
            {
                return false;
            }

            return true;
        }

#if TEST_BUFFERED_FILE

        private static Semaphore _semaphoreFiles;

        private readonly Queue<ImageLayer> _fileLayers = new Queue<ImageLayer>();

        private static AutoResetEvent _evStartDisplay = new AutoResetEvent(false);

        private static void ThreadProcFile(object obj)
        {
            Thread.CurrentThread.IsBackground = true;

            int fbSize = Properties.Settings.Default.FileBufferSize;

            DisplayEngine de = (DisplayEngine)obj;

            _semaphoreFiles = new Semaphore(initialCount: fbSize, maximumCount: fbSize);

            int lastSleep = 0;

            foreach (ImageLayer i in de._imglayers)
            {
                lastSleep = i.ExpositionDuration;

                _semaphoreFiles.WaitOne();

                // Force loading image
                i.LoadImage();

                // Enqueue image
                de._fileLayers.Enqueue(i);

                if (de._imglayers.Count <= fbSize || de._fileLayers.Count >= fbSize)
                {
                    _evStartDisplay.Set(); // Start ThreadProcDisplay
                }
            }

            // leave ThreadProcDisplay dequeue
            while (de._fileLayers.Count > 0)
            {
                Thread.Sleep(100);
            }

            Thread.Sleep(lastSleep + 100); // To be sure last one is done...

            de._stop = true; // end ThreadProcDisplay
        }
#endif

        #region Public interface

        /// <summary>
        /// Start Engine
        /// </summary>
        public void Start() 
        {
            _stop = false;

#if TEST_BUFFERED_FILE

            this._threadFiles = new Thread(ThreadProcFile);

            this._threadFiles.Start((object)this);
#endif

            this._threadDisplay = new Thread(ThreadProcDisplay);

            this._threadDisplay.Start((object)this);

            this.EngineStatusNotify?.Invoke(this, EngineStatus.Started);
        }

        /// <summary>
        /// Stop Engine
        /// </summary>
        public void Stop() 
        {
            this._stop = true;

            if (this._threadDisplay != null && this._threadDisplay.ThreadState == System.Threading.ThreadState.Running)
            {
                this._threadDisplay.Join(); // Wait for the end of the thread excecution
            }

#if TEST_BUFFERED_FILE
            // TODO kill _threadFiles 

            // release to unlock waitone and test _stop ?
#endif

            this.EngineStatusNotify?.Invoke(this, EngineStatus.Stopped);

            this._threadDisplay = null;
        }

        /// <summary>
        /// Clear and free layers
        /// </summary>
        public void Clear()
        {
            foreach(ImageLayer il in this._imglayers)
            {
                il.Dispose();
            }

#if TEST_BUFFERED_FILE
            this._fileLayers.Clear();
#endif
            this._imglayers.Clear();

            GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Add an image in the DisplayEngine queue
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="expositionDuration"></param>
        public void PushImage(Bitmap bitmap, int expositionDuration)
        {
            _imglayers.Enqueue(new ImageLayer(bitmap, expositionDuration, _imglayers.Count));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageLayer"></param>
        public void PushImage(ImageLayer imageLayer)
        {
            imageLayer.Index = _imglayers.Count;
            _imglayers.Enqueue(imageLayer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ImageList GetImageList()
        {
            ImageList il = new ImageList();

            il.ImageSize = ImageLayer.ThumbnailSize;

            ImageLayer[] arr = _imglayers.ToArray();

            for (int y = 0; y < arr.Length; y++)
            {
                ImageLayer i = arr[y] as ImageLayer;
                string key = y.ToString();
                il.Images.Add(key, i.Thumbnail);
            }

            return il;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ListViewItem> GetListViewItems()
        {
            List<ListViewItem> list = new List<ListViewItem>();

            ImageLayer[] arr = _imglayers.ToArray();

            for (int y = 0; y < arr.Length; y++)
            {
                ImageLayer i = arr[y] as ImageLayer;
                string key = y.ToString();
                ListViewItem lvi = new ListViewItem(key);

                lvi.ImageIndex = i.Index;
                lvi.Tag = i;
                lvi.ImageKey = key;
                list.Add(lvi);
            }

            return list;
        }

        #endregion
    }
}
