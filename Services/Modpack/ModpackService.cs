using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using StellarAdvisorCore.Logging;
using StellarAdvisorCore.Extensions;
using StellarAdvisorCore.Data.Repository.Modpack;
using StellarAdvisorCore.Services.Localization;

namespace StellarAdvisorCore.Services.Modpack
{
    public class ModpackService : IModpackService
    {
        private readonly IModpackRepository _modpackRepository;
        private readonly ILocalizationService _localizationService;

        private readonly ulong _guildId = 1153809293267701840;
        private readonly ulong _channelId = 1160880734521806959;
        private readonly ulong _messageId = 1161620016739909714;

        public ModpackService(IModpackRepository modpackRepository, ILocalizationService localizationService)
        {
            _modpackRepository = modpackRepository;
            _localizationService = localizationService;
        }

        public string GetGameVersion(IReadOnlyList<DiscordEmbed> discordEmbeds) => _modpackRepository.GetGameVersion(discordEmbeds);
        public string GetModpackUrl(IReadOnlyList<DiscordEmbed> discordEmbeds) => _modpackRepository.GetModpackUrl(discordEmbeds);
        public string GetModpackVersion(IReadOnlyList<DiscordEmbed> discordEmbeds) => _modpackRepository.GetModpackVersion(discordEmbeds);
        
        public async Task<DiscordEmbed> GetModpackInformationEmbed(InteractionContext context)
        {
            try
            {
                if (Program.Client == null)
                    return new DiscordEmbedBuilder()
                        .GetErrorEmbed(_localizationService
                        .GetClientLocalizedString("modpack_client_is_not_configured"));

                var guild = await Program.Client.GetGuildAsync(_guildId);
                var channel = guild.GetChannel(_channelId);
                var message = await channel.GetMessageAsync(_messageId);

                if (message == null)
                    return new DiscordEmbedBuilder()
                        .GetErrorEmbed(_localizationService
                        .GetClientLocalizedString("modpack_message_not_found"));

                var modpackUrl = _modpackRepository.GetModpackUrl(message.Embeds);
                var gameVersion = _modpackRepository.GetGameVersion(message.Embeds);
                var modpackVersion = _modpackRepository.GetModpackVersion(message.Embeds);
                var modpackLastUpdate = message.Timestamp.ToLocalTime().ToString("dd:MM:yyyy, HH:mm");

                var embedMessage = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Lilac,
                    Title = _localizationService.GetClientLocalizedString("modpack_embed_Title"),
                    Description = _localizationService.GetClientLocalizedString("modpack_embed_Description"),
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = _localizationService.GetClientLocalizedString("modpack_embed_Footer_Text").Replace("{modpackLastUpdate}", modpackLastUpdate)
                    }
                };

                embedMessage.AddField(_localizationService.GetClientLocalizedString("modpack_embed_Field_ModVer_Title"), 
                    _localizationService.GetClientLocalizedString("modpack_embed_Field_ModVer_Description")
                    .Replace("{modpackVersion}", modpackVersion).Replace("{modpackUrl}", modpackUrl), true);

                embedMessage.AddField(_localizationService.GetClientLocalizedString("modpack_embed_Field_GameVer_Title"),
                    _localizationService.GetClientLocalizedString("modpack_embed_Field_GameVer_Description")
                    .Replace("{gameVersion}", gameVersion), true);

                embedMessage.AddField(_localizationService.GetClientLocalizedString("modpack_embed_Field_Install_Title"),
                    _localizationService.GetClientLocalizedString("modpack_embed_Field_Install_Description"));

                return embedMessage;
            }
            catch (Exception ex)
            {
                await Logger.LogErrorAsync($"Exception: {ex.Message}");

                return new DiscordEmbedBuilder()
                    .GetErrorEmbed(_localizationService
                    .GetClientLocalizedString("modpack_error_embed_text"));
            }
        }
    }
}
