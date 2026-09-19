using EconomicTrendsPolLESSIRE.Contracts.Enums;

namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public sealed class MessageTriageResult
    {
        public bool RequiresAdminReview { get; init; }

        public AdminMessageCategory Category { get; init; }

        public byte Priority { get; init; }

        public double Confidence { get; init; }

        public string ClassificationSource { get; init; } = "Rules";
    }
}





















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.