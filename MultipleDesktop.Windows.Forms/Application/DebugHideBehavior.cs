using MultipleDesktop.Mvc.View;
using System;
using System.Windows.Forms;

namespace MultipleDesktop.Windows.Forms.Application
{
    public class DebugHideBehavior
    {
        private readonly IAppView _view;

        public static void Mediate(IAppView view)
        {
            new DebugHideBehavior(view);
        }

        public DebugHideBehavior(IAppView view)
        {
            _view = view;
            view.CanMinimize = true;

            view.SizeChanged += Form_Resize;
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (_view.WindowState != WindowState.Minimized)
                return;

            _view.HideView();
        }
    }
}
