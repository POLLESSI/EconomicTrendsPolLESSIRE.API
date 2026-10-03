using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Domain.Entities;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace EconomicTrendsPolLESSIRE.Infrastructure.MarketData
{
    public sealed class TechnicalIndicatorCalculator : ITechnicalIndicatorCalculator
    {
        private const int DefaultIntervalCode = 1;

        // Temporary agreement for our validation.
        private const int SmaIndicatorType = 1;
        private const int SmaPeriod = 3;

        private readonly IMarketCandleRepository _candleRepository;
        private readonly ITechnicalIndicatorRepository _indicatorRepository;
        private readonly ILogger<TechnicalIndicatorCalculator> _logger;

        public TechnicalIndicatorCalculator(IMarketCandleRepository candleRepository, ITechnicalIndicatorRepository indicatorRepository, ILogger<TechnicalIndicatorCalculator> logger)
        {
            _candleRepository = candleRepository;
            _indicatorRepository = indicatorRepository;
            _logger = logger;
        }

        public async Task RefreshAsync(long instrumentId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var candles =
                await _candleRepository
                    .GetRecentByInstrumentAsync(
                        instrumentId,
                        DefaultIntervalCode,
                        SmaPeriod,
                        ct);

            if (candles.Count < SmaPeriod)
            {
                _logger.LogDebug(
                    "Not enough candles to calculate SMA{Period}. " +
                    "InstrumentId={InstrumentId}, CandleCount={CandleCount}.",
                    SmaPeriod,
                    instrumentId,
                    candles.Count);

                return;
            }

            var latestCandle = candles[0];

            var sma = candles.Take(SmaPeriod).Average(x => x.ClosePrice);

            var indicator =
                new TechnicalIndicator
                {
                    InstrumentId = instrumentId,
                    IntervalCode = DefaultIntervalCode,
                    TimestampUtc = latestCandle.OpenTimeUtc,
                    IndicatorType = SmaIndicatorType,
                    Value1 = sma,
                    Value2 = 0m,
                    Value3 = 0m,

                    // For this initial validation :
                    // 3 represents the SMA3 period.
                    ParameterHash = CreateParameterHash("SMA", SmaPeriod),
                    Active = true
                };

            await _indicatorRepository.UpsertAsync(indicator, ct);

            _logger.LogInformation(
                "Technical indicator calculated. " +
                "InstrumentId={InstrumentId}, " +
                "Indicator=SMA{Period}, " +
                "TimestampUtc={TimestampUtc}, " +
                "Value={Value}",
                instrumentId,
                SmaPeriod,
                indicator.TimestampUtc,
                sma);
        }

        private static byte[] CreateParameterHash(string indicatorName, int period)
        {
            var value = $"{indicatorName}:{period}";

            return MD5.HashData(Encoding.UTF8.GetBytes(value));
        }
    }
}


























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.