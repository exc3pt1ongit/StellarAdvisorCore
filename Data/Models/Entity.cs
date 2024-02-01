using System.ComponentModel.DataAnnotations;

namespace StellarAdvisorCore.Data.Models
{
    public abstract class Entity
    {
        [Key] public Guid Id { get; private set; }
    }
}
