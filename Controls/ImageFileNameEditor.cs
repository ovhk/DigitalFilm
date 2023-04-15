﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DigitalDarkroom.Tools;

namespace DigitalDarkroom.Controls
{
    /// <summary>
    /// 
    /// </summary>
    internal class ImageFileNameEditor : UITypeEditor
    {
        /// <summary>
        /// 
        /// </summary>
        private OpenFileDialog _ofd = new OpenFileDialog();

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

            if (this._ofd.ShowDialog() == DialogResult.OK)
            {
                return this._ofd.FileName;
            }

            return base.EditValue(context, serviceProvider, value);
        }
    }
}