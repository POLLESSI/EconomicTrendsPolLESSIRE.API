using EconomicTrendsPolLESSIRE.Application.Extensions;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Enums;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Runtime.CompilerServices;
//using EconomicTrendsPolLESSIRE.Domain.ValueObjects;

namespace EconomicTrendsPolLESSIRE.Application.Extensions
{
    public static class MapperExtensions
    {
    #nullable disable
        public static decimal RoundLat(double lat) => Math.Round((decimal)lat, 6);
        public static decimal RoundLon(double lon) => Math.Round((decimal)lon, 6);
        public static DateTime TruncateToSecond(DateTime dt)
        {
            var ticks = dt.Ticks - (dt.Ticks % TimeSpan.TicksPerSecond);
            return new DateTime(ticks, dt.Kind);
        }
        private static DateTimeOffset TruncateToSecond(DateTimeOffset dto)
        {
            var ticks = dto.UtcTicks - (dto.UtcTicks % TimeSpan.TicksPerSecond);
            return new DateTimeOffset(ticks, TimeSpan.Zero); // standardized UTC
        }

        public static Instrument MapToIntrument(this InstrumentDTO dto)
        {
            return new Instrument
            {
                Id = dto.Id,
                Symbol = dto.Symbol,
                Name = dto.Name,
                AssetClass = dto.AssetClass,
                ExchangeCode = dto.ExchangeCode,
                CurrencyCode = dto.CurrencyCode,
                CreatedAtUtc = dto.CreatedAtUtc
            }; 
        }

        public static InstrumentDTO MapToInstrumentDTO(this Instrument entity)
        {
            if (entity is null) return null!;

            return new InstrumentDTO
            {
                Id = entity.Id,
                Symbol = entity.Symbol,
                Name = entity.Name,
                AssetClass = entity.AssetClass,
                ExchangeCode = entity.ExchangeCode,
                CurrencyCode = entity.CurrencyCode,
                CreatedAtUtc = entity.CreatedAtUtc
            };

        }

        public static MarketCandle MapToMarketCandle(this MarketCandleDTO dto)
        {
            return new MarketCandle
            {
                InstrumentId = dto.InstrumentId,
                IntervalCode = dto.IntervalCode,
                OpenTimeUtc = dto.OpenTimeUtc,
                OpenPrice = dto.OpenPrice,
                HighPrice = dto.HighPrice,
                LowPrice = dto.LowPrice,
                ClosePrice = dto.ClosePrice,
                Volume = dto.Volume,
                VWAP = dto.VWAP,
                TradeCount = dto.TradeCount,
                IsFinal = dto.IsFinal
            };

        }

        public static MarketCandleDTO MapToMarketCandleDTO(this MarketCandle entity)
        {
            if (entity is null) return null!;

            return new MarketCandleDTO
            {
                InstrumentId = entity.InstrumentId,
                IntervalCode = entity.IntervalCode,
                OpenTimeUtc = entity.OpenTimeUtc,
                OpenPrice = entity.OpenPrice,
                HighPrice = entity.HighPrice,
                LowPrice = entity.LowPrice,
                ClosePrice = entity.ClosePrice,
                Volume = entity.Volume,
                VWAP = entity.VWAP,
                TradeCount = entity.TradeCount,
                IsFinal = entity.IsFinal
            };
        }

        public static MarketQuote MapToMarketQuote(this MarketQuoteDTO dto)
        {
            return new MarketQuote
            {
                Id = dto.Id,
                InstrumentId = dto.InstrumentId,
                ProviderId = dto.ProviderId,
                TimestampUtc = dto.TimestampUtc,
                ReceivedAtUtc = dto.ReceivedAtUtc,
                BidPrice = dto.BidPrice,
                BidSize = dto.BidSize,
                AskPrice = dto.AskPrice,
                AskSize = dto.AskSize
            };
        }

        public static MarketQuoteDTO MapToMarketQuoteDTO(this MarketQuote entity)
        {
            if (entity is null) return null!;

            return new MarketQuoteDTO
            {
                Id = entity.Id,
                InstrumentId = entity.InstrumentId,
                ProviderId = entity.ProviderId,
                TimestampUtc = entity.TimestampUtc,
                ReceivedAtUtc = entity.ReceivedAtUtc,
                BidPrice = entity.BidPrice,
                BidSize = entity.BidSize,
                AskPrice = entity.AskPrice,
                AskSize = entity.AskSize
            };
        }

        public static MarketSnapshot MapToMarketSnapshot(this MarketSnapshotDTO dto)
        {
            return new MarketSnapshot
            {
                InstrumentId = dto.InstrumentId,
                LastPrice = dto.LastPrice,
                BidPrice = dto.BidPrice,
                AskPrice = dto.AskPrice,
                OpenPrice = dto.OpenPrice,
                HighPrice = dto.HighPrice,
                LowPrice = dto.LowPrice,
                PreviousClose = dto.PreviousClose,
                Volume = dto.Volume,
                LastProviderId = dto.LastProviderId,
                MarketTimestampUtc = dto.MarketTimestampUtc,
                ReceivedAtUtc = dto.ReceivedAtUtc
            };
        }

