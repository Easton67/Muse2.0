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
        bool FindUser(string email);
        bool InsertUser(User user, string password);
        UserVM LoginUser(string email, string password);
        UserVM GetUserVMByEmail(string email);
        List<string> GetRolesByUserID(int employeeID);
        List<User> SelectAllUsers();
        string HashSha256(string source);
        bool AuthenticateUser(string email, string password);
        bool ResetPassword(string email, string oldPassword, string newPassword);
        bool UpdateUser(User oldUser, User newUser);
        bool UpdateMinutesListened(int userID, int newMinutesListened);
        bool UpdateActiveUser(int userID, bool oldActive, bool newActive);
        UserPass ResetPassword(string userID);
        List<UserFriend> SelectFriendsByUserID(int UserID);
        List<string> SelectAllRoles();
        bool AddUserRole(int userID, string role);
        bool DeleteUserRole(int userID, string role);
        int RetrieveUserIDFromEmail(string email);
    }
}
