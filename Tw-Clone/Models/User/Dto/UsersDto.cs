namespace Tw_Clone.Models.User.Dto
{
    public class UsersDto
    {

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string? Biography { get; set; }

        public int? Num_Followers { get; set; }

        public int? Num_Following { get; set; }


    }
}
