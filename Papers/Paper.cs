using MathNet.Numerics;
using System;
using System.Linq;

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
            ScaledDensity = new double[Density.Length];

            for (int i = 0; i < ScaledDensity.Length; i++)
            {
                ScaledDensity[i] = (Density[i] - Density.Min()) / (Density.Max() - Density.Min()) * 255;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ComputeTransfertFunction()
        {
            // TODO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // - entre le fromPaper et le toPaper : Data ou InvertedData : générer la courbe pour voir si on est dans le sens du 0.7 (recherché) ou 1.4
            // NaN ?
            // - faire un filtre les autres type de papier : matt et velvet
            // Any other surface  –  matt causes a decrease in the maximum density value
            // Dmax=2.1

            {
                double velvet = 0.1;
                double matt = 0.2;

                // des que l'on arrive à val >= (max - 0.1) * 2, val += (max - val) / 2
                // 
            }

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
