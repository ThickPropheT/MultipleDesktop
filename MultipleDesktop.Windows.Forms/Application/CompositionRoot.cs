using MultipleDesktop.Configuration;
using MultipleDesktop.Mvc;
using MultipleDesktop.Windows.Forms.Application;
using MultipleDesktop.Windows.Forms.View;
using System.IO.Windows;
using System.Windows.Forms;

namespace MultipleDesktop.Windows.Forms
{
    public static class CompositionRoot
    {
        public static Form Compose()
        {
            var view = new AppView();

#if DEBUG
            DebugHideBehavior.Mediate(view);
#else
            ReleaseHideBehavior.Mediate(view);
#endif

            var configurationFactory = new XmlConfigurationFactory();

            var controller = new AppController(
                view,
                FileSystem.GetFileSystem(),
                new VirtualDesktopStateProvider(new WindowsDesktopRegistry()),
                new XmlConfigurationProvider(configurationFactory),
                configurationFactory);

            view.Controller = controller;

            AppViewMediator.Mediate(view, controller);

            return view;
        }
    }
}
