using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultipleDesktop.Configuration.Xml;
using Should.Fluent;
using MultipleDesktop.Mvc.Desktop;
using Moq;
using MultipleDesktop.Mvc.Configuration;

namespace Test.MultipleDesktop.Configuration.Xml
{
    [TestClass]
    public class VirtualDesktopConfigurationTest
    {
        [TestClass]
        public class WhenInitializing : VirtualDesktopConfigurationTest
        {
            private VirtualDesktopConfiguration _virtualDesktopConfiguration;

            [TestClass]
            public sealed class ToDefault : WhenInitializing
            {
                [TestInitialize]
                public void UsingThisConfiguration()
                {
                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration();
                }

                [TestMethod]
                public void GuidShouldBeDefault()
                {
                    _virtualDesktopConfiguration.Guid.Should().Equal(Guid.Empty);
                }

                [TestMethod]
                public void BackgroundPathElementShouldBeDefault()
                {
                    _virtualDesktopConfiguration.BackgroundPathElement.Should().Equal((FilePath)string.Empty);
                }

                [TestMethod]
                public void BackgroundPathShouldBeDefault()
                {
                    _virtualDesktopConfiguration.BackgroundPath.Should().Equal((FilePath)string.Empty);
                }

                [TestMethod]
                public void FitElementShouldBeDefault()
                {
                    _virtualDesktopConfiguration.FitElement.Should().Equal(Fit.Center);
                }

                [TestMethod]
                public void FitShouldBeDefault()
                {
                    _virtualDesktopConfiguration.Fit.Should().Equal(Fit.Center);
                }

                [TestMethod]
                public void TargetDesktopShouldBeNull()
                {
                    _virtualDesktopConfiguration.TargetDesktop.Should().Be.Null();
                }
            }

            [TestClass]
            public sealed class ToTargetDesktopAndFactory : WhenInitializing
            {
                private Mock<IVirtualDesktop> _virtualDesktopMock;
                private Mock<IConfigurationFactory> _configurationFactoryMock;

                [TestInitialize]
                public void UsingThisConfiguration()
                {
                    _virtualDesktopMock = new Mock<IVirtualDesktop>();
                    _configurationFactoryMock = new Mock<IConfigurationFactory>();

                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _configurationFactoryMock.Object);
                }
            }
        }

        [TestClass]
        public sealed class WhenSettingBackgroundPathElement : VirtualDesktopConfigurationTest
        {

        }

        [TestClass]
        public sealed class WhenGettingBacgroundPathElement : VirtualDesktopConfigurationTest
        {

        }

        [TestClass]
        public sealed class WhenSettingBackgroundPath : VirtualDesktopConfigurationTest
        {

        }

        [TestClass]
        public sealed class WhenSettingFitElement : VirtualDesktopConfigurationTest
        {

        }

        [TestClass]
        public sealed class WhenGettingFitElement : VirtualDesktopConfigurationTest
        {

        }

        [TestClass]
        public sealed class WhenSettingFit : VirtualDesktopConfigurationTest
        {

        }

        [TestClass]
        public sealed class WhenGettingFit : VirtualDesktopConfigurationTest
        {

        }

        [TestClass]
        public sealed class WhenGettingTargetDesktop : VirtualDesktopConfigurationTest
        {

        }

        [TestClass]
        public sealed class WhenBindingToTarget : VirtualDesktopConfigurationTest
        {

        }

        [TestClass]
        public class OnTargetPropertyChanged : VirtualDesktopConfigurationTest
        {
            [TestClass]
            public sealed class WhenPropertyIsBackground : OnTargetPropertyChanged
            {

            }
        }
    }
}
