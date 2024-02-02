using DSharpPlus.Entities;
using StellarAdvisorCore.Logging;
using System.Text.RegularExpressions;

namespace StellarAdvisorCore.Services
{
    public class UtilityService
    {
        public Dictionary<string, DateTime> CommandCooldowns { get; set; } = new Dictionary<string, DateTime>();
        public DiscordRole GetRoleFromInput(DiscordGuild guild, string roleInput)
        {
            var mentionMatch = Regex.Match(roleInput, @"<@&(\d+)>");
            if (mentionMatch.Success && ulong.TryParse(mentionMatch.Groups[1].Value, out var roleId)) return guild.GetRole(roleId);
            return guild.Roles.FirstOrDefault(r => r.Value.Name == roleInput).Value;
        }
        public bool CheckCooldown(string commandName, TimeSpan cooldownDuration, out TimeSpan remainingCooldown)
        {
            remainingCooldown = TimeSpan.Zero;

            if (CommandCooldowns.TryGetValue(commandName, out DateTime lastExecutionTime))
            {
                TimeSpan elapsedTime = DateTime.UtcNow - lastExecutionTime;
                remainingCooldown = cooldownDuration - elapsedTime;
                return (elapsedTime >= cooldownDuration);
            }
            else
            {
                CommandCooldowns[commandName] = DateTime.UtcNow;
                return true;
            }
        }
    }
}
