using System.Text.Json.Serialization;

namespace EconomicTrendsPolLESSIRE.Shared.Security
{
    public class CspReportModel
    {
#nullable disable
        [JsonPropertyName("csp-report")]
        public CspReportContent Report { get; set; }
    }
}
