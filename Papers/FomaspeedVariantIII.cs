using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalFilm.Papers
{
    /// <summary>
    /// // Data are extracted from https://www.foma.cz/en/fomaspeed-variant-III with https://automeris.io/WebPlotDigitizer/
    /// </summary>
    internal class FomaspeedVariantIIIGlossyG0 : Paper
    {
        public override string Name => "Fomaspeed Variant III, Glossy, Grade 0";
        public override string Description => "Fomaspeed Variant III - Grade 0 extra soft";
        protected override double[] RelativeLogExposure => new double[] { 0.1, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.1 };
        protected override double[] Density => new double[] { 0.1, 0.11, 0.3, 0.6, 1, 1.4, 1.75, 1.8, 1.9 };
        
    }

    /// <summary>
    /// // Data are extracted from https://www.foma.cz/en/fomaspeed-variant-III with https://automeris.io/WebPlotDigitizer/
    /// </summary>
    internal class FomaspeedVariantIIIGlossyG1 : Paper
    {
        public override string Name => "Fomaspeed Variant III, Glossy, Grade 1";
        public override string Description => "Fomaspeed Variant III - Grade 1 soft";
        protected override double[] RelativeLogExposure => new double[] { 0.1, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.1 };
        protected override double[] Density => new double[] { 0.1, 0.11, 0.3, 0.6, 1, 1.4, 1.75, 1.8, 1.9 };
    }
}
