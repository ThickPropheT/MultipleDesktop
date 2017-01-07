﻿using MultipleDesktop.Configuration;
using MultipleDesktop.Mvc;
using MultipleDesktop.Windows.Forms.Application;
using MultipleDesktop.Windows.Forms.View;
using MultipleDesktop.Windows.Interop.Shell;
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

            var adapter = new WindowsShellAdapter(configurationFactory);

            var controller = new AppController(
                view,
                FileSystem.GetFileSystem(),
                new VirtualDesktopStateProvider(
                    new WindowsDesktop(adapter),
                    adapter,
                    configurationFactory),
                new XmlConfigurationProvider(configurationFactory),
                configurationFactory);

            view.Controller = controller;

            AppViewMediator.Mediate(view, controller);

            return view;
        }
    }
}
