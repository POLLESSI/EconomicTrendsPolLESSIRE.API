namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public class Provider
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public bool Active { get; set; } 
    }
}
