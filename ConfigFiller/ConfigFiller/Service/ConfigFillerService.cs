using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Rugal.ConfigFiller.Service
{
    public class ConfigFillerService
    {
        private readonly IConfiguration Configuration;
        private readonly string Pattern = @"\{([^}]+)\}";
        public ConfigFillerService(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
        }

        public string Get(string Key)
        {
            var Result = Configuration[Key];

            if (Result is null)
                return Result;

            var ReplaceValues = Regex
                .Matches(Result, Pattern)
                .Select(Item => Item.Groups)
                .Select(Item =>
                {
                    var ReplaceKey = Item[0].Value;
                    var SplitKey = Item[1].Value;

                    var GetConfig = Configuration;
                    var SplitArray = SplitKey.Split(':');
                    foreach (var Key in SplitArray.SkipLast(1))
                        GetConfig = GetConfig.GetSection(Key);

                    var Section = GetConfig as ConfigurationSection;
                    var LastKey = SplitArray.Last();
                    var Value = Section[LastKey];
                    var KeyValue = new KeyValuePair<string, string>(ReplaceKey, Value);
                    return KeyValue;
                })
                .ToArray();

            foreach (var Item in ReplaceValues)
                Result = Result.Replace(Item.Key, Item.Value);

            return Result;
        }


    }
}