        public static MarketSnapshotDTO MapToMarketSnapshotDTO(this MarketSnapshot entity)
        {
            if (entity is null) return null!;

            return new MarketSnapshotDTO
            {
                InstrumentId = entity.InstrumentId,
                LastPrice = entity.LastPrice,
                BidPrice = entity.BidPrice,
                AskPrice = entity.AskPrice,
                OpenPrice = entity.OpenPrice,
                HighPrice = entity.HighPrice,
                LowPrice = entity.LowPrice,
                PreviousClose = entity.PreviousClose,
                Volume = entity.Volume,
                LastProviderId = entity.LastProviderId,
                MarketTimestampUtc = entity.MarketTimestampUtc,
                ReceivedAtUtc = entity.ReceivedAtUtc
            };
        }

        public static MarketTrade MapToMarketTrade(this MarketTradeDTO dto)
        {
            return new MarketTrade
            {
                Id = dto.Id,
                InstrumentId = dto.InstrumentId,
                ProviderId = dto.ProviderId,
                TimestampUtc = dto.TimestampUtc,
                ReceivedAtUtc = dto.ReceivedAtUtc,
                Price = dto.Price,
                Quantity = dto.Quantity,
                SequenceNumber = dto.SequenceNumber
            };
        }

        public static MarketTradeDTO MapToMarketTradeDTO(this MarketTrade entity)
        {
            if (entity is null) return null!;

            return new MarketTradeDTO
            {
                Id = entity.Id,
                InstrumentId = entity.InstrumentId,
                ProviderId = entity.ProviderId,
                TimestampUtc = entity.TimestampUtc,
                ReceivedAtUtc = entity.ReceivedAtUtc,
                Price = entity.Price,
                Quantity = entity.Quantity,
                SequenceNumber = entity.SequenceNumber
            };
        }

        // Entity -> DTO (UserMessage -> ClientMessageDTO)
        public static ClientMessageDTO MapToClientMessageDTO(this UserMessage m)
        {
            if (m is null) return null!;

            return new ClientMessageDTO
            {
                Id = m.Id,
                UserId = m.UserId,
                SourceType = m.SourceType,
                SourceId = m.SourceId,
                RelatedName = m.RelatedName,
                Latitude = m.Latitude.HasValue ? (double?)m.Latitude.Value : null,
                Longitude = m.Longitude.HasValue ? (double?)m.Longitude.Value : null,
                Tags = m.Tags,
                Content = m.Content ?? string.Empty,
                CreatedAt = m.CreatedAt
            };
        }

        // Optional: collection helper
        public static List<ClientMessageDTO> MapToClientMessageDTOs(this IEnumerable<UserMessage> items)
            => items?.Select(x => x.MapToClientMessageDTO()).ToList() ?? new List<ClientMessageDTO>();

        public static MessageTriageResult MapToMessageTriageResult(this MessageTriageResultDTO dto)
        {
            return new MessageTriageResult
            {
                RequiresAdminReview = dto.RequiresAdminReview,
                Category = dto.Category,
                Priority = dto.Priority,
                Confidence = dto.Confidence,
                ClassificationSource = dto.ClassificationSource
            };
        }

        public static MessageTriageResultDTO MapToMessageTriageResultDTO(this MessageTriageResult entity)
        {
            if (entity is null) return null!;

            return new MessageTriageResultDTO
            {
                RequiresAdminReview = entity.RequiresAdminReview,
                Category = entity.Category,
                Priority = entity.Priority,
                Confidence = entity.Confidence,
                ClassificationSource = entity.ClassificationSource
            };
        }

        public static ProfanityWord MapToProfanityWord(this ProfanityWordDTO dto)
        {
            return new ProfanityWord
            {
                Id = dto.Id,
                Word = dto.Word,
                NormalizedWord = dto.NormalizedWord,
                LanguageCode = dto.LanguageCode,
                Weight = dto.Weight,
                IsRegex = dto.IsRegex,
                Category = dto.Category,
                CreatedAtUtc = dto.CreatedAtUtc,
                UpdatedAtUtc = dto.UpdatedAtUtc
            };
        }

        public static ProfanityWordDTO MapToProfanityWordDTO(this ProfanityWord entity)
        {
            if (entity is null) return null!;

            return new ProfanityWordDTO
            {
                Id = entity.Id,
                Word = entity.Word,
                NormalizedWord = entity.NormalizedWord,
                LanguageCode = entity.LanguageCode,
                Weight = entity.Weight,
                IsRegex = entity.IsRegex,
                Category = entity.Category,
                CreatedAtUtc = entity.CreatedAtUtc,
                UpdatedAtUtc = entity.UpdatedAtUtc
            };
        }

