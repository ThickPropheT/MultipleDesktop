using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MultipleDesktop.Configuration;
using MultipleDesktop.Mvc.Configuration;
using Should.Fluent;
using Should.Fluent.Invocation;
using System;
using System.IO;
using System.Xml;
using VisualStudio.TestTools.UnitTesting;
// ReSharper disable RedundantAssignment

namespace Test.MultipleDesktop.Configuration
{
    [TestClass]
    public class ConfigurationControllerTest
    {
        public static readonly IoResult SuccessResult = IoResult.ForSuccess();
        public static readonly IoResult ErrorResult = IoResult.ForException(new ExceptionMock());

        private Mock<IConfigurationProvider> _configurationProviderMock;

        private ConfigurationController _configurationController;

        [TestInitialize]
        public virtual void UsingThisConfiguration()
        {
            _configurationProviderMock = new Mock<IConfigurationProvider>();

            _configurationController = new ConfigurationController(_configurationProviderMock.Object);
        }

        [TestClass]
        public class WhenLoading : ConfigurationControllerTest
        {
            [TestClass]
            public abstract class TryLoad : WhenLoading
            {
                public abstract IoResult LoadResult { get; }

                private Mock<IAppConfiguration> _appConfigurationMock;

                [TestInitialize]
                public sealed override void UsingThisConfiguration()
                {
                    base.UsingThisConfiguration();

                    _appConfigurationMock = new Mock<IAppConfiguration>();
                    IAppConfiguration expected = _appConfigurationMock.Object;

                    _configurationProviderMock.Setup(provider =>
                        provider.Load(It.IsAny<string>(), out expected))
                            .Returns(LoadResult);
                }

                [TestClass]
                public sealed class OnLoadSuccess : TryLoad
                {
                    public override IoResult LoadResult => SuccessResult;

                    [TestMethod]
                    public void ShouldReturnLoadedConfiguration()
                    {
                        var result = _configurationController.Load();

                        result.Should().Be.SameAs(_appConfigurationMock.Object);
                    }
                }

                [TestClass]
                public abstract class OnLoadFailure : TryLoad
                {
                    [TestClass]
                    public sealed class FromFileNotFound : OnLoadFailure
                    {
                        public static readonly IoResult NotFoundResult = IoResult.ForNotFound(new FileNotFoundException());

                        public override IoResult LoadResult => NotFoundResult;

                        [TestMethod]
                        public void ShouldTryCreate()
                        {
                            ShouldTryCreateImpl();
                        }

                        [TestMethod]
                        public void OnSuccess()
                        {
                            ShouldReturnCreatedConfiguration();
                        }

                        [TestMethod]
                        public void OnFailure()
                        {
                            ShouldThrow();
                        }
                    }

                    [TestClass]
                    public sealed class FromDirectoryNotFound : OnLoadFailure
                    {
                        public static readonly IoResult NotFoundResult = IoResult.ForNotFound(new DirectoryNotFoundException());

                        public override IoResult LoadResult => NotFoundResult;

                        [TestMethod]
                        public void ShouldTryCreate()
                        {
                            ShouldTryCreateImpl();
                        }

                        [TestMethod]
                        public void OnSuccess()
                        {
                            ShouldReturnCreatedConfiguration();
                        }

                        [TestMethod]
                        public void OnFailure()
                        {
                            ShouldThrow();
                        }
                    }

                    [TestClass]
                    public sealed class FromEmptyFile : OnLoadFailure
                    {
                        public static readonly IoResult EmptyFileResult = IoResult.ForReadError(new InvalidOperationException(string.Empty, new XmlException()));

                        public override IoResult LoadResult => EmptyFileResult;

                        [TestMethod]
                        public void ShouldTryCreate()
                        {
                            ShouldTryCreateImpl();
                        }

                        [TestMethod]
                        public void OnSuccess()
                        {
                            ShouldReturnCreatedConfiguration();
                        }

                        [TestMethod]
                        public void OnFailure()
                        {
                            ShouldThrow();
                        }
                    }

                    [TestClass]
                    public sealed class ForAnyOtherReason : OnLoadFailure
                    {
                        public override IoResult LoadResult => ErrorResult;

                        [TestMethod]
                        public void ShouldThrow()
                        {
                            ShouldThrowImpl(LoadResult);
                        }
                    }

                    protected void ShouldTryCreateImpl()
                    {
                        IAppConfiguration returns = _appConfigurationMock.Object;

                        _configurationController.Load();

                        _configurationProviderMock.Verify(provider =>
                            provider.Create(It.IsAny<string>(), out returns), Times.Once);
                    }

                    protected void ShouldReturnCreatedConfiguration()
                    {
                        SetupProviderMockFor(SuccessResult);

                        var result = _configurationController.Load();

                        result.Should().Be.SameAs(_appConfigurationMock.Object);
                    }

                    protected void ShouldThrow()
                    {
                        SetupProviderMockFor(ErrorResult);

                        ShouldThrowImpl(ErrorResult);
                    }

                    private void SetupProviderMockFor(IoResult result)
                    {
                        IAppConfiguration expected = _appConfigurationMock.Object;

                        _configurationProviderMock.Setup(provider =>
                            provider.Create(It.IsAny<string>(), out expected))
                            .Returns(result);
                    }
                }

                protected void ShouldThrowImpl(IoResult result)
                {
                    Invoke.Delegate(() =>
                        _configurationController.Load())
                        .Should()
                        .Throw(result.Exception);
                }
            }
        }

        [TestClass]
        public sealed class WhenSaving : ConfigurationControllerTest
        {
            private Mock<IAppConfiguration> _appConfigurationMock;

            [TestInitialize]
            public override void UsingThisConfiguration()
            {
                base.UsingThisConfiguration();

                _appConfigurationMock = new Mock<IAppConfiguration>();
            }

            [TestMethod]
            public void ShouldTrySave()
            {
                SetupProviderWithResult(SuccessResult);

                _configurationController.Save(_appConfigurationMock.Object);

                _configurationProviderMock.Verify(provider =>
                    provider.Save(
                        It.Is<IAppConfiguration>(configuration =>
                            configuration == _appConfigurationMock.Object),
                        It.IsAny<string>()));
            }

            [TestMethod]
            public void OnFailure()
            {
                SetupProviderWithResult(ErrorResult);

                Invoke.Delegate(() =>
                    _configurationController.Save(_appConfigurationMock.Object))
                    .Should()
                    .Throw(ErrorResult.Exception);
            }

            private void SetupProviderWithResult(IoResult result)
            {
                _configurationProviderMock.Setup(provider =>
                    provider.Save(It.IsAny<IAppConfiguration>(), It.IsAny<string>()))
                    .Returns(result);
            }
        }
    }
}
