using DSharpPlus.Entities;
using StellarAdvisorCore.Data.Models.Entities.Characters;
using StellarAdvisorCore.Data.Models.Entities.Fractions;
using StellarAdvisorCore.Data.Models.Entities.Settlements;

namespace StellarAdvisorCore.Data.Repository.Characters
{
    public interface ICharacterRepository
    {
        Task<Character> CreateCharacter(DiscordUser discordUser, string name);
        Task<bool> DeleteCharacter(string characterName);
        Task<bool> UpdateCharacter(Character character);

        string? GetUserCharacterName(DiscordUser discordUser);
        Character? GetCharacterByName(string characterName);

        FractionBase? GetCharacterFraction(string characterName);
        Settlement? GetCharacterSettlement(string characterName);

        string? GetCharacterFractionName(string characterName);
        string? GetCharacterSettlementName(string characterName);
        string? GetCharacterFractionRoleName(string characterName);
    }
}
