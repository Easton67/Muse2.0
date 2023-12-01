using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AlbumAccessor : IAlbumAccessor
    {
        private string defaultAlbumImg = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png";
        public int CreateAlbum(Album album)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_create_Album";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AlbumID", album.AlbumID);
            cmd.Parameters.AddWithValue("@Title", album.Title);
            cmd.Parameters.AddWithValue("@ImageFilePath", album.ImageFilePath);
            cmd.Parameters.AddWithValue("@Description", album.Description);

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
        public List<Album> SelectAlbumByAlbumID(int AlbumID)
        {
            List<Album> Albums = new List<Album>();

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_Album_by_AlbumID";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AlbumID", AlbumID);

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
                        ImageFilePath = reader.IsDBNull(2) ? defaultAlbumImg : reader.GetString(2),
                        Description = reader.GetString(3)
                    };
                    Albums.Add(Album);
                }
                return Albums;
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
            cmd.Parameters.AddWithValue("@OldTitle", oldAlbum.Title);
            cmd.Parameters.AddWithValue("@OldImageFilePath", oldAlbum.ImageFilePath);
            cmd.Parameters.AddWithValue("@OldDescription", oldAlbum.Description);

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
            var cmd = new SqlCommand(cmdText, conn);
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
    }
}
