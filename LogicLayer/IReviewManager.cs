using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IReviewManager
    {
        bool CreateReview(Review review);
        List<Review> SelectReviewByUserID(int userID);
        Review SelectReviewByReviewID(int userID, int reviewID);
        bool UpdateReview(Review oldReview, Review newReview);
        bool DeleteReview(int reviewID);
    }
}
