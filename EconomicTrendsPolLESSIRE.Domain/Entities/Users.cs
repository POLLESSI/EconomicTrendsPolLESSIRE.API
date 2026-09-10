namespace EconomicTrendsPolLESSIRE.Domain.Entities
{
    public class Users
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHachV2 { get; set; }
        public Guid SecurityStamp { get; set; }
        public int Role { get; set; }
        public int Status { get; set; }
        public bool Active { get; set; }
    }
}
