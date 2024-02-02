using Microsoft.EntityFrameworkCore;

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using StellarAdvisorCore.Logging;
using StellarAdvisorCore.Services;
using StellarAdvisorCore.Extensions;
using StellarAdvisorCore.Data.Models;
using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Services.Localization;
using StellarAdvisorCore.Data.Repository.Localization;
using StellarAdvisorCore.Services.Modpack;
using StellarAdvisorCore.Data.Repository.Modpack;

namespace StellarAdvisorCore.Commands
{
    public class UtilityCommands : ApplicationCommandModule
    {
        private readonly UtilityService _utilityService = new UtilityService();
        private readonly ILocalizationService _localizationService = new LocalizationService(new LocalizationRepository());
        private readonly IModpackService _modpackService = new ModpackService(new ModpackRepository(), new LocalizationService(new LocalizationRepository()));

        #region Testing Facility (mutes)

        [SlashCommand("checkusers", "Checking the users without the roles.")]
        public async Task CheckUsersWithoutRoles(InteractionContext context)
        {
            var embedDescription = "Список користувачів:";

            if (context.Guild != null)
            {
                var membersWithoutRoles = context.Guild.Members
                    .Where(m => m.Value.Roles.Count() <= 0 || m.Value.Roles == null);

                if (membersWithoutRoles.Any())
                {
                    foreach (var member in membersWithoutRoles)
                    {
                        Logger.Log(member.Value.Username);

                        embedDescription += $"\n{member.Value.Mention}, на сервері з {member.Value.JoinedAt.ToLocalTime().ToString("dd/MM/yyyy, HH:mm") ?? "Невідома дата приєднання"}";
                    }
                }
                else
                {
                    embedDescription = "У всіх користувачів є ролі.";
                }
            }
            else
            {
                Logger.LogError("Main guild not found.");
                embedDescription = "Головний сервер не налаштовано.";
            }

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Lilac,
                Title = "Користувачі без жодної ролі",
                Description = embedDescription
            };

            await context.ResponseWithEmbedAsync(embedMessage);
        }

        [SlashCommand("addmute", "Testing Facility -> Pseudo muting member")]
        public async Task AddMuteTFCommand(InteractionContext context,
            [Option("memberId", "Discord member Id")] long memberId,
            [Option("expirationTime", "Mute expiration time (DAYS)")] long mutedDays,
            [Option("mutedReason", "Reason to mute the member")] string mutedReason)
        {
            var mute = new MutedUser()
            {
                MemberId = (ulong)memberId,
                MutedReason = mutedReason,
                MutedExpiration = DateTime.Now.AddDays(mutedDays),
                MutedById = context.Member.Id,
            };

            using (SqliteContext sqlite = new SqliteContext())
            {
                sqlite.MutedUsers.Add(mute);
                await sqlite.SaveChangesAsync();

                var count = sqlite.MutedUsers.Count();

                var embedMessage = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Lilac,
                    Title = "Блокування чату",
                    Description = $"{context.Member.Username} заблокував чат користувачу з Id: {mute.MemberId}\n" +
                                  $"Кількість учасників з заблокованим чатом: {count}"
                };

                await context.ResponseWithEmbedAsync(embedMessage);
            }
        }

        [SlashCommand("getmuted", "Testing Facility -> Getting pseudo muted members")]
        public async Task GetAllMutedTFCommand(InteractionContext context)
        {
            var cooldownDuration = TimeSpan.FromSeconds(2);

            if (!_utilityService.CheckCooldown("getmuted", cooldownDuration, out TimeSpan remainingCooldown))
            {
                _utilityService.CommandCooldowns["getmuted"] = DateTime.UtcNow;

                var embedMessage = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Lilac,
                    Title = "Користувачі з блокуванням чату",
                    Description = $"Помилка використання. Залишилося часу: {remainingCooldown.TotalSeconds} сек."
                };

                await context.ResponseWithEmbedAsync(embedMessage);
            }

            string description = string.Empty;

            using (SqliteContext sqlite = new SqliteContext())
            {
                var allMutedMembers = sqlite.MutedUsers;

                foreach (var member in allMutedMembers)
                {
                    description += $"Id: {member.Id}, MemberId: {member.MemberId}, exp: {member.MutedExpiration}, reason: {member.MutedReason}\n";
                }

                var embedMessage = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Lilac,
                    Title = "Користувачі з блокуванням чату",
                    Description = $"Список користувачів:\n{description}"
                };

                await context.ResponseWithEmbedAsync(embedMessage);
            }
        }

        [SlashCommand("removemute", "Testing Facility -> Removing pseudo mute")]
        public async Task RemoveMemberMuteTFCommand(InteractionContext context,
            [Option("memberId", "Discord member Id")] long memberId)
        {

            using (SqliteContext sqlite = new SqliteContext())
            {
                var memberToUnmute = await sqlite.MutedUsers.FirstOrDefaultAsync(m => m.MemberId == (ulong)memberId);
                
                if (memberToUnmute == null)
                {
                    await context.ResponseWithMessageAsync("Невідомий користувач / або у користувача немає блокування чату.");
                    return;
                }
                
                sqlite.MutedUsers.Remove(memberToUnmute);
                await sqlite.SaveChangesAsync();

                var embedMessage = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Lilac,
                    Title = "Зняття блокування чату",
                    Description = $"{context.Member.Username} успішно зняв блокування чату для користувача з Id: {memberToUnmute.MemberId}"
                };
                
                await context.ResponseWithEmbedAsync(embedMessage);
            }
        }

        #endregion

        #region Testing Facility (roles)

        [SlashCommand("getuserswithrole", "Get users with a specific role")]
        public async Task GetUsersWithRole(InteractionContext context,
        [Option("role", "The role to search for")] string roleInput)
        {
            var guild = context.Guild;
            var role = _utilityService.GetRoleFromInput(guild, roleInput);

            if (role == null)
            {
                await context.Channel.SendMessageAsync("Група не знайдена.");
                return;
            }

            var usersWithRole = guild.Members
                .Where(m => m.Value.Roles.Any(r => r == role))
                .Select(u => $"{u.Value.Mention}, на сервері з {u.Value.JoinedAt.ToLocalTime().ToString("dd/MM/yyyy, HH:mm") ?? "Невідома дата приєднання"}");

            var usersList = usersWithRole.Any()
                ? string.Join("\n", usersWithRole)
                : "Не знайдено користувачів з цією групою.";

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Lilac,
                Title = $"Користувачі з групою: {role.Name}",
                Description = usersList
            };

            await context.ResponseWithEmbedAsync(embedMessage);
        }

        #endregion

        #region Testing Facility (modpack)

        [SlashCommand("modpack", "Дізнатися інформацію про актуальний модпак")]
        public async Task GetModpackSlashCommand(InteractionContext context) => await context.ResponseWithEmbedAsync(await _modpackService.GetModpackInformationEmbed(context));

        #endregion
    }
}
