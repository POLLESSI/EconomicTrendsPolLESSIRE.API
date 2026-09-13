using EconomicTrendsPolLESSIRE.Contracts.Enums;

namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public sealed class UserMessageAdminQueue
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public AdminMessageCategory Category { get; set; }
        public byte Priority { get; set; } = 1;
        public AdminMessageStatus Status { get; set; } = AdminMessageStatus.Open;
        public decimal? Confidence { get; set; }
        public string ClassificationSource { get; set; } = "Rules";
        public string? AssignedTo { get; set; }
        public string? AdminNote { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; set; }
        public DateTime? ResolvedAtUtc { get; set; }
        public bool Active { get; set; } = true;
    }
}
