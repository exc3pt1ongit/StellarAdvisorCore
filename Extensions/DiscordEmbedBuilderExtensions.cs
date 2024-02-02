using DSharpPlus.Entities;
using StellarAdvisorCore.Data.Repository.Localization;
using StellarAdvisorCore.Services.Localization;

namespace StellarAdvisorCore.Extensions
{
    public static class DiscordEmbedBuilderExtensions
    {
        private static readonly ILocalizationService _localizationService = new LocalizationService(new LocalizationRepository());

        public static DiscordEmbed GetSuccessEmbed(this DiscordEmbedBuilder _discordEmbedBuilder, string successContent)
        {
            return new DiscordEmbedBuilder
            {
                Color = new DiscordColor(0x5EC20A),
                Title = _localizationService.GetClientLocalizedString("ext_success_during_execution"),
                Description = successContent
            };
        }

        public static DiscordEmbed GetSuccessEmbed(this DiscordEmbedBuilder _discordEmbedBuilder, string successHeader, string successContent)
        {
            return new DiscordEmbedBuilder
            {
                Color = new DiscordColor(0x5EC20A),
                Title = successHeader,
                Description = successContent
            };
        }

        public static DiscordEmbed GetErrorEmbed(this DiscordEmbedBuilder _discordEmbedBuilder, string errorContent)
        {
            return new DiscordEmbedBuilder
            {
                Color = new DiscordColor(0xDB5353),
                Title = _localizationService.GetClientLocalizedString("ext_error_during_execution"),
                Description = errorContent
            };
        }

        public static DiscordEmbed GetErrorEmbed(this DiscordEmbedBuilder _discordEmbedBuilder, string errorHeader, string errorContent)
        {
            return new DiscordEmbedBuilder
            {
                Color = new DiscordColor(0xDB5353),
                Title = errorHeader,
                Description = errorContent
            };
        }
    }
}
