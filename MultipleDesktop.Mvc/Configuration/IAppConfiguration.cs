using System.Collections.Generic;

namespace MultipleDesktop.Mvc.Configuration
{
    public interface IAppConfiguration
    {
        IEnumerable<IVirtualDesktopConfiguration> GetAll();
    }
}
