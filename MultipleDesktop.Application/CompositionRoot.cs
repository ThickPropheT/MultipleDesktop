using MultipleDesktop.Configuration;
using MultipleDesktop.Mvc;
using MultipleDesktop.Mvc.Controller;
using MultipleDesktop.Mvc.View;
using MultipleDesktop.Windows;
using MultipleDesktop.Windows.Interop.Shell;
using System.IO.Extended;

namespace MultipleDesktop.Application
{
    public static class CompositionRoot
    {
        public static TAppView Compose<TAppView>() where TAppView : IAppView, new()
        {
            var view = new TAppView();

            //#if DEBUG
            //            DebugHideBehavior.Mediate(view);
            //#else
            //            ReleaseHideBehavior.Mediate(view);
            //#endif

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

            view.Controller = controller;

            AppViewMediator.Mediate(view, controller);

            return view;
        }
    }
}
