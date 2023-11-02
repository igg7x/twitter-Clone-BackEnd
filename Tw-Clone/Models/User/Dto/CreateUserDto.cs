namespace Tw_Clone.Models.User.Dto
{
    public class CreateUserDto
    {
        public string Username { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Biography { get; set; }

        public string Password { get; set; } = null!;

        public DateTime? FhNac { get; set; }
    }
}
