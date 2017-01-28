using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultipleDesktop.Configuration.Xml;
using Should.Fluent;
using MultipleDesktop.Mvc.Desktop;
using Moq;
using MultipleDesktop.Mvc.Configuration;
using VisualStudio.TestTools.UnitTesting;

namespace Test.MultipleDesktop.Configuration.Xml
{
    [TestClass]
    public class VirtualDesktopConfigurationTest
    {
        [TestClass]
        public class WhenInitializing : VirtualDesktopConfigurationTest
        {
            public const string AnyString = "MOCK_VALUE";
            public const Fit AnyFit = Fit.Tile;

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
                    _virtualDesktopConfiguration.BackgroundPathElement.Equals(string.Empty).Should().Be.True();
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
                    ShouldBeTargetBackgroundPath(() => _virtualDesktopConfiguration.BackgroundPathElement);
                }

                [TestMethod]
                public void BackgroundPathShouldBeTargetBackgroundPath()
                {
                    ShouldBeTargetBackgroundPath(() => _virtualDesktopConfiguration.BackgroundPath);
                }

                [TestMethod]
                public void FitElementShouldBeTargetBackgroundFit()
                {
                    ShouldBeTargetBackgroundFit(() => _virtualDesktopConfiguration.FitElement);
                }

                [TestMethod]
                public void FitShouldBeTargetBackgroundFit()
                {
                    ShouldBeTargetBackgroundFit(() => _virtualDesktopConfiguration.Fit);
                }

                [TestMethod]
                public void TargetDesktopShouldBeTarget()
                {
                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, null);

                    _virtualDesktopConfiguration.TargetDesktop.Should().Be.SameAs(_virtualDesktopMock.Object);
                }

                private void ShouldBeTargetBackgroundPath(Func<string> getPath)
                {
                    const string path = AnyString;

                    _backgroundMock.SetupGet(background => background.Path)
                        .Returns(path);

                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, null);

                    getPath().Equals(path).Should().Be.True();
                }

                private void ShouldBeTargetBackgroundFit(Func<Fit> getFit)
                {
                    const Fit fit = AnyFit;

                    _backgroundMock.SetupGet(background => background.Fit)
                        .Returns(fit);

                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, null);

                    getFit().Should().Equal(fit);
                }
            }

            [TestClass]
            public class UsingProperties : WhenInitializing
            {
                [TestInitialize]
                public virtual void UsingThisConfiguration()
                {
                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration();
                }

                [TestClass]
                public sealed class WhenSettingGuid : UsingProperties
                {
                    [TestMethod]
                    public void GuidShouldBeValueSet()
                    {
                        var guid = new Guid("00000000-0000-0000-0000-000000000001");

                        _virtualDesktopConfiguration.Guid = guid;

                        _virtualDesktopConfiguration.Guid.Should().Equal(guid);
                    }
                }

                [TestClass]
                public sealed class WhenSettingBackgroundPathElement : UsingProperties
                {
                    public const string Path = AnyString;

                    [TestMethod]
                    public void BackgroundPathElementShouldBeValueSet()
                    {
                        _virtualDesktopConfiguration.BackgroundPathElement = Path;

                        _virtualDesktopConfiguration.BackgroundPathElement.Equals(Path).Should().Be.True();
                    }

                    [TestMethod]
                    public void BackgroundPathShouldBeBackgroundPathElement()
                    {
                        _virtualDesktopConfiguration.BackgroundPathElement = Path;

                        _virtualDesktopConfiguration.BackgroundPath.Equals(Path).Should().Be.True();
                    }
                }

                [TestClass]
                public sealed class WhenSettingBackgroundPath : UsingProperties
                {
                    [TestMethod]
                    public void ShouldThrow()
                    {
                        Expect.Exception<XmlIgnoreException>(() => _virtualDesktopConfiguration.BackgroundPath = AnyString);
                    }
                }

                [TestClass]
                public sealed class WhenSettingFitElement : UsingProperties
                {
                    [TestMethod]
                    public void FitElementShouldBeValueSet()
                    {
                        const Fit fit = AnyFit;

                        _virtualDesktopConfiguration.FitElement = fit;

                        _virtualDesktopConfiguration.FitElement.Should().Equal(fit);
                    }

                    [TestMethod]
                    public void FitShouldBeFitElement()
                    {
                        const Fit fit = AnyFit;

                        _virtualDesktopConfiguration.FitElement = fit;

                        _virtualDesktopConfiguration.Fit.Should().Equal(fit);
                    }
                }

                [TestClass]
                public sealed class WhenSettingFit : UsingProperties
                {
                    [TestMethod]
                    public void ShouldThrow()
                    {
                        const Fit fit = AnyFit;

                        Expect.Exception<XmlIgnoreException>(() => _virtualDesktopConfiguration.Fit = fit);
                    }
                }

                [TestClass]
                public sealed class DoNotSetTargetDesktop : UsingProperties
                {
                    [TestMethod]
                    public void TargetDesktopShouldBeNull()
                    {
                        _virtualDesktopConfiguration.TargetDesktop.Should().Be.Null();
                    }
                }

                [TestClass]
                public class WhenInitialized : UsingProperties
                {
                    [TestClass]
                    public sealed class WhenNotBoundToTarget : WhenInitialized
                    {
                        [TestMethod]
                        public void SettingBackgroundPathShouldThrow()
                        {
                            Expect.Exception<XmlIgnoreException>(() => _virtualDesktopConfiguration.BackgroundPath = AnyString);
                        }

                        [TestMethod]
                        public void SettingFitShouldThrow()
                        {
                            const Fit fit = AnyFit;

                            Expect.Exception<XmlIgnoreException>(() => _virtualDesktopConfiguration.Fit = fit);
                        }
                    }

                    [TestClass]
                    public sealed class WhenBoundToTarget : WhenInitialized
                    {
                        private Mock<IVirtualDesktop> _virtualDesktopMock;

                        [TestInitialize]
                        public override void UsingThisConfiguration()
                        {
                            base.UsingThisConfiguration();

                            _virtualDesktopMock = new Mock<IVirtualDesktop>();

                            _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, null);
                        }

                        [TestMethod]
                        public void SettingBackgroundPathShouldNotThrow()
                        {
                            const string path = AnyString;

                            _virtualDesktopConfiguration.BackgroundPath = path;
                        }

                        [TestMethod]
                        public void SettingFitShouldNotThrow()
                        {
                            const Fit fit = AnyFit;

                            _virtualDesktopConfiguration.Fit = fit;
                        }

                        [TestMethod]
                        public void TargetDesktopShouldBeValue()
                        {
                            _virtualDesktopConfiguration.TargetDesktop.Should().Be.SameAs(_virtualDesktopMock.Object);
                        }
                    }
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
