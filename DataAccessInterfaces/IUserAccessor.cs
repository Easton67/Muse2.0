using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IUserAccessor
    {
        int AuthenticateUserWithEmailAndPasswordHash(string email, string PasswordHash);
        int InsertUser(User user, string password);
        UserVM SelectUserVMByEmail(string email);
        List<string> SelectRolesByUserID(int UserID);
        List<User> SelectAllUsers();
        int UpdateUser(User oldUser, User newUser);
        int UpdateMinutesListened(int userID, int newMinutesListened);
        int UpdatePasswordHash(string email, string oldPasswordHash, string newPasswordHash);
        int UpdateActiveUser(int userID, bool oldActive, bool newActive);
        UserPass SelectPasswordHashByEmail(string email);
        List<UserFriend> SelectFriendsByUserID(int UserID);
        List<string> SelectAllRoles();
    }
}
