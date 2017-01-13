namespace MultipleDesktop.Mvc.Configuration
{
    public interface IConfigurationController
    {
        IAppConfiguration Load();
        void Save(IAppConfiguration configuration);
    }
}
