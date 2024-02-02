namespace StellarAdvisorCore.Services.Localization
{
    public interface ILocalizationService
    {
        void LoadLocalization(string languageCode);
        Task LoadLocalizationAsync(string languageCode);
        string GetLocalizedString(string languageCode, string key);
        string GetClientLocalizedString(string key);
        string GetServerLocalizedString(string key);
    }
}
