using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using StellarAdvisorCore.Extensions;
using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Services.Characters;
using StellarAdvisorCore.Services.Localization;
using StellarAdvisorCore.Data.Repository.Characters;
using StellarAdvisorCore.Data.Repository.Localization;

namespace StellarAdvisorCore.Commands
{
    public class CharacterCommands : ApplicationCommandModule
    {
        private readonly ICharacterRepository _characterRepository = new CharacterRepository();
        private readonly ILocalizationService _localizationService = new LocalizationService(new LocalizationRepository());
        private readonly ICharacterService _characterService = new CharacterService(new CharacterRepository(), new LocalizationService(new LocalizationRepository()));

        [SlashCommand("createcharacter", "Створити ігрового персонажа")]
        public async Task CreateCharacterCommand(InteractionContext context,
            [Option("characterName", "Ім'я/прізвище ігрового персонажа")] string characterName)
        {
            var character = await _characterService.CreateCharacter(context.User, characterName);
           
            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Lilac,
                Title = $"Створення персонажа, {characterName}",
                Description = $"Розробка створення ігрового персонажа для майнкрафт сервера Ficture Story.\n" +
                $"DiscordUserId: {character.DiscordUserId}, Settlement: {character.Settlement}\n" +
                $"Faction: {character.Faction}, FactionRole: {character.FactionRole}"
            };

            await context.ResponseWithEmbedAsync(embedMessage);
        }

        [SlashCommand("getallcharacters", "Дізнатися список усіх персонажів")]
        public async Task GetaAllCharactersCommand(InteractionContext context)
        {
            using SqliteContext sqlite = new SqliteContext();
            var allCharacters = sqlite.Characters.ToList();
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

        [SlashCommand("mycharacter", "Дізнатися інформацію про свого персонажа")]
        public async Task GetInformationAboutMyCharacterCommand(InteractionContext context)
        {
            var embedMessage = await _characterService.GetCharacterInformationEmbed(context, _characterRepository.GetUserCharacterName(context.User) ?? string.Empty);

            await context.ResponseWithEmbedAsync(embedMessage);
        }

        [SlashCommand("getcharacter", "Дізнатися інформацію про ігрового персонажа")]
        public async Task GetInformationAboutCharacterCommand(InteractionContext context,
            [Option("characterName", "Ім'я/прізвище ігрового персонажа")] string characterName)
        {
            var embedMessage = await _characterService.GetCharacterInformationEmbed(context, characterName);

            await context.ResponseWithEmbedAsync(embedMessage);
        }
    }
}
