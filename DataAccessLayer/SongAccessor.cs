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
    public class SongAccessor : ISongAccessor
    {
        public List<Song> SelectSongsByUserID(int UserID)
        {
            List<Song> songs = new List<Song>();

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_select_songs_by_UserID";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@UserID", SqlDbType.Int);

            // Parameter Values
            cmd.Parameters["@UserID"].Value = UserID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Song song = new Song
                    {
                        SongID = reader.GetInt32(reader.GetOrdinal("SongID")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        ImageFilePath = reader.IsDBNull(reader.GetOrdinal("ImageFilePath"))
                            ? string.Empty
                            : reader.GetString(reader.GetOrdinal("ImageFilePath")),
                        Mp3FilePath = reader.GetString(reader.GetOrdinal("Mp3FilePath")),
                        YearReleased = reader.GetInt32(reader.GetOrdinal("YearReleased")),
                        Lyrics = reader.IsDBNull(reader.GetOrdinal("Lyrics"))
                            ? string.Empty
                            : reader.GetString(reader.GetOrdinal("Lyrics")),
                        Explicit = reader.GetBoolean(reader.GetOrdinal("Explicit")),
                        Private = reader.GetBoolean(reader.GetOrdinal("Private")),
                        Plays = reader.IsDBNull(reader.GetOrdinal("Plays"))
                            ? 0
                            : reader.GetInt32(reader.GetOrdinal("Plays")),
                        CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy"))
                    };
                    songs.Add(song);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return songs;
        }
    }
}
