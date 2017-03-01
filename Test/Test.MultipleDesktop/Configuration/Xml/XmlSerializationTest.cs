using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultipleDesktop.Configuration.Xml;
using MultipleDesktop.Mvc.Configuration;
using MultipleDesktop.Mvc.Desktop;
using Should.Fluent;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Test.MultipleDesktop.Configuration.Xml
{
    [TestClass]
    public abstract class XmlSerializationTest
    {
        protected abstract AppConfiguration GetAppConfiguration();

        [TestClass]
        public abstract class WhenSerializing : XmlSerializationTest
        {
            public const string Schema0Path = @"Configuration\Xml\schema0.xsd";
            public const string Schema0Namespace = @"";
            public const string Schema1Path = @"Configuration\Xml\schema1.xsd";
            public const string Schema1Namespace = @"http://microsoft.com/wsdl/types/";

            private static XmlReaderSettings _settings;

            [ClassInitialize]
            public static void ClassInit(TestContext context)
            {
                _settings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
                _settings.Schemas.Add(Schema0Namespace, XmlReader.Create(new StreamReader(Schema0Path)));
                _settings.Schemas.Add(Schema1Namespace, XmlReader.Create(new StreamReader(Schema1Path)));
            }

            [TestClass]
            public sealed class WhenConfigurationIsEmpty : WhenSerializing
            {
                protected override AppConfiguration GetAppConfiguration()
                {
                    return new AppConfiguration();
                }

                [TestMethod]
                public void SerializedXmlShouldMatchXsd()
                {
                    SerializedXmlShouldMatchXsdImpl();
                }
            }

            [TestClass]
            public abstract class WhenConfigurationHasDesktops : WhenSerializing
            {
                [TestClass]
                public sealed class WhenDesktopHasSetup : WhenConfigurationHasDesktops
                {
                    public const string AnyString = "MOCK_VALUE";
                    public static readonly string MockPath = $@"Configuration\Xml\{typeof(WhenDesktopHasSetup).FullName}\MOCK_PATH";

                    private static IEnumerable<Func<VirtualDesktopConfiguration>> _setups;

                    private Func<VirtualDesktopConfiguration> _currentSetup;

                    protected override AppConfiguration GetAppConfiguration()
                    {
                        return new AppConfiguration(
                            new[]
                            {
                            _currentSetup()
                            });
                    }

                    /// <summary>
                    /// <see cref="UsingThisClassConfiguration(TestContext) below for list of <see cref="_setups"/>."/>
                    /// </summary>
                    [TestMethod]
                    public void ForEachSetupSerializedXmlShouldMatchXsd()
                    {
                        var exceptions = new List<Exception>();

                        foreach (var setup in _setups)
                        {
                            _currentSetup = setup;

                            try
                            {
                                SerializedXmlShouldMatchXsdImpl();
                            }
                            catch (Exception ex)
                            {
                                exceptions.Add(ex);
                            }
                        }

                        if (exceptions.Any())
                            throw new AggregateException(exceptions);
                    }

                    [ClassInitialize]
                    public static void UsingThisClassConfiguration(TestContext context)
                    {
                        var setups = new List<Func<VirtualDesktopConfiguration>>
                        {
                            OfDefault,
                            () => WithBackgroundPath(AnyString),
                            () => WithBackgroundPath(MockPath, thatExists : true),
                            () => WithBackgroundPath(MockPath, thatExists : false)
                        };

                        foreach (Fit fit in Enum.GetValues(typeof(Fit)))
                        {
                            setups.Add(() => WithFit(fit));
                        }

                        _setups = setups;
                    }

                    private static VirtualDesktopConfiguration OfDefault()
                    {
                        return new VirtualDesktopConfiguration();
                    }

                    private static VirtualDesktopConfiguration WithBackgroundPath(string filePath)
                    {
                        return new VirtualDesktopConfiguration { BackgroundPathElement = filePath };
                    }

                    private static VirtualDesktopConfiguration WithBackgroundPath(FilePath filePath, bool thatExists)
                    {
                        var alreadyExists = Directory.Exists(filePath);

                        if (alreadyExists && !thatExists)
                        {
                            Directory.Delete(filePath);

                            if (Directory.Exists(filePath))
                                Assert.Inconclusive($"Could not remove directory '{filePath}'.");
                        }

                        if (!alreadyExists && thatExists)
                        {
                            Directory.CreateDirectory(filePath);

                            if (!Directory.Exists(filePath))
                                Assert.Inconclusive($"Could not create directory '{filePath}.");
                        }

                        return WithBackgroundPath(filePath);
                    }

                    private static VirtualDesktopConfiguration WithFit(Fit fit)
                    {
                        return new VirtualDesktopConfiguration { FitElement = fit };
                    }
                }
            }

            private static void ShouldMatchXsd(Stream stream)
            {
                using (stream)
                using (var xmlReader = XmlReader.Create(stream, _settings))
                {
                    // throws when xml does not match xsd
                    while (xmlReader.Read()) { }
                }
            }

            private void SerializedXmlShouldMatchXsdImpl()
            {
                ShouldMatchXsd(SerializedXmlStream());
            }
        }

        [TestClass]
        public abstract class WhenDeserialized : XmlSerializationTest
        {
            private AppConfiguration _deserializedConfiguration;

            [TestClass]
            public sealed class WhenConfigurationIsEmpty : WhenDeserialized
            {
                [TestInitialize]
                public void UsingThisConfiguration()
                {
                    _deserializedConfiguration = DeserializeAppConfiguration();
                }

                protected override AppConfiguration GetAppConfiguration()
                {
                    return new AppConfiguration();
                }

                [TestMethod]
                public void DesktopConfigurationsShouldBeNull()
                {
                    _deserializedConfiguration.DesktopConfigurations.Should().Be.Null();
                }

                [TestMethod]
                public void GetAllShouldReturnEmpty()
                {
                    _deserializedConfiguration.GetAll().Should().Be.Empty();
                }
            }

            [TestClass]
            [SuppressMessage("ReSharper", "InconsistentNaming")]
            public abstract class WhenConfigurationHasDesktops : WhenDeserialized
            {
                private VirtualDesktopConfiguration _deserializedDesktop;

                protected override AppConfiguration GetAppConfiguration()
                {
                    return new AppConfiguration(new[]
                    {
                            new VirtualDesktopConfiguration()
                        });
                }

                [TestInitialize]
                public virtual void UsingThisConfiguration()
                {
                    _deserializedConfiguration = DeserializeAppConfiguration();
                    _deserializedDesktop = _deserializedConfiguration.DesktopConfigurations.First();
                }

                [TestClass]
                public sealed class WhenDestkopIsDefault : WhenConfigurationHasDesktops
                {
                    [TestInitialize]
                    public void UsingThisConfiguration()
                    {
                        _deserializedConfiguration = DeserializeAppConfiguration();

                        _deserializedDesktop = _deserializedConfiguration.DesktopConfigurations.First();
                    }

                    [TestMethod]
                    public void BackgroundPathElementShouldBeEmpty()
                    {
                        _deserializedDesktop.BackgroundPathElement.Should().Be.Empty();
                    }

                    [TestMethod]
                    public void BackgroundPathShouldBeEmpty()
                    {
                        // ReSharper disable once SuspiciousTypeConversion.Global
                        _deserializedDesktop.BackgroundPath.Equals(string.Empty).Should().Be.True();
                    }

                    [TestMethod]
                    public void FitElementShouldBeDefault()
                    {
                        _deserializedDesktop.FitElement.Should().Equal(default(Fit));
                    }

                    [TestMethod]
                    public void FitShouldBeDefault()
                    {
                        _deserializedDesktop.Fit.Should().Equal(default(Fit));
                    }
                }

                public const string app = Tag.AppConfiguration.Name;
                public const string desktops = Tag.AppConfiguration.DesktopConfigurations.ArrayName;
                public const string desktop = Tag.AppConfiguration.DesktopConfigurations.ArrayItemName;

                [TestClass]
                public sealed class WhenDesktopHasNullPath : WhenConfigurationHasDesktops
                {
                    [TestInitialize]
                    public override void UsingThisConfiguration()
                    {
                        var serializedXmlDocument = SerializeToXmlDocument();

                        const string backroundPath = Tag.VirtualDesktopConfiguration.BackgroundPath;

                        // ReSharper disable once PossibleNullReferenceException
                        serializedXmlDocument.SelectSingleNode($"/{app}/{desktops}/{desktop}/{backroundPath}").InnerXml = null;

                        using (var stream = new MemoryStream())
                        {
                            serializedXmlDocument.Save(stream);

                            stream.Position = 0;

                            _deserializedConfiguration = (AppConfiguration)new XmlSerializer(typeof(AppConfiguration)).Deserialize(stream);
                        }

                        _deserializedDesktop = _deserializedConfiguration.DesktopConfigurations.First();
                    }

                    [TestMethod]
                    public void BackgroundPathElementShouldBeEmpty()
                    {
                        _deserializedDesktop.BackgroundPathElement.Should().Be.Empty();
                    }

                    [TestMethod]
                    public void BackgroundPathShouldBeEmpty()
                    {
                        // ReSharper disable once SuspiciousTypeConversion.Global
                        _deserializedDesktop.BackgroundPath.Equals(string.Empty).Should().Be.True();
                    }
                }

                [TestClass]
                public sealed class WhenDesktopHasEmptyPath : WhenConfigurationHasDesktops
                {
                    [TestMethod]
                    public void BackgroundPathElementShouldBeEmpty()
                    {
                        _deserializedDesktop.BackgroundPathElement.Should().Be.Empty();
                    }

                    [TestMethod]
                    public void BackgroundPathShouldBeEmpty()
                    {
                        // ReSharper disable once SuspiciousTypeConversion.Global
                        _deserializedDesktop.BackgroundPath.Equals(string.Empty).Should().Be.True();
                    }
                }

                [TestClass]
                public sealed class WhenDesktopHasEmptyFit : WhenConfigurationHasDesktops
                {
                    [TestMethod]
                    public void FitElementShouldBeDefault()
                    {
                        _deserializedDesktop.FitElement.Should().Equal(default(Fit));
                    }

                    [TestMethod]
                    public void FitShouldBeDefault()
                    {
                        _deserializedDesktop.Fit.Should().Equal(default(Fit));
                    }
                }

                [TestClass]
                public sealed class WhenDesktopHasValidPath : WhenConfigurationHasDesktops
                {
                    public const string AnyString = "MOCK_VALUE";

                    protected override AppConfiguration GetAppConfiguration()
                    {
                        return new AppConfiguration(new[]
                        {
                            new VirtualDesktopConfiguration
                            {
                                BackgroundPathElement = AnyString
                            }
                        });
                    }

                    [TestMethod]
                    public void BackgroundPathElementShouldMatch()
                    {
                        _deserializedDesktop.BackgroundPathElement.Should().Equal(AnyString);
                    }

                    [TestMethod]
                    public void BackgroundPathShouldMatch()
                    {
                        // ReSharper disable once SuspiciousTypeConversion.Global
                        _deserializedDesktop.BackgroundPath.Equals(AnyString).Should().Be.True();
                    }
                }

                [TestClass]
                public sealed class WhenDesktopHasValidFit : WhenConfigurationHasDesktops
                {
                    public const Fit AnyFit = Fit.Tile;

                    protected override AppConfiguration GetAppConfiguration()
                    {
                        return new AppConfiguration(new[]
                        {
                            new VirtualDesktopConfiguration
                            {
                                FitElement = AnyFit
                            }
                        });
                    }

                    [TestMethod]
                    public void FitElementShouldMatch()
                    {
                        _deserializedDesktop.FitElement.Should().Equal(AnyFit);
                    }

                    [TestMethod]
                    public void FitShouldMatch()
                    {
                        _deserializedDesktop.Fit.Should().Equal(AnyFit);
                    }
                }
            }

            private AppConfiguration DeserializeAppConfiguration()
            {
                using (var stream = SerializedXmlStream())
                {
                    return (AppConfiguration)new XmlSerializer(typeof(AppConfiguration)).Deserialize(stream);
                }
            }

            private XmlDocument SerializeToXmlDocument()
            {
                var document = new XmlDocument();
                document.Load(SerializedXmlStream());
                return document;
            }
        }

        private Stream SerializedXmlStream()
        {
            var serializer = new XmlSerializer(typeof(AppConfiguration));

            var stream = new MemoryStream();

            serializer.Serialize(stream, GetAppConfiguration());

            stream.Position = 0;

            return stream;
        }
    }
}
