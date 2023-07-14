﻿using DigitalFilm.Tools;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace DigitalFilm.Controls
{
    /// <summary>
    /// 
    /// </summary>
    internal class ImageFileNameEditor : UITypeEditor
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly OpenFileDialog _ofd = new OpenFileDialog();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider serviceProvider, object value)
        {
            this._ofd.FileName = value as string;
            this._ofd.Filter = ImageFileFilter.GetImageFilter();

            return this._ofd.ShowDialog() == DialogResult.OK ? this._ofd.FileName : base.EditValue(context, serviceProvider, value);
        }
    }
}
