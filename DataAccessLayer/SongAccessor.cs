﻿using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
                        ImageFilePath = reader.IsDBNull(2) ? "\\bin\\Debug\\MuseConfig\\art\\SongDefault.jpg" : reader.GetString(2),
                        Mp3FilePath = reader.GetString(3),
                        YearReleased = reader.IsDBNull(4) ? 2023 : reader.GetInt32(4),
                        Lyrics = reader.IsDBNull(5) ? "No Lyrics Provided" : reader.GetString(5),
                        Explicit = reader.GetBoolean(6),
                        Private = reader.GetBoolean(7),
                        Plays = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                        CreatedBy = reader.GetInt32(9)
                    };
                    songs.Add(song);
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
            return songs;
        }
    }
}
