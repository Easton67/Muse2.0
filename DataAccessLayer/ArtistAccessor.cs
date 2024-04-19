using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessInterfaces;
using System.IO;

namespace DataAccessLayer
{
    public class ArtistAccessor : IArtistAccessor
    {
        private string defaultImg = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png";
        private string artistArtPath = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\ProfileImages\\";

        public Artist SelectArtistByArtistID(string artistID)
        {
            Artist artist = new Artist();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_artist_by_ArtistID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ArtistID", SqlDbType.NVarChar);
            cmd.Parameters["@ArtistID"].Value = artistID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    int fieldIndex = 6; // column of photo data
                    long fieldWidth;
                    byte[] image = null;
                    if (!reader.IsDBNull(fieldIndex))
                    {
                        fieldWidth = reader.GetBytes(fieldIndex, 0, null, 0, Int32.MaxValue); // buffer size 
                        image = new byte[fieldWidth];
                        reader.GetBytes(fieldIndex, 0, image, 0, image.Length);
                    }

                    artist = new Artist
                    {
                        ArtistID = reader.GetString(0), 
                        ImageFilePath = reader.GetString(1),
                        Photo = reader.IsDBNull(2) ? null : image,
                        PhotoMimeType = reader.IsDBNull(3) ? null : reader.GetString(3),
                        FirstName = reader.GetString(4), 
                        LastName = reader.GetString(5), 
                        Description = reader.GetString(6), 
                        isLiked = reader.GetBoolean(7),
                        DateOfBirth = reader.IsDBNull(8) ? new DateTime(1990, 1, 1) : reader.GetDateTime(8)
                    };

                    if (!reader.IsDBNull(fieldIndex))
                    {
                        fieldWidth = reader.GetBytes(fieldIndex, 0, null, 0, Int32.MaxValue);
                        image = new byte[fieldWidth];
                        reader.GetBytes(fieldIndex, 0, image, 0, image.Length);
                    }

                    // turn the image file path into a byte array
                    if (artist.ImageFilePath != null)
                    {
                        string filePath = "";
                        string fileType = "";
                        try
                        {
                            filePath = artistArtPath + artist.ImageFilePath;
                            fileType = Path.GetExtension(filePath);

                            artist.Photo = File.ReadAllBytes(filePath);
                            artist.PhotoMimeType = fileType;
                        }
                        catch (Exception ex)
                        {
                            filePath = defaultImg;
                            fileType = Path.GetExtension(filePath);

                            artist.Photo = File.ReadAllBytes(filePath);
                            artist.PhotoMimeType = fileType;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Artist not found");
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
            return artist;
        }
        public List<Artist> SelectAllArtists()
        {
            List<Artist> artists = new List<Artist>();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_all_artists";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

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
                        var artist = new Artist
                        {
                            ArtistID = reader.GetString(0),
                            ImageFilePath = reader.GetString(1),
                            FirstName = reader.GetString(4),
                            LastName = reader.GetString(5),
                            Description = reader.GetString(6),
                            isLiked = reader.GetBoolean(7),
                            DateOfBirth = reader.IsDBNull(8) ? new DateTime(1990, 1, 1) : reader.GetDateTime(8)
                        };

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

                        artist.PhotoMimeType = reader.IsDBNull(4) ? null : reader.GetString(4);
                        artist.Photo = photo;
                        artists.Add(artist);
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
            return artists;
        }
        public List<Song> SelectSongsByArtistID(string ArtistID)
        {
            List<Song> songs = new List<Song>();
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_songs_by_ArtistID";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ArtistID", SqlDbType.Int);
            cmd.Parameters["@ArtistID"].Value = ArtistID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    byte[] photo = null;
                    long? fieldWidth = null;

                    var song = new Song
                    {
                        SongID = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        ImageFilePath = reader.IsDBNull(2) ? defaultImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\" + reader.GetString(2),
                        Mp3FilePath = reader.GetString(5),
                        YearReleased = reader.IsDBNull(6) ? 2023 : reader.GetInt32(6),
                        Lyrics = reader.IsDBNull(7) ? "No Lyrics Provided" : reader.GetString(7),
                        Explicit = reader.GetBoolean(8),
                        Genre = reader.GetString(9),
                        Plays = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                        UserID = reader.GetInt32(11),
                        Artist = reader.GetString(12),
                        Album = reader.IsDBNull(13) ? "" : reader.GetString(13),
                        isLiked = reader.GetBoolean(14)
                    };

                    int columnIndex = 9;
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

                    song.PhotoMimeType = reader.IsDBNull(4) ? null : reader.GetString(4);
                    song.Photo = photo;
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
    }
}
