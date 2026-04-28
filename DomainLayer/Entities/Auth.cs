
namespace DomainLayer.Entities
{
    public class Auth
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        public Employee? Employees { get; set; }
        public string RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }
    }
}
