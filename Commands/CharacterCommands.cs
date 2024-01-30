using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using StellarAdvisorCore.Services;

namespace StellarAdvisorCore.Commands
{
    public class CharacterCommands : ApplicationCommandModule
    {
        private readonly CharacterService characterService = new CharacterService();

        [SlashCommand("createcharacter", "Створити ігрового персонажа")]
        public async Task CreateCharacterCommand(InteractionContext context)
        {
            await context.DeferAsync();

            var embedMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Lilac,
                Title = "Створення персонажа",
                Description = "Розробка створення ігрового персонажа для майнкрафт сервера Ficture Story."
            };

            await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedMessage));
        }
    }
}
