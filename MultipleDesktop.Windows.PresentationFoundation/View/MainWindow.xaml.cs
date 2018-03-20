using MultipleDesktop.Mvc.Controller;
using MultipleDesktop.Mvc.View;
using MultipleDesktop.Windows.PresentationFoundation.View;
using System;
using System.Windows;
using uWindowState = MultipleDesktop.Mvc.View.WindowState;
using WpfWindowState = System.Windows.WindowState;

namespace MultipleDesktop.Windows.PresentationFoundation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAppView
    {
        IAppController IAppView.Controller
        {
            get { return (IAppController)DataContext; }
            set { DataContext = value; }
        }

        uWindowState IAppView.WindowState
            => (uWindowState)WindowState;

        // NOTE: this assumes _canMinimize to be true.
        // to determine whether or not it is true by default is much
        // too complicated and this property is mainly used for its setter.
        private bool _canMinimize = true;
        bool IAppView.CanMinimize
        {
            get { return _canMinimize; }
            set
            {
                this.SetCanMinimize(value);
                _canMinimize = value;
            }
        }

        // NOTE: this assumes _canMaximize to be true.
        // to determine whether or not it is true by default is much
        // too complicated and this property is mainly used for its setter.
        private bool _canMaximize = true;
        bool IAppView.CanMaximize
        {
            get { return _canMaximize; }
            set
            {
                this.SetCanMaximize(value);
                _canMaximize = value;
            }
        }

        private EventHandler _loaded;
        event EventHandler IAppView.Loaded
        {
            add { _loaded += value; }
            remove { _loaded -= value; }
        }

        event EventHandler IAppView.WindowStateChanged
        {
            add { StateChanged += value; }
            remove { StateChanged -= value; }
        }

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
            => _loaded?.Invoke(sender, e);

        void IAppView.HideView()
        {
            WindowState = WpfWindowState.Minimized;

            return;
            throw new NotImplementedException("Doing this makes the window permanently minimized.");

            ShowInTaskbar = false;
            Hide();
        }

        void IAppView.ShowView()
        {
            Show();
            ShowInTaskbar = true;
            WindowState = WpfWindowState.Normal;
        }
    }
}
