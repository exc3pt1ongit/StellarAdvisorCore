namespace StellarAdvisorCore.Data.Repository.Localization
{
    public interface ILocalizationRepository
    {
        void LoadLocalization(string languageCode);
        Task LoadLocalizationAsync(string languageCode);
        string GetLocalizedString(string languageCode, string key);
    }
}
