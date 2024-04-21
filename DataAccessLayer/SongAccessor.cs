using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccessLayer
{
    public class SongAccessor : ISongAccessor
    {
        private string defaultImg = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png";
        private string albumArtPath = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\";


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
            cmd.Parameters.AddWithValue("@Genre", song.Genre);
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
        public List<string> SelectAllGenres()
        {
            List<String> genres = new List<String>();

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_all_genres_from_songs";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    genres.Add(reader.GetString(0));
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
            return genres;
        }
        public Song SelectSongBySongID(int UserID, int SongID)
        {
            Song song = new Song();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_song_by_SongID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = UserID;
            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters["@SongID"].Value = SongID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int fieldIndex = 3;
                    long fieldWidth;
                    byte[] image = null;

                    song = new Song
                    {
                        SongID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        ImageFilePath = reader.IsDBNull(2) ? defaultImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\" + reader.GetString(2),

                        Photo = reader.IsDBNull(3) ? null : image,
                        PhotoMimeType = reader.IsDBNull(4) ? null : reader.GetString(4),

                        Mp3FilePath = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\SongFiles\\" + reader.GetString(5),
                        YearReleased = reader.IsDBNull(6) ? 2023 : reader.GetInt32(6),
                        Lyrics = reader.IsDBNull(7) ? "No Lyrics Provided" : reader.GetString(7),
                        Explicit = reader.GetBoolean(8),
                        Genre = reader.GetString(9),
                        Plays = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                        UserID = reader.GetInt32(11),
                        Artist = reader.GetString(12),
                        Album = reader.IsDBNull(13) ? "" : reader.GetString(13),
                        DateUploaded = reader.IsDBNull(14) ? (DateTime?)null : reader.GetDateTime(14),
                        DateAdded = reader.GetDateTime(15),
                        isLiked = reader.GetBoolean(16),
                        isPublic = reader.GetBoolean(17)
                    };

                    if (!reader.IsDBNull(fieldIndex))
                    {
                        fieldWidth = reader.GetBytes(fieldIndex, 0, null, 0, Int32.MaxValue);
                        image = new byte[fieldWidth];
                        reader.GetBytes(fieldIndex, 0, image, 0, image.Length);
                    }

                    // turn the image file path into a byte array
                    if (song.ImageFilePath != null)
                    {
                        string filePath = "";
                        string fileType = "";
                        try
                        {
                            filePath = song.ImageFilePath;
                            fileType = Path.GetExtension(filePath);

                            song.Photo = File.ReadAllBytes(filePath);
                            song.PhotoMimeType = fileType;
                        }
                        catch (Exception ex)
                        {
                            filePath = defaultImg;
                            fileType = Path.GetExtension(filePath);

                            song.Photo = File.ReadAllBytes(filePath);
                            song.PhotoMimeType = fileType;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Song not found");
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
            return song;
        }
        public List<Song> SelectSongsByUserID(int UserID)
        {
            List<Song> songs = new List<Song>();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_songs_by_UserID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = UserID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Song song = new Song
                    {
                        SongID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        ImageFilePath = reader.IsDBNull(2) ? defaultImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\" + reader.GetString(2),
                        Mp3FilePath = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\SongFiles\\" + reader.GetString(3),
                        YearReleased = reader.IsDBNull(4) ? 2023 : reader.GetInt32(4),
                        Lyrics = reader.IsDBNull(5) ? "No Lyrics Provided" : reader.GetString(5),
                        Explicit = reader.GetBoolean(6),
                        Genre = reader.GetString(7),
                        Plays = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                        UserID = reader.GetInt32(9),
                        Artist = reader.GetString(10),
                        Album = reader.IsDBNull(11) ? "" : reader.GetString(11),
                        DateUploaded = reader.IsDBNull(12) ? (DateTime?)null : reader.GetDateTime(12),
                        DateAdded = reader.GetDateTime(13),
                        isLiked = reader.GetBoolean(14),
                        isPublic = reader.GetBoolean(15)
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
                        ImageFilePath = reader.IsDBNull(2) ? defaultImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\" + reader.GetString(2),
                        Mp3FilePath = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\SongFiles\\" + reader.GetString(3),
                        YearReleased = reader.IsDBNull(4) ? 2023 : reader.GetInt32(4),
                        Lyrics = reader.IsDBNull(5) ? "No Lyrics Provided" : reader.GetString(5),
                        Explicit = reader.GetBoolean(6),
                        Genre = reader.GetString(7),
                        Plays = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                        UserID = reader.GetInt32(9),
                        Artist = reader.GetString(10),
                        Album = reader.IsDBNull(11) ? "" : reader.GetString(11),
                        DateUploaded = reader.IsDBNull(12) ? (DateTime?)null : reader.GetDateTime(12),
                        DateAdded = reader.GetDateTime(13),
                        isLiked = reader.GetBoolean(14)
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
        public List<Song> SelectSongsByAlbumID(int UserID, int AlbumID)
        {
            List<Song> songs = new List<Song>();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_songs_by_AlbumID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters.Add("@PlaylistID", SqlDbType.Int);
            cmd.Parameters["@UserID"].Value = UserID;
            cmd.Parameters["@AlbumID"].Value = AlbumID;

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
                        ImageFilePath = reader.IsDBNull(2) ? defaultImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\" + reader.GetString(2),
                        Mp3FilePath = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\SongFiles\\" + reader.GetString(3),
                        YearReleased = reader.IsDBNull(4) ? 2023 : reader.GetInt32(4),
                        Lyrics = reader.IsDBNull(5) ? "No Lyrics Provided" : reader.GetString(5),
                        Explicit = reader.GetBoolean(6),
                        Genre = reader.GetString(7),
                        Plays = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                        UserID = reader.GetInt32(9),
                        Artist = reader.GetString(10),
                        Album = reader.IsDBNull(11) ? "" : reader.GetString(11),
                        DateUploaded = reader.IsDBNull(12) ? (DateTime?)null : reader.GetDateTime(12),
                        DateAdded = reader.GetDateTime(13),
                        isLiked = reader.GetBoolean(14)
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

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_update_song_plays";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters.Add("@NewPlays", SqlDbType.Int);
            cmd.Parameters["@SongID"].Value = SongID;
            cmd.Parameters["@NewPlays"].Value = Plays;

            try
            {
                conn.Open();
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
        public int UpdateFavoriteStatus(int SongID, bool newIsLiked)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_update_favorite_song";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SongID", SqlDbType.Int);
            cmd.Parameters.Add("@NewisLiked", SqlDbType.Bit);
            cmd.Parameters["@SongID"].Value = SongID;
            cmd.Parameters["@NewisLiked"].Value = newIsLiked;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not favorite or unfavorite song.");
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

            // turn the image file path into a byte array
            if (newSong.ImageFilePath != null)
            {
                string filePath = "";
                string fileType = "";
                try
                {
                    filePath = albumArtPath + newSong.ImageFilePath;
                    fileType = Path.GetExtension(filePath);

                    newSong.Photo = File.ReadAllBytes(filePath);
                    newSong.PhotoMimeType = fileType;
                }
                catch (Exception)
                {
                    filePath = defaultImg;
                    fileType = Path.GetExtension(filePath);

                    newSong.Photo = File.ReadAllBytes(filePath);
                    newSong.PhotoMimeType = fileType;
                }
            }

            cmd.Parameters.AddWithValue("@SongID", oldSong.SongID);
            cmd.Parameters.AddWithValue("@NewTitle", newSong.Title);
            cmd.Parameters.AddWithValue("@NewImageFilePath", newSong.ImageFilePath);
            cmd.Parameters.AddWithValue("@NewYearReleased", newSong.YearReleased);
            cmd.Parameters.AddWithValue("@NewLyrics", newSong.Lyrics);
            cmd.Parameters.AddWithValue("@NewExplicit", newSong.Explicit);
            cmd.Parameters.AddWithValue("@NewGenre", newSong.Genre); 
            cmd.Parameters.AddWithValue("@NewPlays", newSong.Plays);
            cmd.Parameters.AddWithValue("@NewArtistID", newSong.Artist); 
            cmd.Parameters.AddWithValue("@NewAlbumTitle", newSong.Album); 
            cmd.Parameters.AddWithValue("@NewIsLiked", newSong.isLiked);
            cmd.Parameters.AddWithValue("@NewPhoto", ((object)newSong.Photo) ?? SqlBinary.Null);
            cmd.Parameters.AddWithValue("@NewPhotoMimeType", newSong.PhotoMimeType);

            try
            {
                conn.Open();
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
