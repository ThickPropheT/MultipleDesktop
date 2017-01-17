namespace MultipleDesktop.Mvc.Configuration
{
    public interface IConfigurationProvider
    {
        IoResult Create(string atPath, out IAppConfiguration configuration);
        IoResult Save(IAppConfiguration configuration, string toPath);
        IoResult Load(string fromPath, out IAppConfiguration configuration);
    }
}
