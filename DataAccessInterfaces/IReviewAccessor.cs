using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IReviewAccessor
    {
        int CreateReview(Review review);
        List<Review> SelectReviewByReviewID(int reviewID);
        int UpdateReview(Review oldReview, Review newReview);
        int DeleteReview(int reviewID);

    }
}
