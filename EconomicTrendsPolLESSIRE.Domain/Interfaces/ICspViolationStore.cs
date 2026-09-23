using EconomicTrendsPolLESSIRE.Shared.Security;


namespace EconomicTrendsPolLESSIRE.Domain.Interfaces
{
    public interface ICspViolationStore
    {
        void Add(CspReportContent report);
        IEnumerable<CspReportContent> GetAll();
        void Clear();
    }
}





















































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.