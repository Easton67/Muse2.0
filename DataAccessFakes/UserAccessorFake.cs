using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessInterfaces;
using DataObjects;

namespace DataAccessFakes
{
    public class UserAccessorFake : IUserAccessor
    {
        // create a few fake users for testing
        private List<UserVM> fakeUsers = new List<UserVM>();
        private List<string> passwordHashes = new List<string>();

        public UserAccessorFake()
        {
            fakeUsers.Add(new UserVM()
            {
                UserID = 1,
                FirstName = "Liam",
                LastName = "Easton",
                Email = "Liam@gmail.com",
                Active = true,
                Private = true,
                Roles = new List<string>()
            });
            fakeUsers.Add(new UserVM()
            {
                UserID = 2,
                FirstName = "Jess",
                LastName = "Data",
                Email = "jess@company.com",
                Active = true,
                Private = true,
                Roles = new List<string>()
            });
            fakeUsers.Add(new UserVM()
            {
                UserID = 3,
                FirstName = "SNES",
                LastName = "Data",
                Email = "snes@company.com",
                Active = true,
                Private = true,
                Roles = new List<string>()
            });

            passwordHashes.Add("9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e");
            passwordHashes.Add("badhash");
            passwordHashes.Add("badhash");

            fakeUsers[0].Roles.Add("TestRole1");
            fakeUsers[0].Roles.Add("TestRole2");
        }

        public int AuthenticateUserWithEmailAndPasswordHash(string email, string passwordHash)
        {
            int numAuthenticated = 0;

            // check for employee records in the fake data 
            for (int i = 0; i < fakeUsers.Count; i++)
            {
                if (passwordHashes[i] == passwordHash &&
                    fakeUsers[i].Email == email)
                {
                    numAuthenticated += 1;
                }
            }
            return numAuthenticated;        // should be 1 or 0
        }

        public UserVM SelectUserVMByEmail(string email)
        {
            UserVM user = null;

            foreach (var fakeUser in fakeUsers)
            {
                if (fakeUser.Email == email)
                {
                    user = fakeUser;
                }
            }
            if (user == null) // no one found
            {
                throw new ApplicationException("User not found");
            }
            return user;
        }

        public List<string> SelectRolesByUserID(int userID)
        {
            List<string> roles = new List<string>();

            foreach (var fakeUser in fakeUsers)
            {
                if (fakeUser.UserID == userID)
                {
                    roles = fakeUser.Roles;
                    break;
                }
            }

            return roles;
        }

        public int UpdatePasswordHash(string email, string oldPasswordHash, string newPasswordHash)
        {
            int rows = 0;

            for (int i = 0; i < fakeUsers.Count; i++)
            {
                if (fakeUsers[i].Email == email)
                {
                    if (passwordHashes[i] == oldPasswordHash)
                    {
                        passwordHashes[i] = newPasswordHash;
                        rows += 1;
                        break;
                    }
                }
            }
            if (rows != 1) // no one found
            {
                throw new ApplicationException("Bad Email or Password");
            }
            return rows;
        }
    }
}
