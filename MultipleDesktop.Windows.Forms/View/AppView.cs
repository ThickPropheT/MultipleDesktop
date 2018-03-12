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

        public IAppController Controller
        {
            get { return _controller; }
            internal set
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

        public AppView()
        {
            InitializeComponent();
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
        {
            this.InvokeIfRequired(view =>
            {
                backgroundConfigurationFlowLayoutPanel.Controls.Clear();

                foreach (var configuration in _controller.DesktopConfigurations)
                {
                    backgroundConfigurationFlowLayoutPanel.Controls.Add(new BackgroundConfigurationView(configuration));
                }
            });
        }

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

        public void HideForm()
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
        {
            ShowForm();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            _controller.Save();
        }
    }
}
