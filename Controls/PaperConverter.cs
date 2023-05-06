using DigitalFilm.Papers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DigitalFilm.Controls
{
    /// <summary>
    /// 
    /// </summary>
    class PaperConverter : TypeConverter
    {
        /// <summary>
        /// List of papers
        /// </summary>
        private readonly List<Paper> _papers = new List<Paper>
        {
            // TODO ! move list somewhere else ?
            // Add here new papers
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
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(_papers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                foreach (Paper p in _papers)
                {
                    if (p.Name == (string)value)
                    {
                        return p;
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
