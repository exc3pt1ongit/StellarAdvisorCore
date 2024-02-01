﻿using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Microsoft.EntityFrameworkCore;
using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Extensions;
using StellarAdvisorCore.Logging;
using StellarAdvisorCore.Services;

namespace StellarAdvisorCore.Commands
{
    [SlashCommandGroup("bot", "Група команд, що прямо стосуються бота")]
    public class BotCommands : ApplicationCommandModule
    {
        [RequireOwner]
        [SlashCommand("migrate", "\"Мігрувати\" базу даних Sqlite (Stellar Advisor)")]
        public async Task MigrateSqliteCommand(InteractionContext context)
        {
            await Logger.LogAsync("Manual command migrating");

            try
            {
                await using SqliteContext sqlite = new SqliteContext();

                if (sqlite.Database.GetPendingMigrationsAsync().Result.Any())
                {
                    await sqlite.Database.MigrateAsync();
                    await sqlite.SaveChangesAsync();
                }

                var embedMessage = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Lilac,
                    Title = LocalizationService.GetClientLocalizedString("cmd_migrate_EmbedTitle"),
                    Description = LocalizationService.GetClientLocalizedString("cmd_migrate_EmbedDescription")
                };

                await context.ResponseWithEmbedAsync(embedMessage);
                await Logger.LogSuccessAsync("Sqlite Migration complete");
            }
            catch (Exception ex)
            {
                await Logger.LogErrorAsync(ex.Message);
            }
        }

        [SlashCommand("technical", "Дізнатися технічні деталі про Stellar Advisor Bot")]
        public async Task GetTechnicalInformationCommand(InteractionContext context)
        {
            var embedMessage = new DiscordEmbedBuilder
            {
                Title = LocalizationService.GetClientLocalizedString("cmd_technical_EmbedTitle"),
                Color = DiscordColor.Lilac,
                Description = LocalizationService.GetClientLocalizedString("cmd_technical_EmbedDescription"),
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = LocalizationService.GetClientLocalizedString("cmd_technical_EmbedFooter_Text")
                }
            };

            embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_bot_version"), $"v{Program.BotConfig.Version}");
            
            using (SqliteContext sqlite = new SqliteContext())
            {
                var appliedMigration = sqlite.Database.GetAppliedMigrations().Last();
                embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_bot_appliedMigration"), $"{appliedMigration}", true);

                try
                {
                    var pendingMigration = sqlite.Database.GetPendingMigrations().LastOrDefault() ?? "Жодної міграції на очікуванні";
                    embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_bot_pendingMigration"), $"{pendingMigration}");
                } catch (Exception ex)
                {
                    await Logger.LogErrorAsync(ex.Message);
                }
                
            }

            embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_bot_ping"), $"{context.Client.Ping} ms", true);

            var totalUptime = DateTime.Now - Program.Uptime;

            embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_bot_uptime"), $"{totalUptime.Days} днів, {totalUptime.Hours} годин, {totalUptime.Minutes} хвилин, {totalUptime.Seconds} секунд");

            embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_bot_machineName"), $"{Environment.MachineName}", true);
            embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_bot_processId"), $"{Environment.ProcessId}", true);
            embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_bot_operatingSystem"), $"{Environment.OSVersion.VersionString}");

            embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_bot_library"), "DSharpPlus");
            embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_bot_programmingLanguage"), "C# (.NET Core 8)", true);
            embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_bot_openSourceCode"), $"[GitHub Repository]({Program.BotConfig.Values.OpenSourceUrl})");

            await context.ResponseWithEmbedAsync(embedMessage);
        }

        [SlashCommand("about", "Дізнатися інформацію про Stellar Advisor Bot")]
        public async Task GetAboutInformationCommand(InteractionContext context)
        {
            var embedMessage = new DiscordEmbedBuilder
            {
                Title = LocalizationService.GetClientLocalizedString("cmd_about_EmbedTitle"),
                Color = DiscordColor.Lilac,
                Description = LocalizationService.GetClientLocalizedString("cmd_about_EmbedDescription"),
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = LocalizationService.GetClientLocalizedString("cmd_about_EmbedFooter_Text")
                }
            };

            embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_about_keyFeatures_Header"), LocalizationService.GetClientLocalizedString("field_about_keyFeatures_Content"));
            embedMessage.AddField(LocalizationService.GetClientLocalizedString("field_about_needHelp_Header"), LocalizationService.GetClientLocalizedString("field_about_needHelp_Content"));

            await context.ResponseWithEmbedAsync(embedMessage);
        }
    }
}
