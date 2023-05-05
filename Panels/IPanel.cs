namespace DigitalFilm.Panels
{
    public interface IPanel
    {
        string Name { get; }
        int Width { get; }
        int Height { get; }
        int NumberOfColors { get; }
        int ResponseTime { get; } // Response time of the OnPaint in ms
        bool IsFullScreen { get; }
    }
}
