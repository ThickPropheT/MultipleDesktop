using System.Drawing;

namespace MultipleDesktop.Mvc.Desktop
{
    public interface IBackground
    {
        string Path { get; }

        Fit Fit { get; }
    }
}
