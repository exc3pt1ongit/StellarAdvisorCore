﻿using Newtonsoft.Json;
using StellarAdvisorCore.Logging;

namespace StellarAdvisorCore.Services
{
    public static class LocalizationService
    {
        public static void LoadLocalization(string languageCode)
        {
            if (languageCode.Equals("unload"))
            {
                Logger.LogError("Unloading undefined localization");
                return;
            }

            var filesPath = Directory.GetFiles($"Localization\\{languageCode}");

            foreach (var file in filesPath)
            {
                if (File.Exists(file))
                {
                    try
                    {
                        var json = File.ReadAllText(file);
                        var localization = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                        if (localization == null)
                        {
                            Logger.LogError($"Cannot load localization file: {file}, NULL object");
                            return;
                        }

                        if (!Program.Localizations.ContainsKey(languageCode))
                        {
                            Program.Localizations[languageCode] = new Dictionary<string, string>();
                        }

                        foreach (var kvp in localization)
                        {
                            Program.Localizations[languageCode][kvp.Key] = kvp.Value;
                        }

                        Logger.LogSuccess($"{file} loaded successfully");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Error loading localization file: {file}\n{ex.Message}");
                    }
                }
                else
                {
                    Logger.LogError($"Cannot load localization file: {file}");
                }
            }
        }

        public static async Task LoadLocalizationAsync(string languageCode)
        {
            if(languageCode.Equals("unload"))
            {
                Logger.LogError("Unloading undefined localization");
                return;
            }

            var filesPath = Directory.GetFiles($"Localization\\{languageCode}");

            foreach(var file in filesPath)
            {
                if(File.Exists(file))
                {
                    try
                    {
                        var json = await File.ReadAllTextAsync(file);
                        var localization = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                        if (localization == null)
                        {
                            Logger.LogError($"Cannot load localization file: {file}, NULL object");
                            continue;
                        }

                        if (!Program.Localizations.ContainsKey(languageCode))
                        {
                            Program.Localizations[languageCode] = new Dictionary<string, string>();
                        }

                        foreach (var kvp in localization)
                        {
                            Program.Localizations[languageCode][kvp.Key] = kvp.Value;
                        }

                        await Logger.LogSuccessAsync($"{file} loaded successfully");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Error loading localization file: {file}\n{ex.Message}");
                    }
                }
                else
                {
                    Logger.LogError($"Cannot load localization file: {file}");
                }
            }
        }

        public static string GetLocalizedString(string languageCode, string key)
        {
            if(Program.Localizations.TryGetValue(languageCode, out var language))
            {
                if(language.TryGetValue(key, out var localizedString))
                {
                    return localizedString;
                }
            }

            return $"[{languageCode}-{key}]";
        }

        public static string GetClientLocalizedString(string key) => GetLocalizedString(Program.BotConfig.Values.ClientLocalization ?? "uk-UA", key);
        public static string GetServerLocalizedString(string key) => GetLocalizedString(Program.BotConfig.Values.ServerLocalization ?? "en-US", key);
    }
}
