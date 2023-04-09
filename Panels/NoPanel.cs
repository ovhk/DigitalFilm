using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalDarkroom.Panels
{
    internal class NoPanel : IPanel
    {
        private string _name;
        string IPanel.Name => this._name;

        private int width;

        int IPanel.Width => this.width;

        private int height;
        int IPanel.Height => this.height;

        int IPanel.NumberOfColors => 256;

        bool IPanel.IsFullScreen => false;

        public override string ToString()
        {
            return this._name;
        }

        public NoPanel()
        {
        }

        public NoPanel(MyPictureBox pb)
        {
            this.height = pb.Height;
            this.width = pb.Width;
            this._name = "No Panel";
        }
    }
}
