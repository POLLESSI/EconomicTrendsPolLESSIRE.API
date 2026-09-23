using EconomicTrendsPolLESSIRE.Contracts.Enums;
using System.Text;

namespace EconomicTrendsPolLESSIRE.Domain.DTOs
{
    /// <summary>
    /// Contains the context to generate an OutZen smart suggestion.
    /// Passed to AstroIA and GPT-4 to produce a suggestion.
    /// </summary>
    public class SuggestionContextDTO
    {
    #nullable disable
        /// <summary>
        /// Time of day (dawn, day, sunset, night).
        /// </summary>
        public MomentOfDay Moment { get; set; }

        /// <summary>
        /// Optional: Name of the place or region concerned (e.g.: "Newyork", "Shanghai", "Tokyo", "Ottawa",  "Sidney", "Melbourne", "Brussel", "Amsterdam", "Paris", ...).
        /// </summary>
        public string? PlaceName { get; set; }

        /// <summary>
        /// Optional: OutZen theme (e.g.: “Ventes”, “achats”, “observation”, "hausses", "baisses", etc.).
        /// </summary>
        public string? Theme { get; set; }

        /// <summary>
        /// Optional: User context (tredder, profile, etc.).
        /// </summary>
        public string? UserProfile { get; set; }

        /// <summary>
        /// Optional: Preferred language for response (e.g. "fr", "en", "nl").
        /// </summary>
        public string UserPrompt { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int RadiusKm { get; set; } = 25;
        public string? Language { get; set; } = "fr";
        /// <summary>
        /// Generates a text prompt to GPT based on context.
        /// </summary>
        public string ToPromptString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("You are a smart assistant in a Belgian outing suggestion app called OutZen.");
            sb.AppendLine($"The current context is as follows :");

            sb.AppendLine($"- Time of day : {Moment}");

            if (!string.IsNullOrWhiteSpace(Theme))
                sb.AppendLine($"- Theme sought : {Theme}");

            if (!string.IsNullOrWhiteSpace(UserProfile))
                sb.AppendLine($"- User profile : {UserProfile}");

            sb.AppendLine();
            sb.AppendLine("Give a suitable, original and kind suggestion to do now in this region.");
            sb.AppendLine("The suggestion should be concise, inspiring, local and not involve long travel.");

            if (Language?.ToLower() == "en")
                sb.AppendLine("Please answer in English.");
            else if (Language?.ToLower() == "nl")
                sb.AppendLine("Beantwoord dit in het Nederlands.");
            else
                sb.AppendLine("Réponds en français.");

            return sb.ToString();
        }
    }
}























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.