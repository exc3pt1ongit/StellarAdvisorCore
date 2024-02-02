using StellarAdvisorCore.Data.Models.Interfaces;

namespace StellarAdvisorCore.Data.Models.Entities.Characters
{
    public class Character : Entity, ICharacter
    {
        public ulong DiscordUserId { get; set; }

        public string? Name { get; set; }
        public string? Settlement { get; set; }
        public string? Fraction { get; set; }
        public string? FractionRole { get; set; }
    }
}
