using MathNet.Numerics;
using System;
using System.Linq;
using System.Windows.Forms.VisualStyles;

namespace DigitalFilm.Papers
{
    /// <summary>
    /// Abstract paper class
    /// </summary>
    public abstract class Paper
    {
        #region Abstract parameters

        protected abstract double[] Density { get; }
        protected abstract double[] RelativeLogExposure { get; }
        protected virtual double NewDmax { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public Paper()
        {
            ComputeDensity();
            ComputeTransfertFunction();
        }

        protected double[] ScaledDensity { get; private set; }

        public int[] InvertedData
        { get; private set; }

        public int[] Data
        { get; private set; }


        #region Compute function

        /// <summary>
        /// 
        /// </summary>
        private void ComputeDensity()
        {
            double[] filteredDensity;

            // change Dmax for other type of surface
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

            //ScaledDensity = new double[Density.Length];

            //for (int i = 0; i < ScaledDensity.Length; i++)
            //{
            //    ScaledDensity[i] = (Density[i] - Density.Min()) / (Density.Max() - Density.Min()) * 255;
            //}

            ScaledDensity = new double[filteredDensity.Length];

            for (int i = 0; i < ScaledDensity.Length; i++)
            {
                ScaledDensity[i] = (filteredDensity[i] - filteredDensity.Min()) / (filteredDensity.Max() - filteredDensity.Min()) * 255d;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeTransfertFunction()
        {
            // TODO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // [ ] entre le fromPaper et le toPaper : Data ou InvertedData : générer la courbe pour voir si on est dans le sens du 0.7 (recherché) ou 1.4
            // [ ]  NaN ?

            // https://numerics.mathdotnet.com/Interpolation

            // We have lots of points so Linear should be ok.
            var i = Interpolate.Linear(ScaledDensity, RelativeLogExposure);
            //var i = Interpolate.LogLinear(ScaledDensity, RelativeLogExposure);
            //var i = Interpolate.RationalWithoutPoles(ScaledDensity, RelativeLogExposure);

            Data = new int[256];
            InvertedData = new int[256];

            for (int j = 0; j < Data.Length; j++)
            {
                double inter = i.Interpolate((double)j);
                inter = (inter is double.NaN) ? 255 : inter; // TODO : bof... 
                double res    = 1d * Math.Pow((double)j / 255d, inter);
                double resInv = 1d * Math.Pow((double)j / 255d, 1d / inter);

                //Log.WriteLine("Cacl=" + inter + "/" + res + "/" + res2);

                // 255-j to invert black and white
                Data[255 - j]         = Convert.ToInt32(res * 255d);
                InvertedData[255 - j] = Convert.ToInt32(resInv * 255d);

                //Log.WriteLine("Data: {0:000} : {1:000} / {2:000}", j, Data[255-j], InvertedData[255 - j]);
            }
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
