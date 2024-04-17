using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DataAccessInterfaces;
using System.Diagnostics.CodeAnalysis;
using System.Data.SqlTypes;
using System.IO;

namespace DataAccessLayer
{
    [SuppressMessage("ReSharper", "PossibleIntendedRethrow")]
    public class PlaylistAccessor : IPlaylistAccessor
    {
        private string defaultImg = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\PlaylistImages\\defaultAlbumImage.png";
        private string playlistArtPath = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\PlaylistImages\\";

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
            cmd.Parameters.AddWithValue("@UserID", userId);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        byte[] photo = null;
                        long? fieldWidth = null;
                        Playlist playlist = new Playlist();
                        playlist.PlaylistID = reader.GetInt32(0);
                        playlist.Title = reader.IsDBNull(1) ? "Playlist" : reader.GetString(1);
                        playlist.ImageFilePath = reader.IsDBNull(2) ? defaultImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\PlaylistImages\\" + reader.GetString(2);
                        playlist.Description = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        playlist.UserID = reader.GetInt32(6);

                        int columnIndex = 3;

                        try
                        {
                            fieldWidth = reader.GetBytes(columnIndex, 0, null, 0, Int32.MaxValue);
                        }
                        catch (Exception)
                        {
                            photo = null;
                        }

                        if (photo != null)
                        {
                            int width = (int)fieldWidth;
                            photo = new byte[width];
                            reader.GetBytes(columnIndex, 0, photo, 0, photo.Length);
                        }

                        playlist.PhotoMimeType = reader.IsDBNull(4) ? null : reader.GetString(4);

                        playlist.Photo = photo;

                        playlists.Add(playlist);
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
            return playlists;
        }
        public Playlist SelectPlaylistByUserID(int userId, int playlistID)
        {
            Playlist playlist = new Playlist();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_playlist_by_UserID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@PlaylistID", playlistID);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int fieldIndex = 3;
                    long fieldWidth;
                    byte[] image = null;

                    playlist = new Playlist
                    {
                        PlaylistID = reader.GetInt32(0),
                        Title = reader.IsDBNull(1) ? "Playlist" : reader.GetString(1),
                        ImageFilePath = reader.IsDBNull(2) ? defaultImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\PlaylistImages\\" + reader.GetString(2),

                        Photo = reader.IsDBNull(3) ? null : image,
                        PhotoMimeType = reader.IsDBNull(4) ? null : reader.GetString(4),

                        Description = reader.IsDBNull(5) ? "" : reader.GetString(5),
                        UserID = reader.GetInt32(6)
                    };

                    if (!reader.IsDBNull(fieldIndex))
                    {
                        fieldWidth = reader.GetBytes(fieldIndex, 0, null, 0, Int32.MaxValue);
                        image = new byte[fieldWidth];
                        reader.GetBytes(fieldIndex, 0, image, 0, image.Length);
                    }

                    // turn the image file path into a byte array
                    if (playlist.ImageFilePath != null)
                    {
                        string filePath = "";
                        string fileType = "";
                        try
                        {
                            filePath = playlistArtPath + playlist.ImageFilePath;
                            fileType = Path.GetExtension(filePath);

                            playlist.Photo = File.ReadAllBytes(filePath);
                            playlist.PhotoMimeType = fileType;
                        }
                        catch (Exception ex)
                        {
                            filePath = defaultImg;
                            fileType = Path.GetExtension(filePath);

                            playlist.Photo = File.ReadAllBytes(filePath);
                            playlist.PhotoMimeType = fileType;
                        }
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

            cmd.Parameters.AddWithValue("@PlaylistID", oldPlaylist.PlaylistID);
            cmd.Parameters.AddWithValue("@NewTitle", newPlaylist.Title);
            cmd.Parameters.AddWithValue("@NewImageFilePath", newPlaylist.ImageFilePath);
            cmd.Parameters.AddWithValue("@NewDescription", newPlaylist.Description);
            cmd.Parameters.AddWithValue("@OldTitle", oldPlaylist.Title);
            cmd.Parameters.AddWithValue("@OldImageFilePath", oldPlaylist.ImageFilePath);
            cmd.Parameters.AddWithValue("@OldDescription", oldPlaylist.Description);
            cmd.Parameters.AddWithValue("@NewPhoto", ((object)newPlaylist.Photo) ?? SqlBinary.Null);
            cmd.Parameters.AddWithValue("@NewPhotoMimeType", newPlaylist.PhotoMimeType);

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
