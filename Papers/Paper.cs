using DigitalFilm.Tools;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms.VisualStyles;

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

        protected double[] ScaledDensity 
        { 
            get
            {
                List<double> list = new List<double>();
                foreach(PaperDataItem paperDataItem in this.paperDataItems) 
                {
                    list.Add(paperDataItem.ScaledDensity);
                }
                return list.ToArray();
            }
        }
        
        protected double[] RelativeLogExposure
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

        public int[] InvertedData
        { get; private set; }

        public int[] Data
        { get; private set; }


        #region Compute function

        /// <summary>
        /// 
        /// </summary>
        private class PaperDataItem
        {
            public double Density;
            public double ScaledDensity;
            public double RelativeLogExposure;
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

        private double Dmin = Double.MinValue;
        private double Dmax = Double.MinValue;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            string filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\" + this.RawDataFileName;

            if (File.Exists(filepath) == false)
            {
                return false;
            }

            // 1. Read and parse CVS file

            string[] lines = File.ReadAllLines(filepath);

            foreach (string line in lines)
            {
                string[] data = line.Split(';');

                double RelativeLogExposure = 0;
                double Density = 0;

                bool valid = double.TryParse(data[0].Trim(), out RelativeLogExposure) 
                                && double.TryParse(data[1].Trim(), out Density);

                if (valid)
                {
                    paperDataItems.Add(new PaperDataItem()
                    {
                        Density = Density,
                        ScaledDensity = 0,
                        RelativeLogExposure = RelativeLogExposure
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

            // 3. Scale Density to 0-255
            foreach (PaperDataItem item in paperDataItems)
            {
                item.ScaledDensity = (item.Density - this.Dmin) / (this.Dmax - this.Dmin) * 255d;
            }

            // 4. Compute Transfert Function

            // https://numerics.mathdotnet.com/Interpolation

            // We have lots of points so Linear should be ok.
            var i = LinearSpline.InterpolateSorted(this.ScaledDensity, this.RelativeLogExposure);
            //var i = Interpolate.LogLinear(ScaledDensity, RelativeLogExposure);
            //var i = Interpolate.RationalWithoutPoles(ScaledDensity, RelativeLogExposure);

            Data = new int[256];
            InvertedData = new int[256];

            for (int j = 0; j < Data.Length; j++)
            {
                double inter = i.Interpolate((double)j);
                inter = (inter is double.NaN) ? 255 : inter; // TODO : bof... !!!!!! en fait on a 2 valeur diff de X pour la même valeur de Y
                double res = 1d * Math.Pow((double)j / 255d, inter);
                double resInv = 1d * Math.Pow((double)j / 255d, 1d / inter);

                //Log.WriteLine("Cacl=" + inter + "/" + res + "/" + res2);

                // 255-j to invert black and white
                Data[255 - j] = Convert.ToInt32(res * 255d);
                InvertedData[255 - j] = Convert.ToInt32(resInv * 255d);

                //Log.WriteLine("Data: {0:000} : {1:000} / {2:000}", j, Data[255-j], InvertedData[255 - j]);
            }

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
