using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
        [Browsable(false)]
        public string Name => "GrayToTime picture";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Display the selected picture in 256 B&W pictures with GrayToTime algorithm.";

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Source file to display")]
        [Editor(typeof(ImageFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ImagePath
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            if (ImagePath == null || ImagePath.Length == 0) return false;

            DisplayEngine engine = DisplayEngine.GetInstance();

            // test cache

            // if (IsInCache(ImagePath) == true)
            //{
            // LoadFromCache()
            //}

            string md5 = Tools.Checksum.CalculateMD5(ImagePath);

            string cachedir = @"cache\" + md5 + @"\" + engine.Panel.Name; // TODO : Add panel name in the path ?

            //if (Directory.Exists(cachedir) == true)
            //{
            //    foreach (string file in Directory.GetFiles(cachedir))
            //    {
            //        engine.PushImage(new ImageLayer(file)); // TODO : à tester
            //    }
            //}
            //else
            {
                //Directory.CreateDirectory(cachedir);

                // TODO : ou est-ce que l'on passe le md5 ...

                // end test

                Task t = Task.Run(() =>
                {

                    // this way permit to not lock the file : https://stackoverflow.com/questions/6576341/open-image-from-file-then-release-lock
                    using (var bmpTemp = new Bitmap(ImagePath))
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
            }

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

        /// <summary>
        /// Return the Name parameter
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
