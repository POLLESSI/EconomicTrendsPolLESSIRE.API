namespace EconomicTrendsPolLESSIRE.Shared.Interfaces
{
    public interface IDeviceHasher
    {
        byte[] ComputeHash(string rawIdentifier);
        string ComputeHashBase64(string rawIdentifier); // optionally
    }
}



































































// Copyrigtht (c) EconomicTrendsPolLESSIRE https://github.com/POLLESSI/EconomicTrendsPolLESSIRE. All rights reserved.