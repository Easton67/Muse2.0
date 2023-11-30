using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class SongAccessor : ISongAccessor
    {
        private string defaultImg = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png";
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
            cmd.Parameters.AddWithValue("@ArtistID", song.Artist);
            cmd.Parameters.AddWithValue("@AlbumTitle", song.Album);

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
                        ImageFilePath = reader.IsDBNull(2) ? defaultImg : reader.GetString(2),
                        Mp3FilePath = reader.GetString(3),
                        YearReleased = reader.IsDBNull(4) ? 2023 : reader.GetInt32(4),
                        Lyrics = reader.IsDBNull(5) ? "No Lyrics Provided" : reader.GetString(5),
                        Explicit = reader.GetBoolean(6),
                        Private = reader.GetBoolean(7),
                        Plays = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                        UserID = reader.GetInt32(9),
                        Artist = reader.GetString(10),
                        Album = reader.IsDBNull(11) ? "" : reader.GetString(11)
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
        public List<Song> SelectSongsByPlaylistID(int UserID, int PlaylistID)
        {
            List<Song> songs = new List<Song>();

            var conn = SqlConnectionProvider.GetConnection();

            var cmdText = "sp_select_songs_by_PlaylistID";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters.Add("@PlaylistID", SqlDbType.Int);

            cmd.Parameters["@UserID"].Value = UserID;
            cmd.Parameters["@PlaylistID"].Value = PlaylistID;

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
                        ImageFilePath = reader.IsDBNull(2) ? defaultImg : reader.GetString(2),
                        Mp3FilePath = reader.GetString(3),
                        YearReleased = reader.IsDBNull(4) ? 2023 : reader.GetInt32(4),
                        Lyrics = reader.IsDBNull(5) ? "No Lyrics Provided" : reader.GetString(5),
                        Explicit = reader.GetBoolean(6),
                        Private = reader.GetBoolean(7),
                        Plays = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                        UserID = reader.GetInt32(9),
                        Artist = reader.GetString(10),
                        Album = reader.IsDBNull(11) ? "" : reader.GetString(11)
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
        public int UpdateSong(Song oldSong, Song newSong)
        {
            int rows = 0;
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_update_song";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters.Add("@NewTitle", SqlDbType.NVarChar);
            cmd.Parameters.Add("@NewImageFilePath", SqlDbType.NVarChar);
            cmd.Parameters.Add("@NewYearReleased", SqlDbType.Int);
            cmd.Parameters.Add("@NewLyrics", SqlDbType.NVarChar);
            cmd.Parameters.Add("@NewExplicit", SqlDbType.Bit);
            cmd.Parameters.Add("@NewPrivate", SqlDbType.Bit);
            cmd.Parameters.Add("@NewPlays", SqlDbType.Int);
            cmd.Parameters.Add("@OldTitle", SqlDbType.NVarChar);
            cmd.Parameters.Add("@OldImageFilePath", SqlDbType.NVarChar);
            cmd.Parameters.Add("@OldYearReleased", SqlDbType.Int);
            cmd.Parameters.Add("@OldLyrics", SqlDbType.Text);
            cmd.Parameters.Add("@OldExplicit", SqlDbType.Bit);
            cmd.Parameters.Add("@OldPrivate", SqlDbType.Bit);
            cmd.Parameters.Add("@OldPlays", SqlDbType.Int);

            cmd.Parameters["@SongID"].Value = newSong.SongID;
            cmd.Parameters["@NewTitle"].Value = newSong.Title;
            cmd.Parameters["@NewImageFilePath"].Value = newSong.ImageFilePath;
            cmd.Parameters["@NewYearReleased"].Value = newSong.YearReleased;
            cmd.Parameters["@NewLyrics"].Value = newSong.Lyrics;
            cmd.Parameters["@NewExplicit"].Value = newSong.Explicit;
            cmd.Parameters["@NewPrivate"].Value = newSong.Private;
            cmd.Parameters["@NewPlays"].Value = newSong.Plays;
            cmd.Parameters["@OldTitle"].Value = oldSong.Title;
            cmd.Parameters["@OldImageFilePath"].Value = oldSong.ImageFilePath;
            cmd.Parameters["@OldYearReleased"].Value = oldSong.YearReleased;
            cmd.Parameters["@OldLyrics"].Value = oldSong.Lyrics;
            cmd.Parameters["@OldExplicit"].Value = oldSong.Explicit;
            cmd.Parameters["@OldPrivate"].Value = oldSong.Private;
            cmd.Parameters["@OldPlays"].Value = oldSong.Plays;

            try
            {
                // open the connection
                conn.Open();

                // an update is executed nonquery - returns an int
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update your song.");
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
        public int DeleteSong(int SongID)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_delete_song";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters["@SongID"].Value = SongID;

            try
            {
                conn.Open();

                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not remove this song.");
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
