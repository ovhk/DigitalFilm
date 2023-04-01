﻿using DigitalDarkroom.Panels;
using System;
using System.Collections;
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

        /// <summary>
        /// 
        /// </summary>
        private Thread thread;

        /// <summary>
        /// Display Engine private constructor, use only the GetInstance
        /// </summary>
        private DisplayEngine()
        {
            this.EngineStatusNotify += DisplayEngine_EngineStatusNotify;
        }

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

        /// <summary>
        /// 
        /// </summary>
        private IPanel panel;

        /// <summary>
        /// 
        /// </summary>
        public IPanel Panel
        {
            get => this.panel;
            set
            {
                this.panel = value;
                this.OnNewPanel?.Invoke(this.panel);
            }
        }

        /// <summary>
        /// Engine self notification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayEngine_EngineStatusNotify(object sender, EngineStatus e)
        {
            switch (e) 
            { 
                case EngineStatus.Stopped:
                case EngineStatus.Ended:
                    this.layers.Clear();
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
            layers.Enqueue(new ImageLayer(bitmap, expositionDuration, layers.Count));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageLayer"></param>
        public void PushImage(ImageLayer imageLayer)
        {
            imageLayer.Index = layers.Count;
            layers.Enqueue(imageLayer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ImageList GetImageList()
        {
            ImageList il = new ImageList();
            
            il.ImageSize = new Size(128, 128); // TODO must match with ImageLayer 256 max, get value from TileSize ?

            ImageLayer[] arr = layers.ToArray();

            for (int y = 0; y < arr.Length; y++)
            {
                ImageLayer i = arr[y] as ImageLayer;
                string key = y.ToString();
                il.Images.Add(key, i.GetThumbnail());
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

            ImageLayer[] arr = layers.ToArray();

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

        /// <summary>
        /// Thread Engine
        /// </summary>
        /// <param name="obj"></param>
        private static void ThreadProc(object obj)
        {
            DisplayEngine de = (DisplayEngine)obj;

            Thread.CurrentThread.IsBackground = true;

            TimeSpan tsTotalDuration = TimeSpan.Zero;

            foreach (ImageLayer i in de.layers)
            {
                tsTotalDuration += TimeSpan.FromMilliseconds(i.GetExpositionDuration());
            }

            de.EngineStatusNotify?.Invoke(de, EngineStatus.Started);
            Stopwatch stopwatch = Stopwatch.StartNew();
            de.OnNewProgress?.Invoke(0, TimeSpan.Zero, tsTotalDuration);

            while (de.layers.Count > 0)
            {
                // Please do as fast as possible here !
                DateTime dtStart = DateTime.Now;

                ImageLayer il = de.layers.Dequeue();

                if (de._stop)
                {
                    break;
                }

                // Subscribers need to Clone the object to keep it
                de.OnNewImage?.Invoke(il.GetBitmap());

                int duration = il.GetExpositionDuration();

                // if duration is small, we don't cut it in seconds
                if (duration <= 1000)
                {
                    Thread.Sleep(duration);
                    //de.OnNewProgress?.Invoke(il.Index, stopwatch.Elapsed, tsTotalDuration); // TODO : ATTENTION C'EST TROP LONG
                } 
                else
                {
                    int iter = duration / 1000; // convert in number of secondes

                    for (int i = 0; i < iter; i++)
                    {
                        Thread.Sleep(1000);

                        if (de._stop)
                        {
                            break;
                        }
                    }

                    Thread.Sleep(duration - (iter * 1000) - 1); // -1 for calls and calculation

                    //de.OnNewProgress?.Invoke(il.Index, stopwatch.Elapsed, tsTotalDuration);  // TODO : ATTENTION C'EST TROP LONG
                }

                //il.Dispose();
                //TODO : mesurer un peu plus les temps
                //Console.WriteLine("Step Count=" + de.layers.Count + ", " + duration + "ms, measured: " + (DateTime.Now - dtStart).TotalMilliseconds);
                Log.WriteLine("Step Count=" + de.layers.Count + ", " + duration + "ms, measured: " + (DateTime.Now - dtStart).TotalMilliseconds);
            }

            stopwatch.Stop();

            // Last black image
            de.OnNewImage?.Invoke(null);

            de.EngineStatusNotify?.Invoke(de, EngineStatus.Ended);
        }

        /// <summary>
        /// Start Engine
        /// </summary>
        public void Start() 
        {
            _stop = false;

            this.thread = new Thread(ThreadProc);

            this.thread.Start((object)this);
        }

        /// <summary>
        /// Thread stop flag
        /// </summary>
        private bool _stop = false;

        /// <summary>
        /// Stop Engine
        /// </summary>
        public void Stop() 
        {
            _stop = true;

            if (this.thread != null && this.thread.ThreadState == System.Threading.ThreadState.Running)
            {
                this.thread.Join(); // TODO : nedded ?
            }

            this.EngineStatusNotify?.Invoke(this, EngineStatus.Stopped);

            this.thread = null;
        }

        /// <summary>
        /// Clear and free layers
        /// </summary>
        public void Clear()
        {
            foreach(ImageLayer il in this.layers)
            {
                il.Dispose();
            }

            this.layers.Clear();

            GC.Collect();
        }
    }
}
