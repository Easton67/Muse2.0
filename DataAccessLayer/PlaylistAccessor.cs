using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DataAccessInterfaces;

namespace DataAccessLayer
{
    public class PlaylistAccessor : IPlaylistAccessor
    {
        private string defaultImg = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png";
        public int CreatePlaylist(Playlist newPlaylist)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_create_playlist";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Title", newPlaylist.Title);
            cmd.Parameters.AddWithValue("@ImageFilePath", newPlaylist.ImageFilePath);
            cmd.Parameters.AddWithValue("@Description", newPlaylist.Description);
            cmd.Parameters.AddWithValue("@UserID", newPlaylist.UserID);

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
        public int InsertSongIntoPlaylist(int songID, int playlistID)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_insert_song_into_playlist";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SongID", songID);
            cmd.Parameters.AddWithValue("@PlaylistID", playlistID);

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
        public List<Playlist> SelectPlaylistsByUserID(int userId)
        {
            List<Playlist> playlists = new List<Playlist>();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_playlists_by_UserID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userId;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var playlist = new Playlist
                    {
                        PlaylistID = reader.GetInt32(0),
                        Title = reader.IsDBNull(1) ? "Playlist" : reader.GetString(1),
                        ImageFilePath = reader.IsDBNull(2) ? defaultImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\PlaylistImages\\" + reader.GetString(2),
                        Description = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        UserID = reader.GetInt32(4)
                    };
                    playlists.Add(playlist);
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
            return playlists;
        }
        public Playlist SelectPlaylistByUserID(int userId, int playlistID)
        {
            Playlist playlist = new Playlist();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_playlist_by_UserID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = userId;
            cmd.Parameters.Add("@PlaylistID", SqlDbType.Int);
            cmd.Parameters["@PlaylistID"].Value = playlistID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    playlist = new Playlist
                    {
                        PlaylistID = reader.GetInt32(0),
                        Title = reader.IsDBNull(1) ? "Playlist" : reader.GetString(1),
                        ImageFilePath = reader.IsDBNull(2) ? defaultImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\PlaylistImages\\" + reader.GetString(2),
                        Description = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        UserID = reader.GetInt32(4)
                    };
                }
                else
                {
                    throw new ArgumentException("Playlist not found");
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
            return playlist;
        }
        public int DeletePlaylist(int playlistID)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_delete_playlist";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@PlaylistID", SqlDbType.Int);
            cmd.Parameters["@PlaylistID"].Value = playlistID;

            try
            {
                conn.Open();

                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not remove this playlist.");
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
        public int UpdatePlaylist(Playlist oldPlaylist, Playlist newPlaylist)
        {
            int rows = 0;
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_update_playlist";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@PlaylistID", SqlDbType.Int);
            cmd.Parameters.Add("@NewTitle", SqlDbType.NVarChar);
            cmd.Parameters.Add("@NewImageFilePath", SqlDbType.NVarChar);
            cmd.Parameters.Add("@NewDescription", SqlDbType.NVarChar);
            cmd.Parameters.Add("@OldTitle", SqlDbType.NVarChar);
            cmd.Parameters.Add("@OldImageFilePath", SqlDbType.NVarChar);
            cmd.Parameters.Add("@OldDescription", SqlDbType.NVarChar);

            cmd.Parameters["@PlaylistID"].Value =  oldPlaylist.PlaylistID;
            cmd.Parameters["@NewTitle"].Value =  newPlaylist.Title;
            cmd.Parameters["@NewImageFilePath"].Value =  newPlaylist.ImageFilePath;
            cmd.Parameters["@NewDescription"].Value =  newPlaylist.Description;
            cmd.Parameters["@OldTitle"].Value =  oldPlaylist.Title;
            cmd.Parameters["@OldImageFilePath"].Value =  oldPlaylist.ImageFilePath;
            cmd.Parameters["@OldDescription"].Value =  oldPlaylist.Description;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new ArgumentException("Could not update playlist.");
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
        public int RemoveSongFromPlaylist(int songID)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_remove_song_from_playlist";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters["@SongID"].Value = songID;

            try
            {
                conn.Open();

                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not remove this song from your playlist.");
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
