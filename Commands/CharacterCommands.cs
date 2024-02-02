using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using StellarAdvisorCore.Data;
using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Data.Repository.Characters;
using StellarAdvisorCore.Data.Repository.Localization;

using StellarAdvisorCore.Extensions;
using StellarAdvisorCore.Logging;
using StellarAdvisorCore.Services.Characters;
using StellarAdvisorCore.Services.Localization;

namespace StellarAdvisorCore.Commands
{
    [SlashCommandGroup("character", "Група команд, що прямо стосуються персонажів")]
    public class CharacterCommands : ApplicationCommandModule
    {
        private readonly ICharacterRepository _characterRepository = new CharacterRepository(new LocalizationService(new LocalizationRepository()));
        private readonly ILocalizationService _localizationService = new LocalizationService(new LocalizationRepository());
        private readonly ICharacterService _characterService = new CharacterService(new CharacterRepository(new LocalizationService(new LocalizationRepository())), new LocalizationService(new LocalizationRepository()));

        [SlashCommand("create", "Створити ігрового персонажа")]
        public async Task CreateCharacterCommand(InteractionContext context,
            [Option("characterName", "Ім'я/прізвище ігрового персонажа")] string characterName)
        {
            await _characterService.CreateCharacter(context.User, characterName);
            var embedMessage = await _characterService.GetCharacterInformationEmbed(context, characterName);
            await context.ResponseWithEmbedAsync(embedMessage);
        }

        [SlashCommand("delete", "Видалити ігрового персонажа")]
        public async Task DeleteCharacterCommand(InteractionContext context,
            [Option("characterName", "Ім'я/прізвище ігрового персонажа")] string characterName)
        {
            using SqliteContext sqlite = new SqliteContext();

            try
            {
                var character = sqlite.Characters?.FirstOrDefault(c => c.Name == characterName);

                if (character == null)
                {
                    await context.ResponseWithErrorEmbedAsync("Персонажа з таким ім'ям не існує.");
                    return;
                }

                var deleteCharacter = await _characterService.DeleteCharacter(characterName);

                if (deleteCharacter)
                {
                    await Logger.LogSuccessAsync($"{context.User.Username} успішно видалив персонажа {characterName}.");
                    await context.ResponseWithSuccessEmbedAsync($"**{context.User.Username}** успішно видалив персонажа **{characterName}**.");
                }
                else
                {
                    await Logger.LogErrorAsync($"Виникла проблема під час видалення персонажа {characterName}.");
                    await context.ResponseWithErrorEmbedAsync($"Виникла проблема під час видалення персонажа **{characterName}**.");
                }
            }
            catch (Exception ex)
            {
                await Logger.LogErrorAsync($"Exception: {ex.Message}");
            }
        }

        [SlashCommand("getall", "Дізнатися список усіх персонажів")]
        public async Task GetAllCharactersCommand(InteractionContext context)
        {
            using (SqliteContext sqlite = new SqliteContext())
            {
                var allCharacters = sqlite.Characters?.ToList();

                if (allCharacters == null)
                {
                    await context.ResponseWithErrorEmbedAsync("Список ігрових персонажів пустий.");
                    return;
                }

                var description = "Список ігрових персонажів:\n";

                foreach (var character in allCharacters)
                {
                    var mention = context.Guild.Members.FirstOrDefault(m => m.Value.Id == character.DiscordUserId).Value.Mention;
                    description += $"- **{character.Name}**, ігровий персонаж: {mention}\n";
                }

                var embedMessage = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Lilac,
                    Title = "Усі ігрові персонажі (Stellar Advisor)",
                    Description = description
                };

                await context.ResponseWithEmbedAsync(embedMessage);
            }
        }

        [SlashCommand("my", "Дізнатися інформацію про свого персонажа")]
        public async Task GetInformationAboutMyCharacterCommand(InteractionContext context)
        {
            var embedMessage = await _characterService.GetCharacterInformationEmbed(context, _characterRepository.GetUserCharacterName(context.User) ?? string.Empty);

            await context.ResponseWithEmbedAsync(embedMessage);
        }

        [SlashCommand("get", "Дізнатися інформацію про ігрового персонажа")]
        public async Task GetInformationAboutCharacterCommand(InteractionContext context,
            [Option("characterName", "Ім'я/прізвище ігрового персонажа")] string characterName)
        {
            var embedMessage = await _characterService.GetCharacterInformationEmbed(context, characterName);

            await context.ResponseWithEmbedAsync(embedMessage);
        }

        [SlashCommand("set", "Змінити інформацію про ігрового персонажа")]
        public async Task SetInformationAboutCharacterCommand(InteractionContext context,
            [Option("characterName", "Ім'я/прізвище ігрового персонажа")] string characterName,
            [Option("settlementName", "Майбутнє поселення ігрового персонажа")] string settlementName,
            [Option("fractionName", "Майбутня фракція ігрового персонажа")] string fractionName = "")
        {
            var character = _characterRepository.GetCharacterByName(characterName);

            if (character == null)
            {
                await context.ResponseWithErrorEmbedAsync("Цього ігрового персонажа не існує.");
                return;
            }

            var settlement = World.Instance.Settlements?.FirstOrDefault(s => s.Name == settlementName);

            if(settlement == null)
            {
                await context.ResponseWithErrorEmbedAsync("Це поселення не зареєстроване.");
                return;
            }

            using (SqliteContext sqlite = new SqliteContext())
            {
                character.Settlement = settlement?.Name;
                
                if (!string.IsNullOrEmpty(fractionName))
                {
                    var fraction = settlement?.Fractions?.FirstOrDefault(f => f.Name == fractionName);
                    character.Fraction = fraction?.Name;
                }

                await _characterService.UpdateCharacter(character);
            }
            
            await context.ResponseWithSuccessEmbedAsync($"Зміни для персонажа: {characterName}", 
                "Зміни успішно збережено для ігрового персонажа.");
        }
    }
}
