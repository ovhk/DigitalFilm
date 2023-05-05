using DigitalFilm.Panels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DigitalFilm.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public enum EngineStatus
    {
        Started, Running, Stopped
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
    public delegate void NewProgessEvent(int imageLayerIndex, TimeSpan elapseTime);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="engineStatus"></param>
    /// <param name="totalExposureTime"></param>
    public delegate void NewEngineStatusNotify(EngineStatus engineStatus, TimeSpan? totalExposureTime = null);

    /// <summary>
    /// This is this Display engine. It manage the scheduling of displaying the list of image
    /// </summary>
    public class DisplayEngine
    {
        #region Singleton & Constructor

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

        /// <summary>
        /// Display Engine private constructor, use only the GetInstance
        /// </summary>
        private DisplayEngine()
        {
            this.Cache = new CacheManager(this);
            this.EngineStatusNotify += DisplayEngine_EngineStatusNotify;
        }

        #endregion

        #region Cache Management

        public CacheManager Cache
        { get; private set; }

        #endregion

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
        public event NewEngineStatusNotify EngineStatusNotify;

        #endregion

        #region Panel

        /// <summary>
        /// Current panel
        /// </summary>
        private IPanel _panel;

        /// <summary>
        /// Current panel accessor
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

        #endregion

        #region Engine Status

        /// <summary>
        /// 
        /// </summary>
        private EngineStatus _status;

        /// <summary>
        /// 
        /// </summary>
        public EngineStatus Status => this._status;

        /// <summary>
        /// Engine self notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayEngine_EngineStatusNotify(EngineStatus engineStatus, TimeSpan? totalExposureTime)
        {
            this._status = engineStatus;
            switch (engineStatus)
            {
                case EngineStatus.Started:
                    break;
                case EngineStatus.Running:
                    break;
                case EngineStatus.Stopped:
                    this.Clear();
                    break;
            }
        }

        #endregion

        #region Threads

        /// <summary>
        /// Thread stop flag
        /// </summary>
        private bool _threadStop = false;

        #region Thread Display

        /// <summary>
        /// ImageLayer queue 
        /// </summary>
        private readonly Queue<ImageLayer> _qToDisplay = new Queue<ImageLayer>();

        /// <summary>
        /// 
        /// </summary>
        public ImageLayer[] Items
        {
            get { return _qToDisplay.ToArray(); }
        }

        /// <summary>
        /// Thread Display instance
        /// </summary>
        private Thread _threadDisplay;

        /// <summary>
        /// Thread Display Worker
        /// </summary>
        /// <param name="obj">DisplayEngine object</param>
        private static void ThreadProcDisplay(object obj)
        {
            Thread.CurrentThread.IsBackground = true;

            DisplayEngine de = (DisplayEngine)obj;

            TimeSpan tsTotalExposureTime = TimeSpan.Zero;
            int iNotificationInterval = 0;

            foreach (ImageLayer i in de._qToDisplay)
            {
                tsTotalExposureTime += TimeSpan.FromMilliseconds(i.ExposureTime);
            }

            de.EngineStatusNotify?.Invoke(EngineStatus.Running, tsTotalExposureTime);

            Stopwatch stopwatch = Stopwatch.StartNew();

            // We're waiing to start
            _evStartDisplay.WaitOne();

            while (!de._threadStop)
            {
                // Please do as fast as possible here !
                DateTime dtStart = DateTime.Now;

                if (de._qToPreload.Count == 0)
                {
                    Thread.Yield();
                    continue;
                }

                ImageLayer il = de._qToPreload.Dequeue();
                _semaphorePreload.Release();

                if (de._threadStop)
                {
                    break;
                }

                de.OnNewImage?.Invoke(il.Bitmap);
                //Thread.Yield(); // Do not use
                Application.DoEvents();

                int exposureTime = il.ExposureTime;

                iNotificationInterval += exposureTime;

                // This timing is important!
                // Thats why we have a thread to preload and another to dispose
                // We consider that subscriber's OnPaint (from Refresh to OnPaint) have a fix execution time, so this timing must be directly the needed exposition time
                if (de.SleepMsWithBreak(exposureTime, ref de._threadStop) == false)
                {
                    break;
                }

                if (iNotificationInterval >= 500) // Do not send progress event too much...
                {
                    iNotificationInterval = 0;
                    de.OnNewProgress?.Invoke(il.Index, stopwatch.Elapsed);
                }

                double mesured = (DateTime.Now - dtStart).TotalMilliseconds;
                Log.WriteLine("Step Count={0}, {1}ms, measured: {2}ms, delta: {3}ms", il.Index, il.ExposureTime, mesured, string.Format("{0:N1}", (mesured - exposureTime)));

                de._qToDispose.Enqueue(il);
            }

            stopwatch.Stop();

            // Last black image
            de.OnNewImage?.Invoke(null);

            de.EngineStatusNotify?.Invoke(EngineStatus.Stopped);
        }

        #endregion

        #region Thread Dispose

        /// <summary>
        /// Queue of ImageLayer to dispose. We use a ConcurrentQueue because multiple thread use it at the same time
        /// </summary>
        private readonly ConcurrentQueue<ImageLayer> _qToDispose = new ConcurrentQueue<ImageLayer>();

        /// <summary>
        /// Thread Dispose instance
        /// </summary>
        private Thread _threadDispose;

        /// <summary>
        /// Thread Dispose Worker
        /// </summary>
        /// <param name="obj">DisplayEngine object</param>
        private static void ThreadProcDispose(object obj)
        {
            Thread.CurrentThread.IsBackground = true;

            DisplayEngine de = (DisplayEngine)obj;

            while (!de._threadStop)
            {
                // Do not Dequeue immediatly because last image may be still displayed
                // so delayed the dispose, but it's not magic, if you stack to much image to display and the OnPaint isn't fast enough, you will crash.
                if (de._qToDispose.Count < Properties.Settings.Default.FileBufferSize)
                {
                    //Thread.Yield(); // remove, this cause CPU high run
                    Thread.Sleep(100);
                    continue;
                }

                ImageLayer il;
                if (de._qToDispose.TryDequeue(out il) == true)
                {
                    il.Dispose();
                }
            }

            // if stop, dispose all
            foreach (ImageLayer il in de._qToDispose)
            {
                //Log.WriteLine("STOP Dispose : " + il.Index);
                il.Dispose();
            }
        }

        #endregion

        #region Thread Preload

        /// <summary>
        /// 
        /// </summary>
        private static Semaphore _semaphorePreload;

        /// <summary>
        /// 
        /// </summary>
        private readonly Queue<ImageLayer> _qToPreload = new Queue<ImageLayer>();

        /// <summary>
        /// 
        /// </summary>
        private static AutoResetEvent _evStartDisplay = new AutoResetEvent(false);

        /// <summary>
        /// Thread Preload instance
        /// </summary>
        private Thread _threadPreload;

        /// <summary>
        /// Thread Preload Worker
        /// </summary>
        /// <param name="obj">DisplayEngine object</param>
        private static void ThreadProcPreload(object obj)
        {
            Thread.CurrentThread.IsBackground = true;

            int fbSize = Properties.Settings.Default.FileBufferSize;

            DisplayEngine de = (DisplayEngine)obj;

            _semaphorePreload = new Semaphore(initialCount: fbSize, maximumCount: fbSize);

            int lastSleep = 0;

            foreach (ImageLayer i in de._qToDisplay)
            {
                lastSleep = i.ExposureTime;

                _semaphorePreload.WaitOne();

                if (de._threadStop)
                {
                    break;
                }

                // Force loading image
                i.LoadImage();

                // Enqueue image
                de._qToPreload.Enqueue(i);

                if (de._qToDisplay.Count <= fbSize || de._qToPreload.Count >= fbSize)
                {
                    _evStartDisplay.Set(); // Start ThreadProcDisplay
                }
            }

            // leave ThreadProcDisplay dequeue
            while (de._qToPreload.Count > 0)
            {
                Thread.Sleep(100);
            }

            de.SleepMsWithBreak(lastSleep, ref de._threadStop); // To be sure last one is done...

            de._threadStop = true; // end ThreadProcDisplay
        }

        #endregion

        #region Sleep

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

            // if exposure time is small, we don't cut it in seconds
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

        #endregion

        #endregion

        #region Public interface

        /// <summary>
        /// Start Engine
        /// </summary>
        public void Start()
        {
            this._threadStop = false;

            // One thread to preload images
            this._threadPreload = new Thread(ThreadProcPreload);
            this._threadPreload.Start((object)this);

            // One thread to unload images
            this._threadDispose = new Thread(ThreadProcDispose);
            this._threadDispose.Start((object)this);

            // One thread to display images
            this._threadDisplay = new Thread(ThreadProcDisplay);
            this._threadDisplay.Start((object)this);

            this.EngineStatusNotify?.Invoke(EngineStatus.Started);
        }

        /// <summary>
        /// Stop Engine
        /// </summary>
        public void Stop()
        {
            if (this.Status == EngineStatus.Stopped)
            {
                return;
            }

            this._threadStop = true;
            _evStartDisplay.Set();

            try
            {
                _semaphorePreload?.Release();
            }
            catch { }

            this._qToPreload.Clear(); // free _threadPreload from work and go to end

            // Wait for the end of threads excecution
            this._threadPreload?.Join();
            this._threadDisplay?.Join();
            this._threadDispose?.Join();

            this.EngineStatusNotify?.Invoke(EngineStatus.Stopped);
        }

        /// <summary>
        /// Clear and free layers
        /// </summary>
        public void Clear()
        {
            foreach (ImageLayer il in this._qToDisplay) // useful only if there's a stop before the end
            {
                il.Dispose();
            }

            foreach (ImageLayer il in this._qToPreload) // useful only if there's a stop before the end
            {
                il.Dispose();
            }

            foreach (ImageLayer il in this._qToDispose) // useful only if there's a stop before the end
            {
                il.Dispose();
            }

            this._qToPreload.Clear(); // useful only if there's a stop before the end
            this._qToDisplay.Clear(); // useful only if there's a stop before the end

            this.Cache.ClearTmpCache();

            GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Add an image in the DisplayEngine queue
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="exposureTime"></param>
        public void PushImage(Bitmap bitmap, int exposureTime)
        {
            if (exposureTime < this.Panel.ResponseTime)
            {
                Log.WriteLine("ExposureTime has been filtered from {0} to {1} on index {2}.", exposureTime, this.Panel.ResponseTime, _qToDisplay.Count);
                exposureTime = this.Panel.ResponseTime;
            }
            _qToDisplay.Enqueue(new ImageLayer(bitmap, exposureTime, _qToDisplay.Count));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageLayer"></param>
        public void PushImage(ImageLayer imageLayer)
        {
            imageLayer.Index = _qToDisplay.Count;
            if (imageLayer.ExposureTime < this.Panel.ResponseTime)
            {
                Log.WriteLine("ExposureTime has been filtered from {0} to {1} on index {2}.", imageLayer.ExposureTime, this.Panel.ResponseTime, imageLayer.Index);
                imageLayer.ExposureTime = this.Panel.ResponseTime;
            }
            _qToDisplay.Enqueue(imageLayer);
        }
        #endregion
    }
}
