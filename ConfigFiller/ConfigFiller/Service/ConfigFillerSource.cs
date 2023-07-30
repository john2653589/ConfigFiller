using Microsoft.Extensions.Configuration;

namespace Rugal.ConfigFiller.Service
{
    public class ConfigFillerSource : IConfigurationSource
    {
        private readonly IConfigurationRoot Root;
        public ConfigFillerSource(IConfigurationRoot _Root)
        {
            Root = _Root;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ConfigFillerProvider(Root);
        }
    }
}
