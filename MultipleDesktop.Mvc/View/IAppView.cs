using MultipleDesktop.Mvc.Controller;
using System;
using System.ComponentModel;

namespace MultipleDesktop.Mvc.View
{
    public interface IAppView
    {
        IAppController Controller { get; set; }

        WindowState WindowState { get; }
        bool CanMinimize { get; set; }

        event EventHandler Loaded;
        event EventHandler SizeChanged;
        event CancelEventHandler Closing;

        void HideView();
    }
}
