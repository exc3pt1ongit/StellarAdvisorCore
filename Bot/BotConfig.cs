using DSharpPlus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StellarAdvisorCore.Logging;

namespace StellarAdvisorCore.Bot
{
    public class BotConfig
    {
        public string Version { get; set; } = "1.1.0/Alpha";
        public BotConfigStructure Values { get; set; } = new BotConfigStructure();

        public async Task Load(string configFile = "BotConfig.json")
        {
            using (StreamReader streamReader = new StreamReader(configFile))
            {
                try
                {
                    var json = await streamReader.ReadToEndAsync();
                    var data = JsonConvert.DeserializeObject<BotConfigStructure>(json);

                    if(data == null)
                    {
                        await Logger.LogErrorAsync("The JSON load data is NULL");

                        return;
                    }

                    Values.Token = data.Token;
                    Values.Prefix = data.Prefix;
                    Values.MainGuildId = data.MainGuildId;
                    Values.UnverifiedRoleId = data.UnverifiedRoleId;
                    Values.OpenSourceUrl = data.OpenSourceUrl;
                    Values.ClientLocalization = data.ClientLocalization;
                    Values.ServerLocalization = data.ServerLocalization;
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.UtcNow} Error: {exception.Message}");
                }
            }
        }

        public DiscordConfiguration GetDiscordConfiguration()
        {
            return new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = Values.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Error // change in the future
            };
        }
    }
}
