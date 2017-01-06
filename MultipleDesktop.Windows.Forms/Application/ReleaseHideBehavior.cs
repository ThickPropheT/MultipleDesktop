using MultipleDesktop.Windows.Forms.View;
using System;
using System.Windows.Forms;

namespace MultipleDesktop.Windows.Forms.Application
{
    public class ReleaseHideBehavior
    {
        private readonly AppView _form;

        public static void Mediate(AppView form)
        {
            new ReleaseHideBehavior(form);
        }

        public ReleaseHideBehavior(AppView form)
        {
            _form = form;
            form.MinimizeBox = false;

            form.Load += Form_Load;
            form.FormClosing += Form_FormClosing;
            form.Resize += Form_Resize;
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (_form.WindowState != FormWindowState.Minimized)
                return;

            _form.HideForm();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            _form.HideForm();
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            e.Cancel = true;

            _form.HideForm();
        }
    }
}
