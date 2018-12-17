
namespace LearnWithMentorDTO
{
    public class UserLoginDTO
    {
        public UserLoginDTO(string email,  string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { set; get; }
        public string Password { get; set; }
    }
}
