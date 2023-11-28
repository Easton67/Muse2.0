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
        bool InsertUser(User user, string password);
        UserVM LoginUser(string email, string password);
        // public helper methods
        UserVM GetUserVMByEmail(string email);
        List<string> GetRolesByUserID(int employeeID);
        List<User> SelectAllUsers();
        string HashSha256(string source);
        bool AuthenticateUser(string email, string password);
        bool ResetPassword(string email, string oldPassword, string newPassword);
        bool UpdateUser(User oldUser, User newUser);

        // DEAD
        bool UpdateFirstName(string Email, string FirstName);
        bool UpdateLastName(string email, string LastName);
        bool UpdateAccountImage(string email, string accountImage);
    }
}
