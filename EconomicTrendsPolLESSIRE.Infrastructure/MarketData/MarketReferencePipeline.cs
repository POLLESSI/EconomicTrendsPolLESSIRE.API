using EconomicTrendsPolLESSIRE.Application.Interfaces;
using EconomicTrendsPolLESSIRE.Application.MarketData;
using EconomicTrendsPolLESSIRE.Domain.Interfaces;

namespace EconomicTrendsPolLESSIRE.Infrastructure.MarketData
{
    public sealed class MarketReferencePipeline : IMarketReferencePipeline
    {
        private readonly IMarketDataSource _source;

        private readonly IProviderRepository _providerRepository;
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IProviderInstrumentRepository
            _providerInstrumentRepository;

        public MarketReferencePipeline(IMarketDataSource source, IProviderRepository providerRepository, IInstrumentRepository instrumentRepository, IProviderInstrumentRepository providerInstrumentRepository)
        {
            _source = source;

            _providerRepository = providerRepository;
            _instrumentRepository = instrumentRepository;
            _providerInstrumentRepository = providerInstrumentRepository;
        }

        public async Task SynchronizeAsync(CancellationToken ct = default)
        {
            /*
             * 1. Ensure Provider exists.
             */
            var provider = await _providerRepository.GetOrCreateByCodeAsync(_source.Code, ct);
            var catalog = await _source.GetCatalogAsync(ct);

            foreach (var item in catalog)
            {
                /*
                 * 2. Canonical instrument.
                 */
                var instrument =
                    await _instrumentRepository
                        .UpsertByNaturalKeyAsync(
                            item.Symbol,
                            item.Name,
                            item.AssetClass,
                            item.ExchangeCode,
                            item.CurrencyCode,
                            ct);

                /*
                 * 3. Mapping provider -> canonical instrument.
                 */
                await _providerInstrumentRepository
                    .UpsertAsync(
                        provider.Id,
                        instrument.Id,
                        item.ProviderSymbol,
                        item.Realtime,
                        item.DelaySeconds,
                        ct);
            }
        }
    }
}
























































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.