using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShowData.Data;
using ShowData.Model;
using ShowData.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShowData.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;

        public UserRepository(ApplicationDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        public User AuthenticateUser(string userName, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == userName && u.Password == password);
            if(user != null)
            {
                var tokenHanlder = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptior = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHanlder.CreateToken(tokenDescriptior);
                user.Token = tokenHanlder.WriteToken(token);
                user.Password = "";

                return user;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if user name is already taken.
        /// </summary>
        /// <param name="userName">String user name to check.</param>
        /// <returns>Bool depended if user name is taken or not.</returns>
        public bool IsUserNameTaken(string userName)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == userName);
            if (user != null)
                return false;
            else
                return true;
        }

        public User RegisterUser(string userName, string password)
        {
            User userToSave = new User()
            {
                Username = userName,
                Password = password,
                Role = "User"                
            };

            _context.Users.Add(userToSave);
            _context.SaveChanges();
            userToSave.Password = "";

            return userToSave;

        }
    }
}
