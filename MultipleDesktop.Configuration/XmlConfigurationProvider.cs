using MultipleDesktop.Mvc.Configuration;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

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
            configuration = new AppConfiguration();

            return Save(configuration, atPath);
        }

        public IoResult Load(string fromPath, out IAppConfiguration configuration)
        {
            return LoadFromFile(fromPath, out configuration);
        }

        //private IoResult LoadFromIsolatedStorage(string fromPath, out IAppConfiguration configuration)
        //{
        //    var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();


        //}

        private IoResult LoadFromFile(string fromPath, out IAppConfiguration configuration)
        {
            configuration = null;

            var serializer = new XmlSerializer(typeof(AppConfiguration));

            try
            {
                using (var stream = new StreamReader(fromPath))
                {
                    configuration = (IAppConfiguration)serializer.Deserialize(stream);
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

        public IoResult Save(IAppConfiguration configuration, string toPath)
        {
            var serializer = new XmlSerializer(typeof(AppConfiguration));

            try
            {
                using (var stream = new StreamWriter(toPath))
                {
                    serializer.Serialize(stream, _configurationFactory.ToXmlConfiguration(configuration));
                    return IoResult.ForSuccess();
                }
            }
            catch (Exception ex)
            {
                return IoResult.ForException(ex);
            }
        }
    }
}
