using System.Security.Claims;
using System.Security.Principal;

namespace LearnWithMentor.Models
{
    public class JwtFactory : IJwtFactory
    {
        public ClaimsIdentity GenerateClaimsIdentity(string userName)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"));
        }
    }
}
