using StellarAdvisorCore.Logging;
using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Data.Models.Interfaces;
using StellarAdvisorCore.Data.Models.Entities.Fractions;
using StellarAdvisorCore.Data.Models.Entities.Characters;

namespace StellarAdvisorCore.Data.Models.Entities.Settlements
{
    public class Settlement : Entity, ISettlement
    {
        public string? Name { get; set; }
        public List<Character>? Residents { get; set; }
        public List<FractionBase>? Fractions { get; set; }

        public Settlement(string name)
        {
            Name = name;
            Fractions = new List<FractionBase>();
        }
        public Settlement(string name, List<FractionBase> fractions) : this(name) => Fractions = fractions;

        public void LoadResidents()
        {
            using SqliteContext sqlite = new SqliteContext();

            try
            {
                Residents = sqlite.Characters.Where(r => r.Settlement == Name).ToList();
                Logger.LogSuccess($"Settlement [{Name}]: {Residents.Count} residents loaded successfully");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Settlement [{Name}]: Cannot load residents. Exception: {ex.Message}");
            }
        }
    }
}
