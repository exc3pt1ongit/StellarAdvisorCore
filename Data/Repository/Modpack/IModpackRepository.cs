using DSharpPlus.Entities;

namespace StellarAdvisorCore.Data.Repository.Modpack
{
    public interface IModpackRepository
    {
        string GetModpackVersion(IReadOnlyList<DiscordEmbed> discordEmbeds);
        string GetModpackUrl(IReadOnlyList<DiscordEmbed> discordEmbeds);
        string GetGameVersion(IReadOnlyList<DiscordEmbed> discordEmbeds);
    }
}
