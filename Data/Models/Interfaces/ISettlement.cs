using StellarAdvisorCore.Data.Models.Entities.Characters;

namespace StellarAdvisorCore.Data.Models.Interfaces
{
    public interface ISettlement
    {
        string? Name { get; set; }
        List<Character>? Residents { get; set; }
    }
}
