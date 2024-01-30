using Microsoft.EntityFrameworkCore;

using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using StellarAdvisorCore.Context;
using StellarAdvisorCore.Logging;
using StellarAdvisorCore.Models;
using StellarAdvisorCore.Services;
using StellarAdvisorCore.Extensions;

namespace StellarAdvisorCore.Commands
{
    public class UtilityCommands : ApplicationCommandModule
    {
        private readonly UtilityService _utilityService = new UtilityService();

        [RequireOwner]
        [SlashCommand("migrate", "Migrates the database")]
        public async Task MigrateSqliteCommand(InteractionContext context)
        {
            Logger.Log("Manual command migrating");

            await using SqliteContext sqlite = new SqliteContext();

            if (sqlite.Database.GetPendingMigrationsAsync().Result.Any())
            {
                await sqlite.Database.MigrateAsync();
            }

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Lilac,
                Title = "Міграція бази даних",
                Description = "Базу даних Sqlite успішно мігровано."
            };

            await context.ResponseWithEmbedAsync(embedMessage);
            await Logger.LogSuccessAsync("Sqlite Migration complete");
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
                MutedExpiration = DateTime.Now.AddDays(mutedDays)
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

        [SlashCommand("unverified", "Get role for unverified users.")]
        public async Task GetRoleForUnverifiedUsers(InteractionContext context)
        {
            var role = context.Guild.GetRole(Program.BotConfig.Values.UnverifiedRoleId);

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Lilac,
                Title = "Роль для непідтверджених користувачів",
                Description = $"Id: {role.Id}\n" +
                $"Назва ролі: {role.Name}\n" +
                $"Колір ролі: {role.Color}"
            };

            await context.ResponseWithEmbedAsync(embedMessage);
        }

        #region ModpackCommand

        [SlashCommand("modpack", "Get information about actual server modpack.")]
        public async Task GetModpackSlashCommand(InteractionContext context)
        {
            try
            {
                ulong guildId = 1153809293267701840;
                ulong channelId = 1160880734521806959;
                ulong messageId = 1161620016739909714;

                if (Program.Client == null)
                {
                    await context.ResponseWithMessageAsync("Клієнт не налаштовано. Зверніться до технічного адміністратора");
                    return;
                }

                var guild = await Program.Client.GetGuildAsync(guildId);
                await Console.Out.WriteLineAsync(guild.Name);

                var channel = guild.GetChannel(channelId);
                await Console.Out.WriteLineAsync(channel.Name);

                var message = await channel.GetMessageAsync(messageId);

                if (message != null)
                {
                    var messageEmbeds = message.Embeds;

                    foreach (var embed in messageEmbeds)
                    {
                        Logger.Log(embed.Description);
                    }

                    var embedMessage = new DiscordEmbedBuilder
                    {
                        Title = $"Оригінальне повідомлення від {message.Author.Username}",
                        Description = $"{message.Content}\n\nTimestamp: {message.Timestamp}",
                        Color = DiscordColor.Lilac
                    };

                    await context.ResponseWithEmbedAsync(embedMessage);
                }
                else
                {
                    await context.ResponseWithMessageAsync("Повідомлення не знайдено");
                }
            }
            catch (Exception ex)
            {
                await Logger.LogErrorAsync($"Error: {ex.Message}");
                await context.ResponseWithMessageAsync("Під час обробки запиту виникла помилка");
            }
        }

        #endregion

        [SlashCommand("stellaradvisor", "Getting the information about Stellar Advisor")]
        public async Task StellarAdvisorCommand(InteractionContext context)
        {
            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Lilac,
                Title = $"Stellar Advisor",
                Description = $"Версія: {Program.BotConfig.Version}",
            };

            await context.ResponseWithEmbedAsync(embedMessage);
        }

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
    }
}
