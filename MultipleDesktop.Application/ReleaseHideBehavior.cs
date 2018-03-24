using MultipleDesktop.Application.ViewModel;
using MultipleDesktop.Mvc.View;
using System;
using System.ComponentModel;

namespace MultipleDesktop.Application
{
    public class ReleaseHideBehavior
    {
        private readonly IAppView _view;

        public static void Mediate(IAppView view, IWindowViewModel viewModel)
            => new ReleaseHideBehavior(view, viewModel);

        public ReleaseHideBehavior(IAppView view, IWindowViewModel viewModel)
        {
            _view = view;
            viewModel.CanMinimize = false;
            viewModel.CanMaximize = false;

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
