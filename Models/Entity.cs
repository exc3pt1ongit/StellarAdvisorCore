using System.ComponentModel.DataAnnotations;

namespace StellarAdvisorCore.Models
{
    public abstract class Entity
    {
        [Key] public int Id { get; set; }
    }
}
