using DataAccessFakes;
using DataAccessInterfaces;
using DataObjects;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LogicLayerTests
{
    [TestClass]
    public class ReviewManagerTests
    {
        IReviewManager _reviewManager = null;

        [TestInitialize]
        public void TestSetUp()
        {
            _reviewManager = new ReviewManager(new ReviewAccessorFakes());
        }

        [TestMethod]
        public void TestSelectReviewsByUserIDWorksCorrectly()
        {
            int userID = 100001;
            int expectedPlaylistCount = 3;
            int actualPlaylistCount = 0;

            actualPlaylistCount = _reviewManager.SelectReviewsByUserID(userID).Count;

            Assert.AreEqual(expectedPlaylistCount, actualPlaylistCount);
        }
        [TestMethod]
        public void TestSelectOneReviewByReviewIDWorksCorrectly()
        {
            int reviewID = 3;
            int userID = 100001;
            int expectedReviewCount = 1;
            int actualReviewCount;

            actualReviewCount = _reviewManager.SelectReviewByReviewID(userID, reviewID) != null ? 1 : 0;

            Assert.AreEqual(expectedReviewCount, actualReviewCount);
        }
        [TestMethod]
        public void TestDeleteReviewWorksCorrectly()
        {
            int testID = 3;
            bool expectedPlaylistCount = true;
            bool actualPlaylistCount = false;

            actualPlaylistCount = _reviewManager.DeleteReview(testID);

            Assert.AreEqual(expectedPlaylistCount, actualPlaylistCount);
        }
    }
}
