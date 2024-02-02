using DSharpPlus.Entities;
using System.Text.RegularExpressions;

namespace StellarAdvisorCore.Data.Repository.Modpack
{
    public class ModpackRepository : IModpackRepository
    {
        public string GetGameVersion(IReadOnlyList<DiscordEmbed> discordEmbeds)
        {
            string versionPattern = @"Forge (\d+\.\d+(\.\d+)?)";

            foreach (var embed in discordEmbeds)
            {
                if (embed == null || string.IsNullOrEmpty(embed?.Description)) continue;
                Match versionPatternMatch = Regex.Match(embed.Description, versionPattern);
                if (versionPatternMatch.Success) return versionPatternMatch.Groups[1].Value;
            }

            return "0.0.0";
        }

        public string GetModpackUrl(IReadOnlyList<DiscordEmbed> discordEmbeds)
        {
            string urlPattern = @"(?:https?:\/\/)?drive\.google\.com\/(?:[^\/\s]+\/){3}([^\/\s]+)";

            foreach (var embed in discordEmbeds)
            {
                if (embed == null || string.IsNullOrEmpty(embed?.Description)) continue;
                Match urlPatternMatch = Regex.Match(embed.Description, urlPattern);
                if (urlPatternMatch.Success)
                {
                    return urlPatternMatch.Value.Remove(urlPatternMatch.Length);
                }
            }

            return string.Empty;
        }

        public string GetModpackVersion(IReadOnlyList<DiscordEmbed> discordEmbeds)
        {
            string versionPattern = @"v(\d+\.\d+\.\d+)";

            foreach (var embed in discordEmbeds)
            {
                if (embed == null || string.IsNullOrEmpty(embed?.Title)) continue;
                Match versionPatternMatch = Regex.Match(embed.Title, versionPattern);
                if (versionPatternMatch.Success) return versionPatternMatch.Groups[1].Value;
            }

            return "v0.0.0";
        }
    }
}
