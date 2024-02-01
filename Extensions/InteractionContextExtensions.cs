using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace StellarAdvisorCore.Extensions
{
    public static class InteractionContextExtensions
    {
        public static async Task ResponseWithMessageAsync(this InteractionContext context, string content)
        {
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent(content));
        }

        public static async Task ResponseWithEmbedAsync(this InteractionContext context, DiscordEmbed embed)
        {
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(embed));
        }

        public static async Task ResponseWithErrorEmbedAsync(this InteractionContext context, string errorMessage)
        {
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(new DiscordEmbedBuilder().GetErrorEmbed(errorMessage)));
        }

        public static async Task ResponseWithErrorEmbedAsync(this InteractionContext context, string errorHeader, string errorContent)
        {
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(new DiscordEmbedBuilder().GetErrorEmbed(errorHeader, errorContent)));
        }

        public static async Task ResponseWithSuccessEmbedAsync(this InteractionContext context, string successMessage)
        {
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(new DiscordEmbedBuilder().GetSuccessEmbed(successMessage)));
        }

        public static async Task ResponseWithSuccessEmbedAsync(this InteractionContext context, string successHeader, string successContent)
        {
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(new DiscordEmbedBuilder().GetSuccessEmbed(successHeader, successContent)));
        }
    }
}
