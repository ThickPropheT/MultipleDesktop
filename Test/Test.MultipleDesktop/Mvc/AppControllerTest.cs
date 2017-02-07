using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultipleDesktop.Mvc.Configuration;
using Moq;
using MultipleDesktop.Mvc;
using MultipleDesktop.Mvc.Desktop;
using System.Linq;
using Should.Fluent;
using System.Collections.Generic;

namespace Test.MultipleDesktop.Mvc
{
    [TestClass]
    public class AppControllerTest
    {
        private static Mock<IVirtualDesktopState> _desktopStateMock;

        [TestInitialize]
        public virtual void UsingThisConfiguration()
        {
            _desktopStateMock = new Mock<IVirtualDesktopState>();
        }

        [TestClass]
        public class WhenLoading : AppControllerTest
        {
            private Mock<IAppConfiguration> _appConfigurationMock;
            private Mock<IConfigurationController> _configurationControllerMock;

            private AppController _appController;

            [TestInitialize]
            public override void UsingThisConfiguration()
            {
                base.UsingThisConfiguration();

                _appConfigurationMock = new Mock<IAppConfiguration>();
                _appConfigurationMock.Setup(configuration => configuration.GetAll())
                    .Returns(Enumerable.Empty<IVirtualDesktopConfiguration>());

                _configurationControllerMock = new Mock<IConfigurationController>();
                _configurationControllerMock.Setup(controller => controller.Load())
                    .Returns(_appConfigurationMock.Object);

                InitializeAppController();
            }

            protected virtual void InitializeAppController()
            {
                _appController =
                    new AppController(
                        null,
                        null,
                        _desktopStateMock.Object,
                        _configurationControllerMock.Object,
                        null);
            }

            [TestClass]
            public sealed class BeforMappingDesktopsToConfigurations : WhenLoading
            {
                [TestMethod]
                public void ShouldLoadConfiguration()
                {
                    _appController.Load();

                    _configurationControllerMock.Verify(controller => controller.Load(), Times.Once);
                }

                [TestMethod]
                public void ShouldGetVirtualDesktopConfigurations()
                {
                    _appController.Load();

                    _appConfigurationMock.Verify(configuration => configuration.GetAll(), Times.Once);
                }
            }

            [TestClass]
            public class WhenMappingDesktopToConfiguration : WhenLoading
            {
                public static readonly Guid Guid1 = new Guid("00000000-0000-0000-0000-000000000001");
                public static readonly Guid Guid2 = new Guid("00000000-0000-0000-0000-000000000002");

                private Mock<IVirtualDesktop> _desktopMock;
                private Mock<IVirtualDesktopConfiguration> _configurationMock;

                [TestInitialize]
                public override void UsingThisConfiguration()
                {
                    base.UsingThisConfiguration();

                    _desktopMock = new Mock<IVirtualDesktop>();

                    _desktopMock.SetupGet(desktop => desktop.Guid)
                        .Returns(Guid1);

                    _desktopStateMock.SetupGet(state => state.AllDesktops)
                        .Returns(new[] { _desktopMock.Object });

                    _configurationMock = new Mock<IVirtualDesktopConfiguration>();

                    _appConfigurationMock.Setup(configuration => configuration.GetAll())
                        .Returns(new[] { _configurationMock.Object });
                }

                [TestClass]
                public sealed class WhenDesktopHasConfiguration : WhenMappingDesktopToConfiguration
                {
                    [TestMethod]
                    public void ShouldBindConfigurationToDesktop()
                    {
                        _configurationMock.SetupGet(configuration => configuration.Guid)
                            .Returns(Guid1);

                        _configurationMock.Setup(configuraiton =>
                            configuraiton.BindToTarget(
                                It.IsAny<IVirtualDesktop>(),
                                It.IsAny<IConfigurationFactory>()));

                        _appController.Load();

                        _configurationMock.Verify(configuration =>
                            configuration.BindToTarget(
                                It.Is<IVirtualDesktop>(desktop => desktop == _desktopMock.Object),
                                It.IsAny<IConfigurationFactory>()));
                    }
                }

