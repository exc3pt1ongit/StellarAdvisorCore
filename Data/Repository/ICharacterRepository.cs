using DSharpPlus.Entities;
using StellarAdvisorCore.Data.Models.Entities.Characters;

namespace StellarAdvisorCore.Data.Repository
{
    public interface ICharacterRepository
    {
        Task<Character> CreateCharacter(DiscordUser discordUser, string name);
        string? GetUserCharacterName(DiscordUser discordUser);
        Character? GetCharacterByName(string characterName);
        Task<bool> DeleteCharacter(string characterName);
        Task<bool> UpdateCharacter(Character character);
    }
}
