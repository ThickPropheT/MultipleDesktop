using MultipleDesktop.Windows.Forms.View;
using System;
using System.Windows.Forms;

namespace MultipleDesktop.Windows.Forms.Application
{
    public class DebugHideBehavior
    {
        private readonly AppView _form;

        public static void Mediate(AppView form)
        {
            new DebugHideBehavior(form);
        }

        public DebugHideBehavior(AppView form)
        {
            _form = form;
            form.MinimizeBox = true;

            form.Resize += Form_Resize;
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (_form.WindowState != FormWindowState.Minimized)
                return;

            _form.HideForm();
        }
    }
}
