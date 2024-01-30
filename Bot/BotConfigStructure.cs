namespace StellarAdvisorCore.Bot
{
    public sealed class BotConfigStructure
    {
        public string Token { get; set; } = "0";
        public string Prefix { get; set; } = "!";
        public ulong MainGuildId { get; set; }
        public ulong UnverifiedRoleId { get; set; }
    }
}
