using MultipleDesktop.Application.View;
using MultipleDesktop.Application.ViewModel;
using MultipleDesktop.Mvc.Controller;
using MultipleDesktop.Mvc.View;
using MultipleDesktop.Windows.PresentationFoundation.View;
using System;
using System.Windows;
using uWindowState = MultipleDesktop.Mvc.View.WindowState;
using WpfWindowState = System.Windows.WindowState;

namespace MultipleDesktop.Windows.PresentationFoundation
{
    public partial class MainWindow : Window, IAppView, IMainView
    {
        // NOTE: it is assumed that _canMinimize and _canMaximize
        // will be true by default. to determine whether or not
        // thare actually ARE true by default is much too complicated
        // and this their respective properties are mainly used
        // for their setters.
        private bool _canMinimize = true;
        private bool _canMaximize = true;

        IAppController IAppView.Controller
        {
            get { return null; }
            set { }
        }

        public IMainViewModel ViewModel
        {
            get { return DataContext as IMainViewModel; }
            set
            {
                DataContext = value;

                if (value == null)
                    return;

                CanMinimize = value.CanMinimize;
                CanMaximize = value.CanMaximize;
            }
        }

        uWindowState IAppView.WindowState
            => (uWindowState)WindowState;

        public bool CanMinimize
        {
            get { return _canMinimize; }
            set
            {
                if (_canMinimize == value)
                    return;

                this.SetCanMinimize(value);
                _canMinimize = value;
            }
        }

        public bool CanMaximize
        {
            get { return _canMaximize; }
            set
            {
                if (_canMaximize == value)
                    return;

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
