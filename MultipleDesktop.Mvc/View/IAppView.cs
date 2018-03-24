using MultipleDesktop.Mvc.Controller;
using System;
using System.ComponentModel;

namespace MultipleDesktop.Mvc.View
{
    public interface IAppView
    {
        IAppController Controller { get; set; }

        WindowState WindowState { get; }

        event EventHandler Loaded;
        event EventHandler WindowStateChanged;
        event CancelEventHandler Closing;

        void ShowView();
        void HideView();
    }
}
