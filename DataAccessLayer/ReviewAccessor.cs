﻿using DataObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using DataAccessInterfaces;

namespace DataAccessLayer
{
    public class ReviewAccessor : IReviewAccessor
    {
        private string defaultImg = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png";
        public int CreateReview(Review review)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_create_review";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Rating", review.Rating);
            cmd.Parameters.AddWithValue("@Message", review.Message);
            cmd.Parameters.AddWithValue("@UserID", review.UserID);
            cmd.Parameters.AddWithValue("@SongID", review.SongID);

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
        public List<Review> SelectReviewsByUserID(int userID)
        {
            List<Review> reviews = new List<Review>();

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_reviews_by_UserID";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userID);

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var song = new Song
                    {
                        SongID = reader.GetInt32(4),
                        Title = reader.GetString(5),
                        YearReleased = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                        Artist = reader.GetString(7),
                        ImageFilePath = reader.IsDBNull(8) ? defaultImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\" + reader.GetString(8),
                        Mp3FilePath = reader.GetString(9),
                        Explicit = reader.GetBoolean(10),
                    };

                    var review = new Review
                    {
                        ReviewID = reader.GetInt32(0),
                        Rating = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                        Message = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        UserID = reader.GetInt32(3),
                        SongID = reader.GetInt32(4),
                        ReviewedSong = song
                    };

                    reviews.Add(review);
                }
                return reviews;
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
        public Review SelectReviewByReviewID(int userID, int reviewID)
        {
            Review review = null;
            Song song = null;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_select_review_by_ReviewID";
            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@ReviewID", reviewID);

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    song = new Song
                    {
                        SongID = reader.GetInt32(4),
                        Title = reader.GetString(5),
                        YearReleased = reader.IsDBNull(6) ? 0 : reader.GetInt32(6),
                        Artist = reader.GetString(7),
                        ImageFilePath = reader.IsDBNull(8) ? defaultImg : AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\" + reader.GetString(8),
                        Mp3FilePath = reader.GetString(9),
                        Explicit = reader.GetBoolean(10),
                    };

                    review = new Review
                    {
                        ReviewID = reader.GetInt32(0),
                        Rating = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                        Message = reader.IsDBNull(2) ? "" : reader.GetString(2),
                        UserID = reader.GetInt32(3),
                        SongID = reader.GetInt32(4),
                        ReviewedSong = song
                    };
                }
                else
                {
                    throw new ArgumentException("Review not found");
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
            return (review);
        }
        public int UpdateReview(Review oldReview, Review newReview)
        {
            int rows = 0;
            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_update_review";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReviewID", oldReview.ReviewID);
            cmd.Parameters.AddWithValue("@NewRating", newReview.Rating);
            cmd.Parameters.AddWithValue("@NewMessage", newReview.Message);
            cmd.Parameters.AddWithValue("@NewUserID", newReview.UserID);
            cmd.Parameters.AddWithValue("@NewSongID", newReview.SongID);
            cmd.Parameters.AddWithValue("@OldRating", oldReview.Rating);
            cmd.Parameters.AddWithValue("@OldMessage", oldReview.Message);
            cmd.Parameters.AddWithValue("@OldUserID", oldReview.UserID);
            cmd.Parameters.AddWithValue("@OldSongID", oldReview.SongID);

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
        public int DeleteReview(int reviewID)
        {
            int rows = 0;

            var conn = SqlConnectionProvider.GetConnection();
            var cmdText = "sp_delete_review";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ReviewID", SqlDbType.Int);
            cmd.Parameters["@ReviewID"].Value = reviewID;

            try
            {
                conn.Open();

                rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                {
                    throw new ArgumentException("Could not remove this review.");
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
