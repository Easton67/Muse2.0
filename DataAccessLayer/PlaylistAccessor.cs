﻿using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using DataAccessInterfaces;

namespace DataAccessLayer
{
    public class PlaylistAccessor : IPlaylistAccessor
    {
        public List<Playlist> SelectPlaylistByUserID(int userId)
        {
            List<Playlist> playlists = new List<Playlist>();

            //connection
            var conn = SqlConnectionProvider.GetConnection();

            //command text
            var cmdText = "sp_select_playlists_by_UserID";

            //command
            var cmd = new SqlCommand(cmdText, conn);

            //command type
            cmd.CommandType = CommandType.StoredProcedure;

            // Add parameters
            cmd.Parameters.Add("@UserID", SqlDbType.Int);

            // Parameter Values
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
                        ImageFilePath = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        Description = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        UserID = reader.GetInt32(4),
                    };
                    playlists.Add(playlist);
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
            return playlists;
        }

        public int InsertSongIntoPlaylist(int songID, int playlistID)
        {
            int rows = 0;
            Song song = new Song();

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
    }
}
