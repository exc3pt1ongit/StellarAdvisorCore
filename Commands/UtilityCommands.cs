using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using StellarAdvisorCore.Logging;
using StellarAdvisorCore.Services;

namespace StellarAdvisorCore.Commands
{
    public class UtilityCommands : ApplicationCommandModule
    {
        private readonly UtilityService _utilityService = new UtilityService();

        [SlashCommand("getuserswithrole", "Get users with a specific role")]
        public async Task GetUsersWithRole(InteractionContext context,
        [Option("role", "The role to search for")] string roleInput)
        {
            await context.DeferAsync();

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

            await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }

        [SlashCommand("unverified", "Get role for unverified users.")]
        public async Task GetRoleForUnverifiedUsers(InteractionContext context)
        {
            await context.DeferAsync();

            var role = context.Guild.GetRole(Program.BotConfig.Values.UnverifiedRoleId);

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Lilac,
                Title = "Роль для непідтверджених користувачів",
                Description = $"Id: {role.Id}\n" +
                $"Назва ролі: {role.Name}\n" +
                $"Колір ролі: {role.Color}"
            };

            await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }

        #region ModpackCommand

        [SlashCommand("modpack", "Get information about actual server modpack.")]
        public async Task GetModpackSlashCommand(InteractionContext context)
        {
            await context.DeferAsync();

            try
            {
                ulong guildId = 1153809293267701840;
                ulong channelId = 1160880734521806959;
                ulong messageId = 1161620016739909714;

                var guild = await Program.Client.GetGuildAsync(guildId);
                await Console.Out.WriteLineAsync(guild.Name);

                var channel = guild.GetChannel(channelId);
                await Console.Out.WriteLineAsync(channel.Name);

                var message = await channel.GetMessageAsync(messageId);

                if (message != null)
                {
                    var embedMessage = new DiscordEmbedBuilder
                    {
                        Title = $"Оригінальне повідомлення від {message.Author.Username}",
                        Description = $"{message.Content}\n\nTimestamp: {message.Timestamp}",
                        Color = DiscordColor.Lilac
                    };

                    await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
                }
                else
                {
                    await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Message not found."));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent("An error occurred while processing the request."));
            }
        }

        #endregion

        [SlashCommand("checkusers", "Checking the users without the roles.")]
        public async Task CheckUsersWithoutRoles(InteractionContext context)
        {
            await context.DeferAsync();

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

            await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
    }
}
