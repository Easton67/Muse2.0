using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class ReviewAccessFake : IReviewAccessor
    {
        private List<Review> fakeReviews = new List<Review>();
        public ReviewAccessFake()
        {

        }
        public int CreateReview(Review review)
        {
            throw new NotImplementedException();
        }
        public int DeleteReview(int reviewID)
        {
            throw new NotImplementedException();
        }
        public List<Review> SelectReviewsByUserID(int userID)
        {
            throw new NotImplementedException();
        }
        public int UpdateReview(Review oldReview, Review newReview)
        {
            throw new NotImplementedException();
        }
    }
}
