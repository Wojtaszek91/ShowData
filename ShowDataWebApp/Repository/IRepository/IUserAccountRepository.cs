using ShowDataWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowDataWebApp.Repository.IRepository
{
    public interface IUserAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User user);
        Task<bool> RegisterAsync(string url, User user);
        Task<IEnumerable<string>> GetUsernames(string url, string token);
    }
}
