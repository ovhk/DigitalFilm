using DigitalDarkroom.Panels;
using System;
using System.Collections.Generic;
using System.Drawing;
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

    internal class DisplayEngine // TODO : faire un Singleton + FSM
    {

        private Thread thread;

        private System.Timers.Timer timer = new System.Timers.Timer();

        public DisplayEngine() 
        {
            this.timer.Interval = 1000;
            this.timer.Elapsed += Timer_Elapsed;
        }

        public event NewImageEvent OnNewImage;

        public event EventHandler<EngineStatus> EngineStatusNotify;

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            EngineStatusNotify?.Invoke(this, EngineStatus.Running);
        }

        Queue<ImageLayer> layers = new Queue<ImageLayer>();

        static ManualResetEvent oStopEvent = new ManualResetEvent(false);

        public void PushImage(Bitmap bitmap, int expositionDuration)
        {
            // TODO : put grayslace filter here ?
            layers.Enqueue(new ImageLayer(bitmap, expositionDuration));
        }

        private static void ThreadProc(object obj)
        {
            DisplayEngine de = (DisplayEngine)obj;

            Thread.CurrentThread.IsBackground = true;

            de.timer.Start();
            de.EngineStatusNotify?.Invoke(de, EngineStatus.Started);

            while (de.layers.Count > 0)
            {
                //if (oStopEvent.WaitOne() == true)
                {
                //    return;
                }

                ImageLayer il = de.layers.Dequeue();

                Bitmap b = il.GetBitmap();

                de.OnNewImage?.Invoke(b);

                //de.frmDisplay.setImage(b);
                //de.frmDisplay.setImage(b);

                int iter = il.GetExpositionDuration() / 1000; // convert in seconds

                for (int i = 0; i < iter; i++)
                {
                    Thread.Sleep(1000);

                    if (de._stop)
                    {
                        break;
                    }
                }
            }

            de.timer.Stop();
            de.EngineStatusNotify?.Invoke(de, EngineStatus.Ended);
        }

        public void Start() 
        {
            _stop = false;

            this.thread = new Thread(ThreadProc);

            oStopEvent.Reset();
            this.thread.Start((object)this);
        }

        private bool _stop = false; // TODO do an FSM ?

        public void Stop() 
        {
            if (this.thread == null) return;
            if (this.thread.ThreadState == ThreadState.Stopped)
            {
                return;
            }

            _stop = true;

            oStopEvent.Set();
            this.thread.Join();
            this.EngineStatusNotify?.Invoke(this, EngineStatus.Stopped);
        }

        public void Pause() { }
        public void Next() { }
        public void Reset() 
        { 
        }
    }
}
