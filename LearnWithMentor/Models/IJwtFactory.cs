using System.Security.Claims;

namespace LearnWithMentor.Models
{
    public interface IJwtFactory
    {
        ClaimsIdentity GenerateClaimsIdentity(string userName);
    }
}
