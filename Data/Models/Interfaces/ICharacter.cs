namespace StellarAdvisorCore.Data.Models.Interfaces
{
    public interface ICharacter
    {
        ulong DiscordUserId { get; set; }

        string? Name { get; set;  }
        string? Settlement { get; set; }

        string? Fraction { get; set; }
        string? FractionRole { get; set; }
    }
}
