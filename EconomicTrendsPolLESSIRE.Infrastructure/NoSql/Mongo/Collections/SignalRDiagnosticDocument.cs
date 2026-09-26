using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTrendsPolLESSIRE.Infrastructure.NoSql.Mongo.Collections
{
    public sealed class SignalRDiagnosticDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string HubName { get; set; } = string.Empty;

        public string? MethodName { get; set; }

        public string? Direction { get; set; }

        public string? ConnectionIdHash { get; set; }

        public string? UserId { get; set; }

        public string? UserName { get; set; }

        public int? EventId { get; set; }

        public int? PlaceId { get; set; }

        public string? GroupName { get; set; }

        public string? CorrelationId { get; set; }

        public long? PayloadSizeBytes { get; set; }

        public long? DurationMs { get; set; }

        public bool Success { get; set; } = true;

        public string? ErrorType { get; set; }

        public string? ErrorMessage { get; set; }

        public string? Transport { get; set; }

        public string? ClientIpHash { get; set; }

        public string? UserAgentHash { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}









































































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.