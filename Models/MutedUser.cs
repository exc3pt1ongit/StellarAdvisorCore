namespace StellarAdvisorCore.Models
{
    public class MutedUser : Entity
    {
        public ulong MemberId { get; set; }
        public string? MutedReason { get; set; }
        public DateTime MutedExpiration { get; set; }
    }
}
