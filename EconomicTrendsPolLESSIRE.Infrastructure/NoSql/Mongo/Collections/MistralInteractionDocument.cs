using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Collections
{
    public sealed class MistralInteractionDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public int? SqlInteractionId { get; set; }

        public string PromptHash { get; set; } = string.Empty;

        public string PromptPreview { get; set; } = string.Empty;

        public string Response { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public bool Success { get; set; }

        public string? Error { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}












































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.