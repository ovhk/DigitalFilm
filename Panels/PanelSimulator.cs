﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DigitalFilm.Panels
{
    public class PanelSimulator : IPanel
    {
        private string _name;
        string IPanel.Name => this._name;

        int IPanel.Width => 1024;

        int IPanel.Height => 768;

        int IPanel.NumberOfColors => 256;

        int IPanel.ResponseTime => 40; // TODO : 60 Hz so consider x2 = 33 ms ????

        bool IPanel.IsFullScreen => false;

        public override string ToString()
        {
            return this._name;
        }

        public PanelSimulator()
        {
            this._name = "Simulator";
        }
    }
}
