using System.ComponentModel.DataAnnotations;

namespace StellarAdvisorCore.Models
{
    public abstract class Entity
    {
        [Key] public Guid Id { get; private set; }
    }
}
