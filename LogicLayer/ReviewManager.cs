using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;

namespace LogicLayer
{
    public class ReviewManager
    {
        private IReviewAccessor _reviewAccessor = null;
        public ReviewManager()
        {
            _reviewAccessor = new ReviewAccessor();
        }
        public ReviewManager(IReviewAccessor reviewAccessor)
        {
            _reviewAccessor = reviewAccessor;
        }
        public bool CreateReview(Review review)
        {
            bool result = false;

            try
            {
                result = (1 == _reviewAccessor.CreateReview(review));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Review not Added", ex);
            }
            return result;
        }
        public List<Review> SelectReviewByReviewID(int reviewID)
        {
            List<Review> reviews = new List<Review>();

            try
            {
                reviews = _reviewAccessor.SelectReviewByReviewID(reviewID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Review not Added", ex);
            }
            return reviews;
        }
        public bool UpdateReview(Review oldReview, Review newReview)
        {
            bool result = false;

            try
            {
                result = (1 == _reviewAccessor.UpdateReview(oldReview, newReview));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Review not updated", ex);
            }
            return result;

        }
        public bool DeleteReview(int reviewID)
        {
            bool result = false;

            try
            {
                result = (1 == _reviewAccessor.DeleteReview(reviewID));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Review not deleted", ex);
            }
            return result;
        }
    }
}
