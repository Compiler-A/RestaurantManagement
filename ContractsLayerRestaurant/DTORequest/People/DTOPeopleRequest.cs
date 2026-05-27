

using System.ComponentModel.DataAnnotations;

namespace ContractsLayerRestaurant.DTORequest.People
{
    public class DTOPeopleRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "PersonID must be greater than 0")]
        public int PersonID { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required")]
        public char Gender { get; set; } = 'M';

        [Required(ErrorMessage = "DateOfBirth is required")]
        public DateTime DateOfBirth { get; set; }

        public string? ProfileImage { get; set; }
    }
}