        public static Provider MapToProvider(this ProviderDTO dto)
        {
            return new Provider
            {
                Id = dto.Id,
                Code = dto.Code,
                Name = dto.Name,
                CreatedAtUtc = dto.CreatedAtUtc
            };
        }

        public static ProviderDTO MapToProviderDTO(this Provider entity)
        {
            if (entity is null) return null!;

            return new ProviderDTO
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                CreatedAtUtc = entity.CreatedAtUtc
            };
        }

        public static ProviderInstrument MapToProviderInstrument(this ProviderInstrumentDTO dto)
        {
            return new ProviderInstrument
            {
                ProviderId = dto.ProviderId,
                InstrumentId = dto.InstrumentId,
                ProviderSymbol = dto.ProviderSymbol,
                Realtime = dto.Realtime,
                DelaySeconds = dto.DelaySeconds
            };
        }

        public static ProviderInstrumentDTO MapToProviderInstrumentDTO(this ProviderInstrument entity)
        {
            if (entity is null) return null!;

            return new ProviderInstrumentDTO
            {
                ProviderId = entity.ProviderId,
                InstrumentId = entity.InstrumentId,
                ProviderSymbol = entity.ProviderSymbol,
                Realtime = entity.Realtime,
                DelaySeconds = entity.DelaySeconds
            };
        }

        public static TechnicalIndicator MapToTechnicalIndicator(this TechnicalIndicatorDTO dto)
        {
            return new TechnicalIndicator
            {
                InstrumentId = dto.InstrumentId,
                IntervalCode = dto.IntervalCode,
                TimestampUtc = dto.TimestampUtc,
                IndicatorType = dto.IndicatorType,
                Value1 = dto.Value1,
                Value2 = dto.Value2,
                Value3 = dto.Value3,
                ParameterHash = dto.ParameterHash
            };
        }

        public static TechnicalIndicatorDTO MapToTechnicalIndicatorDTO(this TechnicalIndicator entity)
        {
            if (entity is null) return null!;

            return new TechnicalIndicatorDTO
            {
                InstrumentId = entity.InstrumentId,
                IntervalCode = entity.IntervalCode,
                TimestampUtc = entity.TimestampUtc,
                IndicatorType = entity.IndicatorType,
                Value1 = entity.Value1,
                Value2 = entity.Value2,
                Value3 = entity.Value3,
                ParameterHash = entity.ParameterHash
            };
        }

        public static UserMessage MapToUserMessage(this UserMessageDTO dto)
        {
            return new UserMessage
            {
                Id = dto.Id,
                UserId = dto.UserId,
                SourceType = dto.SourceType,
                SourceId = dto.SourceId,
                RelatedName = dto.RelatedName,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Tags = dto.Tags,
                Content = dto.Content,
                CreatedAt = dto.CreatedAt,
            };
        }

        public static UserMessageDTO MapToUserMessageDTO(this UserMessage entity)
        {
            if (entity is null) return null!;

            return new UserMessageDTO
            {
                Id = entity.Id,
                UserId = entity.UserId,
                SourceType = entity.SourceType,
                SourceId = entity.SourceId,
                RelatedName = entity.RelatedName,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Tags = entity.Tags,
                Content = entity.Content,
                CreatedAt = entity.CreatedAt,
            };
        }

        public static UserMessageAdminQueue MapToUsermessageAdminQueue(this UserMessageAdminQueueDTO dto)
        {
            return new UserMessageAdminQueue
            {
                Id = dto.Id,
                MessageId = dto.MessageId,
                Category = dto.Category,
                Priority = dto.Priority,
                Status = dto.Status,
                Confidence = dto.Confidence,
                ClassificationSource = dto.ClassificationSource,
                AssignedTo = dto.AssignedTo,
                AdminNote = dto.AdminNote,
                CreatedAtUtc = dto.CreatedAtUtc,
                UpdatedAtUtc = dto.UpdatedAtUtc,
                ResolvedAtUtc = dto.ResolvedAtUtc
            };
        }

        public static UserMessageAdminQueueDTO MapToUsermessageAdminQueueDTO(this UserMessageAdminQueue entity)
        {
            return new UserMessageAdminQueueDTO
            {
                Id = entity.Id,
                MessageId = entity.MessageId,
                Category = entity.Category,
                Priority = entity.Priority,
                Status = entity.Status,
                Confidence = entity.Confidence,
                ClassificationSource = entity.ClassificationSource,
                AssignedTo = entity.AssignedTo,
                AdminNote = entity.AdminNote,
                CreatedAtUtc = entity.CreatedAtUtc,
                UpdatedAtUtc = entity.UpdatedAtUtc,
                ResolvedAtUtc = entity.ResolvedAtUtc
            };
        }

