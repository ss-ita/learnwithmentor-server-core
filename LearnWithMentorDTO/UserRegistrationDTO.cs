
namespace LearnWithMentorDTO
{
    public class UserRegistrationDTO
    {
        public UserRegistrationDTO(string email, string lastName, string firstName, string password)
        {
            Email = email;
            LastName = lastName;
            FirstName = firstName;
            Password = password;
        }

        public string Email { set; get; }
        public string LastName { set; get; }
        public string FirstName { set; get; }
        public string Password { get; set; }
    }
}
