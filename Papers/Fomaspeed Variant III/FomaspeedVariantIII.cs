namespace DigitalFilm.Papers
{
    #region Fomaspeed Variant III Grade 0

    internal class FomaspeedVariantIIIVelvetG0 : FomaspeedVariantIIIGlossyG0
    {
        public override string Name => "Fomaspeed Variant III, Velvet, Grade 0";

        /// <summary>
        /// Data from Foma, thanks!
        /// </summary>
        protected override double NewDmax => 1.95d;
    }

    internal class FomaspeedVariantIIIMattG0 : FomaspeedVariantIIIGlossyG0
    {
        public override string Name => "Fomaspeed Variant III, Matt, Grade 0";

        /// <summary>
        /// Data from Foma, thanks!
        /// </summary>
        protected override double NewDmax => 1.75d;
    }

    /// <summary>
    /// Data are extracted from https://www.foma.cz/en/fomaspeed-variant-III with https://automeris.io/WebPlotDigitizer/
    /// Sorted by X Ascending 
    /// </summary>
    internal class FomaspeedVariantIIIGlossyG0 : Paper
    {
        public override string Name => "Fomaspeed Variant III, Glossy, Grade 0";
        public override string RawDataFileName => "Papers/Fomaspeed Variant III/0 - extra soft.csv";
    }

    #endregion

    #region Glossy G1

    /// <summary>
    /// // Data are extracted from https://www.foma.cz/en/fomaspeed-variant-III with https://automeris.io/WebPlotDigitizer/
    /// </summary>
    internal class FomaspeedVariantIIIGlossyG1 : Paper
    {
        public override string Name => "Fomaspeed Variant III, Glossy, Grade 1";
        public override string RawDataFileName => "Papers/Fomaspeed Variant III/1 - soft.csv";

    
    }

    #endregion

    #region Glossy G2

    /// <summary>
    /// // Data are extracted from https://www.foma.cz/en/fomaspeed-variant-III with https://automeris.io/WebPlotDigitizer/
    /// </summary>
    internal class FomaspeedVariantIIIGlossyG2 : Paper
    {
        public override string Name => "Fomaspeed Variant III, Glossy, Grade 2";
        public override string RawDataFileName => @"Papers/Fomaspeed Variant III/2 - special.csv";
    }

    #endregion

    #region Glossy G3

    /// <summary>
    /// // Data are extracted from https://www.foma.cz/en/fomaspeed-variant-III with https://automeris.io/WebPlotDigitizer/
    /// </summary>
    internal class FomaspeedVariantIIIGlossyG3 : Paper
    {
        public override string Name => "Fomaspeed Variant III, Glossy, Grade 3";
        public override string RawDataFileName => @"Papers/Fomaspeed Variant III/3 - normal.csv";
    }

    #endregion

    #region Glossy G4

    /// <summary>
    /// // Data are extracted from https://www.foma.cz/en/fomaspeed-variant-III with https://automeris.io/WebPlotDigitizer/
    /// </summary>
    internal class FomaspeedVariantIIIGlossyG4 : Paper
    {
        public override string Name => "Fomaspeed Variant III, Glossy, Grade 4";
        public override string RawDataFileName => @"Papers/Fomaspeed Variant III/4 - hard.csv";
    }

    #endregion

    #region Glossy G5

    /// <summary>
    /// // Data are extracted from https://www.foma.cz/en/fomaspeed-variant-III with https://automeris.io/WebPlotDigitizer/
    /// </summary>
    internal class FomaspeedVariantIIIGlossyG5 : Paper
    {
        public override string Name => "Fomaspeed Variant III, Glossy, Grade 5";
        public override string RawDataFileName => @"Papers/Fomaspeed Variant III/5 - extra hard.csv";
    }

    #endregion
}
