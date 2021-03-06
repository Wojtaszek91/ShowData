﻿using Microsoft.Extensions.Options;
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
                var secret = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptior = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(secret),
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
                return true;
            else
                return false;
        }

        public User RegisterUser(string userName, string password)
        {
            User newUser = new User()
            {
                Username = userName,
                Password = password,
                Role = "Admin"                
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
            newUser.Password = "";

            return newUser;
        }

        /// <summary>
        /// Gets all usernames sorted alphabeticaly
        /// </summary>
        public ICollection<string> GetUsernames()
        {
            var usersList = _context.Users.OrderBy(a => a.Username).ToList();
            List<string> userNames = new List<string>();
            foreach(var user in usersList)
            {
                userNames.Add(user.Username);
            }

            return userNames;
        }
    }
}
