using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessInterfaces;
using DataObjects;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class UserAccessor : IUserAccessor
    {
        public int AuthenticateUserWithEmailAndPasswordHash(string email, string passwordHash)
        {
            int rows = 0;

            // start with a connection object
            var conn = SqlConnectionProvider.GetConnection();

            // set the command text
            var commandText = "sp_authenticate_user";

            // create the command object
            var cmd = new SqlCommand(commandText, conn);

            // set the command text
            cmd.CommandType = CommandType.StoredProcedure;


            // Add parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            // we Need to set the parameter values
            cmd.Parameters["@Email"].Value = email;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            // now that we are all set up, we execute command in a try-catch-finally
            try
            {
                // open the connection
                conn.Open();

                // execute the command and capture the result
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

                //process the results
                if (reader.HasRows)
                {
                    //change if to while if multiple rows
                    if (reader.Read())
                    {
                        userVM.UserID = reader.GetInt32(0);
                        userVM.ProfileName = reader.GetString(1);
                        userVM.Email = reader.GetString(2);
                        userVM.FirstName = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        userVM.LastName = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        userVM.ImageFilePath = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        userVM.Active = reader.GetBoolean(6);
                        userVM.Private = reader.GetBoolean(7);
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
                        ImageFilePath = reader.IsDBNull(5) ? "" : reader.GetString(5),
                        Active = reader.GetBoolean(6),
                        Private = reader.GetBoolean(7),
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
        public int UpdateUser(User oldUser, User newUser) 
        {
            int rows = 0;
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_update_update";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters.Add("@NewFirstName", SqlDbType.VarChar);
            cmd.Parameters.Add("@OldFirstName", SqlDbType.VarChar);
            cmd.Parameters.Add("@NewLastName", SqlDbType.VarChar);
            cmd.Parameters.Add("@OldLastName", SqlDbType.VarChar);
            cmd.Parameters.Add("@NewImageFilePath", SqlDbType.VarChar);
            cmd.Parameters.Add("@OldImageFilePath", SqlDbType.VarChar);

            cmd.Parameters["@UserID"].Value = newUser.UserID;
            cmd.Parameters["@NewFirstName"].Value = newUser.FirstName;
            cmd.Parameters["@OldFirstName"].Value = oldUser.FirstName;
            cmd.Parameters["@NewLastName"].Value = newUser.LastName;
            cmd.Parameters["@OldLastName"].Value = oldUser.LastName;
            cmd.Parameters["@NewImageFilePath"].Value = newUser.ImageFilePath;
            cmd.Parameters["@OldImageFilePath"].Value = oldUser.ImageFilePath;

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

        // DEAD
        public int UpdateFirstName(string Email, string FirstName)
        {
            int rows = 0;

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_update_FirstName";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar);
            cmd.Parameters.Add("@NewFirstName", SqlDbType.NVarChar);

            // Parameter Values
            cmd.Parameters["@Email"].Value = Email;
            cmd.Parameters["@NewFirstName"].Value = FirstName;

            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update first name.");
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
        public int UpdateLastName(string Email, string LastName)
        {
            int rows = 0;

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_update_LastName";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar);
            cmd.Parameters.Add("@NewLastName", SqlDbType.NVarChar);

            // Parameter Values
            cmd.Parameters["@Email"].Value = Email;
            cmd.Parameters["@NewLastName"].Value = LastName;

            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update last name.");
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
        public int UpdateAccountImage(string Email, string AccountImage)
        {
            int rows = 0;

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_update_AccountImage";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar);
            cmd.Parameters.Add("@NewAccountImage", SqlDbType.NVarChar);

            // Parameter Values
            cmd.Parameters["@Email"].Value = Email;
            cmd.Parameters["@NewAccountImage"].Value = AccountImage;

            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update account image.");
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
    }
}
