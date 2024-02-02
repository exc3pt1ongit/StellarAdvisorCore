using StellarAdvisorCore.Data.Models.Entities.Characters;
using StellarAdvisorCore.Data.Models.Entities.Fractions;

namespace StellarAdvisorCore.Data.Models.Interfaces
{
    public interface ISettlement
    {
        string? Name { get; set; }
        List<Character>? Residents { get; set; }
        List<FractionBase>? Fractions { get; set; }
    }
}
