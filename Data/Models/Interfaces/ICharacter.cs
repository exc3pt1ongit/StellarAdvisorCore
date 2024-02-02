namespace StellarAdvisorCore.Data.Models.Interfaces
{
    public interface ICharacter
    {
        ulong DiscordUserId { get; set; }

        string? Name { get; set;  }
        string? Settlement { get; set; }

        string? Faction { get; set; }
        string? FactionRole { get; set; }
    }
}
