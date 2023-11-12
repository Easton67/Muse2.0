using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LogicLayer;
using DataAccessFakes;
using DataAccessInterfaces;
using DataObjects;


namespace LogicLayerTests
{
    [TestClass]
    public class SongManagerTests
    {
        ISongManager _songManager = null;

        [TestInitialize]
        public void TestSetUp()
        {
            _songManager = new SongManager(new SongAccessorFake());
        }
        [TestMethod]
        public void TestGetSongsByProfileNameReturnsCorrectSongs()
        {
            // arrange
            string testName = "Easton67";
            int expectedSongCount = 2;
            int actualSongCount = 0;

            // act
            actualSongCount = _songManager.SelectSongsByProfileName(testName).Count;


            // assert
            Assert.AreEqual(expectedSongCount, actualSongCount);
        }
        [TestMethod]
        public void TestGetSongsByUserIDReturnsCorrectSongs()
        {
            // arrange
            int testUserID = 100000;
            int expectedSongCount = 2;
            int actualSongCount = 0;

            // act
            actualSongCount = _songManager.SelectSongsByUserID(testUserID).Count;


            // assert
            Assert.AreEqual(expectedSongCount, actualSongCount);
        }
    }
}
