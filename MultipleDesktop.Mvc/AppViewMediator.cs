using System;

namespace MultipleDesktop.Mvc
{
    public class AppViewMediator
    {
        private readonly IAppController _controller;
        private readonly IAppView _view;

        public AppViewMediator(IAppView view, IAppController controller)
        {
            _view = view;
            _controller = controller;

            view.Load += View_Load;
        }

        public static void Mediate(IAppView view, IAppController controller)
        {
            new AppViewMediator(view, controller);
        }

        private void View_Load(object sender, EventArgs e)
        {
            _view.Load -= View_Load;

            _controller.Load();
        }
    }
}
