using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace DataAccessLayer
{
    public class UserAccessor : IUserAccessor
    {
        private string defaultAccountImg = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\ProfileImages\\defaultAccount.png";
        public int InsertUser(User user, string password)
        {
            int rows = 0;
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_create_new_user";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PasswordHash", password);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@ProfileName", user.ProfileName);
            cmd.Parameters.AddWithValue("@ImageFilePath", user.ImageFilePath);

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rows;
        }
        public int AuthenticateUserWithEmailAndPasswordHash(string email, string passwordHash)
        {
            int rows = 0;
            var conn = SqlConnectionProvider.GetConnection();
            var commandText = "sp_authenticate_user";
            var cmd = new SqlCommand(commandText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters["@Email"].Value = email;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            try
            {
                conn.Open();

                rows = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return rows;

        }
        public UserPass SelectPasswordHashByEmail(string email)
        {
            UserPass userPass = new UserPass();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_passwordHash_by_Email";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            cmd.Parameters["@Email"].Value = email;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        userPass.PasswordHash = reader.GetString(0);
                    }
                    else
                    {
                        throw new ArgumentException("Password not found");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return userPass;
        }
        public UserVM SelectUserVMByEmail(string email)
        {
            UserVM userVM = new UserVM();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_user_by_Email";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            cmd.Parameters["@Email"].Value = email;
            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        userVM.UserID = reader.GetInt32(0);
                        userVM.ProfileName = reader.GetString(1);
                        userVM.Email = reader.GetString(2);
                        userVM.FirstName = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        userVM.LastName = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        userVM.ImageFilePath = reader.IsDBNull(5) ? defaultAccountImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\ProfileImages\\" + reader.GetString(5);
                        userVM.Active = reader.GetBoolean(6);
                        userVM.MinutesListened = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                        userVM.isPublic = reader.GetBoolean(8);
                    }
                    else
                    {
                        throw new ArgumentException("User not found");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return userVM;
        }
        public List<User> SelectAllUsers()
        {
            List<User> allUsers = new List<User>();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_all_users";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var user = new User
                    {
                        UserID = reader.GetInt32(0),
                        ProfileName = reader.GetString(1),
                        Email = reader.GetString(2),
                        FirstName = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        LastName = reader.IsDBNull(4) ? "" : reader.GetString(4),
                        ImageFilePath = reader.IsDBNull(5) ? defaultAccountImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\ProfileImages\\" + reader.GetString(5),
                        Active = reader.GetBoolean(6),
                        MinutesListened = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                        isPublic = reader.GetBoolean(8)
                };
                    allUsers.Add(user);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return allUsers;
        }
        public List<string> SelectRolesByUserID(int userID)
        {
            List<string> roles = new List<string>();

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_role_by_UserID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return roles;
        }
        public int UpdateUser(User oldUser, User newUser)
        {
            int rows = 0;
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_update_user";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", newUser.UserID);
            cmd.Parameters.AddWithValue("@NewFirstName", newUser.FirstName);
            cmd.Parameters.AddWithValue("@NewLastName", newUser.LastName);
            cmd.Parameters.AddWithValue("@NewImageFilePath", newUser.ImageFilePath);
            cmd.Parameters.AddWithValue("@OldFirstName", oldUser.FirstName);
            cmd.Parameters.AddWithValue("@OldLastName", oldUser.LastName);
            cmd.Parameters.AddWithValue("@OldImageFilePath", oldUser.ImageFilePath);
            cmd.Parameters.AddWithValue("@OldMinutesListened", oldUser.MinutesListened);
            cmd.Parameters.AddWithValue("@NewMinutesListened", newUser.MinutesListened);
            cmd.Parameters.AddWithValue("@OldActive", oldUser.Active);
            cmd.Parameters.AddWithValue("@NewActive", newUser.Active);

            try
            {
                conn.Open();

                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update your profile.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rows;
        }
        public int UpdateMinutesListened(int userID, int newMinutesListened)
        {
            int rows = 0;
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_update_minutesListened";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@NewMinutesListened", newMinutesListened);

            try
            {
                conn.Open();

                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update your minutes listened.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rows;
        }
        public int UpdatePasswordHash(string email, string oldPasswordHash, string newPasswordHash)
        {
            int rows = 0;

            // start with a connection object
            var conn = SqlConnectionProvider.GetConnection();

            // set the command text
            var commandText = "sp_update_PasswordHash";

            // create the command object
            var cmd = new SqlCommand(commandText, conn);

            // set the command text
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);

            // we Need to set the parameter values
            cmd.Parameters["@Email"].Value = email;
            cmd.Parameters["@NewPasswordHash"].Value = newPasswordHash;
            cmd.Parameters["@OldPasswordHash"].Value = oldPasswordHash;


            // now that we are all set up, we execute command in a try-catch-finally
            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Bad Email or Password");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }
        public int UpdateActiveUser(int UserID, bool oldActive, bool newActive)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var commandText = "sp_update_active_user";
            var cmd = new SqlCommand(commandText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters

            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@NewActive", newActive);
            cmd.Parameters.AddWithValue("@OldActive", oldActive);
            
            try
            {
                conn.Open();

                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Unable to update active status");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }

        #region FriendAccessors
        public List<UserFriend> SelectFriendsByUserID(int userID)
        {
            List<UserFriend> friends = new List<UserFriend>();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_friends_from_UserID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UserFriend userFriend = new UserFriend();

                    userFriend.UserID = reader.GetInt32(0);
                    userFriend.ProfileName = reader.GetString(1);
                    userFriend.FirstName = reader.GetString(2);
                    userFriend.LastName = reader.GetString(3);
                    userFriend.Email = reader.GetString(4);
                    userFriend.ImageFilePath = reader.IsDBNull(5) ? defaultAccountImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\ProfileImages\\" + reader.GetString(5);
                    userFriend.Active = reader.GetBoolean(6);
                    userFriend.MinutesListened = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                    userFriend.isPublic = reader.GetBoolean(8);
                    userFriend.DateAddedAsFriend = reader.GetDateTime(9);
                    friends.Add(userFriend); 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return friends;
        }
        #endregion
    }
}
