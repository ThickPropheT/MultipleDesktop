using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultipleDesktop.Mvc.Configuration;
using Moq;
using MultipleDesktop.Mvc.Desktop;
using Should.Fluent;

namespace Test.MultipleDesktop.Mvc.Configuration
{
    [TestClass]
    public abstract class DesktopConfigurationMapperTest
    {
        public static readonly Guid Guid1 = new Guid("00000000-0000-0000-0000-000000000001");
        public static readonly Guid Guid2 = new Guid("00000000-0000-0000-0000-000000000002");

        protected abstract Guid ConfigurationGuid { get; }
        protected abstract Guid DesktopGuid { get; }

        private Mock<IVirtualDesktopConfiguration> _configurationMock;
        private Mock<IVirtualDesktop> _desktopMock;

        [TestInitialize]
        public void UsingThisConfiguration()
        {
            _configurationMock = new Mock<IVirtualDesktopConfiguration>();
            _configurationMock.SetupGet(configuration =>
                configuration.Guid)
                .Returns(ConfigurationGuid);

            _desktopMock = new Mock<IVirtualDesktop>();
            _desktopMock.SetupGet(desktop =>
                desktop.Guid)
                .Returns(DesktopGuid);
        }

        [TestClass]
        public sealed class WhenGuidsMatch : DesktopConfigurationMapperTest
        {
            protected override Guid ConfigurationGuid => Guid1;
            protected override Guid DesktopGuid => Guid1;

            [TestMethod]
            public void ShouldReturnTrue()
            {
                var result = _configurationMock.Object.IsConfigurationFor(_desktopMock.Object);

                result.Should().Be.True();
            }
        }

        [TestClass]
        public sealed class WhenGuidsDoNotMatch : DesktopConfigurationMapperTest
        {
            protected override Guid ConfigurationGuid => Guid1;
            protected override Guid DesktopGuid => Guid2;

            [TestMethod]
            public void ShouldReturnFalse()
            {
                var result = _configurationMock.Object.IsConfigurationFor(_desktopMock.Object);

                result.Should().Be.False();
            }
        }
    }
}
