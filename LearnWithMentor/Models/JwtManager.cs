using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LearnWithMentorDTO;
using Microsoft.IdentityModel.Tokens;

namespace LearnWithMentor.Models
{
    public static class JwtManager
    {
        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        public static string GenerateToken(UserIdentityDTO user, int expireDays = 1, int expireHours = 0)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("Id",user.Id.ToString()),
                            new Claim("EmailConfirmed",user.EmailConfirmed.ToString()),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName ),
                            new Claim(ClaimTypes.Role, user.Role)
                        }),

                Expires = now.AddDays(expireDays).AddHours(expireHours),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
