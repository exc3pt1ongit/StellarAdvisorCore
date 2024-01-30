namespace StellarAdvisorCore.Models
{
    public class MutedUser : Entity
    {
        public ulong MemberId { get; set; }
        public string? MutedReason { get; set; }
        public DateTime MutedExpiration { get; set; }
        public ulong MutedById { get; set; }
        public string? Type { get; set; }
    }
}
