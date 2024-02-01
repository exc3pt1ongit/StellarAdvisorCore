using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using StellarAdvisorCore.Data.Models.Entities.Characters;

namespace StellarAdvisorCore.Services.Characters
{
    public interface ICharacterService
    {
        Task<Character> CreateCharacter(DiscordUser discordUser, string name);
        Task<bool> DeleteCharacter(string characterName);
        Task<bool> UpdateCharacter(Character character);
        Task<DiscordEmbed> GetCharacterInformationEmbed(InteractionContext context, string characterName);
    }
}
