using DSharpPlus.Entities;
using System.Text.RegularExpressions;

namespace StellarAdvisorCore.Services
{
    public class UtilityService
    {
        public DiscordRole GetRoleFromInput(DiscordGuild guild, string roleInput)
        {
            var mentionMatch = Regex.Match(roleInput, @"<@&(\d+)>");

            if (mentionMatch.Success && ulong.TryParse(mentionMatch.Groups[1].Value, out var roleId)) return guild.GetRole(roleId);

            return guild.Roles.FirstOrDefault(r => r.Value.Name == roleInput).Value;
        }
    }
}
