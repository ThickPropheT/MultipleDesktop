using MultipleDesktop.Mvc.Configuration;
using System;
using System.IO;
using System.Xml;

namespace MultipleDesktop.Configuration
{
    public class XmlConfigurationProvider : IConfigurationProvider
    {
        private readonly IXmlConfigurationFactory _configurationFactory;

        public XmlConfigurationProvider(IXmlConfigurationFactory factory)
        {
            _configurationFactory = factory;
        }

        public IoResult Create(string atPath, out IAppConfiguration configuration)
        {
            try
            {
                var xmlConfiguration = _configurationFactory.CreateXmlConfiguration();
                configuration = xmlConfiguration;

                return Save(xmlConfiguration, atPath);
            }
            catch (Exception ex)
            {
                configuration = null;
                return IoResult.ForException(ex);
            }
        }

        public IoResult Save(IAppConfiguration configuration, string toPath)
        {
            try
            {
                return Save(_configurationFactory.ToXmlConfiguration(configuration), toPath);
            }
            catch (Exception ex)
            {
                return IoResult.ForException(ex);
            }
        }

        private IoResult Save(AppConfiguration configuration, string toPath)
        {
            try
            {
                var serializer = _configurationFactory
                    .CreateSerializerFor<AppConfiguration>();

                using (var writer = _configurationFactory
                    .CreateSerializationWriterFor(toPath))
                {
                    serializer.Serialize(writer, configuration);
                    return IoResult.ForSuccess();
                }
            }
            catch (Exception ex)
            {
                return IoResult.ForException(ex);
            }
        }

        public IoResult Load(string fromPath, out IAppConfiguration configuration)
        {
            configuration = null;

            try
            {
                var serializer = _configurationFactory.CreateSerializerFor<AppConfiguration>();

                using (var reader = _configurationFactory
                    .CreateSerializationReaderFor(fromPath))
                {
                    configuration = (IAppConfiguration)serializer.Deserialize(reader);
                    return IoResult.ForSuccess();
                }
            }
            catch (InvalidOperationException ex) when (ex.InnerException is XmlException)
            {
                return IoResult.ForReadError();
            }
            catch (FileNotFoundException)
            {
                return IoResult.ForNotFound();
            }
            catch (DirectoryNotFoundException)
            {
                return IoResult.ForNotFound();
            }
            catch (Exception ex)
            {
                return IoResult.ForException(ex);
            }
        }
    }
}
