using MultipleDesktop.Mvc;
using MultipleDesktop.Mvc.Configuration;

namespace MultipleDesktop.Configuration
{
    public class ConfigurationController : IConfigurationController
    {
        private readonly IConfigurationProvider _configurationProvider;

        public ConfigurationController(IConfigurationProvider provider)
        {
            _configurationProvider = provider;
        }

        public IAppConfiguration Load()
        {
            var configFileName = Constants.Default.Config.FileName;

            IAppConfiguration appConfiguration;
            var result = _configurationProvider.Load(configFileName, out appConfiguration);

            if (result.DoesExist == false || result.ReadError == true)
                result = _configurationProvider.Create(configFileName, out appConfiguration);

            if (result.DidFail)
                throw result.Exception;

            return appConfiguration;
        }

        public void Save(IAppConfiguration configuration)
        {
            var result = _configurationProvider.Save(configuration, Constants.Default.Config.FileName);

            if (result.DidFail)
                throw result.Exception;
        }
    }
}
