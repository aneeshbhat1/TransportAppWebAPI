using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BusinessInterfaces
{
    public interface IUserServices
    {
        int AuthenticateUser(string userName, string password);
        UserModel GetUserDetails(string userName);
        string RegisterUserDetails(UserModel user);
    }
}
