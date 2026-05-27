
namespace DomainLayer.Entities
{
    public class Person
    {
        public int PersonID { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public char Gender { get; set; } = 'M';
        public DateTime DateOfBirth { get; set; }
        public string? ProfileImage { get; set; }
    }
}
