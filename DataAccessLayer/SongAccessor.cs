using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using static System.Net.Mime.MediaTypeNames;

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

                if (reader.HasRows)
                {
                    reader.Read();
                    song = new Song
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

            // Add parameters
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

            cmd.Parameters.AddWithValue("@OldTitle", oldSong.Title);
            cmd.Parameters.AddWithValue("@OldImageFilePath", oldSong.ImageFilePath);
            cmd.Parameters.AddWithValue("@OldYearReleased", oldSong.YearReleased);
            cmd.Parameters.AddWithValue("@OldLyrics", oldSong.Lyrics);
            cmd.Parameters.AddWithValue("@OldExplicit", oldSong.Explicit);
            cmd.Parameters.AddWithValue("@OldGenre", oldSong.Genre);
            cmd.Parameters.AddWithValue("@OldPlays", oldSong.Plays);
            cmd.Parameters.AddWithValue("@OldArtistID", oldSong.Artist);
            cmd.Parameters.AddWithValue("@OldAlbumTitle", oldSong.Album);
            cmd.Parameters.AddWithValue("@OldIsLiked", oldSong.isLiked);

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
