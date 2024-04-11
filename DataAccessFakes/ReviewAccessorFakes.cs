using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class ReviewAccessorFakes : IReviewAccessor
    {
        private List<Review> fakeReviews = new List<Review>();
        public ReviewAccessorFakes()
        {
            fakeReviews.Add(new Review()
            {
                ReviewID = 1,
                Rating = 1,
                Message = "this song sucks",
                UserID = 100001,
                SongID = 1
            });
            fakeReviews.Add(new Review()
            {
                ReviewID = 2,
                Rating = 2,
                Message = "this song is pretty bad",
                UserID = 100001,
                SongID = 2
            });
            fakeReviews.Add(new Review()
            {
                ReviewID = 3,
                Rating = 3,
                Message = "this song is decent",
                UserID = 100001,
                SongID = 3
            });
            fakeReviews.Add(new Review()
            {
                ReviewID = 4,
                Rating = 4,
                Message = "this song is pretty good",
                UserID = 100000,
                SongID = 4
            });
            fakeReviews.Add(new Review()
            {
                ReviewID = 5,
                Rating = 5,
                Message = "this song is amazing",
                UserID = 100000,
                SongID = 5
            });
        }
        public int CreateReview(Review review)
        {
            fakeReviews.Add(new Review()
            {
                ReviewID = 1,
                Rating = 4,
                Message = "this song is pretty good",
                UserID = 100001,
                SongID = 1
            });

            return fakeReviews.Count;
        }
        public List<Review> SelectReviewsByUserID(int userID)
        {
            return fakeReviews.FindAll(r => r.UserID == userID);
        }
        public Review SelectReviewByReviewID(int userID, int reviewID)
        {
            return fakeReviews.Find(r => r.ReviewID == reviewID);
        }
        public int UpdateReview(Review oldReview, Review newReview)
        {
            throw new NotImplementedException();
        }
        public int DeleteReview(int reviewID)
        {
            return fakeReviews.RemoveAll(review => review.ReviewID == reviewID);
        }
    }
}
