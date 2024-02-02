using StellarAdvisorCore.Data.Repository.Localization;

namespace StellarAdvisorCore.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private readonly ILocalizationRepository _localizationRepository;

        public LocalizationService(ILocalizationRepository localizationRepository)
        {
            _localizationRepository = localizationRepository;
        }

        public void LoadLocalization(string languageCode) => _localizationRepository.LoadLocalization(languageCode);
        public async Task LoadLocalizationAsync(string languageCode) => await _localizationRepository.LoadLocalizationAsync(languageCode);

        public string GetLocalizedString(string languageCode, string key) => _localizationRepository.GetLocalizedString(languageCode, key);
        public string GetClientLocalizedString(string key) => _localizationRepository.GetLocalizedString(Program.BotConfig.Values.ClientLocalization ?? "uk-UA", key);
        public string GetServerLocalizedString(string key) => _localizationRepository.GetLocalizedString(Program.BotConfig.Values.ServerLocalization ?? "en-US", key);
    }
}
