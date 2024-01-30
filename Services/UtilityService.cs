using DSharpPlus.Entities;
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

            // Check if the command is in the cooldown dictionary
            if (CommandCooldowns.TryGetValue(commandName, out DateTime lastExecutionTime))
            {
                // Calculate the remaining cooldown time
                TimeSpan elapsedTime = DateTime.UtcNow - lastExecutionTime;
                remainingCooldown = cooldownDuration - elapsedTime;

                // Check if enough time has passed since the last execution
                if (elapsedTime >= cooldownDuration)
                {
                    return true;
                }
                else
                {
                    return false; // Command is still on cooldown
                }
            }
            else
            {
                // Add the command to the cooldown dictionary with the current time
                CommandCooldowns[commandName] = DateTime.UtcNow;
                return true;
            }
        }
    }
}
