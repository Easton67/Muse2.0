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
        // dependency inversion for the data provider
        private IUserAccessor _userAccessor = null;

        // the default constructor will use the database
        public UserManager()
        {
            _userAccessor = new UserAccessor();
        }
        // the optional constructor can accept any data provider
        public UserManager(IUserAccessor employeeAccessor)
        {
            _userAccessor = employeeAccessor;
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
            }
            catch (Exception ex)
            {
                throw new ApplicationException("User not found", ex);
            }
            return userVM;
        }

        public List<string> GetRolesByUserID(int employeeID)
        {
            List<string> roles = new List<string>();

            //roles.Add("");
            //roles.Add("");

            try
            {
                roles = _userAccessor.SelectRolesByUserID(employeeID);
            }
            catch (Exception)
            {

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

            oldPassword = HashSha256(oldPassword);
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
    }
}
