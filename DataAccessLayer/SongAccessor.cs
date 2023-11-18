using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

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
                    var song = new Song
                    {
                        SongID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        ImageFilePath = reader.IsDBNull(2) ? "\"C:\\Users\\67Eas\\Downloads\\Fruit-O-Pedia\\Fruit-O-Pedia\\FruitPresentationViews\\bin\\Debug\\fruitData\\raspberries.png\"" : reader.GetString(2),
                        Mp3FilePath = reader.GetString(3),
                        YearReleased = reader.IsDBNull(4) ? 2023 : reader.GetInt32(4),
                        Lyrics = reader.IsDBNull(5) ? "No Lyrics Provided" : reader.GetString(5),
                        Explicit = reader.GetBoolean(6),
                        Private = reader.GetBoolean(7),
                        Plays = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                        UserID = reader.GetInt32(9),
                        Artist = reader.GetString(10),
                        Album = reader.GetString(11)
                    };
                    songs.Add(song);
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
            return songs;
        }
        public int UpdatePlaysBySongID(int SongID, int Plays)
        {
            int rows = 0;

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_update_song_plays";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters.Add("@NewPlays", SqlDbType.Int);

            // Parameter Values
            cmd.Parameters["@SongID"].Value = SongID;
            cmd.Parameters["@NewPlays"].Value = Plays;

            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update song's play count.");
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
        public int InsertSong(Song song)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_insert_song";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Title", song.Title);
            cmd.Parameters.AddWithValue("@ImageFilePath", song.ImageFilePath);
            cmd.Parameters.AddWithValue("@Mp3FilePath", song.Mp3FilePath);
            cmd.Parameters.AddWithValue("@YearReleased", song.YearReleased);
            cmd.Parameters.AddWithValue("@Lyrics", song.Lyrics);
            cmd.Parameters.AddWithValue("@Explicit", song.Explicit);
            cmd.Parameters.AddWithValue("@Private", song.Private);
            cmd.Parameters.AddWithValue("@Plays", song.Plays);
            cmd.Parameters.AddWithValue("@UserID", song.UserID);
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
        public int UpdateTitleBySongID(int SongID, string Title)
        {
            int rows = 0;

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_update_song_title";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters.Add("@NewSongTitle", SqlDbType.VarChar);

            // Parameter Values
            cmd.Parameters["@SongID"].Value = SongID;
            cmd.Parameters["@NewSongTitle"].Value = Title;

            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update your song's title.");
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
        public int UpdateArtistBySongID(int SongID, string Artist)
        {
            int rows = 0;

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_update_song_artist";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters.Add("@NewSongArtist", SqlDbType.VarChar);

            // Parameter Values
            cmd.Parameters["@SongID"].Value = SongID;
            cmd.Parameters["@NewSongArtist"].Value = Artist;

            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update your song's artist.");
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
        public int UpdateAlbumBySongID(int SongID, string Album)
        {
            int rows = 0;

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_update_song_album";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters.Add("@NewSongAlbum", SqlDbType.VarChar);

            // Parameter Values
            cmd.Parameters["@SongID"].Value = SongID;
            cmd.Parameters["@NewSongAlbum"].Value = Album;

            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update your song's album.");
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
        public int UpdateYearBySongID(int SongID, int YearReleased)
        {
            int rows = 0;

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_update_song_release_year";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters.Add("@NewYearReleased", SqlDbType.Int);

            // Parameter Values
            cmd.Parameters["@SongID"].Value = SongID;
            cmd.Parameters["@NewYearReleased"].Value = YearReleased;

            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update your song's year released.");
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
        public int UpdateExplicitBySongID(int SongID, bool Explicit)
        {
            int rows = 0;

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_update_song_explicit_status";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters.Add("@NewStatus", SqlDbType.Bit);

            // Parameter Values
            cmd.Parameters["@SongID"].Value = SongID;
            cmd.Parameters["@NewStatus"].Value = Explicit;

            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update your song's parental advisory status.");
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
        public int UpdateSongImageBySongID(int SongID, string ImageFilePath)
        {
            int rows = 0;

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_update_song_image";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters.Add("@NewImageFilePath", SqlDbType.Bit);

            // Parameter Values
            cmd.Parameters["@SongID"].Value = SongID;
            cmd.Parameters["@NewImageFilePath"].Value = ImageFilePath;

            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update your song's image.");
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
        public int UpdateLyricsBySongID(int SongID, string Lyrics)
        {
            int rows = 0;

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_update_lyrics";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters.Add("@NewLyrics", SqlDbType.Bit);

            // Parameter Values
            cmd.Parameters["@SongID"].Value = SongID;
            cmd.Parameters["@NewLyrics"].Value = Lyrics;

            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update your song's lyrics.");
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
