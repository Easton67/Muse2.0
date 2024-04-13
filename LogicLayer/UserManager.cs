using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class UserManager : IUserManager
    {
        private IUserAccessor _userAccessor = null;
        public UserManager()
        {
            _userAccessor = new UserAccessor();
        }
        public UserManager(IUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }
        public bool AuthenticateUser(string email, string password)
        {
            bool result = false;
            password = HashSha256(password);
            result = (1 == _userAccessor.AuthenticateUserWithEmailAndPasswordHash(email, password));
            return result;
        }
        public UserVM GetUserVMByEmail(string email)
        {
            UserVM userVM = null;

            try
            {
                userVM = _userAccessor.SelectUserVMByEmail(email);
                userVM.Roles = _userAccessor.SelectRolesByUserID(userVM.UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("User not found", ex);
            }
            return userVM;
       }
        public UserPass SelectPasswordHashByEmail(string email)
        {
            UserPass userPass = null;

            try
            {
                userPass = _userAccessor.SelectPasswordHashByEmail(email);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to find your old password. ", ex);
            }
            return userPass;
        }
        public List<string> GetRolesByUserID(int UserID)
        {
            List<string> roles = new List<string>();

            try
            {
                roles = _userAccessor.SelectRolesByUserID(UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Roles not found", ex);
            }

            return roles;
        }
        public List<string> SelectAllRoles()
        {
            List<string> roles = new List<string>();

            try
            {
                roles = _userAccessor.SelectAllRoles();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Roles not found", ex);
            }

            return roles;
        }
        public string HashSha256(string source)
        {
            string hashValue = "";

            // hash functions run at the bits and bytes level
            // so we create a byte array
            byte[] data;

            // create a .NET hash provider object
            using (SHA256 sha256hasher = SHA256.Create())
            {
                // hash the source string
                data = sha256hasher.ComputeHash(Encoding.UTF8.GetBytes(source));
            }

            // create an output stringbuilder object
            var s = new StringBuilder();

            // loop through the byte array and build a return string
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2")); // outputs the byte as two hex digits
            }
            hashValue = s.ToString();
            return hashValue;
        }
        public UserVM LoginUser(string email, string password)
        {
            UserVM userVM = null;

            try
            {
                if (AuthenticateUser(email, password))
                {
                    userVM = GetUserVMByEmail(email);
                    userVM.Roles = GetRolesByUserID(userVM.UserID);
                }
                else
                {
                    throw new ArgumentException("Bad email or password");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Authentication Failed", ex);
            }
            return userVM;
        }
        public bool ResetPassword(string email, string oldPassword, string newPassword)
        {
            bool result = false;
            newPassword = HashSha256(newPassword);

            try
            {
                result = (1 == _userAccessor.UpdatePasswordHash(email, oldPassword, newPassword));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("User or password not found.", ex);
            }
            return result;
        }
        public bool InsertUser(User user, string password)
        {
            bool result = false;

            password = HashSha256(password);

            try
            {
                result = (1 == _userAccessor.InsertUser(user, password));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("User not Created", ex);
            }
            return result;
        }
        public List<User> SelectAllUsers()
        {
            List<User> allUsers = new List<User>();

            try
            {
                allUsers = _userAccessor.SelectAllUsers();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Users not found.", ex);
            }
            return allUsers;
        }
        public bool UpdateUser(User oldUser, User newUser)
        {
            bool result = false;

            try
            {
                result = (1 == _userAccessor.UpdateUser(oldUser, newUser));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to update profile ", ex);
            }
            return result;
        }
        public bool UpdateMinutesListened(int userID, int newMinutesListened)
        {
            bool result = false;

            try
            {
                result = (1 == _userAccessor.UpdateMinutesListened(userID, newMinutesListened));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to update the minutes listened ", ex);
            }
            return result;
        }
        public bool UpdateActiveUser(int userID, bool oldActive, bool newActive)
        {
            bool result = false;

            try
            {
                result = (1 == _userAccessor.UpdateActiveUser(userID, oldActive, newActive));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to deactivate or reactivate the account ", ex);
            }
            return result;
        }
        public UserPass ResetPassword(string employeeID)
        {
            throw new NotImplementedException();
        }
        public List<UserFriend> SelectFriendsByUserID(int userID)
        {
            List <UserFriend> allFriends = new List<UserFriend>();  

            try
            {
                allFriends = _userAccessor.SelectFriendsByUserID(userID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Unable to find friends lol.", ex);
            }
            return allFriends;
        }
        public bool FindUser(string email)
        {
            try
            {
                return _userAccessor.SelectUserVMByEmail(email) != null;
            }
            catch (ApplicationException ax)
            {
                if(ax.Message == "User not found")
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Database Error", ex);
            }
        }
        public int RetrieveUserIDFromEmail(string email)
        {
            try
            {
                return _userAccessor.SelectUserVMByEmail(email).UserID;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Database Error", ex);
            }
        }

    }
}
