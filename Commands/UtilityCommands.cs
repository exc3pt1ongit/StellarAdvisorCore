using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using StellarAdvisorCore.Services;
using StellarAdvisorCore.Extensions;
using StellarAdvisorCore.Services.Modpack;
using StellarAdvisorCore.Services.Localization;
using StellarAdvisorCore.Data.Repository.Modpack;
using StellarAdvisorCore.Data.Repository.Localization;

namespace StellarAdvisorCore.Commands
{
    public class UtilityCommands : ApplicationCommandModule
    {
        private readonly UtilityService _utilityService = new UtilityService();
        private readonly ILocalizationService _localizationService = new LocalizationService(new LocalizationRepository());
        private readonly IModpackService _modpackService = new ModpackService(new ModpackRepository(), new LocalizationService(new LocalizationRepository()));

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
