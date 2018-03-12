using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultipleDesktop.Windows.PresentationFoundation.Application
{
    public class MainViewModel
    {
        public string Title => "Main Title";
    }

    public static class CompositionRoot
    {
        public static Window Compose()
        {
            var window = new MainWindow();
            window.DataContext = new MainViewModel();

            return window;
        }
    }
}
