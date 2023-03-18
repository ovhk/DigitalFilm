using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Tests
{
    internal class Test1 : ITest
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name => "Test 1";

        /// <summary>
        /// 
        /// </summary>
        public Test1() { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool ITest.Load(int duration)
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            Bitmap b = new Bitmap(engine.Width, engine.Height);

            using (Graphics gfx = Graphics.FromImage(b))
            {

                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    gfx.FillRectangle(brush, 0, 0, engine.Width / 2, engine.Height);
                }

                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    gfx.FillRectangle(brush, engine.Width / 2, 0, engine.Width / 2, engine.Height);
                }
            }

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
    }
}
