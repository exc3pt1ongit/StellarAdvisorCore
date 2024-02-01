using DSharpPlus.Entities;
using StellarAdvisorCore.Logging;
using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Data.Models.Entities.Characters;

namespace StellarAdvisorCore.Data.Repository
{
    internal class CharacterRepository : ICharacterRepository
    {
        public async Task<Character> CreateCharacter(DiscordUser discordUser, string name)
        {
            var character = new Character()
            {
                Name = name,
                Settlement = string.Empty,
                Faction = string.Empty,
                FactionRole = string.Empty,
                DiscordUserId = discordUser.Id
            };

            using (SqliteContext sqlite = new SqliteContext())
            {
                sqlite.Characters.Add(character);
                World.Characters?.Add(character);

                await sqlite.SaveChangesAsync();
                Logger.LogSuccess($"Character: {character.Name} [id: {character.DiscordUserId}] added successfully");
            }

            return character;
        }

        public async Task<bool> DeleteCharacter(string characterName)
        {
            using (SqliteContext sqlite = new SqliteContext())
            {
                var characterToDelete = sqlite.Characters.FirstOrDefault(c => c.Name == characterName);

                if (characterToDelete != null)
                {
                    sqlite.Characters.Remove(characterToDelete);
                    await sqlite.SaveChangesAsync();
                    return true;
                }

                return false;
            }
        }

        public Character? GetCharacterByName(string characterName)
        {
            using (SqliteContext sqlite = new SqliteContext())
            {
                return sqlite.Characters.FirstOrDefault(c => c.Name == characterName);
            }
        }

        public string? GetUserCharacterName(DiscordUser discordUser)
        {
            using (SqliteContext sqlite = new SqliteContext())
            {
                return sqlite.Characters.FirstOrDefault(c => c.DiscordUserId == discordUser.Id)?.Name;
            }
        }

        public async Task<bool> UpdateCharacter(Character character)
        {
            using (SqliteContext sqlite = new SqliteContext())
            {
                var existingCharacter = sqlite.Characters.FirstOrDefault(c => c.Name == character.Name);

                if (existingCharacter != null)
                {
                    existingCharacter.Settlement = character.Settlement;
                    existingCharacter.Faction = character.Faction;
                    existingCharacter.FactionRole = character.FactionRole;

                    await sqlite.SaveChangesAsync();
                    return true;
                }

                return false;
            }
        }
    }
}
