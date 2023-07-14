using DigitalFilm.Controls;

namespace DigitalFilm.Panels
{
    internal class NoPanel : IPanel
    {
        private readonly string _name;
        string IPanel.Name => this._name;

        private readonly int width;

        int IPanel.Width => this.width;

        private readonly int height;
        int IPanel.Height => this.height;

        int IPanel.NumberOfColors => 256;

        int IPanel.ResponseTime => 40;

        bool IPanel.IsFullScreen => false;

        double IPanel.RatioCorrection => 1d;

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
            this._name = "In App";
        }
    }
}
