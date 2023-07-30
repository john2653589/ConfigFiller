using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Rugal.ConfigFiller.Service
{
    public class ConfigFillerProvider : IConfigurationProvider
    {
        private readonly IConfiguration Configuration;
        private readonly ConfigFillerService Filler;
        public ConfigFillerProvider(IConfigurationRoot _Root)
        {
            Configuration = new ChainConfiguration(_Root);
            Filler = new ConfigFillerService(Configuration);
        }
        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            var FindConfiguration = Configuration;
            if (parentPath != null)
                FindConfiguration = Configuration.GetSection(parentPath);

            var Children = FindConfiguration.GetChildren();
            var Result = Children
                .Select(Item => Item.Key)
                .Concat(earlierKeys)
                .OrderBy(Item => Item, ConfigurationKeyComparer.Instance);

            return Result;
        }
        public IChangeToken GetReloadToken()
        {
            return Configuration.GetReloadToken();
        }
        public void Load() { }
        public void Set(string key, string value)
        {
            Configuration[key] = value;
        }
        public bool TryGet(string key, out string value)
        {
            value = Filler.Get(key);
            return value != null;
        }
    }
}
