using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using StellarAdvisorCore.Bot;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using StellarAdvisorCore.Commands;
using StellarAdvisorCore.Logging;
using StellarAdvisorCore.Services;
using StellarAdvisorCore.Data;

internal class Program
{
    public static DiscordClient? Client { get; private set; }
    public static CommandsNextExtension? Commands { get; private set; }
    public static BotConfig BotConfig { get; private set; } = new BotConfig();
    public static DateTime Uptime { get; private set; } = DateTime.Now;
    public static Dictionary<string, Dictionary<string, string>> Localizations { get; } = new Dictionary<string, Dictionary<string, string>>();

    private static async Task Main(string[] args)
    {
        await BotConfig.Load();

        Client = new DiscordClient(BotConfig.GetDiscordConfiguration());

        Client.UseInteractivity(new InteractivityConfiguration()
        {
            Timeout = TimeSpan.FromMinutes(2),
        });

        Client.Ready += OnClientReady;
        Client.ClientErrored += OnClientErrored;
        Client.GuildMemberAdded += OnNewMemberJoinedGuild;

        var commandsConfig = new CommandsNextConfiguration()
        {
            StringPrefixes = new string[] { BotConfig.Values.Prefix ?? "!" },
            EnableMentionPrefix = true,
            EnableDms = true,
            EnableDefaultHelp = false
        };

        var slashCommandsConfig = Client.UseSlashCommands();
        slashCommandsConfig.RegisterCommands<BotCommands>(BotConfig.Values.MainGuildId);
        slashCommandsConfig.RegisterCommands<UtilityCommands>(BotConfig.Values.MainGuildId);
        slashCommandsConfig.RegisterCommands<CharacterCommands>(BotConfig.Values.MainGuildId);

        await Client.ConnectAsync();
        await Task.Delay(-1);
    }

    private static Task OnClientErrored(DiscordClient sender, ClientErrorEventArgs e)
    {
        Logger.LogError($"Event: {e.EventName}\nException: {e.Exception}");
        
        return Task.CompletedTask;
    }

    private static Task OnNewMemberJoinedGuild(DiscordClient sender, GuildMemberAddEventArgs e)
    {
        e.Member.GrantRoleAsync(e.Guild.GetRole(BotConfig.Values.UnverifiedRoleId));

        return Task.CompletedTask;
    }

    private static async Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
    {
        await Logger.LogAsync($"Bot started successfully. Version: {BotConfig.Version}");

        await Logger.LogAsync($"Client localization: {BotConfig.Values.ClientLocalization}");
        await Logger.LogAsync($"Server localization: {BotConfig.Values.ServerLocalization}");
        
        await Logger.LogAsync("Library: DSharpPlus (C# - .NET Core 8)");
        await Logger.LogAsync($"Open-Source Code: {BotConfig.Values.OpenSourceUrl}");

        await LocalizationService.LoadLocalizationAsync(BotConfig.Values.ServerLocalization ?? "unload");
        await LocalizationService.LoadLocalizationAsync(BotConfig.Values.ClientLocalization ?? "unload");

        foreach (var guild in sender.Guilds)
        {
            var allMembers = await guild.Value.GetAllMembersAsync();
            var membersWithoutAnyRole = allMembers.Where(u => u?.Roles.Count() <= 0 || u?.Roles == null);

            foreach (var member in membersWithoutAnyRole)
            {
                await member.GrantRoleAsync(guild.Value.GetRole(BotConfig.Values.UnverifiedRoleId));
                await Logger.LogAsync($"Unverified role given to {member.Username}");
            }
        }

        World.Load();
    }
}