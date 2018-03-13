using MultipleDesktop.Mvc.View;
using System;
using System.ComponentModel;

namespace MultipleDesktop.Application
{
    public class ReleaseHideBehavior
    {
        private readonly IAppView _view;

        public static void Mediate(IAppView view)
            => new ReleaseHideBehavior(view);

        public ReleaseHideBehavior(IAppView view)
        {
            _view = view;
            view.CanMinimize = false;

            view.Loaded += View_Load;
            view.SizeChanged += View_SizeChanged;
            view.Closing += View_Closing;
        }

        private void View_Load(object sender, EventArgs e)
            => _view.HideView();

        private void View_SizeChanged(object sender, EventArgs e)
        {
            if (_view.WindowState != WindowState.Minimized)
                return;

            _view.HideView();
        }

        private void View_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;

            _view.HideView();
        }
    }
}
