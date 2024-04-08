﻿using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DataAccessLayer
{
    public class AlbumAccessor : IAlbumAccessor
    {
        private string defaultAlbumImg = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png";
        public int CreateAlbum(Album album)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_insert_album";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Title", album.Title);
            cmd.Parameters.AddWithValue("@IsExplicit", album.isExplicit);
            cmd.Parameters.AddWithValue("@ArtistID", album.ArtistID);
            cmd.Parameters.AddWithValue("@ImageFilePath", album.ImageFilePath);
            cmd.Parameters.AddWithValue("@Description", album.Description);
            cmd.Parameters.AddWithValue("@YearReleased", album.YearReleased);

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
        public List<Album> SelectAllAlbums()
        {
            List<Album> albums = new List<Album>();

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_all_albums";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var Album = new Album
                    {
                        AlbumID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        ArtistID = reader.GetString(2),
                        isExplicit = reader.GetBoolean(3),
                        ImageFilePath = reader.IsDBNull(4) ? defaultAlbumImg : reader.GetString(4),
                        Description = reader.IsDBNull(5) ? "No description." : reader.GetString(5),
                        YearReleased = reader.IsDBNull(6) ? 2002 : reader.GetInt32(6),
                        DateAdded = reader.IsDBNull(7) ? DateTime.Now.Date : reader.GetDateTime(7)
                    };
                    albums.Add(Album);
                }
                return albums;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int SelectAlbumIDFromTitle(string albumTitle, string artistID)
        {
            int albumID = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_albumid_from_albumtitle";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AlbumTitle", albumTitle);
            cmd.Parameters.AddWithValue("@ArtistID", artistID);

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    albumID = reader.GetInt32(0);
                }
                else
                {
                    throw new ArgumentException("AlbumID not found");
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
            return albumID;
        }
        public int InsertSongIntoAlbumID(int songID, int albumID)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_insert_song_into_playlist";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SongID", songID);
            cmd.Parameters.AddWithValue("@AlbumID", albumID);

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
        public Album SelectAlbumByAlbumID(int AlbumID)
        {
            Album album = new Album();

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_Album_by_AlbumID";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AlbumID", AlbumID);

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    album = new Album
                    {
                        AlbumID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        ArtistID = reader.GetString(2),
                        isExplicit = reader.GetBoolean(3),
                        ImageFilePath = reader.IsDBNull(4) ? defaultAlbumImg : reader.GetString(4),
                        Description = reader.IsDBNull(5) ? "No description." : reader.GetString(5),
                        YearReleased = reader.IsDBNull(6) ? 2002 : reader.GetInt32(6),
                        DateAdded = reader.IsDBNull(7) ? DateTime.Now.Date : reader.GetDateTime(7)
                    };
                }
                else
                {
                    throw new ArgumentException("Album not found");
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
            return album;
        }
        public int UpdateAlbum(Album oldAlbum, Album newAlbum)
        {
            int rows = 0;
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_update_Album";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AlbumID", oldAlbum.AlbumID);
            cmd.Parameters.AddWithValue("@NewTitle", newAlbum.Title);
            cmd.Parameters.AddWithValue("@NewImageFilePath", newAlbum.ImageFilePath);
            cmd.Parameters.AddWithValue("@NewDescription", newAlbum.Description);
            cmd.Parameters.AddWithValue("@NewYearReleased", newAlbum.YearReleased);

            cmd.Parameters.AddWithValue("@OldTitle", oldAlbum.Title);
            cmd.Parameters.AddWithValue("@OldImageFilePath", oldAlbum.ImageFilePath);
            cmd.Parameters.AddWithValue("@OldDescription", oldAlbum.Description);
            cmd.Parameters.AddWithValue("@OldYearReleased", oldAlbum.YearReleased);


            try
            {
                conn.Open();

                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not update your album.");
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
        public int DeleteAlbum(int albumId)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_delete_Album";
            SqlCommand cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AlbumID", SqlDbType.Int);
            cmd.Parameters["@AlbumID"].Value = albumId;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not remove this album.");
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
        public int RemoveSongFromAlbum(int songID)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_remove_song_from_album";
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
                    throw new ArgumentException("Could not remove this song from your album.");
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
        public List<Song> SelectSongsByAlbumID (string AlbumID)
        {
            List<Song> songs = new List<Song>();

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_songs_by_albumID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

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
                        ImageFilePath = reader.IsDBNull(2) ? defaultAlbumImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\" + reader.GetString(2),
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
                return songs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
