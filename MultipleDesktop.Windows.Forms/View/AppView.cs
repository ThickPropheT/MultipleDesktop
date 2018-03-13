using MultipleDesktop.Mvc.Controller;
using MultipleDesktop.Mvc.View;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using uApplication = System.Windows.Forms.Application;

namespace MultipleDesktop.Windows.Forms.View
{
    public partial class AppView : Form, IAppView
    {
        private IAppController _controller;

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

        WindowState IAppView.WindowState
            => (WindowState)WindowState;

        bool IAppView.CanMinimize
        {
            get { return MinimizeBox; }
            set { MinimizeBox = value; }
        }

        event EventHandler IAppView.Loaded
        {
            add { Load += value; }
            remove { Load -= value; }
        }

        private CancelEventHandler _closing;
        event CancelEventHandler IAppView.Closing
        {
            add { _closing += value; }
            remove { _closing -= value; }
        }

        public AppView()
        {
            InitializeComponent();

            FormClosing += AppView_FormClosing;
        }

        private void AppView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            _closing?.Invoke(sender, e);
        }

        private void Controller_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IAppController.DesktopConfigurations):
                    UpdateConfigurations();
                    break;
                default:
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

        private void AppView_Load(object sender, EventArgs e)
        {
            lblCopy.Text = "\u00A9 Blondy";

            notifyIcon1.Visible = true;
            notifyIcon1.Text = "Virtual Desktop";
            notifyIcon1.Icon = Properties.Resources._virtual;

            var contextMenu = new ContextMenu();

            contextMenu.MenuItems.Add("Show", (s, args) => ShowForm());

            contextMenu.MenuItems.Add("Exit", (s, args) => uApplication.Exit());

            notifyIcon1.ContextMenu = contextMenu;
        }

        void IAppView.HideView()
        {
            ShowInTaskbar = false;
            Hide();
        }

        private void ShowForm()
        {
            Show();
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
            => ShowForm();

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _controller.Save();
        }
    }
}
