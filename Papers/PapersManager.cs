using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DigitalFilm.Papers
{
    public class PapersManager
    {
        #region Singleton & Constructor

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static PapersManager _instance;

        /// <summary>
        /// Lock object
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// Get the instance of the singleton
        /// </summary>
        /// <returns></returns>
        public static PapersManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new PapersManager();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Papers Manager private constructor, use only the GetInstance
        /// </summary>
        private PapersManager()
        {

        }

        #endregion

        /// <summary>
        /// List of papers
        /// Add new papers here !
        /// </summary>
        public static List<Paper> Papers = new List<Paper>
                                    {
                                        // Add here new paper Class!
                                        new FomaspeedVariantIIIGlossyG0(),
                                        new FomaspeedVariantIIIVelvetG0(),
                                        new FomaspeedVariantIIIMattG0(),
                                        new FomaspeedVariantIIIGlossyG1(),
                                        new FomaspeedVariantIIIGlossyG2(),
                                        new FomaspeedVariantIIIGlossyG3(),
                                        new FomaspeedVariantIIIGlossyG4(),
                                        new FomaspeedVariantIIIGlossyG5()
                                    };

        /// <summary>
        /// Read data file and compute data
        /// </summary>
        public void Load()
        {
            foreach (Paper p in PapersManager.Papers)
            {
                _ = Task.Run(() =>
                {
                    if (p.Load() == false)
                    {
                        Log.WriteLine("Fail to Load paper data : " + p.Name);
                        _ = Papers.Remove(p);
                    }
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            StringBuilder s = new StringBuilder();

            _ = s.Append("Index");
            _ = s.Append(";");

            foreach (Paper p in PapersManager.Papers)
            {
                _ = s.Append(p.GetType().Name);
                _ = s.Append(";");
                _ = s.Append("Inv-");
                _ = s.Append(p.GetType().Name);
                _ = s.Append(";");
            }

            _ = s.Append("0.7");
            _ = s.Append(";");
            _ = s.Append("1/0.7");
            _ = s.Append("\r\n");

            for (int i = 0; i < 256; i++)
            {
                _ = s.Append(i);
                _ = s.Append(";");

                foreach (Paper p in PapersManager.Papers)
                {
                    _ = s.Append(p.DataToPaper?[i]);
                    _ = s.Append(";");
                    _ = s.Append(p.DataFromPaper?[i]);
                    _ = s.Append(";");
                }

                _ = s.Append(1d * Math.Pow((255d - i) / 255d, 0.7) * 255d);
                _ = s.Append(";");
                _ = s.Append(1d * Math.Pow((255d - i) / 255d, 1d / 0.7) * 255d);
                _ = s.Append("\r\n");
            }

            File.WriteAllText(fileName, s.ToString());
        }
    }
}
