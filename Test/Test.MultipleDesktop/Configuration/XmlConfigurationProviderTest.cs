using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MultipleDesktop.Configuration;
using MultipleDesktop.Mvc.Configuration;
using Should.Fluent;
using System;
using System.IO;
using System.IO.Extended;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Serialization.Extended;
using VisualStudio.TestTools.UnitTesting;

namespace Test.MultipleDesktop.Configuration
{
    [TestClass]
    public abstract class XmlConfigurationProviderTest
    {
        public const string FilePath = @"C:\";

        public static readonly Exception AnyError = new ExceptionMock();

        protected abstract Func<IoResult> MethodUnderTest { get; }

        private Mock<IXmlConfigurationFactory> _configurationFactoryMock;

        private XmlConfigurationProvider _xmlConfigurationProvider;

        [TestInitialize]
        public virtual void UsingThisConfiguration()
        {
            _configurationFactoryMock = new Mock<IXmlConfigurationFactory>();

            _xmlConfigurationProvider = new XmlConfigurationProvider(_configurationFactoryMock.Object);
        }

        [TestClass]
        public abstract class WhenWriting : XmlConfigurationProviderTest
        {
            [TestClass]
            public class WhenCreating : WhenWriting
            {
                protected sealed override Func<IoResult> MethodUnderTest =>
                    () => _xmlConfigurationProvider.Create(
                        FilePath,
                        out _result);

                private IAppConfiguration _result;

                [TestClass]
                public sealed class TryCreateConfiguration : WhenCreating
                {
                    [TestMethod]
                    public void ShouldRequestInstanceFromFactory()
                    {
                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateXmlConfiguration());

                        MethodUnderTest();

                        _configurationFactoryMock.Verify(factory =>
                            factory.CreateXmlConfiguration(),
                            Times.Once);
                    }

                    [TestMethod]
                    public void OnFailure()
                    {
                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateXmlConfiguration())
                            .Throws(AnyError);

                        ShouldReturnErrorResultFor(AnyError);
                    }
                }

                [TestClass]
                public sealed class TryCreateXmlSerializer : WhenCreating
                {
                    [TestMethod]
                    public void ShouldRequestInstanceFromFactory()
                    {
                        ShouldRequestSerializerFromFactory();
                    }

                    [TestMethod]
                    public void OnFailure()
                    {
                        WhenSerializationFails();

                        ShouldReturnErrorResultFor(AnyError);
                    }
                }

                [TestClass]
                public sealed class TryCreateWriterForFilePath : WhenCreating
                {
                    [TestMethod]
                    public void ShouldRequestInstanceFromFactory()
                    {
                        ShouldRequestWriterFromFactory();
                    }

                    [TestMethod]
                    public void OnFailure()
                    {
                        WhenWriterCreationFails();

                        ShouldReturnErrorResultFor(AnyError);
                    }
                }

                [TestClass]
                public sealed class TrySerializeConfigurationUsingWriter : WhenCreating
                {
                    [TestMethod]
                    public void ShouldPassConfigurationAndWriterToSerializer()
                    {
                        var xmlConfiguration = new Mock<AppConfiguration>().Object;

                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateXmlConfiguration())
                            .Returns(xmlConfiguration);

                        ShouldSerializeConfigurationUsingWriter(xmlConfiguration);
                    }

                    [TestMethod]
                    public void OnSuccess()
                    {
                        var xmlConfiguration = new Mock<AppConfiguration>().Object;

                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateXmlConfiguration())
                            .Returns(xmlConfiguration);

                        WhenSerializationSucceeds();

                        ShouldReturnSuccessResult();

                        _result.Should().Be.SameAs(xmlConfiguration);
                    }

