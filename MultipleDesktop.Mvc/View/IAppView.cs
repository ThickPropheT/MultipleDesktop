using MultipleDesktop.Mvc.Controller;
using System;

namespace MultipleDesktop.Mvc.View
{
    public interface IAppView
    {
        IAppController Controller { get; set; }

        event EventHandler Load;
    }
}
