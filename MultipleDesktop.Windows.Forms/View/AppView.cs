using MultipleDesktop.Application.View;
using MultipleDesktop.Application.ViewModel;
using MultipleDesktop.Mvc.Controller;
using MultipleDesktop.Mvc.View;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using uApplication = System.Windows.Forms.Application;

namespace MultipleDesktop.Windows.Forms.View
{
    public partial class AppView : Form, IAppView, IMainView
    {
        private bool _didLoad;

        private IAppController _controller;
        private IMainViewModel _viewModel;

        IAppController IAppView.Controller
        {
            get { return _controller; }
            set
            {
                if (Equals(_controller, value))
                    return;

                if (_controller != null)
                {
                    _controller.PropertyChanged -= Controller_PropertyChanged;
                }

                _controller = value;

                if (value != null)
                {
                    value.PropertyChanged += Controller_PropertyChanged;
                }
            }
        }

        public IMainViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (_viewModel == value)
                    return;

                _viewModel = value;

                if (value == null)
                    return;

                Text = value.Title;
            }
        }

        private WindowState _windowState;
        public new WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                if (_windowState == value)
                    return;

                _windowState = value;

                WindowStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        bool IAppView.CanMinimize
        {
            get { return MinimizeBox; }
            set { MinimizeBox = value; }
        }

        bool IAppView.CanMaximize
        {
            get { return MaximizeBox; }
            set { MaximizeBox = value; }
        }

        event EventHandler IAppView.Loaded
        {
            add { Load += value; }
            remove { Load -= value; }
        }

        public event EventHandler WindowStateChanged;

        private CancelEventHandler _closing;
        event CancelEventHandler IAppView.Closing
        {
            add { _closing += value; }
            remove { _closing -= value; }
        }

        public AppView()
        {
            InitializeComponent();

            BuildNotifyIcon();

            FormClosing += AppView_FormClosing;
            SizeChanged += AppView_SizeChanged;
        }

        private void AppView_SizeChanged(object sender, EventArgs e)
            => WindowState = (WindowState)base.WindowState;

        private void BuildNotifyIcon()
        {
            lblCopy.Text = "\u00A9 Blondy";

            notifyIcon1.Visible = true;
            notifyIcon1.Text = "Virtual Desktop";
            notifyIcon1.Icon = Properties.Resources._virtual;

            var contextMenu = new ContextMenu();

            contextMenu.MenuItems.Add("Show", (s, args) => ShowView());

            contextMenu.MenuItems.Add("Exit", (s, args) => uApplication.Exit());

            notifyIcon1.ContextMenu = contextMenu;
        }

        private void Controller_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IAppController.DesktopConfigurations):
                    UpdateConfigurations();
                    break;
            }
        }

        private void UpdateConfigurations()
            => this.InvokeIfRequired(view =>
            {
                backgroundConfigurationFlowLayoutPanel.Controls.Clear();

                foreach (var configuration in _controller.DesktopConfigurations)
                {
                    backgroundConfigurationFlowLayoutPanel.Controls.Add(new BackgroundConfigurationView(configuration));
                }
            });

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _didLoad = true;
        }

        public void ShowView()
        {
            if (_didLoad) Show();
            ShowInTaskbar = true;
            base.WindowState = FormWindowState.Normal;
        }

        void IAppView.HideView()
        {
            base.WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            if (_didLoad) Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
            => ShowView();

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _controller.Save();
        }

        private void AppView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            _closing?.Invoke(sender, e);
        }
    }
}
