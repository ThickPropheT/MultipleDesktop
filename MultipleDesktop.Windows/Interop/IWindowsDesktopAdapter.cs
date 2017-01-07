using MultipleDesktop.Mvc.Desktop;
using System;
using System.Collections.Generic;

namespace MultipleDesktop.Windows.Interop
{
    public interface IWindowsDesktopAdapter
    {
        Guid LoadCurrentDesktopUuid();
        IEnumerable<Guid> LoadDesktopUuidList();

        void SaveBackground(IBackground background);
        IBackground LoadBackground();
    }
}
