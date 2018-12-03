
namespace LearnWithMentorDTO
{
    public class UserWithImageDTO
    {
        public UserWithImageDTO(
            string email, 
            int id, 
            string firstName, 
            string lastName, 
            string role, 
            bool blocked, 
            ImageDTO image)
        {
            Email = email;
            LastName = lastName;
            FirstName = firstName;
            Id = id;
            Role = role;
            Blocked = blocked;
            Image = image;
        }

        public string Email { set; get; }
        public string LastName { set; get; }
        public string FirstName { set; get; }
        public int Id { set; get; }
        public string Role { set; get; }
        public bool? Blocked { set; get; }
        public ImageDTO Image { set; get; }
    }
}
