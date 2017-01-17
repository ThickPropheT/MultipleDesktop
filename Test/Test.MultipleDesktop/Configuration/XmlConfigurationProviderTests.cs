using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MultipleDesktop.Configuration;
using MultipleDesktop.Mvc.Configuration;
using Should.Fluent;
using System;
using System.IO;
using System.IO.Extended;
using System.Xml.Serialization;
using System.Xml.Serialization.Extended;
using VisualStudio.TestTools.UnitTesting;

namespace Test.MultipleDesktop.Configuration
{
    [TestClass]
    public abstract class XmlConfigurationProviderTests
    {
        public const string FilePath = @"C:\";

        public static readonly Exception Error = new ExceptionMock();
        public static readonly IoResult ErrorResult = IoResult.ForException(Error);

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
        public abstract class WhenWriting : XmlConfigurationProviderTests
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
                            .Throws(Error);

                        ShouldReturnError();
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

                        ShouldReturnError();
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

                        ShouldReturnError();
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

                        ShouldReturnSuccess();

                        _result.Should().Be.SameAs(xmlConfiguration);
                    }

                    [TestMethod]
                    public void OnFailure()
                    {
                        WhenSerializationFails();

                        ShouldReturnError();
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
                        WhenSerializerCreationFails();

                        ShouldReturnError();
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

                        ShouldReturnError();
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
                            .Throws(Error);

                        ShouldReturnError();
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

                        ShouldReturnSuccess();
                    }

                    [TestMethod]
                    public void OnFailure()
                    {
                        WhenSerializationFails();

                        ShouldReturnError();
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
                    .Throws(Error);
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
                    .Throws(Error);

                _configurationFactoryMock.Setup(factory =>
                    factory.CreateSerializerFor<AppConfiguration>())
                    .Returns(serializerMock.Object);
            }
        }

        [TestClass]
        public abstract class WhenReading : XmlConfigurationProviderTests
        {
            [TestClass]
            public class WhenLoading : XmlConfigurationProviderTests
            {
                protected override Func<IoResult> MethodUnderTest =>
                    () => _xmlConfigurationProvider.Load(FilePath, out _result);

                private IAppConfiguration _result;

                [TestClass]
                public sealed class TryCreateXmlSerializer : WhenLoading
                {
                    [TestMethod]
                    public void ShouldRequestInstanceFromFactory()
                    {
                        ShouldRequestSerializerFromFactory();
                    }

                    [TestMethod]
                    public void OnFailure()
                    {
                        WhenSerializerCreationFails();

                        ShouldReturnError();
                    }
                }

                [TestClass]
                public sealed class TryCreateReaderForFilePath : WhenLoading
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

                    [TestMethod]
                    public void OnFailure()
                    {
                        _configurationFactoryMock.Setup(factory =>
                            factory.CreateSerializationReaderFor(
                                It.IsAny<string>()))
                            .Throws(Error);

                        ShouldReturnError();
                    }
                }

                [TestClass]
                public class TryDeserializeConfigurationUsingReader : WhenLoading
                {
                    private Mock<IXmlSerializer> _serializerMock;

                    [TestInitialize]
                    public override void UsingThisConfiguration()
                    {
                        base.UsingThisConfiguration();

                        _serializerMock = new Mock<IXmlSerializer>();
                    }

                    [TestClass]
                    public sealed class OnSuccess : TryDeserializeConfigurationUsingReader
                    {
                        [TestMethod]
                        public void ShouldPassReaderToDeserialize()
                        {
                            var reader = new Mock<TextReader>().Object;

                            _configurationFactoryMock.Setup(factory =>
                                factory.CreateSerializationReaderFor(
                                It.IsAny<string>()))
                                .Returns(reader);

                            _serializerMock.Setup(serializer =>
                                serializer.Deserialize(It.IsAny<TextReader>()));

                            _configurationFactoryMock.Setup(factory =>
                                factory.CreateSerializerFor<AppConfiguration>())
                                .Returns(_serializerMock.Object);

                            MethodUnderTest();

                            _serializerMock.Verify(serializer =>
                                serializer.Deserialize(
                                    It.Is<TextReader>(textReader =>
                                        textReader == reader)),
                                Times.Once);
                        }

                        [TestMethod]
                        public void ShouldReturnSuccessResult()
                        {
                            _serializerMock.Setup(serializer =>
                                serializer.Deserialize(It.IsAny<TextReader>()));

                            _configurationFactoryMock.Setup(factory =>
                                factory.CreateSerializerFor<AppConfiguration>())
                                .Returns(_serializerMock.Object);

                            ShouldReturnSuccess();
                        }
                    }

                    [TestClass]
                    public sealed class OnFailure : TryDeserializeConfigurationUsingReader
                    {
                        [TestMethod]
                        public void From()
                        {

                        }
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

        private void WhenSerializerCreationFails()
        {
            _configurationFactoryMock.Setup(factory =>
                factory.CreateSerializerFor<AppConfiguration>())
                .Throws(Error);
        }

        private void ShouldReturnSuccess()
        {
            var result = MethodUnderTest();

            result.DidFail.Should().Be.False();
        }

        private void ShouldReturnError()
        {
            var result = MethodUnderTest();

            result.DidFail.Should().Be.True();
            result.Exception.Should().Be.SameAs(Error);
        }
    }
}