using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Data = new int[256];

            // https://numerics.mathdotnet.com/Interpolation
            var i = Interpolate.LogLinear(ScaledDensity, RelativeLogExposure);

            for (int j = 0; j < Data.Length; j++) // TODO : voir qui est à l'endroit et à l'envers
            {
                Data[j] = Convert.ToInt32(1d * Math.Pow((double)j / 255, i.Interpolate(j)) * 255);
                Log.WriteLine("Data: {0}={1}", j, Data[j]);
            }

            InvertedData = new int[256];

            for (int j = 0; j < InvertedData.Length; j++) // TODO : voir qui est à l'endroit et à l'envers
            {
                InvertedData[255-j] = Convert.ToInt32(1d * Math.Pow((double)j / 255, i.Interpolate(j)) * 255);
                Log.WriteLine("InvertedData: {0}={1}", j, InvertedData[255-j]);
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
