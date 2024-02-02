using StellarAdvisorCore.Data.Models.Entities.Characters;

namespace StellarAdvisorCore.Data.Models.Interfaces
{
    public interface IFraction
    {
        string? Name { get; set; }
        List<Character>? Members { get; set; }
    }
}
