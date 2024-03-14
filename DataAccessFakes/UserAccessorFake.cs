using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessFakes
{
    public class UserAccessorFake : IUserAccessor
    {
        // fake users for testing
        private List<UserVM> fakeUsers = new List<UserVM>();
        private List<User> fakeUsersNoRoles = new List<User>();
        private List<string> passwordHashes = new List<string>();

        public UserAccessorFake()
        {

            // when using just the user and not the UserVM
            fakeUsersNoRoles.Add(new User()
            {
                UserID = 7,
                FirstName = "Liam",
                LastName = "Easton",
                ProfileName = "Easton67",
                Email = "Liam@gmail.com",
                Active = true,
                MinutesListened = 0
            });

            fakeUsersNoRoles.Add(new User()
            {
                UserID = 8,
                FirstName = "Jess",
                LastName = "Data",
                ProfileName = "Jester",
                Email = "jess@company.com",
                Active = true,
                MinutesListened = 0
            });

            fakeUsersNoRoles.Add(new User()
            {
                UserID = 9,
                FirstName = "Bess",
                LastName = "Data",
                ProfileName = "Bessy",
                Email = "Bess@company.com",
                Active = true,
                MinutesListened = 90432
            });

            fakeUsers.Add(new UserVM()
            {
                UserID = 1,
                FirstName = "Liam",
                LastName = "Easton",
                ProfileName = "Easton67",
                Email = "Liam@gmail.com",
                Active = true,
                MinutesListened = 0,
                Roles = new List<string>()
            });
            fakeUsers.Add(new UserVM()
            {
                UserID = 2,
                FirstName = "Jess",
                LastName = "Data",
                ProfileName = "Jester",
                Email = "jess@company.com",
                Active = true,
                MinutesListened = 0,
                Roles = new List<string>()
            });
            fakeUsers.Add(new UserVM()
            {
                UserID = 3,
                FirstName = "Bess",
                LastName = "Data",
                ProfileName = "Bessy",
                Email = "Bess@company.com",
                Active = true,
                MinutesListened = 0,
                Roles = new List<string>()
            });

            passwordHashes.Add("9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e");
            passwordHashes.Add("badhash");
            passwordHashes.Add("badhash");

            fakeUsers[0].Roles.Add("TestRole1");
            fakeUsers[0].Roles.Add("TestRole2");
        }
        public int InsertUser(User user, string password)
        {
            bool userEmailExists = fakeUsers.Any(u => u.Email == user.Email);
            bool userProfileNameExists = fakeUsers.Any(u => u.ProfileName == user.ProfileName);

            if (userEmailExists)
            {
                throw new ApplicationException("User with this email already exists");
            }

            if (userProfileNameExists)
            {
                throw new ApplicationException("User with this profile name already exists");
            }

            fakeUsers.Add(new UserVM()
            {
                UserID = 4,
                FirstName = "Liam",
                LastName = "Easton",
                ProfileName = "Bessy",
                Email = "Liam@company.com",
                Active = true,
                MinutesListened = 12341234,
                Roles = new List<string>()
            });

            passwordHashes.Add("5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8");

            return 1; 
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
        public List<User> SelectAllUsers()
        {
            List<User> allUsers = new List<User>();

            foreach (var fakeUser in fakeUsers)
            {
                allUsers.Add(fakeUser);
            }

            return allUsers;
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
        public int UpdateUser(User oldUser, User newUser)
        {
            int rows = 0;

            for (int i = 0; i < fakeUsersNoRoles.Count; i++)
            {
                if (fakeUsersNoRoles[i].Equals(oldUser))
                {
                    // Update the existing user's values with the new user's values
                    fakeUsersNoRoles[i].UserID = newUser.UserID;
                    fakeUsersNoRoles[i].ProfileName = newUser.ProfileName;
                    fakeUsersNoRoles[i].Email = newUser.Email;
                    fakeUsersNoRoles[i].FirstName = newUser.FirstName;
                    fakeUsersNoRoles[i].LastName = newUser.LastName;
                    fakeUsersNoRoles[i].ImageFilePath = newUser.ImageFilePath;
                    fakeUsersNoRoles[i].Active = newUser.Active;
                    fakeUsersNoRoles[i].MinutesListened = newUser.MinutesListened;
                    rows = 1;
                    break;
                }
            }
            if (rows != 1) // no one found
            {
                throw new ApplicationException("One or more fields were not able to be updated");
            }

            return rows;
        }
        public int UpdateMinutesListened(int userID, int MinutesListened)
        {
            int rows = 0; 

            for (int i = 0; i < fakeUsersNoRoles.Count; i++)
            {
                if (fakeUsersNoRoles[i].UserID == userID)
                {
                    fakeUsersNoRoles[i].MinutesListened = MinutesListened; 
                    rows = 1;
                    break; 
                }
            }
            if (rows != 1) // no one found
            {
                throw new ApplicationException("Unable to update your minutes listened");
            }

            return rows; 
        }
        public int UpdateActiveUser(int userID, bool oldActive, bool newActive)
        {
            int rows = 0;

            for (int i = 0; i < fakeUsersNoRoles.Count; i++)
            {
                if (fakeUsersNoRoles[i].UserID == userID && fakeUsersNoRoles[i].Active == oldActive)
                {
                    fakeUsersNoRoles[i].Active = newActive;
                    rows = 1;
                    break;
                }
            }

            if (rows == 0)
            {
                throw new ApplicationException("Update not completed. User not found or active status could not be updated");
            }

            return rows;
        }

        public UserPass SelectPasswordHashByEmail(string email)
        {
            throw new NotImplementedException();
        }

        List<UserFriend> IUserAccessor.SelectFriendsByUserID(int UserID)
        {
            throw new NotImplementedException();
        }
    }
}
