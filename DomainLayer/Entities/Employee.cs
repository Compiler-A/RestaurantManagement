
namespace DomainLayer.Entities
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; } = null!;
        public int JobRoleID { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public JobRole JobRole { get; set; } = null!;

    }
}
