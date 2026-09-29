namespace EconomicTrendsPolLESSIRE.Application.MarketData
{
    public sealed record ProviderCatalogItem(
        string Symbol,
        string Name,
        int AssetClass,
        string? ExchangeCode,
        string? CurrencyCode,
        string ProviderSymbol,
        bool Realtime,
        int? DelaySeconds);
}



























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.