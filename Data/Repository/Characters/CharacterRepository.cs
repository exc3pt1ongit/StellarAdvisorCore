using DSharpPlus.Entities;
using StellarAdvisorCore.Logging;
using StellarAdvisorCore.Services.Localization;

using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Data.Models.Entities.Fractions;
using StellarAdvisorCore.Data.Models.Entities.Characters;
using StellarAdvisorCore.Data.Models.Entities.Settlements;

namespace StellarAdvisorCore.Data.Repository.Characters
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly ILocalizationService _localizationService;
        public CharacterRepository(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public async Task<Character> CreateCharacter(DiscordUser discordUser, string name)
        {
            var character = new Character()
            {
                Name = name,
                Settlement = string.Empty,
                Fraction = string.Empty,
                FractionRole = string.Empty,
                DiscordUserId = discordUser.Id
            };

            using (SqliteContext sqlite = new SqliteContext())
            {
                sqlite.Characters.Add(character);
                await sqlite.SaveChangesAsync();

                Logger.LogSuccess($"Character: {character.Name} [id: {character.DiscordUserId}] added successfully");
            }

            return character;
        }

        public async Task<bool> DeleteCharacter(string characterName)
        {
            using SqliteContext sqlite = new SqliteContext();
            var characterToDelete = sqlite.Characters.FirstOrDefault(c => c.Name == characterName);

            if (characterToDelete != null)
            {
                sqlite.Characters.Remove(characterToDelete);
                await sqlite.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public Character? GetCharacterByName(string characterName)
        {
            using SqliteContext sqlite = new SqliteContext();
            return sqlite.Characters.FirstOrDefault(c => c.Name == characterName);
        }

        public string? GetUserCharacterName(DiscordUser discordUser)
        {
            using SqliteContext sqlite = new SqliteContext();
            return sqlite.Characters.FirstOrDefault(c => c.DiscordUserId == discordUser.Id)?.Name;
        }

        public FractionBase? GetCharacterFraction(string characterName)
        {
            var settlement = GetCharacterSettlement(characterName);
            return settlement?.Fractions?.FirstOrDefault(f => f.Name == characterName);
        }

        public Settlement? GetCharacterSettlement(string characterName)
        {
            var character = GetCharacterByName(characterName);
            return World.Instance?.Settlements?.FirstOrDefault(s => s.Name == character?.Settlement);
        }

        public string? GetCharacterFractionName(string characterName)
        {
            var characterFraction = GetCharacterFraction(characterName)?.Name;
            return string.IsNullOrEmpty(characterFraction) ? _localizationService.GetClientLocalizedString("ch_not_in_the_fraction") : characterFraction;
        }
        
        public string? GetCharacterSettlementName(string characterName)
        {
            var characterSettlement = GetCharacterSettlement(characterName)?.Name;
            return string.IsNullOrEmpty(characterSettlement) ? "Невідоме" : characterSettlement;
        }

        public string? GetCharacterFractionRoleName(string characterName)
        {
            return "Невідома роль";
        }

        public async Task<bool> UpdateCharacter(Character character)
        {
            using SqliteContext sqlite = new SqliteContext();
            var existingCharacter = sqlite.Characters.FirstOrDefault(c => c.Name == character.Name);

            if (existingCharacter != null)
            {
                existingCharacter.Fraction = character.Fraction;
                existingCharacter.Settlement = character.Settlement;
                existingCharacter.FractionRole = character.FractionRole;

                await sqlite.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
