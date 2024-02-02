using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Data.Models.Entities.Characters;
using StellarAdvisorCore.Data.Models.Interfaces;
using StellarAdvisorCore.Logging;

namespace StellarAdvisorCore.Data.Models.Entities.Settlements
{
    public class SettlementBase : Entity, ISettlement
    {
        public string? Name { get; set; }
        public List<Character>? Residents { get; set; }

        public SettlementBase(string name) => Name = name;

        public void LoadResidents()
        {
            using (SqliteContext sqlite = new SqliteContext())
            {
                try
                {
                    Residents = sqlite.Characters.Where(r => r.Settlement == Id.ToString()).ToList();
                    Logger.LogSuccess($"Settlement [{Name}]: {Residents.Count} residents loaded successfully");
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Settlement [{Name}]: Cannot load residents. Exception: {ex.Message}");
                }
            }
        }
    }
}
