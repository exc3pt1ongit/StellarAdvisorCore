namespace StellarAdvisorCore.Bot
{
    public sealed class BotConfigStructure
    {
        public string? Token { get; set; }
        public string? Prefix { get; set; }
        public ulong MainGuildId { get; set; }
        public ulong UnverifiedRoleId { get; set; }
        public string? OpenSourceUrl { get; set; }
        public string? ClientLocalization { get; set; }
        public string? ServerLocalization { get; set; }
    }
}
