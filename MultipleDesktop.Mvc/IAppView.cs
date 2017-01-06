using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleDesktop.Mvc
{
    public interface IAppView
    {
        event EventHandler Load;
    }
}
