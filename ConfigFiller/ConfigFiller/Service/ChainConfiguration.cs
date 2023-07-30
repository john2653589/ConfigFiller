using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Rugal.ConfigFiller.Service
{
    public class ChainConfiguration : IConfigurationRoot
    {
        private readonly IConfigurationRoot SetRoot;
        private ConfigurationRoot _Root;
        private ConfigurationRoot Root
        {
            get
            {
                _Root ??= GetRoot();
                return _Root;
            }
        }
        public string this[string key]
        {
            get => Root[key];
            set => Root[key] = value;
        }
        public IEnumerable<IConfigurationProvider> Providers => Root.Providers;
        private ConfigurationRoot GetRoot()
        {
            var OtherProvider = SetRoot.Providers?
                .Where(Item => Item is not ConfigFillerProvider)
                .ToList();

            var Result = new ConfigurationRoot(OtherProvider);
            return Result;
        }
        public ChainConfiguration(IConfigurationRoot _SetRoot)
        {
            SetRoot = _SetRoot;
        }
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return Root.GetChildren();
        }
        public IChangeToken GetReloadToken()
        {
            return Root.GetReloadToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return Root.GetSection(key);
        }
        public void Reload()
        {
            Root.Reload();
        }
    }
}