                    [TestMethod]
                    public void OnFailure()
                    {
                        WhenSerializationFails();

                        ShouldReturnErrorResultFor(AnyError);
                    }
                }
            }

            [TestClass]
            public class WhenSaving : WhenWriting
            {
                protected sealed override Func<IoResult> MethodUnderTest =>
                    () => _xmlConfigurationProvider.Save(_appConfigurationMock.Object, FilePath);

                private Mock<IAppConfiguration> _appConfigurationMock;

                [TestInitialize]
                public override void UsingThisConfiguration()
                {
                    base.UsingThisConfiguration();

                    _appConfigurationMock = new Mock<IAppConfiguration>();
                }

                [TestClass]
                public sealed class TryCreateXmlSerializer : WhenSaving
                {
                    [TestMethod]
                    public void ShouldRequestInstanceFromFactory()
                    {
                        ShouldRequestSerializerFromFactory();
                    }

                    [TestMethod]
                    public void OnFailure()
                    {
                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateSerializerFor<AppConfiguration>())
                            .Throws(AnyError);

                        ShouldReturnErrorResultFor(AnyError);
                    }
                }

                [TestClass]
                public sealed class TryCreateWriterForFilePath : WhenSaving
                {
                    [TestMethod]
                    public void ShouldRequestInstanceFromFactory()
                    {
                        ShouldRequestWriterFromFactory();
                    }

                    [TestMethod]
                    public void OnFailure()
                    {
                        WhenWriterCreationFails();

                        ShouldReturnErrorResultFor(AnyError);
                    }
                }

                [TestClass]
                public sealed class TryConvertConfigurationToXml : WhenSaving
                {
                    [TestInitialize]
                    public sealed override void UsingThisConfiguration()
                    {
                        base.UsingThisConfiguration();

                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateSerializerFor<AppConfiguration>())
                            .Returns(new Mock<IXmlSerializer>().Object);
                    }

                    [TestMethod]
                    public void ShouldRequestConversionFromFactory()
                    {
                        _configurationFactoryMock.Setup(factory =>
                            factory.ToXmlConfiguration(It.IsAny<IAppConfiguration>()));

                        _xmlConfigurationProvider.Save(_appConfigurationMock.Object, FilePath);

                        _configurationFactoryMock.Verify(factory =>
                            factory.ToXmlConfiguration(
                                It.Is<IAppConfiguration>(configuration =>
                                    configuration == _appConfigurationMock.Object)),
                            Times.Once);
                    }

                    [TestMethod]
                    public void OnFailure()
                    {
                        _configurationFactoryMock.Setup(factory =>
                            factory.ToXmlConfiguration(It.IsAny<IAppConfiguration>()))
                            .Throws(AnyError);

                        ShouldReturnErrorResultFor(AnyError);
                    }
                }

                [TestClass]
                public sealed class TrySerializeConfigurationUsingWriter : WhenSaving
                {
                    [TestMethod]
                    public void ShouldPassConfigurationAndWriterToSerialize()
                    {
                        var xmlConfiguration = new Mock<AppConfiguration>().Object;

                        _configurationFactoryMock.Setup(factory =>
                            factory.ToXmlConfiguration(It.IsAny<IAppConfiguration>()))
                            .Returns(xmlConfiguration);

                        ShouldSerializeConfigurationUsingWriter(xmlConfiguration);
                    }

                    [TestMethod]
                    public void OnSuccess()
                    {
                        WhenSerializationSucceeds();

                        ShouldReturnSuccessResult();
                    }

                    [TestMethod]
                    public void OnFailure()
                    {
                        WhenSerializationFails();

                        ShouldReturnErrorResultFor(AnyError);
                    }
                }
            }

            private void ShouldRequestWriterFromFactory()
            {
                _configurationFactoryMock.Setup(factory =>
                    factory.CreateSerializationWriterFor(It.IsAny<string>()));

                MethodUnderTest();

                _configurationFactoryMock.Verify(factory =>
                    factory.CreateSerializationWriterFor(
                        It.Is<string>(path => path == FilePath)),
                    Times.Once);
            }

            private void WhenWriterCreationFails()
            {
                _configurationFactoryMock.Setup(factory =>
                    factory.CreateSerializationWriterFor(It.IsAny<string>()))
                    .Throws(AnyError);
            }

            private void ShouldSerializeConfigurationUsingWriter(AppConfiguration expectedConfiguration)
            {
                var serializerMock = WhenSerializationSucceeds();

                var writer = new Mock<TextWriter>().Object;

                _configurationFactoryMock.Setup(factory =>
                    factory.CreateSerializationWriterFor(It.IsAny<string>()))
                    .Returns(writer);

                MethodUnderTest();

                serializerMock.Verify(serializer =>
                    serializer.Serialize(
                        It.Is<TextWriter>(textWriter => textWriter == writer),
                        It.Is<object>(o => o == expectedConfiguration)),
                    Times.Once);
            }

            private Mock<IXmlSerializer> WhenSerializationSucceeds()
            {
                var serializerMock = new Mock<IXmlSerializer>();
                serializerMock.Setup(serializer =>
                    serializer.Serialize(
                        It.IsAny<TextWriter>(),
                        It.IsAny<object>()));

                _configurationFactoryMock.Setup(factory =>
                    factory.CreateSerializerFor<AppConfiguration>())
                    .Returns(serializerMock.Object);

                return serializerMock;
            }

            private void WhenSerializationFails()
            {
                var serializerMock = new Mock<IXmlSerializer>();
                serializerMock.Setup(serializer =>
                    serializer.Serialize(
                        It.IsAny<TextWriter>(),
                        It.IsAny<object>()))
                    .Throws(AnyError);

                _configurationFactoryMock.Setup(factory =>
                    factory.CreateSerializerFor<AppConfiguration>())
                    .Returns(serializerMock.Object);
            }
        }

        [TestClass]
        public class WhenLoading : XmlConfigurationProviderTest
        {
            /// <summary>
            /// Add exception handling unit tests here as abstract
            /// to ensure that all implementing error handling
            /// test classes appropriately implement tests for
            /// all exceptions.
            /// </summary>
            public abstract class LoadFailureTest : WhenLoading
            {
                protected abstract void SetupThrows(Exception error);

                [TestMethod]
                public abstract void FromXmlException();
                [TestMethod]
                public abstract void FromFileNotFoundException();
                [TestMethod]
                public abstract void FromDirectoryNotFoundException();
                [TestMethod]
                public abstract void FromAnyOtherException();

                protected void ShouldReturnErrorForXmlException()
                {
                    SetupThrowAndAssertErrorResult(new InvalidOperationException(string.Empty, new XmlException()));
                }

                protected void ShouldReturnErrorForFileNotFoundException()
                {
                    SetupThrowAndAssertErrorResult(new FileNotFoundException());
                }

                protected void ShouldReturnErrorForDirectoryNotFoundException()
                {
                    SetupThrowAndAssertErrorResult(new DirectoryNotFoundException());
                }

                protected void ShouldReturnErrorForAnyException()
                {
                    SetupThrowAndAssertErrorResult(AnyError);
                }

                private void SetupThrowAndAssertErrorResult(Exception error)
                {
                    SetupThrows(error);

                    ShouldReturnErrorResultFor(error);
                }
            }

            protected override Func<IoResult> MethodUnderTest =>
                () => _xmlConfigurationProvider.Load(FilePath, out _result);

            private IAppConfiguration _result;

            [TestClass]
            public class TryCreateXmlSerializer : WhenLoading
            {
                [TestClass]
                public sealed class WhenCreatingXmlSerializer : TryCreateXmlSerializer
                {
                    [TestMethod]
                    public void ShouldRequestInstanceFromFactory()
                    {
                        ShouldRequestSerializerFromFactory();
                    }
                }

                [TestClass]
                public sealed class OnFailure : LoadFailureTest
                {
                    protected override void SetupThrows(Exception error)
                    {
                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateSerializerFor<AppConfiguration>())
                            .Throws(error);
                    }

                    [TestMethod]
                    public override void FromXmlException()
                    {
                        ShouldReturnErrorForXmlException();
                    }

                    [TestMethod]
                    public override void FromFileNotFoundException()
                    {
                        ShouldReturnErrorForFileNotFoundException();
                    }

                    [TestMethod]
                    public override void FromDirectoryNotFoundException()
                    {
                        ShouldReturnErrorForDirectoryNotFoundException();
                    }

                    [TestMethod]
                    public override void FromAnyOtherException()
                    {
                        ShouldReturnErrorForAnyException();
                    }
                }
            }

            [TestClass]
            public class TryCreateReaderForFilePath : WhenLoading
            {
                [TestClass]
                public sealed class WhenCreatingReader : TryCreateReaderForFilePath
                {
                    [TestMethod]
                    public void ShouldRequestInstanceFromFactory()
                    {
                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateSerializationReaderFor(
                                It.IsAny<string>()));

                        MethodUnderTest();

                        _configurationFactoryMock.Verify(factory =>
                            factory.CreateSerializationReaderFor(
                                It.Is<string>(s => s == FilePath)),
                            Times.Once);
                    }
                }

                [TestClass]
                public sealed class OnFailure : LoadFailureTest
                {
                    protected override void SetupThrows(Exception error)
                    {
                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateSerializationReaderFor(
                                It.IsAny<string>()))
                            .Throws(error);
                    }

                    [TestMethod]
                    public override void FromXmlException()
                    {
                        ShouldReturnErrorForXmlException();
                    }

                    [TestMethod]
                    public override void FromFileNotFoundException()
                    {
                        ShouldReturnErrorForFileNotFoundException();
                    }

                    [TestMethod]
                    public override void FromDirectoryNotFoundException()
                    {
                        ShouldReturnErrorForDirectoryNotFoundException();
                    }

                    [TestMethod]
                    public override void FromAnyOtherException()
                    {
                        ShouldReturnErrorForAnyException();
                    }
                }
            }

            [TestClass]
            public class TryDeserializeConfigurationUsingReader : WhenLoading
            {
                [TestClass]
                public sealed class WhenDeserializing : TryDeserializeConfigurationUsingReader
                {
                    [TestMethod]
                    public void ShouldPassReaderToDeserialize()
                    {
                        var reader = new Mock<TextReader>().Object;

                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateSerializationReaderFor(
                            It.IsAny<string>()))
                            .Returns(reader);

                        var serializerMock = new Mock<IXmlSerializer>();

                        serializerMock.Setup(serializer =>
                            serializer.Deserialize(It.IsAny<TextReader>()));

                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateSerializerFor<AppConfiguration>())
                            .Returns(serializerMock.Object);

                        MethodUnderTest();

                        serializerMock.Verify(serializer =>
                            serializer.Deserialize(
                                It.Is<TextReader>(textReader =>
                                    textReader == reader)),
                            Times.Once);
                    }

                    [TestMethod]
                    public void OnSuccess()
                    {
                        var serializerMock = new Mock<IXmlSerializer>();

                        serializerMock.Setup(serializer =>
                            serializer.Deserialize(It.IsAny<TextReader>()));

                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateSerializerFor<AppConfiguration>())
                            .Returns(serializerMock.Object);

                        ShouldReturnSuccessResult();
                    }
                }

                [TestClass]
                public sealed class OnFailure : LoadFailureTest
                {
                    protected override void SetupThrows(Exception error)
                    {
                        var serializerMock = new Mock<IXmlSerializer>();

                        serializerMock.Setup(serializer =>
                            serializer.Deserialize(It.IsAny<TextReader>()))
                            .Throws(error);

                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateSerializerFor<AppConfiguration>())
                            .Returns(serializerMock.Object);
                    }

                    [TestMethod]
                    public override void FromXmlException()
                    {
                        ShouldReturnErrorForXmlException();
                    }

                    [TestMethod]
                    public override void FromFileNotFoundException()
                    {
                        ShouldReturnErrorForFileNotFoundException();
                    }

                    [TestMethod]
                    public override void FromDirectoryNotFoundException()
                    {
                        ShouldReturnErrorForDirectoryNotFoundException();
                    }

                    [TestMethod]
                    public override void FromAnyOtherException()
                    {
                        ShouldReturnErrorForAnyException();
                    }
                }
            }
        }

        private void ShouldRequestSerializerFromFactory()
        {
            _configurationFactoryMock.Setup(factory =>
                factory.CreateSerializerFor<AppConfiguration>());

            MethodUnderTest();

            _configurationFactoryMock.Verify(factory =>
                factory.CreateSerializerFor<AppConfiguration>(),
                Times.Once);
        }

        private void ShouldReturnSuccessResult()
        {
            var result = MethodUnderTest();

            result.DidFail.Should().Be.False();
        }

        private void ShouldReturnErrorResultFor(Exception error)
        {
            var result = MethodUnderTest();

            result.DidFail.Should().Be.True();
            result.Exception.Should().Be.SameAs(error);
        }
    }
}