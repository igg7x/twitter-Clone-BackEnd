using System.ComponentModel.DataAnnotations;

namespace Tw_Clone.Dto.User
{
    public class CreateUserDto
    {

        [Required]
        [MaxLength(128)]
        public string Username { get; set; } = null!;
        [Required]
        [MaxLength(40)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(40)]
        public string LastName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [MaxLength(128)]
        public string? Biography { get; set; }
        [Required]
        public string Image { get; set; } = null!; 

    }
}
