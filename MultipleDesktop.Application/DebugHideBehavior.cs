using MultipleDesktop.Application.ViewModel;
using MultipleDesktop.Mvc.View;
using System;

namespace MultipleDesktop.Application
{
    public class DebugHideBehavior
    {
        private readonly IAppView _view;

        public static void Mediate(IAppView view, IWindowViewModel viewModel)
            => new DebugHideBehavior(view, viewModel);

        public DebugHideBehavior(IAppView view, IWindowViewModel viewModel)
        {
            _view = view;
            viewModel.CanMinimize = true;
            viewModel.CanMaximize = false;

            view.WindowStateChanged += View_WindowStateChanged;

            view.ShowView();
        }

        private void View_WindowStateChanged(object sender, EventArgs e)
        {
            if (_view.WindowState != WindowState.Minimized)
                return;

            _view.HideView();
        }
    }
}
