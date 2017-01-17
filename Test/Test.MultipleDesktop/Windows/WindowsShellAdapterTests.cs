using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultipleDesktop.Windows.Interop.Shell;
using MultipleDesktop.Mvc.Configuration;
using Moq;
using static MultipleDesktop.Windows.Interop.Shell.NativeMethods;
using Castle.Core.Internal;

namespace Test.MultipleDesktop.Windows
{
    [TestClass]
    public class WindowsShellAdapterTests
    {
        /// <summary>
        /// dummy code to illustrate that the InternalsVisibleToAttribute is working
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            var configurationFactoryMock = new Mock<IConfigurationFactory>();

            var desktopMock = new Mock<IDesktopWallpaper>();

            var managerInternalMock = new Mock<IVirtualDesktopManagerInternal>();

            var adapter = new WindowsShellAdapter(
                configurationFactoryMock.Object,
                desktopMock.Object,
                managerInternalMock.Object);
        }
    }
}
