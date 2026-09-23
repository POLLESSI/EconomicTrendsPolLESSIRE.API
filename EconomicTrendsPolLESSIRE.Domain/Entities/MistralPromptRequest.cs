namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public sealed class MistralPromptRequest
    {
        public string Prompt { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? LanguageCode { get; set; } = "fr-FR";
    }
}
