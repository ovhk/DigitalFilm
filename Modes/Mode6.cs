﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DigitalDarkroom.Tools;

namespace DigitalDarkroom.Modes
{
    internal class Mode6 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name => "GrayToTime picture";

        public string Description => "Display the selected picture in 256 B&W pictures with GrayToTime algorithm.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public bool Load(string[] imgPaths, int duration)
        {
            if (imgPaths == null || imgPaths.Length == 0) return false;

            DisplayEngine engine = DisplayEngine.GetInstance();

            Task t = Task.Run(() => {

                // this way permit to not lock the file : https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
                using (var bmpTemp = new Bitmap(imgPaths[0]))
                {
                    Size sz = new Size(engine.Panel.Width, engine.Panel.Height);
                    Bitmap origin = GrayScale.MakeGrayscale3(new Bitmap(bmpTemp, sz));

                    List<ImageLayer> ils = GrayToTime.GetImageLayers(origin, engine.Panel.Width, engine.Panel.Height);

                    foreach (ImageLayer il in ils)
                    {
                        engine.PushImage(il);
                    }
                }
            });

            t.Wait();

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
