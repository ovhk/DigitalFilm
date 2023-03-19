using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Modes
{
    internal class Mode3 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name => "Mode 3";

        public string Description => "";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IMode.Load(string[] imgPaths, int duration)
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            Bitmap b = new Bitmap(engine.Width, engine.Height);

            Graphics g = Graphics.FromImage(b);

            drawFibonacci(g);

            engine.PushImage(b, duration);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Unload()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();
            engine.Clear();

            return true;
        }

        private void drawFibonacci(Graphics g)
        {
            // the current fibonacci numbers
            int current = 1;
            int previous = 0;

            // the current bounding box
            int left = 0;
            int right = 1;
            int top = 0;
            int bottom = 0;

            // the number of boxes you want to draw
            const int N = 10;

            for (int i = 0; i < N; i++)
            {
                switch (i % 4)
                {
                    case 0: // attach to bottom of current rectangle
                        drawRectangle(g, left, right, bottom, bottom + current, i);
                        bottom += current;
                        break;
                    case 1: // attach to right of current rectangle
                        drawRectangle(g, right, right + current, top, bottom, i);
                        right += current;
                        break;
                    case 2: // attach to top of current rectangle
                        drawRectangle(g, left, right, top - current, top, i);
                        top -= current;
                        break;
                    case 3: // attach to left of current rectangle
                        drawRectangle(g, left - current, left, top, bottom, i);
                        left -= current;
                        break;
                }

                // update fibonacci number
                int temp = current;
                current += previous;
                previous = temp;
            }
        }



        private void drawRectangle(Graphics g, int left, int right, int top, int bottom, int iteration)
        {
            const int SCALE = 10;
            const int OFFSET = 150;

            DisplayEngine engine = DisplayEngine.GetInstance();

            //int OFFSET = engine.Width / 2;

            g.DrawRectangle(Pens.Red, new Rectangle(SCALE * left + OFFSET,
                                                    SCALE * top + OFFSET,
                                                    SCALE * (right - left),
                                                    SCALE * (bottom - top)));

            int a = iteration;

            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                g.FillRectangle(brush, new Rectangle(SCALE * left + OFFSET + a,
                                                        SCALE * top + OFFSET + a,
                                                        SCALE * (right - left) - 2*a,
                                                        SCALE * (bottom - top) - 2*a ));
            }

        }
    }
}
