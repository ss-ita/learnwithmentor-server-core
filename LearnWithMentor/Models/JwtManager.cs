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
                        }, "Token"),

                Expires = now.AddDays(expireDays).AddHours(expireHours),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if ((expires != null) && (DateTime.UtcNow < expires))
            {
                return true;
            }
            return false;
        }

        public static bool ValidateToken(string token, out string email, out string userrole)
        {
            email = null;
            userrole = null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return false;

            var symmetricKey = Convert.FromBase64String(Secret);

            var validationParameters = new TokenValidationParameters()
            {
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                LifetimeValidator = LifetimeValidator,
                IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

            var identity = principal?.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return false;
            }

            if (!identity.IsAuthenticated)
            {
                return false;
            }

            var userEmailClaim = identity.FindFirst(ClaimTypes.Email);
            email = userEmailClaim?.Value;
            var userRoleClaim = identity.FindFirst(ClaimTypes.Role);
            userrole = userRoleClaim?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userRoleClaim?.Value))
            {
                return false;
            }
            return true;
        }
    }
}
