using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SellingManagementSystem.Common;
using SellingManagementSystem.Models;
using SellingManagementSystem.Tools;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SellingManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public string Auth(User credenciales)
        {
            using (SellingDBContext db = new SellingDBContext())
            {
                string encriptPassword = Encrypt.GetSHA256(credenciales.Password);

                User user = db.Users.Where(user =>
                    user.Email == credenciales.Email
                    && user.Password == encriptPassword
                    ).FirstOrDefault();

                if (user == null)
                {
                    throw new Exception("User o password invalid");
                }

                // create token
                string token = GetToken(user);

                return token;
            }
        }

        private string GetToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email)

                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
