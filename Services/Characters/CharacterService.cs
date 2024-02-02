using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using StellarAdvisorCore.Extensions;
using StellarAdvisorCore.Services.Localization;

using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Data.Repository.Characters;
using StellarAdvisorCore.Data.Models.Entities.Characters;

namespace StellarAdvisorCore.Services.Characters
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;
        private readonly ILocalizationService _localizationService;

        public CharacterService(ICharacterRepository characterRepository, ILocalizationService localizationService)
        {
            _characterRepository = characterRepository;
            _localizationService = localizationService;
        }

        public async Task<Character> CreateCharacter(DiscordUser discordUser, string name) => await _characterRepository.CreateCharacter(discordUser, name);
        public async Task<bool> UpdateCharacter(Character character) => await _characterRepository.UpdateCharacter(character);
        public async Task<bool> DeleteCharacter(string characterName) => await _characterRepository.DeleteCharacter(characterName);

        public async Task<DiscordEmbed> GetCharacterInformationEmbed(InteractionContext context, string characterName)
        {
            var character = _characterRepository.GetCharacterByName(characterName);

            if (character == null)
            {
                var embedError = new DiscordEmbedBuilder()
                    .GetErrorEmbed(_localizationService.GetClientLocalizedString("ch_error_undefined_character"));

                await context.ResponseWithEmbedAsync(embedError);
            }

            var oocUser = context.Guild.Members.FirstOrDefault(u => u.Value.Id == character?.DiscordUserId).Value;

            if (oocUser == null)
            {
                return new DiscordEmbedBuilder()
                    .GetErrorEmbed(_localizationService
                    .GetClientLocalizedString("ch_error_ooc_user_is_null"));
            }

            // TODO: refactor topside code in this function

            var characterSettlement = _characterRepository.GetCharacterSettlementName(characterName);
            var characterFraction = _characterRepository.GetCharacterFractionName(characterName);
            var characterFractionRole = _characterRepository.GetCharacterFractionRoleName(characterName);

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Lilac,
                Title = $":family_mwg: Інформація про персонажа: {characterName}",
                Description = $"Розробка створення ігрового персонажа для майнкрафт сервера Ficture Story."
            };

            embedMessage.AddField(":homes: - Поселення", characterSettlement, true);
            embedMessage.AddField(":construction_worker: - Фракція", characterFraction, true);
            embedMessage.AddField(":farmer: - Роль у фракції", characterFractionRole, true);

            return embedMessage;
        }
    }
}
