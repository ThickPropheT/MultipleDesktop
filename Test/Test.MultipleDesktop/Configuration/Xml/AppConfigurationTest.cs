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

namespace Test.MultipleDesktop.Configuration.Xml
{
    [TestClass]
    public class AppConfigurationTest
    {
        [TestClass]
        public class WhenInitializing : AppConfigurationTest
        {
            [TestClass]
            public sealed class FromConstructor : WhenInitializing
            {
                [TestMethod]
                public void ShouldPopulateDesktopConfigurations()
                {
                    var configurations = new[]
                    {
                    new Mock<VirtualDesktopConfiguration>().Object,
                    new Mock<VirtualDesktopConfiguration>().Object
                };

                    var appConfiguration = new AppConfiguration(configurations);

                    appConfiguration.DesktopConfigurations.Should().Contain.One(configurations[0]);
                    appConfiguration.DesktopConfigurations.Should().Contain.One(configurations[1]);
                }
            }

            [TestClass]
            public sealed class FromProperties : WhenInitializing
            {
                [TestMethod]
                public void MyTestMethod()
                {

                }
            }
        }
    }
}
