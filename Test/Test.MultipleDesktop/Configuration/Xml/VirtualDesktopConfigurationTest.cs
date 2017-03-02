using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Language.Flow;
using MultipleDesktop.Configuration.Xml;
using MultipleDesktop.Mvc;
using MultipleDesktop.Mvc.Configuration;
using MultipleDesktop.Mvc.Desktop;
using Should.Fluent;
using Should.Fluent.Invocation;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Test.MultipleDesktop.Configuration.Xml
{
    [TestClass]
    public class VirtualDesktopConfigurationTest
    {
        public const string AnyString = "MOCK_VALUE";
        public const Fit AnyFit = Fit.Tile;

        private static VirtualDesktopConfiguration _virtualDesktopConfiguration;

        private interface IWhenInitializingWithTarget : IWhenTargetIsValid { }

        private interface IWhenConfigurationConfigured
        {
            void ShouldRequestBackgroundFromFactory();
            void ShouldSetTargetBackground();
        }

        private interface IWhenConfigurationNotConfigured
        {
            void ShouldUpdateBackgroundPathElement();
            void ShouldUpdateBackgroundPath();
            void ShouldUpdateFitElement();
            void ShouldUpdateFit();
            void ShouldNotRequestBackgroundFromFactory();
            void ShouldNotSetTargetBackground();
        }

        private interface IWhenBindingToNull : IWhenTargetIsNull
        {
            void TargetDesktopShouldBeNull();
        }

        private interface IWhenTargetIsValid
        {
            void SettingBackgroundPathShouldNotThrow();
            void SettingFitShouldNotThrow();
        }

        private interface IWhenTargetIsNull
        {
            void SettingBackgroundPathShouldThrow();
            void SettingFitShouldThrow();
        }

        private interface IWhenAlreadyBound
        {
            void ShouldUnbindExistingBinding();
        }

        [TestClass]
        public class WhenInitializing : VirtualDesktopConfigurationTest
        {
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
                    // ReSharper disable once SuspiciousTypeConversion.Global
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
            public class UsingTargetDesktopAndFactory : WhenInitializing, IWhenInitializingWithTarget
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

                [TestMethod]
                public void SettingBackgroundPathShouldNotThrow()
                {
                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                    Invoke.Delegate(() =>
                            _virtualDesktopConfiguration.BackgroundPath = AnyString)
                        .Should()
                        .Not
                        .Throw<NoTargetBoundException>();
                }

                [TestMethod]
                public void SettingFitShouldNotThrow()
                {
                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                    Invoke.Delegate(() =>
                            _virtualDesktopConfiguration.Fit = AnyFit)
                        .Should()
                        .Not
                        .Throw<NoTargetBoundException>();
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
                // TODO hides class @ bottom
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

                        // ReSharper disable once SuspiciousTypeConversion.Global
                        _virtualDesktopConfiguration.BackgroundPath.Equals(Path).Should().Be.True();
                    }
                }

                [TestClass]
                public sealed class WhenSettingBackgroundPath : UsingProperties
                {
                    [TestMethod]
                    public void ShouldThrow()
                    {
                        Invoke.Delegate(() =>
                            _virtualDesktopConfiguration.BackgroundPath = AnyString)
                            .Should()
                            .Throw<NoTargetBoundException>();
                    }
                }

                [TestClass]
                // TODO hides class @ bottom
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

                        Invoke.Delegate(() =>
                            _virtualDesktopConfiguration.Fit = fit)
                            .Should()
                            .Throw<NoTargetBoundException>();
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

                // TODO re-evaluate this being in WhenInitializing
                // TODO determine if any test methods here need to move
                [TestClass]
                public sealed class BindToTargetToCompleteInitialization : UsingProperties, IWhenTargetIsValid
                {
                    private Mock<IVirtualDesktop> _virtualDesktopMock;
                    private Mock<IConfigurationFactory> _factoryMock;

                    [TestInitialize]
                    public override void UsingThisConfiguration()
                    {
                        base.UsingThisConfiguration();

                        _virtualDesktopMock = new Mock<IVirtualDesktop>();
                        _virtualDesktopMock.SetupGet(desktop => desktop.Background)
                            .Returns(new Mock<IBackground>().Object);

                        _factoryMock = new Mock<IConfigurationFactory>();
                    }

                    [TestMethod]
                    public void SettingBackgroundPathShouldNotThrow()
                    {
                        const string path = AnyString;

                        _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                        _virtualDesktopConfiguration.BackgroundPath = path;
                    }

                    // TODO move these to OnceInitialized.UsingProperties.WhenBinding
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

                    [TestMethod]
                    public void ShouldRequestBackgroundFromFactory()
                    {
                        _virtualDesktopConfiguration.BackgroundPathElement = AnyString;
                        _virtualDesktopConfiguration.FitElement = AnyFit;

                        _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                        _factoryMock.Verify(factory =>
                            factory.BackgroundFrom(
                                It.Is<string>(s => s == AnyString),
                                It.Is<Fit>(f => f == AnyFit)),
                            Times.Once);

                    }

                    // TODO move these
                }

                // TODO re-evaluate this being in WhenInitializing
                // TODO determine if any test methods here need to move
                [TestClass]
                public sealed class BindingToNullDoesNotCompleteInitialization : UsingProperties, IWhenBindingToNull
                {
                    private Mock<IConfigurationFactory> _factoryMock;

                    [TestInitialize]
                    public override void UsingThisConfiguration()
                    {
                        base.UsingThisConfiguration();

                        _factoryMock = new Mock<IConfigurationFactory>();

                        _virtualDesktopConfiguration.BindToTarget(null, _factoryMock.Object);
                    }

                    [TestMethod]
                    public void SettingBackgroundPathShouldThrow()
                    {
                        Invoke.Delegate(() =>
                                _virtualDesktopConfiguration.BackgroundPath = AnyString)
                            .Should()
                            .Throw<NoTargetBoundException>();
                    }

                    [TestMethod]
                    public void SettingFitShouldThrow()
                    {
                        Invoke.Delegate(() =>
                                _virtualDesktopConfiguration.Fit = AnyFit)
                            .Should()
                            .Throw<NoTargetBoundException>();
                    }

                    [TestMethod]
                    public void TargetDesktopShouldBeNull()
                    {
                        _virtualDesktopConfiguration.TargetDesktop.Should().Be.Null();
                    }
                }
            }
        }

        [TestClass]
        public class OnceInitialized : VirtualDesktopConfigurationTest
        {
            public const string SameString = AnyString;

            private static Mock<IBackground> _backgroundMock;
            private static Mock<IVirtualDesktop> _virtualDesktopMock;
            private static Mock<IConfigurationFactory> _factoryMock;

            [TestClass]
            public class UsingTargetDesktopAndFactory : OnceInitialized
            {
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
                public class WhenBinding : UsingTargetDesktopAndFactory, IWhenAlreadyBound
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
                    }

                    [TestMethod]
                    public void ShouldUnbindExistingBinding()
                    {
                        _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                        _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                        _bindingMock.Verify(binding => binding.Unbind(), Times.Once);
                    }

                    [TestClass]
                    public class WhenTargetIsValid : WhenBinding, IWhenTargetIsValid
                    {
                        [TestMethod]
                        public void SettingBackgroundPathShouldNotThrow()
                        {
                            _virtualDesktopConfiguration =
                                new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                            _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object,
                                _factoryMock.Object);

                            Invoke.Delegate(() =>
                                    _virtualDesktopConfiguration.BackgroundPath = AnyString)
                                .Should()
                                .Not
                                .Throw<NoTargetBoundException>();
                        }

                        [TestMethod]
                        public void SettingFitShouldNotThrow()
                        {
                            _virtualDesktopConfiguration =
                                new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                            _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object,
                                _factoryMock.Object);

                            Invoke.Delegate(() =>
                                    _virtualDesktopConfiguration.Fit = AnyFit)
                                .Should()
                                .Not
                                .Throw<NoTargetBoundException>();
                        }

                        [TestClass]
                        public sealed class WhenConfigurationNotConfigured : WhenTargetIsValid, IWhenConfigurationNotConfigured
                        {
                            [TestInitialize]
                            public override void UsingThisConfiguration()
                            {
                                base.UsingThisConfiguration();

                                _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);
                            }

                            [TestMethod]
                            public void ShouldUpdateBackgroundPathElement()
                            {
                                WhenSetupToUpdateBackgroundPath();

                                _virtualDesktopConfiguration.BackgroundPathElement.Should().Equal(AnyString);
                            }

                            [TestMethod]
                            public void ShouldUpdateBackgroundPath()
                            {
                                WhenSetupToUpdateBackgroundPath();

                                // ReSharper disable once SuspiciousTypeConversion.Global
                                _virtualDesktopConfiguration.BackgroundPath.Equals(AnyString).Should().Be.True();
                            }

                            [TestMethod]
                            public void ShouldUpdateFitElement()
                            {
                                WhenSetupToUpdateFit();

                                _virtualDesktopConfiguration.FitElement.Should().Equal(AnyFit);
                            }

                            [TestMethod]
                            public void ShouldUpdateFit()
                            {
                                WhenSetupToUpdateFit();

                                _virtualDesktopConfiguration.Fit.Should().Equal(AnyFit);
                            }

                            [TestMethod]
                            public void ShouldNotRequestBackgroundFromFactory()
                            {
                                SetupToRequestBackground();

                                _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                                ShouldNotRequestBackground();
                            }

                            [TestMethod]
                            public void ShouldNotSetTargetBackground()
                            {
                                var newBackground = new Mock<IBackground>().Object;

                                SetupToRequestBackground()
                                    .Returns(newBackground);

                                ShouldNotSetTargetBackgroundImpl();
                            }
                        }

                        [TestClass]
                        public sealed class WhenConfigurationConfigured : WhenTargetIsValid, IWhenConfigurationConfigured
                        {
                            [TestMethod]
                            public void ShouldRequestBackgroundFromFactory()
                            {
                                var backgroundMock = new Mock<IBackground>();
                                backgroundMock.SetupGet(background => background.Path)
                                    .Returns(AnyString);
                                backgroundMock.SetupGet(background => background.Fit)
                                    .Returns(AnyFit);

                                _virtualDesktopMock.SetupGet(factory => factory.Background)
                                    .Returns(backgroundMock.Object);

                                _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                                var newTargetMock = new Mock<IVirtualDesktop>();

                                _virtualDesktopConfiguration.BindToTarget(newTargetMock.Object, _factoryMock.Object);

                                _factoryMock.Verify(factory =>
                                    factory.BackgroundFrom(
                                        It.Is<string>(s => s == AnyString),
                                        It.Is<Fit>(f => f == AnyFit)),
                                    Times.Once);
                            }

                            [TestMethod]
                            public void ShouldSetTargetBackground()
                            {
                                var backgroundMock = new Mock<IBackground>();
                                backgroundMock.SetupGet(background => background.Path)
                                    .Returns(AnyString);
                                backgroundMock.SetupGet(background => background.Fit)
                                    .Returns(AnyFit);


                                _virtualDesktopMock.SetupGet(desktop => desktop.Background)
                                    .Returns(backgroundMock.Object);

                                _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                                var newBackgroundMock = new Mock<IBackground>();

                                _factoryMock.Setup(factory => factory.BackgroundFrom(It.IsAny<string>(), It.IsAny<Fit>()))
                                    .Returns(newBackgroundMock.Object);

                                var newTargetMock = new Mock<IVirtualDesktop>();
                                newTargetMock.SetupSet(desktop => desktop.Background = It.IsAny<IBackground>());

                                _virtualDesktopConfiguration.BindToTarget(newTargetMock.Object, _factoryMock.Object);

                                newTargetMock.VerifySet(desktop => desktop.Background = It.Is<IBackground>(b => b == newBackgroundMock.Object), Times.Once);
                            }
                        }
                    }

                    [TestClass]
                    public sealed class WhenTargetIsNull : WhenBinding, IWhenBindingToNull
                    {
                        [TestMethod]
                        public void TargetDesktopShouldBeNull()
                        {
                            _virtualDesktopConfiguration.BindToTarget(null, _factoryMock.Object);

                            _virtualDesktopConfiguration.TargetDesktop.Should().Be.Null();
                        }

                        [TestMethod]
                        public void SettingBackgroundPathShouldThrow()
                        {
                            _virtualDesktopConfiguration.BindToTarget(null, _factoryMock.Object);

                            Invoke.Delegate(() =>
                                _virtualDesktopConfiguration.BackgroundPath = AnyString)
                                .Should()
                                .Throw<NoTargetBoundException>();
                        }

                        [TestMethod]
                        public void SettingFitShouldThrow()
                        {
                            _virtualDesktopConfiguration.BindToTarget(null, _factoryMock.Object);

                            Invoke.Delegate(() =>
                                _virtualDesktopConfiguration.Fit = AnyFit)
                                .Should()
                                .Throw<NoTargetBoundException>();
                        }
                    }
                }

                [TestClass]
                public class WhenSettingBackgroundPath : UsingTargetDesktopAndFactory
                {
                    [TestClass]
                    public sealed class WhenValueIsSame : WhenSettingBackgroundPath
                    {
                        [TestInitialize]
                        public override void UsingThisConfiguration()
                        {
                            base.UsingThisConfiguration();

                            _backgroundMock.SetupGet(background => background.Path)
                                .Returns(SameString);

                            SetupToSetTargetBackground();
                        }

                        [TestMethod]
                        public void ShouldNotSetTargetBackground()
                        {
                            _virtualDesktopConfiguration.BackgroundPath = SameString;

                            ShouldNotSetTargetBackgroundImpl();
                        }
                    }

                    [TestClass]
                    public sealed class WhenValueIsDifferent : WhenSettingBackgroundPath
                    {
                        public static readonly string DifferentString = $"DIFFERENT_{AnyString}";

                        [TestInitialize]
                        public override void UsingThisConfiguration()
                        {
                            base.UsingThisConfiguration();

                            _backgroundMock.SetupGet(background => background.Path)
                                .Returns(AnyString);
                        }

                        [TestMethod]
                        public void ShouldRequestNewBackgroundFromFactory()
                        {
                            _backgroundMock.SetupGet(background => background.Fit)
                                .Returns(AnyFit);

                            SetupToRequestBackground();

                            // ReSharper disable once UseObjectOrCollectionInitializer
                            _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                            _virtualDesktopConfiguration.BackgroundPath = DifferentString;

                            ShouldRequestBackgroundWith(DifferentString, AnyFit);
                        }

                        [TestMethod]
                        public void ShouldSetTargetBackground()
                        {
                            var newBackground = new Mock<IBackground>().Object;

                            SetupToRequestBackground()
                                .Returns(newBackground);

                            SetupToSetTargetBackground();

                            _virtualDesktopConfiguration.BackgroundPath = DifferentString;

                            ShouldSetTargetBackgroundTo(newBackground);
                        }
                    }
                }

                [TestClass]
                public class WhenSettingFit : UsingTargetDesktopAndFactory
                {
                    [TestClass]
                    public sealed class WhenValueIsSame : WhenSettingFit
                    {
                        public const Fit SameFit = AnyFit;

                        [TestInitialize]
                        public override void UsingThisConfiguration()
                        {
                            base.UsingThisConfiguration();

                            _backgroundMock.SetupGet(background => background.Fit)
                                .Returns(SameFit);

                            SetupToSetTargetBackground();
                        }

                        [TestMethod]
                        public void ShouldNotSetTargetBackground()
                        {
                            _virtualDesktopConfiguration.Fit = SameFit;

                            ShouldNotSetTargetBackgroundImpl();
                        }
                    }

                    [TestClass]
                    public sealed class WhenValueIsDifferent : WhenSettingFit
                    {
                        public const Fit DifferentFit = AnyFit + 1;

                        [TestInitialize]
                        public override void UsingThisConfiguration()
                        {
                            base.UsingThisConfiguration();

                            _backgroundMock.SetupGet(background => background.Fit)
                                .Returns(AnyFit);
                        }

                        [TestMethod]
                        public void ShouldRequestNewBackgroundFromFactory()
                        {
                            _backgroundMock.SetupGet(background => background.Path)
                                .Returns(AnyString);

                            SetupToRequestBackground();

                            // ReSharper disable once UseObjectOrCollectionInitializer
                            _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);

                            _virtualDesktopConfiguration.Fit = DifferentFit;

                            ShouldRequestBackgroundWith(AnyString, DifferentFit);
                        }

                        [TestMethod]
                        public void ShouldSetTargetBackground()
                        {
                            var newBackground = new Mock<IBackground>().Object;

                            SetupToRequestBackground()
                                .Returns(newBackground);

                            SetupToSetTargetBackground();

                            _virtualDesktopConfiguration.Fit = DifferentFit;

                            ShouldSetTargetBackgroundTo(newBackground);
                        }
                    }
                }

                private void SetupToSetTargetBackground()
                {
                    _virtualDesktopMock.SetupSet(desktop =>
                        desktop.Background = It.IsAny<IBackground>());

                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration(_virtualDesktopMock.Object, _factoryMock.Object);
                }
            }

            [TestClass]
            public class UsingProperties : OnceInitialized
            {
                [TestInitialize]
                public virtual void UsingThisConfiguration()
                {
                    _virtualDesktopConfiguration = new VirtualDesktopConfiguration();
                }

                [TestClass]
                public class WhenBinding : UsingProperties
                {
                    [TestInitialize]
                    public override void UsingThisConfiguration()
                    {
                        base.UsingThisConfiguration();

                        _backgroundMock = new Mock<IBackground>();

                        _virtualDesktopMock = new Mock<IVirtualDesktop>();
                        _virtualDesktopMock.SetupGet(desktop => desktop.Background)
                            .Returns(_backgroundMock.Object);

                        _factoryMock = new Mock<IConfigurationFactory>();
                    }

                    [TestClass]
                    public class WhenTargetIsValid : WhenBinding, IWhenTargetIsValid
                    {
                        [TestInitialize]
                        public override void UsingThisConfiguration()
                        {
                            base.UsingThisConfiguration();

                            _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);
                        }

                        [TestMethod]
                        public void SettingBackgroundPathShouldNotThrow()
                        {
                            Invoke.Delegate(() => _virtualDesktopConfiguration.BackgroundPath = AnyString)
                                .Should()
                                .Not
                                .Throw<NoTargetBoundException>();
                        }

                        [TestMethod]
                        public void SettingFitShouldNotThrow()
                        {
                            Invoke.Delegate(() =>
                                    _virtualDesktopConfiguration.Fit = AnyFit)
                                .Should()
                                .Not
                                .Throw<NoTargetBoundException>();
                        }

                        [TestClass]
                        public sealed class WhenConfigurationNotConfigured : WhenTargetIsValid, IWhenConfigurationNotConfigured
                        {
                            [TestMethod]
                            public void ShouldUpdateBackgroundPathElement()
                            {
                                WhenSetupToUpdateBackgroundPath();

                                _virtualDesktopConfiguration.BackgroundPathElement.Should().Equal(AnyString);
                            }

                            [TestMethod]
                            public void ShouldUpdateBackgroundPath()
                            {
                                WhenSetupToUpdateBackgroundPath();

                                // ReSharper disable once SuspiciousTypeConversion.Global
                                _virtualDesktopConfiguration.BackgroundPath.Equals(AnyString).Should().Be.True();
                            }

                            [TestMethod]
                            public void ShouldUpdateFitElement()
                            {
                                WhenSetupToUpdateFit();

                                _virtualDesktopConfiguration.FitElement.Should().Equal(AnyFit);
                            }

                            [TestMethod]
                            public void ShouldUpdateFit()
                            {
                                WhenSetupToUpdateFit();

                                _virtualDesktopConfiguration.Fit.Should().Equal(AnyFit);
                            }

                            [TestMethod]
                            public void ShouldNotRequestBackgroundFromFactory()
                            {
                                Assert.Inconclusive("Not Implemented");
                                throw new NotImplementedException();
                            }

                            [TestMethod]
                            public void ShouldNotSetTargetBackground()
                            {
                                Assert.Inconclusive("Not Implemented");
                                throw new NotImplementedException();
                            }
                        }

                        [TestClass]
                        public sealed class WhenConfigurationConfigured : WhenTargetIsValid, IWhenConfigurationConfigured
                        {
                            [TestInitialize]
                            public override void UsingThisConfiguration()
                            {
                                base.UsingThisConfiguration();

                                _virtualDesktopMock = new Mock<IVirtualDesktop>();
                                _virtualDesktopMock.SetupGet(desktop => desktop.Background)
                                    .Returns(new Mock<IBackground>().Object);

                                _factoryMock = new Mock<IConfigurationFactory>();

                                _virtualDesktopConfiguration.BackgroundPathElement = AnyString;
                                _virtualDesktopConfiguration.FitElement = AnyFit;
                            }

                            // TODO investigate unified setup and test methods in common with other test methods
                            [TestMethod]
                            public void ShouldRequestBackgroundFromFactory()
                            {
                                Assert.Inconclusive("Not Implemented");
                                throw new NotImplementedException();
                            }

                            [TestMethod]
                            public void ShouldSetTargetBackground()
                            {
                                var backgroundMock = new Mock<IBackground>();

                                _factoryMock.Setup(factory => factory.BackgroundFrom(It.IsAny<string>(), It.IsAny<Fit>()))
    .Returns(backgroundMock.Object);

                                _virtualDesktopMock.SetupSet(desktop => desktop.Background = It.IsAny<IBackground>());

                                _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                                _virtualDesktopMock.VerifySet(desktop => desktop.Background = It.Is<IBackground>(b => b == backgroundMock.Object), Times.Once);
                            }
                        }
                    }

                    [TestClass]
                    public sealed class WhenTargetIsNull : WhenBinding, IWhenBindingToNull
                    {
                        [TestInitialize]
                        public override void UsingThisConfiguration()
                        {
                            base.UsingThisConfiguration();

                            _virtualDesktopConfiguration.BindToTarget(null, _factoryMock.Object);
                        }

                        [TestMethod]
                        public void SettingBackgroundPathShouldThrow()
                        {
                            Invoke.Delegate(() =>
                                    _virtualDesktopConfiguration.BackgroundPath = AnyString)
                                .Should()
                                .Throw<NoTargetBoundException>();
                        }

                        [TestMethod]
                        public void SettingFitShouldThrow()
                        {
                            Invoke.Delegate(() =>
                                    _virtualDesktopConfiguration.Fit = AnyFit)
                                .Should().Throw<NoTargetBoundException>();
                        }

                        [TestMethod]
                        public void TargetDesktopShouldBeNull()
                        {
                            _virtualDesktopConfiguration.TargetDesktop.Should().Be.Null();
                        }
                    }
                }

                [TestClass]
                public class WhenRebinding : UsingProperties
                {
                    private Mock<IPropertyChangedBinding> _bindingMock;

                    [TestInitialize]
                    public override void UsingThisConfiguration()
                    {
                        base.UsingThisConfiguration();

                        _virtualDesktopMock = new Mock<IVirtualDesktop>();
                        _virtualDesktopMock.SetupGet(desktop => desktop.Background)
                            .Returns(new Mock<IBackground>().Object);

                        _factoryMock = new Mock<IConfigurationFactory>();

                        _bindingMock = new Mock<IPropertyChangedBinding>();
                        _bindingMock.Setup(binding => binding.Unbind());

                        _factoryMock.Setup(factory =>
                                factory.Bind(
                                    It.IsAny<Action>(),
                                    It.IsAny<INotifyPropertyChanged>()))
                            .Returns(_bindingMock.Object);

                        // this is the initial binding. tests are for rebinding
                        _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);
                    }

                    [TestClass]
                    public sealed class WhenAlreadyBound : WhenRebinding, IWhenAlreadyBound
                    {
                        [TestMethod]
                        public void ShouldUnbindExistingBinding()
                        {
                            _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                            _bindingMock.Verify(binding => binding.Unbind(), Times.Once);
                        }
                    }

                    [TestClass]
                    public class WhenTargetIsValid : WhenRebinding
                    {
                        [TestClass]
                        public sealed class ShouldEnableSetters : WhenTargetIsValid, IWhenTargetIsValid
                        {
                            [TestMethod]
                            public void SettingBackgroundPathShouldNotThrow()
                            {
                                _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object,
                                    _factoryMock.Object);

                                Invoke.Delegate(() =>
                                        _virtualDesktopConfiguration.BackgroundPath = AnyString)
                                    .Should()
                                    .Not
                                    .Throw<NoTargetBoundException>();
                            }

                            [TestMethod]
                            public void SettingFitShouldNotThrow()
                            {
                                _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object,
                                    _factoryMock.Object);

                                Invoke.Delegate(() =>
                                        _virtualDesktopConfiguration.Fit = AnyFit)
                                    .Should()
                                    .Not
                                    .Throw<NoTargetBoundException>();
                            }
                        }

                        [TestClass]
                        public sealed class WhenConfigurationNotConfigured : WhenTargetIsValid, IWhenConfigurationNotConfigured
                        {
                            [TestMethod]
                            public void ShouldUpdateBackgroundPath()
                            {
                                Assert.Inconclusive("Not Implemented");
                                throw new NotImplementedException();
                            }

                            [TestMethod]
                            public void ShouldUpdateFit()
                            {
                                Assert.Inconclusive("Not Implemented");
                                throw new NotImplementedException();
                            }

                            [TestMethod]
                            public void ShouldNotRequestBackgroundFromFactory()
                            {
                                Assert.Inconclusive("Not Implemented");
                                throw new NotImplementedException();
                            }

                            [TestMethod]
                            public void ShouldNotSetTargetBackground()
                            {
                                Assert.Inconclusive("Not Implemented");
                                throw new NotImplementedException();
                            }

                            [TestMethod]
                            public void ShouldUpdateBackgroundPathElement()
                            {
                                Assert.Inconclusive("Not Implemented");
                                throw new NotImplementedException();
                            }

                            [TestMethod]
                            public void ShouldUpdateFitElement()
                            {
                                Assert.Inconclusive("Not Implemented");
                                throw new NotImplementedException();
                            }
                        }

                        [TestClass]
                        public sealed class WhenConfigurationConfigured : WhenTargetIsValid, IWhenConfigurationConfigured
                        {
                            [TestMethod]
                            public void ShouldRequestBackgroundFromFactory()
                            {
                                _virtualDesktopConfiguration.BackgroundPathElement = AnyString;
                                _virtualDesktopConfiguration.FitElement = AnyFit;

                                _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                                _factoryMock.Verify(factory =>
                                        factory.BackgroundFrom(
                                            It.Is<string>(s => s == AnyString),
                                            It.Is<Fit>(f => f == AnyFit)),
                                    Times.Once);
                            }

                            [TestMethod]
                            public void ShouldSetTargetBackground()
                            {
                                var backgroundMock = new Mock<IBackground>();

                                _factoryMock.Setup(factory => factory.BackgroundFrom(AnyString, AnyFit))
                                    .Returns(backgroundMock.Object);

                                _virtualDesktopMock.SetupSet(desktop => desktop.Background = It.IsAny<IBackground>());

                                _virtualDesktopConfiguration.BackgroundPathElement = AnyString;
                                _virtualDesktopConfiguration.FitElement = AnyFit;

                                _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);

                                _virtualDesktopMock.VerifySet(
                                    desktop => desktop.Background = It.Is<IBackground>(b => b == backgroundMock.Object),
                                    Times.Once);
                            }
                        }
                    }
                }

                // TODO evaluate whether this test class is necessary due to changes in WhenRebindingToTarget -> WhenRebinding
                [TestClass]
                public sealed class WhenRebindingToNull : UsingProperties, IWhenBindingToNull, IWhenAlreadyBound
                {
                    private Mock<IPropertyChangedBinding> _bindingMock;

                    [TestInitialize]
                    public override void UsingThisConfiguration()
                    {
                        base.UsingThisConfiguration();

                        _virtualDesktopConfiguration = new VirtualDesktopConfiguration();

                        _virtualDesktopMock = new Mock<IVirtualDesktop>();
                        _virtualDesktopMock.SetupGet(desktop => desktop.Background)
                            .Returns(new Mock<IBackground>().Object);

                        _bindingMock = new Mock<IPropertyChangedBinding>();

                        _factoryMock = new Mock<IConfigurationFactory>();
                        _factoryMock.Setup(factory =>
                                factory.Bind(
                                    It.IsAny<Action>(),
                                    It.IsAny<IVirtualDesktop>()))
                            .Returns(_bindingMock.Object);

                        _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);
                    }

                    [TestMethod]
                    public void ShouldUnbindExistingBinding()
                    {
                        _bindingMock.Setup(binding => binding.Unbind());

                        _virtualDesktopConfiguration.BindToTarget(null, _factoryMock.Object);

                        _bindingMock.Verify(binding =>
                                binding.Unbind(),
                            Times.Once);
                    }

                    [TestMethod]
                    public void TargetDesktopShouldBeNull()
                    {
                        _virtualDesktopConfiguration.BindToTarget(null, _factoryMock.Object);

                        _virtualDesktopConfiguration.TargetDesktop.Should().Be.Null();
                    }

                    [TestMethod]
                    public void SettingBackgroundPathShouldThrow()
                    {
                        _virtualDesktopConfiguration.BindToTarget(null, _factoryMock.Object);

                        Invoke.Delegate(() =>
                                _virtualDesktopConfiguration.BackgroundPath = AnyString)
                            .Should()
                            .Throw<NoTargetBoundException>();
                    }

                    [TestMethod]
                    public void SettingFitShouldThrow()
                    {
                        _virtualDesktopConfiguration.BindToTarget(null, _factoryMock.Object);

                        Invoke.Delegate(() =>
                                _virtualDesktopConfiguration.Fit = AnyFit)
                            .Should()
                            .Throw<NoTargetBoundException>();
                    }
                }

                [TestClass]
                public class WhenSettingBackgroundPath : UsingProperties
                {
                    [TestClass]
                    public sealed class WhenValueIsSame : WhenSettingBackgroundPath
                    {
                        [TestMethod]
                        public void MyTestMethod()
                        {
                            Assert.Inconclusive("Not Implemented");
                            throw new NotImplementedException();
                        }
                    }

                    [TestClass]
                    public sealed class WhenValueIsDifferent : WhenSettingBackgroundPath
                    {
                        [TestMethod]
                        public void MyTestMethod()
                        {
                            Assert.Inconclusive("Not Implemented");
                            throw new NotImplementedException();
                        }
                    }
                }

                [TestClass]
                public class WhenSettingFit : UsingProperties
                {
                    [TestClass]
                    public sealed class WhenValueIsSame : WhenSettingFit
                    {
                        [TestMethod]
                        public void MyTestMethod()
                        {
                            Assert.Inconclusive("Not Implemented");
                            throw new NotImplementedException();
                        }
                    }

                    [TestClass]
                    public sealed class WhenValueIsDifferent : WhenSettingFit
                    {
                        [TestMethod]
                        public void MyTestMethod()
                        {
                            Assert.Inconclusive("Not Implemented");
                            throw new NotImplementedException();
                        }
                    }
                }
            }

            private void WhenSetupToUpdateBackgroundPath()
            {
                _backgroundMock.SetupGet(background =>
                        background.Path)
                    .Returns(AnyString);

                _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);
            }

            private void WhenSetupToUpdateFit()
            {
                _backgroundMock.SetupGet(background => background.Fit)
                    .Returns(AnyFit);

                _virtualDesktopConfiguration.BindToTarget(_virtualDesktopMock.Object, _factoryMock.Object);
            }

            private ISetup<IConfigurationFactory, IBackground> SetupToRequestBackground()
            {
                return _factoryMock.Setup(factory =>
                    factory.BackgroundFrom(
                        It.IsAny<string>(),
                        It.IsAny<Fit>()));
            }

            private void ShouldRequestBackgroundWith(string path, Fit fit)
            {
                _factoryMock.Verify(factory =>
                        factory.BackgroundFrom(
                            It.Is<string>(s => path == s),
                            It.Is<Fit>(f => fit == f)),
                    Times.Once);
            }


            private void ShouldNotRequestBackground()
            {
                _factoryMock.Verify(factory =>
                        factory.BackgroundFrom(
                            It.IsAny<string>(),
                            It.IsAny<Fit>()),
                    Times.Never);
            }

            private void ShouldNotSetTargetBackgroundImpl()
            {
                _virtualDesktopMock.VerifySet(desktop =>
                        desktop.Background = It.IsAny<IBackground>(),
                    Times.Never);
            }

            private void ShouldSetTargetBackgroundTo(IBackground newBackground)
            {
                _virtualDesktopMock.VerifySet(desktop =>
                        desktop.Background =
                            It.Is<IBackground>(background =>
                                newBackground == background),
                    Times.Once);
            }
        }

        [TestClass]
        public sealed class WhenSettingBackgroundPathElement : VirtualDesktopConfigurationTest
        {

        }

        [TestClass]
        public sealed class WhenGettingBackgroundPathElement : VirtualDesktopConfigurationTest
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