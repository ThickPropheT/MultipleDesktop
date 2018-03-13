using MultipleDesktop.Mvc.Configuration;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace MultipleDesktop.Windows.Forms.View
{
    public partial class BackgroundConfigurationView : UserControl
    {
        private IVirtualDesktopConfiguration _configuration;

        public BackgroundConfigurationView()
        {
            InitializeComponent();
        }

        public BackgroundConfigurationView(IVirtualDesktopConfiguration configuration)
            : this()
        {
            _configuration = configuration;
            configuration.PropertyChanged += Configuration_PropertyChanged;

            txtPic1.Text = configuration.BackgroundPath;
        }

        private void Configuration_PropertyChanged(object sender, PropertyChangedEventArgs e)
            => UpdateFromConfiguration();

        private void UpdateFromConfiguration()
            => this.InvokeIfRequired(view => txtPic1.Text = _configuration.BackgroundPath);

        private void btn1_Click(object sender, EventArgs e)
            => ChooseFile(txtPic1);

        private void ChooseFile(TextBox box)
        {
            using (var dlg = new OpenFileDialog
            {
                Filter = "Image files (*.bmp, *.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp) | *.bmp; *.jpg; *.jpeg; *.jpe; *.jfif; *.png |All Files (*.*)|*.*",
                InitialDirectory = string.IsNullOrEmpty(box.Text) ? null : Path.GetDirectoryName(box.Text)
            })
            {
                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                box.Text = dlg.FileName;

                _configuration.BackgroundPath = dlg.FileName;
            }
        }

        private void txtPic1_Leave(object sender, EventArgs e)
            => _configuration.BackgroundPath = txtPic1.Text;
    }
}
