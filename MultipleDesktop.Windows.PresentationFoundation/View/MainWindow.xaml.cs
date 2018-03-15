using MultipleDesktop.Mvc.Controller;
using MultipleDesktop.Mvc.View;
using MultipleDesktop.Windows.PresentationFoundation.View;
using System;
using System.Windows;
using uWindowState = MultipleDesktop.Mvc.View.WindowState;

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

        private EventHandler _sizeChanged;
        event EventHandler IAppView.SizeChanged
        {
            add { _sizeChanged += value; }
            remove { _sizeChanged -= value; }
        }

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;

            // TODO fix problems below.
            // SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
            => _loaded?.Invoke(sender, e);

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            throw new NotImplementedException("Only the Forms implementation does something meaningful here. Find a way to reconcile this with Wpf implementation.");

            _sizeChanged?.Invoke(sender, e);
        }

        void IAppView.HideView()
        {
            throw new NotImplementedException("Doing this makes the window permanently minimized.");

            // TODO ?? ShowInTaskbar = false; ?? >> See AppView
            Hide();
        }

        void IAppView.ShowView()
        {
            Show();
        }
    }
}
