using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace StellarAdvisorCore.Services.Modpack
{
    public interface IModpackService
    {
        string GetModpackVersion(IReadOnlyList<DiscordEmbed> discordEmbeds);
        string GetModpackUrl(IReadOnlyList<DiscordEmbed> discordEmbeds);
        string GetGameVersion(IReadOnlyList<DiscordEmbed> discordEmbeds);
        Task<DiscordEmbed> GetModpackInformationEmbed(InteractionContext context);
    }
}