                [TestClass]
                public sealed class WhenDesktopHasNoConfiguration : WhenMappingDesktopToConfiguration
                {
                    private Mock<IConfigurationFactory> _configurationFactoryMock;

                    protected override void InitializeAppController()
                    {
                        _configurationFactoryMock = new Mock<IConfigurationFactory>();

                        _appController =
                            new AppController(
                                null,
                                null,
                                _desktopStateMock.Object,
                                _configurationControllerMock.Object,
                                _configurationFactoryMock.Object);
                    }

                    [TestMethod]
                    public void ShouldCreateConfigurationForDesktop()
                    {
                        _configurationFactoryMock.Setup(factory => factory.ConfigurationFor(It.IsAny<IVirtualDesktop>()));

                        _appController.Load();

                        _configurationFactoryMock.Verify(factory =>
                            factory.ConfigurationFor(
                                It.Is<IVirtualDesktop>(desktop => desktop == _desktopMock.Object)));
                    }
                }

                [TestClass]
                public sealed class Always : WhenMappingDesktopToConfiguration
                {
                    public static readonly Guid Guid3 = new Guid("00000000-0000-0000-0000-000000000003");

                    private Mock<IConfigurationFactory> _configurationFactoryMock;

                    protected override void InitializeAppController()
                    {
                        _configurationFactoryMock = new Mock<IConfigurationFactory>();

                        _appController =
                            new AppController(
                                null,
                                null,
                                _desktopStateMock.Object,
                                _configurationControllerMock.Object,
                                _configurationFactoryMock.Object);
                    }

                    [TestMethod]
                    public void ShouldStoreEachConfiguration()
                    {
                        var desktopMock2 = new Mock<IVirtualDesktop>();
                        desktopMock2.SetupGet(desktop => desktop.Guid)
                            .Returns(Guid3);

                        _desktopStateMock.SetupGet(state => state.AllDesktops)
                            .Returns(new[] { _desktopMock.Object, desktopMock2.Object });

                        var existingConfigurationMock1 = _configurationMock;
                        existingConfigurationMock1.SetupGet(configuration => configuration.Guid)
                            .Returns(Guid1);

                        var existingConfigurationMock2 = new Mock<IVirtualDesktopConfiguration>();
                        existingConfigurationMock2.SetupGet(configuration => configuration.Guid)
                            .Returns(Guid2);

                        _appConfigurationMock.Setup(configuration => configuration.GetAll())
                            .Returns(new[] { existingConfigurationMock1.Object, existingConfigurationMock2.Object });

                        var createdConfigurationMock = new Mock<IVirtualDesktopConfiguration>();
                        createdConfigurationMock.SetupGet(configuration => configuration.Guid)
                            .Returns(Guid2);

                        _configurationFactoryMock.Setup(factory => factory.ConfigurationFor(It.IsAny<IVirtualDesktop>()))
                            .Returns(createdConfigurationMock.Object);

                        _appController.Load();

                        _appController.DesktopConfigurations.Should().Contain.Item(existingConfigurationMock1.Object);
                        _appController.DesktopConfigurations.Should().Contain.Item(existingConfigurationMock2.Object);
                        _appController.DesktopConfigurations.Should().Contain.Item(createdConfigurationMock.Object);
                    }

                    [TestMethod]
                    public void ShouldLoadDesktopState()
                    {
                        _appConfigurationMock.Setup(configuration => configuration.GetAll())
                            .Returns(new List<IVirtualDesktopConfiguration>());

                        _desktopStateMock.Setup(state => state.Load());

                        _appController.Load();

                        _desktopStateMock.Verify(state => state.Load(), Times.Once);
                    }
                }
            }
        }
    }
}
