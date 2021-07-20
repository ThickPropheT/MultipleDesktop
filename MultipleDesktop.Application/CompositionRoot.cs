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

            var viewModel = new MainViewModel(controller);

            view.Controller = controller;

#if DEBUG
            DebugHideBehavior.Mediate(view, viewModel);
#else
            ReleaseHideBehavior.Mediate(view, viewModel);
#endif

            view.ViewModel = viewModel;

            controller.Load();

            return view;
        }
    }
}
