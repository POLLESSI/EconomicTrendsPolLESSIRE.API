using EconomicTrendsPolLESSIRE.Application.Extensions;
using EconomicTrendsPolLESSIRE.Contracts.DTOs;
using EconomicTrendsPolLESSIRE.Contracts.Enums;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using System.Runtime.CompilerServices;
//using EconomicTrendsPolLESSIRE.Domain.ValueObjects;
//using EconomicTrendsPolLESSIRE.DTOs.DTOs;


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

        //public static MessageTriageResult MapToMessageTriageResult(this MessageTriageResultDTO dto)
        //{

        //}

        //public static MessageTriageResultDTO MapToMessageTriageResultDTO(this MessageTriageResult entity)
        //{
            //if (entity is null) return null!;
        //}

        //public static ProfanityWord MapToProfanityWord(this ProfanityWordDTO dto)
        //{

        //}

        //public static ProfanityWordDTO MapToProfanityWordDTO(this ProfanityWord entity)
        //{
            //if (entity is null) return null!;
        //}

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

        //public static UserMessageAdminQueue MapToUsermessageAdminQueue(this UserMessageAdminQueDTO dto)
        //{

        //}

        //public static UserMessageAdminQueueDTO MapToUsermessageAdminQueueDTO(this UserMessageAdminQueue entity)
        //{

        //}

        public static Users MapToUsers(this UserDTO dto)
        {
            return new Users
            {
                Id = dto.Id,
                Email = dto.Email,
                PasswordHachV2 = dto.PasswordHachV2,
                SecurityStamp = dto.SecurityStamp,
                Role = dto.Role,
                Status = dto.Status
            };
        }

        public static UserDTO MapToUserDTO(this Users entity)
        {
            if (entity is null) return null!;

            return new UserDTO
            {
                Id = entity.Id,
                Email = entity.Email,
                PasswordHachV2 = entity.PasswordHachV2,
                SecurityStamp = entity.SecurityStamp,
                Role = entity.Role,
                Status = entity.Status
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
    }
}
