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
    }
}
