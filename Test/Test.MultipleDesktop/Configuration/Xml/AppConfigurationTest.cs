using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MultipleDesktop.Configuration.Xml;
using Should.Fluent;
using Should.Fluent.Invocation;
using System;
using System.Linq;

namespace Test.MultipleDesktop.Configuration.Xml
{
    [TestClass]
    public class AppConfigurationTest
    {
        private AppConfiguration _appConfiguration;

        [TestClass]
        public class WhenInitializing : AppConfigurationTest
        {
            [TestClass]
            public sealed class ToDefault : WhenInitializing
            {
                [TestInitialize]
                public void UsingThisConfiguration()
                {
                    _appConfiguration = new AppConfiguration();
                }

                [TestMethod]
                public void DesktopConfigurationsShouldBeNull()
                {
                    _appConfiguration.DesktopConfigurations.Should().Be.Null();
                }

                [TestMethod]
                public void GetAllShouldReturnEmpty()
                {
                    _appConfiguration.GetAll().Should().Be.Empty();
                }
            }

            [TestClass]
            public class ToConfigurations : WhenInitializing
            {
                [TestClass]
                public sealed class WhenConfigurationsIsNull : ToConfigurations
                {
                    [TestMethod]
                    public void ShouldThrow()
                    {
                        Invocation.Of(() => new AppConfiguration(null))
                            .Should()
                            .Throw<ArgumentNullException>();
                    }
                }

                [TestClass]
                public sealed class WhenConfigurationsIsEmpty : ToConfigurations
                {
                    [TestInitialize]
                    public void UsingThisConfiguration()
                    {
                        _appConfiguration = new AppConfiguration(Enumerable.Empty<VirtualDesktopConfiguration>());
                    }

                    [TestMethod]
                    public void DesktopConfigurationsShouldBeNull()
                    {
                        _appConfiguration.DesktopConfigurations.Should().Be.Null();
                    }

                    [TestMethod]
                    public void GetAllShouldReturnEmpty()
                    {
                        _appConfiguration.GetAll().Should().Be.Empty();
                    }
                }

                [TestClass]
                public sealed class WhenConfigurationsIsNotEmpty : ToConfigurations
                {
                    private VirtualDesktopConfiguration[] _configurations;

                    [TestInitialize]
                    public void UsingThisConfiguration()
                    {
                        _configurations = new[]
                        {
                            new Mock<VirtualDesktopConfiguration>().Object,
                            new Mock<VirtualDesktopConfiguration>().Object
                        };

                        _appConfiguration = new AppConfiguration(_configurations);
                    }

                    [TestMethod]
                    public void DesktopConfigurationsShouldBeConfigurations()
                    {
                        _appConfiguration.DesktopConfigurations.Should().Contain.One(_configurations[0]);
                        _appConfiguration.DesktopConfigurations.Should().Contain.One(_configurations[1]);
                    }

                    [TestMethod]
                    public void GetAllShouldReturnConfigurations()
                    {
                        var all = _appConfiguration.GetAll().ToArray();

                        all.Should().Contain.One(_configurations[0]);
                        all.Should().Contain.One(_configurations[1]);
                    }
                }
            }
        }

        [TestClass]
        public sealed class WhenSettingDesktopConfigurations : AppConfigurationTest
        {
            [TestInitialize]
            public void UsingThisConfiguration()
            {
                _appConfiguration = new AppConfiguration();
            }

            [TestMethod]
            public void ShouldNotAcceptNull()
            {
                _appConfiguration.DesktopConfigurations = null;

                _appConfiguration.GetAll().Should().Not.Be.Null();
            }

            [TestMethod]
            public void ShouldAcceptNonNull()
            {
                var array = new[]
                {
                    new Mock<VirtualDesktopConfiguration>().Object,
                    new Mock<VirtualDesktopConfiguration>().Object
                };

                _appConfiguration.DesktopConfigurations = array;

                _appConfiguration.GetAll().Should().Contain.One(array[0]);
                _appConfiguration.GetAll().Should().Contain.One(array[1]);
            }
        }

        [TestClass]
        public class WhenGettingDesktopConfigurations : AppConfigurationTest
        {
            [TestClass]
            public sealed class WhenDesktopConfigurationsIsEmpty : WhenGettingDesktopConfigurations
            {
                [TestInitialize]
                public void UsingThisConfiguration()
                {
                    _appConfiguration = new AppConfiguration();
                }

                [TestMethod]
                public void ShouldReturnNull()
                {
                    _appConfiguration.DesktopConfigurations.Should().Be.Null();
                }
            }

            [TestClass]
            public sealed class WhenDesktopConfigurationsIsNotEmpty : WhenGettingDesktopConfigurations
            {
                private VirtualDesktopConfiguration[] _configurations;

                [TestInitialize]
                public void UsingThisConfiguration()
                {
                    _configurations = new[]
                    {
                        new Mock<VirtualDesktopConfiguration>().Object,
                        new Mock<VirtualDesktopConfiguration>().Object
                    };

                    _appConfiguration = new AppConfiguration(_configurations);
                }

                [TestMethod]
                public void ShouldGetArray()
                {
                    _appConfiguration.DesktopConfigurations.Should().Contain.One(_configurations[0]);
                    _appConfiguration.DesktopConfigurations.Should().Contain.One(_configurations[1]);
                }
            }
        }
    }
}