        public static Users MapToUsers(this UserDTO dto)
        {
            if (dto is null)
                return null!;

            var user = new Users(
                dto.Id,
                dto.Email,
                dto.PasswordHashV2,
                dto.SecurityStamp,
                dto.Role,
                dto.Status);

            if (dto.Active)
                user.Activate();
            else
                user.Deactivate();

            return user;
        }

        public static UserDTO MapToUserDTO(this Users entity)
        {
            if (entity is null)
                return null!;

            return new UserDTO
            {
                Id = entity.Id,
                Email = entity.Email,
                PasswordHashV2 = entity.PasswordHashV2,
                SecurityStamp = entity.SecurityStamp,
                Role = entity.Role,
                Status = entity.Status,
                Active = entity.Active
            };
        }

        public static UserSessions MapToUserSessions(this UserSessionsDTO dto)
        {
            return new UserSessions
            {
                Id = dto.Id,
                UserEmail = dto.UserEmail,
                Jti = dto.Jti,
                RefreshFamilyId = dto.RefreshFamilyId,
                IssueAtUtc = dto.IssueAtUtc,
                ExpiresAtUtc = dto.ExpiresAtUtc,
                LastSeenUtc = dto.LastSeenUtc,
                Source = dto.Source,
                Ip = dto.Ip,
                UserAgent = dto.UserAgent
            };
        }

        public static UserSessionsDTO MapToUserSessionsDTO(this UserSessions entity)
        {
            if (entity is null) return null!;

            return new UserSessionsDTO
            {
                Id = entity.Id,
                UserEmail = entity.UserEmail,
                Jti = entity.Jti,
                RefreshFamilyId = entity.RefreshFamilyId,
                IssueAtUtc = entity.IssueAtUtc,
                LastSeenUtc = entity.LastSeenUtc,
                Source = entity.Source,
                Ip = entity.Ip,
                UserAgent = entity.UserAgent
            };
        }

        // GPTInteraction → DTO
        public static MistralInteractionDTO MapToMistralInteractionDTO(this MistralInteraction entity)
        {
            if (entity is null)
                return null!;

            return new MistralInteractionDTO
            {
                Id = entity.Id,
                Prompt = entity.Prompt ?? string.Empty,
                Response = entity.Response ?? string.Empty,
                PromptHash = entity.PromptHash ?? string.Empty,
                CreatedAt = entity.CreatedAt,
                Active = entity.Active,

                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                SourceType = entity.SourceType,
                ExecutionSource = entity.ExecutionSource,
                Status = entity.Status
            };
        }

        public static MistralInteraction MapToMistralInteraction(this MistralInteractionDTO dto)
        {
            if (dto is null)
                return null!;

            return new MistralInteraction
            {
                Id = dto.Id,
                Prompt = dto.Prompt,
                Response = dto.Response,
                PromptHash = string.IsNullOrWhiteSpace(dto.PromptHash) ? null! : dto.PromptHash,
                CreatedAt = dto.CreatedAt == default ? DateTime.UtcNow : dto.CreatedAt,
                Active = dto.Active,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                SourceType = dto.SourceType
            };
        }


        // DTO -> Entity partial update)
        public static MistralInteraction UpdateFrom(this MistralInteraction entity, MistralInteractionDTO dto)
        {
            if (entity is null || dto is null)
                return entity!;

            entity.Prompt = dto.Prompt;
            entity.Response = dto.Response;
            if (!string.IsNullOrWhiteSpace(dto.PromptHash))
            {
                entity.PromptHash = dto.PromptHash;
            }
            entity.Active = dto.Active;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;
            entity.SourceType = dto.SourceType;

            return entity;
        }
        // Place → DTOSugges
        public static PlaceDTO MapToPlaceDTO(this Place entity)
        {
            return new PlaceDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Type = entity.Type,
                Indoor = entity.Indoor,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Capacity = entity.Capacity,
                Tag = entity.Tag,
            };
        }
        public static Place MapToPlace(this PlaceDTO dto)
        {
            return new Place
            {
                Id = dto.Id,
                Name = dto.Name,
                Type = dto.Type,
                Indoor = dto.Indoor,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Capacity = dto.Capacity,
                Tag = dto.Tag
            };
        }
        public static PlaceDTO MapToPlaceWithLatitude(this PlaceDTO dto)
        {
            return new PlaceDTO
            {
                Id = dto.Id,
                Name = dto.Name,
                Type = dto.Type,
                Indoor = dto.Indoor,
                Latitude = Math.Round(dto.Latitude, 2),
                Longitude = Math.Round(dto.Longitude, 3),
                Capacity = dto.Capacity,
                Tag = dto.Tag
            };
        }
    }
}

























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.