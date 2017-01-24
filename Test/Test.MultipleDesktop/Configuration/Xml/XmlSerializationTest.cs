using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultipleDesktop.Configuration.Xml;
using System.Xml;
using System.IO;
using System.Xml.Schema;
using System.Xml.Serialization;
using MultipleDesktop.Mvc.Desktop;
using System.Collections.Generic;
using MultipleDesktop.Mvc.Configuration;

namespace Test.MultipleDesktop.Configuration.Xml
{
    [TestClass]
    public abstract class XmlSerializationTest
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

        protected abstract AppConfiguration GetAppConfiguration();

        [TestClass]
        public sealed class WhenConfigurationIsEmpty : XmlSerializationTest
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
        public abstract class WhenConfigurationHasDesktops : XmlSerializationTest
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
                    foreach (var setup in _setups)
                    {
                        _currentSetup = setup;

                        SerializedXmlShouldMatchXsdImpl();
                    }
                }

                [ClassInitialize]
                public static void UsingThisClassConfiguration(TestContext context)
                {
                    var setups = new List<Func<VirtualDesktopConfiguration>>
                        {
                            OfDefault,
                            () => WithBackgroundPath(AnyString),
                            () => WithBackgroundPathThat(MockPath, exists : true),
                            () => WithBackgroundPathThat(MockPath, exists : false)
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

                private static VirtualDesktopConfiguration WithBackgroundPath(FilePath filePath)
                {
                    return new VirtualDesktopConfiguration { BackgroundPathElement = filePath };
                }

                private static VirtualDesktopConfiguration WithBackgroundPathThat(FilePath filePath, bool exists)
                {
                    var alreadyExists = Directory.Exists(filePath);

                    if (alreadyExists && !exists)
                    {
                        Directory.Delete(filePath);

                        if (Directory.Exists(filePath))
                            Assert.Inconclusive($"Could not remove directory '{filePath}'.");
                    }
                    if (!alreadyExists && exists)
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

        private void SerializedXmlShouldMatchXsdImpl()
        {
            var serializer = new XmlSerializer(typeof(AppConfiguration));

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, GetAppConfiguration());

                stream.Position = 0;

                using (var xmlReader = XmlReader.Create(stream, _settings))
                {
                    // throws when xml does not match xsd
                    while (xmlReader.Read()) ;
                }
            }
        }
    }
}
