using MultipleDesktop.Mvc.Desktop;
using System;
using System.Collections.Generic;

namespace MultipleDesktop.Windows
{
    public interface IWindowsDesktop : ISystemDesktop
    {
        IEnumerable<Guid> LoadDesktopUuidList();
        Guid LoadCurrentDesktopUuid();

        void Update();
    }
}
