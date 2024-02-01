﻿using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using StellarAdvisorCore.Data.Models.Entities.Characters;
using StellarAdvisorCore.Data.Repository;
using StellarAdvisorCore.Extensions;

namespace StellarAdvisorCore.Services.Characters
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;

        public CharacterService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public async Task<Character> CreateCharacter(DiscordUser discordUser, string name)
        {
            return await _characterRepository.CreateCharacter(discordUser, name);
        }

        public async Task<bool> UpdateCharacter(Character character)
        {
            return await _characterRepository.UpdateCharacter(character);
        }

        public async Task<bool> DeleteCharacter(string characterName)
        {
            return await _characterRepository.DeleteCharacter(characterName);
        }

        public async Task<DiscordEmbed> GetCharacterInformationEmbed(InteractionContext context, string characterName)
        {
            var character = _characterRepository.GetCharacterByName(characterName);

            if (character == null)
            {
                var embedError = new DiscordEmbedBuilder()
                    .GetErrorEmbed(LocalizationService.GetClientLocalizedString("ch_error_undefined_character"));

                await context.ResponseWithEmbedAsync(embedError);
            }

            var oocUser = context.Guild.Members.FirstOrDefault(u => u.Value.Id == character?.DiscordUserId).Value;

            if (oocUser == null)
            {
                return new DiscordEmbedBuilder()
                    .GetErrorEmbed(LocalizationService
                    .GetClientLocalizedString("ch_error_ooc_user_is_null"));
            }

            var mention = oocUser.Mention;

            var settlement = string.IsNullOrEmpty(character?.Settlement) ? "Невідоме" : character?.Settlement;
            var fraction = string.IsNullOrEmpty(character?.Faction) ? LocalizationService.GetClientLocalizedString("ch_not_in_the_fraction") : character?.Faction;

            return new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Lilac,
                Title = LocalizationService.GetClientLocalizedString("ch_info_about_character").Replace("{characterName}", character?.Name),
                Description = $"OOC користувач: {mention}\n" +
                $"Поселення: {settlement}\n" +
                $"Фракція: {fraction}"
            };
        }
    }
}
