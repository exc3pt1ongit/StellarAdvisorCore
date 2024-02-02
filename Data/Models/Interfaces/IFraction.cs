using StellarAdvisorCore.Data.Models.Entities.Characters;
using StellarAdvisorCore.Data.Models.Entities.Settlements;

namespace StellarAdvisorCore.Data.Models.Interfaces
{
    public interface IFraction
    {
        string? Name { get; set; }
        Settlement? Settlement { get; set; }
        List<Character>? Members { get; set; }
        void LoadMembers();
    }
}
