using ShowData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowData.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUserNameTaken(string userName);
        User RegisterUser(string userName, string password);
        User AuthenticateUser(string userName, string password);
        
    }
}
