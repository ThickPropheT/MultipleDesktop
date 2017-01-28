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
                    _virtualDesktopConfiguration.BackgroundPath.Equals(string.Empty).Should().Be.True();
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
                public const string AnyString = "MOCK_VALUE";

                private Mock<IBackground> _backgroundMock;
                private Mock<IVirtualDesktop> _virtualDesktopMock;

                [TestInitialize]
                public void UsingThisConfiguration()
                {
                    _backgroundMock = new Mock<IBackground>();

                    _virtualDesktopMock = new Mock<IVirtualDesktop>();
                    _virtualDesktopMock.SetupGet(desktop => desktop.Background)
                        .Returns(_backgroundMock.Object);
                }

                [TestMethod]
                public void GuidShouldBeTargetGuid()
                {
                    var guid = new Guid("00000000-0000-0000-0000-000000000001");

                    _virtualDesktopMock.SetupGet(desktop => desktop.Guid)
                        .Returns(guid);

                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, null);

                    _virtualDesktopConfiguration.Guid.Should().Equal(guid);
                }

                [TestMethod]
                public void BackroundPathElementShouldBeTargetBackgroundPath()
                {
                    const string path = AnyString;

                    _backgroundMock.SetupGet(background => background.Path)
                        .Returns(path);

                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, null);

                    _virtualDesktopConfiguration.BackgroundPathElement.Equals(path).Should().Be.True();
                }

                [TestMethod]
                public void BackgroundPathShouldBeTargetBackgroundPath()
                {
                    const string path = AnyString;

                    _backgroundMock.SetupGet(background => background.Path)
                        .Returns(path);

                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, null);

                    _virtualDesktopConfiguration.BackgroundPath.Equals(path).Should().Be.True();
                }
            }

            [TestClass]
            public sealed class UsingProperties : WhenInitializing
            {
                [TestInitialize]
                public void UsingThisConfiguration()
                {
                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration();
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
