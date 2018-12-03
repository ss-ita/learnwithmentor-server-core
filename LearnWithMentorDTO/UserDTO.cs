
namespace LearnWithMentorDTO
{
    public class UserDTO
    {
        public UserDTO(
            int id, 
            string firstName, 
            string lastName, 
            string email, 
            string role, 
            bool blocked, 
            bool emailConfirmed)
        {
            LastName = lastName;
            FirstName = firstName;
            Id = id;
            Email = email;
            Role = role;
            Blocked = blocked;
            EmailConfirmed = emailConfirmed;
        }

        public string LastName { set; get; }
        public string FirstName { set; get; }
        public int Id { set; get; }
        public string Email { set; get; }
        public string Role { set; get; }
        public bool? Blocked { set; get; }
        public bool EmailConfirmed { set; get; }
    }
}
