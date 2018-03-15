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
            view.CanMaximize = false;

            view.Closing += View_Closing;

            view.HideView();
        }

        private void View_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;

            _view.HideView();
        }
    }
}
