using Microsoft.Extensions.Configuration;
using Rugal.ConfigFiller.Service;

namespace Rugal.ConfigFiller.Extention
{
    public static class StartupExtention
    {
        public static IConfigurationBuilder AddConfigFiller(this IConfigurationBuilder Builder)
        {
            Builder.Sources.Add(new ConfigFillerSource(Builder.Build()));
            return Builder;
        }
    }
}