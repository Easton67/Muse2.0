using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IUserAccessor
    {
        int AuthenticateUserWithEmailAndPasswordHash(string email, string PasswordHash);
        UserVM SelectUserVMByEmail(string email);
        List<string> SelectRolesByUserID(int UserID);
        int UpdatePasswordHash(string email, string oldPasswordHash, string newPasswordHash);
        int UpdateFirstName(string Email, string NewFirstName);
        int UpdateLastName(string email, string accountImage);
        int UpdateAccountImage(string email, string accountImage);
    }
}
