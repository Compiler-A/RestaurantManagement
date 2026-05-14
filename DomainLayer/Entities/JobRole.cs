
namespace DomainLayer.Entities
{
    public class JobRole
    {
        public int JobRoleID { get; set; }
        public string JobName { get; set; } = null!; 
        public string? Description { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
