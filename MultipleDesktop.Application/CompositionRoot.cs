using MultipleDesktop.Application.View;
using MultipleDesktop.Application.ViewModel;
using MultipleDesktop.Configuration;
using MultipleDesktop.Mvc.Controller;
using MultipleDesktop.Mvc.View;
using MultipleDesktop.Windows;
using MultipleDesktop.Windows.Interop.Shell;
using System.Extended;
using System.IO.Extended;

namespace MultipleDesktop.Application
{
    public static class CompositionRoot
    {
        public static TAppView Compose<TAppView>() where TAppView : IAppView, IMainView, new()
        {
            var view = new TAppView();

#if DEBUG
            DebugHideBehavior.Mediate(view);
#else
            ReleaseHideBehavior.Mediate(view);
#endif

            var configurationFactory = new XmlConfigurationFactory();

            var adapter = new WindowsShellAdapter(configurationFactory);

            var controller = new AppController(

                FileSystem.GetFileSystem(),

                new VirtualDesktopStateMonitor(
                    new WindowsDesktop(adapter),
                    adapter,
                    configurationFactory),

                new ConfigurationController(
                    new XmlConfigurationProvider(configurationFactory)),

                configurationFactory);

            var viewModel = new MainViewModel();

            view.Controller = controller;
            view.ViewModel = viewModel;

            view.Loaded += Event.HandleOnce(
                e => view.Loaded -= e,
                (o, e) => controller.Load());

            return view;
        }
    }
}
