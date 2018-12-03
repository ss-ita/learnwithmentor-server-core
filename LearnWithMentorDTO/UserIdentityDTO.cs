
namespace LearnWithMentorDTO
{
    public class UserIdentityDTO
    {
        public UserIdentityDTO(
            string email, 
            string password, 
            int id, 
            string firstName, 
            string lastName, 
            string role, 
            bool blocked, 
            bool emailConfirmed)
        {
            Email = email;
            Password = password;
            LastName = lastName;
            FirstName = firstName;
            Id = id;
            Role = role;
            Blocked = blocked;
            EmailConfirmed = emailConfirmed;
        }

        public string Email { set; get; }
        public string LastName { set; get; }
        public string FirstName { set; get; }
        public int Id { set; get; }
        public string Role { set; get; }
        public bool? Blocked { set; get; }
        public string Password { get; set; }
        public bool EmailConfirmed { set; get; }
    }
}
