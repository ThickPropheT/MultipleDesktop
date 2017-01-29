﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MultipleDesktop.Configuration.Xml;
using MultipleDesktop.Mvc;
using MultipleDesktop.Mvc.Configuration;
using MultipleDesktop.Mvc.Desktop;
using Should.Fluent;
using System;
using System.ComponentModel;
using VisualStudio.TestTools.UnitTesting;

namespace Test.MultipleDesktop.Configuration.Xml
{
    [TestClass]
    public class VirtualDesktopConfigurationTest
    {
        private VirtualDesktopConfiguration _virtualDesktopConfiguration;

        [TestClass]
        public class WhenInitializing : VirtualDesktopConfigurationTest
        {
            public const string AnyString = "MOCK_VALUE";
            public const Fit AnyFit = Fit.Tile;

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
            public class ToTargetDesktopAndFactory : WhenInitializing
            {
                private Mock<IBackground> _backgroundMock;
                private Mock<IVirtualDesktop> _virtualDesktopMock;
                private Mock<IConfigurationFactory> _factoryMock;

                [TestInitialize]
                public virtual void UsingThisConfiguration()
                {
                    _backgroundMock = new Mock<IBackground>();

                    _virtualDesktopMock = new Mock<IVirtualDesktop>();
                    _virtualDesktopMock.SetupGet(desktop => desktop.Background)
                        .Returns(_backgroundMock.Object);

                    _factoryMock = new Mock<IConfigurationFactory>();
                }

                [TestClass]
                public sealed class DoInitialize : ToTargetDesktopAndFactory
                {
                    [TestMethod]
                    public void GuidShouldBeTargetGuid()
                    {
                        var guid = new Guid("00000000-0000-0000-0000-000000000001");

                        _virtualDesktopMock.SetupGet(desktop => desktop.Guid)
                            .Returns(guid);

                        _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

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
                        _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                        _virtualDesktopConfiguration.TargetDesktop.Should().Be.SameAs(_virtualDesktopMock.Object);
                    }

                    [TestMethod]
                    public void ShouldBindToTargetPropertyChanged()
                    {
                        _factoryMock.Setup(factory =>
                            factory.Bind(
                                It.IsAny<Action>(),
                                It.IsAny<INotifyPropertyChanged>()));

                        _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                        _factoryMock.Verify(factory =>
                            factory.Bind(
                                It.Is<Action>(a =>
                                    a == _virtualDesktopConfiguration.UpdateFromTarget),
                                It.Is<IVirtualDesktop>(d =>
                                    d == _virtualDesktopMock.Object)),
                            Times.Once);
                    }

                    private void ShouldBeTargetBackgroundPath(Func<string> getPath)
                    {
                        const string path = AnyString;

                        _backgroundMock.SetupGet(background => background.Path)
                            .Returns(path);

                        _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                        getPath().Equals(path).Should().Be.True();
                    }

                    private void ShouldBeTargetBackgroundFit(Func<Fit> getFit)
                    {
                        const Fit fit = AnyFit;

                        _backgroundMock.SetupGet(background => background.Fit)
                            .Returns(fit);

                        _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                        getFit().Should().Equal(fit);
                    }
                }

                [TestClass]
                public sealed class WhenRebinding : ToTargetDesktopAndFactory
                {
                    private Mock<IPropertyChangedBinding> _bindingMock;

                    [TestInitialize]
                    public override void UsingThisConfiguration()
                    {
                        base.UsingThisConfiguration();

                        _bindingMock = new Mock<IPropertyChangedBinding>();
                        _bindingMock.Setup(binding => binding.Unbind());

                        _factoryMock.Setup(factory =>
                            factory.Bind(
                                It.IsAny<Action>(),
                                It.IsAny<INotifyPropertyChanged>()))
                            .Returns(_bindingMock.Object);

                        _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);
                    }

                    [TestMethod]
                    public void ShouldUnbindExistingBinding()
                    {
                        _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                        _bindingMock.Verify(binding => binding.Unbind(), Times.Once);
                    }
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
                        private Mock<IConfigurationFactory> _factoryMock;

                        [TestInitialize]
                        public override void UsingThisConfiguration()
                        {
                            base.UsingThisConfiguration();

                            _virtualDesktopMock = new Mock<IVirtualDesktop>();
                            _factoryMock = new Mock<IConfigurationFactory>();
                        }

                        [TestMethod]
                        public void SettingBackgroundPathShouldNotThrow()
                        {
                            const string path = AnyString;

                            _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                            _virtualDesktopConfiguration.BackgroundPath = path;
                        }

                        [TestMethod]
                        public void SettingFitShouldNotThrow()
                        {
                            const Fit fit = AnyFit;

                            _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                            _virtualDesktopConfiguration.Fit = fit;
                        }

                        [TestMethod]
                        public void TargetDesktopShouldBeTarget()
                        {
                            _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                            _virtualDesktopConfiguration.TargetDesktop.Should().Be.SameAs(_virtualDesktopMock.Object);
                        }

                        [TestMethod]
                        public void ShouldBindToTargetPropertyChanged()
                        {
                            _factoryMock.Setup(factory =>
                                factory.Bind(
                                    It.IsAny<Action>(),
                                    It.IsAny<INotifyPropertyChanged>()));

                            _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                            _factoryMock.Verify(factory =>
                                factory.Bind(
                                    It.Is<Action>(a =>
                                        a == _virtualDesktopConfiguration.UpdateFromTarget),
                                    It.Is<IVirtualDesktop>(d =>
                                        d == _virtualDesktopMock.Object)),
                                Times.Once);
                        }
                    }

                    [TestClass]
                    public sealed class WhenRebinding : WhenInitialized
                    {
                        private Mock<IVirtualDesktop> _virtualDesktopMock;
                        private Mock<IPropertyChangedBinding> _bindingMock;
                        private Mock<IConfigurationFactory> _factoryMock;

                        [TestInitialize]
                        public override void UsingThisConfiguration()
                        {
                            base.UsingThisConfiguration();

                            _virtualDesktopMock = new Mock<IVirtualDesktop>();
                            _factoryMock = new Mock<IConfigurationFactory>();

                            _virtualDesktopConfiguration = new VirtualDesktopConfiguration();

                            _bindingMock = new Mock<IPropertyChangedBinding>();
                            _bindingMock.Setup(binding => binding.Unbind());

                            _factoryMock.Setup(factory =>
                                factory.Bind(
                                    It.IsAny<Action>(),
                                    It.IsAny<INotifyPropertyChanged>()))
                                .Returns(_bindingMock.Object);

                            _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);
                        }

                        [TestMethod]
                        public void ShouldUnbindExistingBinding()
                        {
                            _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                            _bindingMock.Verify(binding => binding.Unbind(), Times.Once);
                        }
                    }
                }
            }
        }

        [TestClass]
        public class WhenBindingToTarget : VirtualDesktopConfigurationTest
        {
            [TestClass]
            public sealed class WhenAlreadyBound : WhenBindingToTarget
            {

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
        public class OnTargetPropertyChanged : VirtualDesktopConfigurationTest
        {
            [TestClass]
            public sealed class WhenPropertyIsBackground : OnTargetPropertyChanged
            {

            }
        }
    }
}
