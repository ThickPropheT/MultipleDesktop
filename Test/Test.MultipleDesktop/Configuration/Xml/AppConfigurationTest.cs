using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultipleDesktop.Configuration;
using Moq;
using System.Linq;
using Should.Fluent;
using MultipleDesktop.Configuration.Xml;
using System.Xml;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Test.MultipleDesktop.Configuration.Xml
{
    [TestClass]
    public class AppConfigurationTest
    {
        private AppConfiguration _appConfiguration;

        [TestClass]
        public sealed class WhenInitializing : AppConfigurationTest
        {
            [TestMethod]
            public void ShouldPopulateDesktopConfigurations()
            {
                var configurations = new[]
                {
                        new Mock<VirtualDesktopConfiguration>().Object,
                        new Mock<VirtualDesktopConfiguration>().Object
                    };

                _appConfiguration = new AppConfiguration(configurations);

                _appConfiguration.DesktopConfigurations.Should().Contain.One(configurations[0]);
                _appConfiguration.DesktopConfigurations.Should().Contain.One(configurations[1]);
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
                var array = new VirtualDesktopConfiguration[0];

                _appConfiguration.DesktopConfigurations = array;

                _appConfiguration.GetAll().Should().Equal(array);
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
                    _configurations = new[] { new VirtualDesktopConfiguration() };

                    _appConfiguration = new AppConfiguration(_configurations);
                }

                [TestMethod]
                public void ShouldGetArray()
                {
                    _appConfiguration.DesktopConfigurations.Should().Equal(_configurations);
                }
            }
        }
    }
}
