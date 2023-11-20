using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IUserManager
    {
        UserVM LoginUser(string email, string password);
        // public helper methods
        string HashSha256(string source);
        bool AuthenticateUser(string email, string password);
        UserVM GetUserVMByEmail(string email);
        List<string> GetRolesByUserID(int employeeID);
        bool InsertUser(User user, string password);
        bool ResetPassword(string email, string oldPassword, string newPassword);
        bool UpdateFirstName(string Email, string FirstName);
        bool UpdateLastName(string email, string LastName);
        bool UpdateAccountImage(string email, string accountImage);
    }
}
