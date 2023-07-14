using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DigitalFilm.Papers
{
    /// <summary>
    /// Abstract paper class
    /// </summary>
    public abstract class Paper
    {
        #region Abstract parameters

        protected virtual double NewDmax { get; }
        public abstract string Name { get; }
        public abstract string RawDataFileName { get; }

        #endregion

        private double[] ScaledDensity
        {
            get
            {
                List<double> list = new List<double>();
                foreach (PaperDataItem paperDataItem in this.paperDataItems)
                {
                    list.Add(paperDataItem.ScaledDensity);
                }
                return list.ToArray();
            }
        }

        public double ContrastRatio => Math.Pow(10, this.Dmax - this.Dmin);

        public double Gamma => (this.DmaxLinear - this.DminLinear) / (this.RLEmax - this.RLEmin);

        public int ISO // TODO à tester
        {
            get
            {
                double Hm = 0;
                foreach (PaperDataItem paperDataItem in this.paperDataItems)
                {
                    if (paperDataItem.Density.Equals(this.DminLinear))
                    {
                        Hm = paperDataItem.RelativeLogExposure;
                        break;
                    }
                }
                return Convert.ToInt32((this.DmaxLinear - this.DminLinear) / Hm);
            }
        }

        public int ISO2 // TODO à tester
        {
            get
            {
                double Hm = 0; // TODO fill Hm

                return Convert.ToInt32(0.8 / Hm);
            }
        }

        private double[] Density
        {
            get
            {
                List<double> list = new List<double>();
                foreach (PaperDataItem paperDataItem in this.paperDataItems)
                {
                    list.Add(paperDataItem.Density);
                }
                return list.ToArray();
            }
        }

        private double[] RelativeLogExposure
        {
            get
            {
                List<double> list = new List<double>();
                foreach (PaperDataItem paperDataItem in this.paperDataItems)
                {
                    list.Add(paperDataItem.RelativeLogExposure);
                }
                return list.ToArray();
            }
        }

        public int[] DataFromPaper
        { get; private set; }

        public int[] DataToPaper
        { get; private set; }


        #region Compute function

        /// <summary>
        /// 
        /// </summary>
        private class PaperDataItem
        {
            // raw data from csv
            public double Density;
            public double RelativeLogExposure;

            // calculated data
            public double ScaledDensity;
        }

        /// <summary>
        /// 
        /// </summary>
        private class PaperDataItemComparer : IComparer<PaperDataItem>
        {
            public int Compare(PaperDataItem x, PaperDataItem y)
            {
                if (x.RelativeLogExposure > y.RelativeLogExposure)
                    return 1;

                if (x.RelativeLogExposure < y.RelativeLogExposure)
                    return -1;

                else
                    return 0;
            }
        }

        private readonly List<PaperDataItem> paperDataItems = new List<PaperDataItem>();

        private double Dmin = double.MinValue; // init to MinValue
        private double Dmax = double.MinValue; // init to MinValue

        private double RLEmin = double.MinValue; // init to MinValue
        private double RLEmax = double.MinValue; // init to MinValue

        private double DminLinear = double.MinValue; // init to MinValue
        private double DmaxLinear = double.MinValue; // init to MinValue

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            string filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + RawDataFileName;

            if (File.Exists(filepath) == false)
            {
                return false;
            }

            // 1. Read and parse CVS file

            string[] lines = File.ReadAllLines(filepath);

            foreach (string line in lines)
            {
                string[] data = line.Split(';');

                double Density = 0;

                bool valid = double.TryParse(data[0].Trim(), out double RelativeLogExposure)
                                && double.TryParse(data[1].Trim(), out Density);

                if (valid)
                {
                    paperDataItems.Add(new PaperDataItem()
                    {
                        Density = Density,
                        ScaledDensity = 0,
                        RelativeLogExposure = RelativeLogExposure,
                    });

                    if (this.Dmin > Density || this.Dmin == Double.MinValue)
                    {
                        this.Dmin = Density;
                    }

                    if (this.Dmax < Density || this.Dmax == Double.MinValue)
                    {
                        this.Dmax = Density;
                    }
                }
            }

            // 2. Sort data

            this.paperDataItems.Sort(new PaperDataItemComparer());

#if TODO_FILTER_DENSITY
            double[] filteredDensity;

            // change Dmax for other type of surface
            // TODO : maybe other solution is just to push exposure time ???
            if (this.NewDmax != 0)
            {
                filteredDensity = new double[Density.Length];

                double trig = Density.Max() - (Density.Max() - this.NewDmax) * 2d;

                trig = (trig > 0) ? trig : 0;

                for (int i = 0; i < Density.Length; i++)
                { 
                    if (Density[i] > trig)
                    {
                        double delta = this.NewDmax - Density[i];
                        //Log.Write("Density[i]={0} ", Density[i]);
                        double newDensity = Density[i] - Math.Sqrt(delta); // TODO : faire la courbe pour voir la tête du filtre
                        newDensity = (newDensity > 0) ? newDensity : this.NewDmax;
                        filteredDensity[i] = newDensity;
                        //Log.WriteLine("NewDensity[i]={0}", newDensity);
                    }
                    else
                    {
                        filteredDensity[i] = Density[i];
                    }
                }
            }
            else
            {
                filteredDensity = Density;
            }
#endif

            // 3. Filter data

            // Following these articles, max exposure latitude seems to be from Dmin + 0.1 to Dmax - 0.1
            // It seems arbitrary but it allows to get rid of the toe (underexposed) and shoulder (overexposed) region, so lets do this
            // https://en.wikipedia.org/wiki/Sensitometry
            // https://en.wikipedia.org/wiki/Gamma_correction
            // https://fr.wikipedia.org/wiki/Absorbance
            // https://fr.wikipedia.org/wiki/Gamma_(photographie)
            // https://ru.wikipedia.org/wiki/%D0%A4%D0%BE%D1%82%D0%BE%D0%B3%D1%80%D0%B0%D1%84%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B0%D1%8F_%D1%88%D0%B8%D1%80%D0%BE%D1%82%D0%B0
            // https://ru.wikipedia.org/wiki/%D0%A1%D0%B2%D0%B5%D1%82%D0%BE%D1%87%D1%83%D0%B2%D1%81%D1%82%D0%B2%D0%B8%D1%82%D0%B5%D0%BB%D1%8C%D0%BD%D0%BE%D1%81%D1%82%D1%8C_%D1%84%D0%BE%D1%82%D0%BE%D0%BC%D0%B0%D1%82%D0%B5%D1%80%D0%B8%D0%B0%D0%BB%D0%BE%D0%B2

            // https://www.photo.net/forums/topic/535264-film-characteristic-curves-and-logluxsec/
            // https://www.filmshooterscollective.com/analog-film-photography-blog/a-practical-guide-to-using-film-characteristic-curves-12-25

            // https://www.filmlabs.org/docs/cours_sensitometrie.pdf

            // https://www.souvenirsdephotographe.fr/technique/gradationpapier.html

            double M = this.Dmin + 0.1;
            double N = this.Dmax - 0.1;

            for (int i = paperDataItems.Count - 1; i >= 0; i--) // reverse iterate to go through all datas
            {
                PaperDataItem item = paperDataItems[i];

                if ((item.Density < M) || (item.Density > N))
                {
                    paperDataItems.Remove(item);
                }
            }

            // update Min/Max on linear part
            foreach (PaperDataItem item in paperDataItems)
            {
                if (this.DminLinear > item.Density || this.DminLinear == double.MinValue)
                {
                    this.DminLinear = item.Density;
                }

                if (this.DmaxLinear < item.Density || this.DmaxLinear == double.MinValue)
                {
                    this.DmaxLinear = item.Density;
                }

                if (this.RLEmin > item.RelativeLogExposure || this.RLEmin == double.MinValue)
                {
                    this.RLEmin = item.RelativeLogExposure;
                }

                if (this.RLEmax < item.RelativeLogExposure || this.RLEmax == double.MinValue)
                {
                    this.RLEmax = item.RelativeLogExposure;
                }
            }

            // 4. Scale data to 0-255

            foreach (PaperDataItem item in paperDataItems)
            {
                item.ScaledDensity = (item.Density - this.DminLinear) / (this.DmaxLinear - this.DminLinear) * 255d;
            }

            // 5. Compute Transfert Function

            // https://numerics.mathdotnet.com/Interpolation
            // We have lots of points so Linear should be ok.           
            LinearSpline ii = LinearSpline.InterpolateSorted(this.ScaledDensity, this.RelativeLogExposure);

            DataToPaper = new int[256];
            DataFromPaper = new int[256];

            for (int x = 0; x < DataToPaper.Length; x++)
            {
                double y = ii.Interpolate(x);

                if (y < 0 || y is double.NaN)
                {
                    // If NaN, you probably have the same Y for 2 different X
                    Log.WriteLine("Load paper : " + this.Name + ", Something goes wrong with the interpolation...");
                    return false;
                }

                // https://ieeexplore.ieee.org/stamp/stamp.jsp?arnumber=7238748

                double res = (y - this.RLEmin) / (this.RLEmax - this.RLEmin);
                double resInv = Math.Pow((double)(255d - x) / 255d, res); // antilog
                ///double resInv = Math.Pow((double)x / 255d, res); // antilog             SI ça, alors il faut DataToPaper[255 - x]

                // 255-x to invert black and white
                // X =   0 = black
                // X = 255 = white
                DataToPaper[x] = Convert.ToInt32(resInv * 255d);
                DataFromPaper[255 - x] = Convert.ToInt32(res * 255d);
            }

            Log.WriteLine("ISO : " + ISO);

            return true;
        }

        #endregion

        /// <summary>
        /// Return the name of the paper
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
