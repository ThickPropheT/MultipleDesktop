using MultipleDesktop.Mvc.View;
using System;

namespace MultipleDesktop.Application
{
    public class DebugHideBehavior
    {
        private readonly IAppView _view;

        public static void Mediate(IAppView view)
            => new DebugHideBehavior(view);

        public DebugHideBehavior(IAppView view)
        {
            _view = view;
            view.CanMinimize = true;
            view.CanMaximize = false;

            view.SizeChanged += Form_Resize;

            view.ShowView();
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            if (_view.WindowState != WindowState.Minimized)
                return;

            _view.HideView();
        }
    }
}
