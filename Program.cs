using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using StellarAdvisorCore.Bot;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using StellarAdvisorCore.Commands;
using StellarAdvisorCore.Logging;

internal class Program
{
    public static DiscordClient? Client { get; private set; }
    public static CommandsNextExtension? Commands { get; private set; }
    public static BotConfig BotConfig { get; private set; } = new BotConfig();
    private static async Task Main(string[] args)
    {
        await BotConfig.Load();

        Client = new DiscordClient(BotConfig.GetDiscordConfiguration());

        Client.UseInteractivity(new InteractivityConfiguration()
        {
            Timeout = TimeSpan.FromMinutes(2)
        });

        Client.Ready += OnClientReady;
        Client.GuildMemberAdded += OnNewMemberJoinedGuild;

        var commandsConfig = new CommandsNextConfiguration()
        {
            StringPrefixes = new string[] { BotConfig.Values.Prefix },
            EnableMentionPrefix = true,
            EnableDms = true,
            EnableDefaultHelp = false
        };

        var slashCommandsConfig = Client.UseSlashCommands();
        slashCommandsConfig.RegisterCommands<UtilityCommands>(BotConfig.Values.MainGuildId);
        slashCommandsConfig.RegisterCommands<CharacterCommands>(BotConfig.Values.MainGuildId);

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }

    private static Task OnNewMemberJoinedGuild(DiscordClient sender, GuildMemberAddEventArgs e)
    {
        e.Member.GrantRoleAsync(e.Guild.GetRole(BotConfig.Values.UnverifiedRoleId));

        return Task.CompletedTask;
    }

    private static async Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
    {
        Logger.Log($"Bot started successfully. Version: {BotConfig.Version}");
        Logger.Log($"MainGuildId: {BotConfig.Values.MainGuildId}");
        Logger.Log($"Role for unverified users: {BotConfig.Values.UnverifiedRoleId}");

        foreach (var guild in sender.Guilds)
        {
            var allMembers = await guild.Value.GetAllMembersAsync();
            var membersWithoutAnyRole = allMembers.Where(u => u.Roles.Count() <= 0 || u.Roles == null);

            foreach (var member in membersWithoutAnyRole)
            {
                await member.GrantRoleAsync(guild.Value.GetRole(BotConfig.Values.UnverifiedRoleId));
                Logger.Log($"Unverified role given to {member.Username}");
            }
        }
    }
}