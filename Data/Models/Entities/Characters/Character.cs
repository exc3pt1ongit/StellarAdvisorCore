using StellarAdvisorCore.Data.Models.Interfaces;

namespace StellarAdvisorCore.Data.Models.Entities.Characters
{
    public class Character : Entity, ICharacter
    {
        public ulong DiscordUserId { get; set; }

        public string? Name { get; set; }
        public string? Settlement { get; set; }
        public string? Faction { get; set; }
        public string? FactionRole { get; set; }
    }
}
