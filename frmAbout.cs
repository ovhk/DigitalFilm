using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalDarkroom
{
    /// <summary>
    /// 
    /// </summary>
    public partial class frmAbout : Form
    {
        /// <summary>
        /// 
        /// </summary>
        public frmAbout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAbout_Load(object sender, EventArgs e)
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version; 
            string about = string.Format(CultureInfo.InvariantCulture, @"Version : {0}.{1}.{2}.{3}", v.Major, v.Minor, v.Build, v.Revision);
            this.lbVersion.Text = about;

            this.lbTitle.Text = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;

            try
            {
                this.tbLicense.Text = System.IO.File.ReadAllText(@"LICENSE");
            }
            catch 
            {
                this.tbLicense.Text = "See LICENSE file.";
            }
        }
    }
}
