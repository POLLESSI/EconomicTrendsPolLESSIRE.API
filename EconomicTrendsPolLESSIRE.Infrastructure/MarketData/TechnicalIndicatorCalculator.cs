using EconomicTrendsPolLESSIRE.Application.Interfaces;

namespace EconomicTrendsPolLESSIRE.Infrastructure.MarketData
{
    public sealed class TechnicalIndicatorCalculator : ITechnicalIndicatorCalculator
    {
        public Task RefreshAsync(long instrumentId, CancellationToken ct = default)
        {
            return Task.CompletedTask;
        }
    }
}



























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.