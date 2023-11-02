namespace Tw_Clone.Models.User.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string? Email { get; set; }

        public string? Biography { get; set; }

        public DateTime? FhNac { get; set; }

        public int? Num_Followers { get; set; }

        public int? Num_Following { get; set; }




    }
}
