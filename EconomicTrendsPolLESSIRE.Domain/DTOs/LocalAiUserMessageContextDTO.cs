namespace EconomicTrendsPolLESSIRE.Domain.DTOs
{
    public sealed class LocalAiUserMessageContextDTO
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? SourceType { get; set; }
        public int? SourceId { get; set; }
        public string? RelatedName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? Tags { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; }
    }
}



























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.