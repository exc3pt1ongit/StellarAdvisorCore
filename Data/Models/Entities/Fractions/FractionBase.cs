using StellarAdvisorCore.Data.Models.Interfaces;
using StellarAdvisorCore.Data.Models.Entities.Characters;
using StellarAdvisorCore.Data.Models.Entities.Settlements;
using StellarAdvisorCore.Data.Context;
using StellarAdvisorCore.Logging;

namespace StellarAdvisorCore.Data.Models.Entities.Fractions
{
    public class FractionBase : Entity, IFraction
    {
        public string? Name { get; set; }
        public Settlement? Settlement { get; set; }
        public List<Character>? Members { get; set; }
        
        public FractionBase(string name)
        {
            Name = name;
        }

        public void LoadMembers()
        {
            using SqliteContext sqlite = new SqliteContext();

            try
            {
                Members = sqlite.Characters.Where(m => m.Fraction == Name).ToList();
                Logger.LogSuccess($"Fraction [{Name}]: {Members.Count} members loaded successfully");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Fraction [{Name}]: Cannot load members. Exception: {ex.Message}");
            }
        }
    }
}
