namespace EconomicTrendsPolLESSIRE.Application.Interfaces
{
    public interface ITechnicalIndicatorCalculator
    {
        Task RefreshAsync(long instrumentId, CancellationToken ct = default);
    }
}
