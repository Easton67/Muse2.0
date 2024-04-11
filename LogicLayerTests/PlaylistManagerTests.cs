using DataAccessFakes;
using DataAccessInterfaces;
using DataObjects;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LogicLayerTests
{
    [TestClass]
    public class PlaylistManagerTests
    {
        IPlaylistManager _playlistManager = null;

        [TestInitialize]
        public void TestSetUp()
        {
            _playlistManager = new PlaylistManager(new PlaylistAccessorFakes());
        }

        [TestMethod]
        public void TestGetPlaylistsByUserIDReturnsCorrectPlaylists()
        {
            int testID = 100001;
            int expectedPlaylistCount = 2;
            int actualPlaylistCount = 0;

            actualPlaylistCount = _playlistManager.SelectPlaylistsByUserID(testID).Count;

            Assert.AreEqual(expectedPlaylistCount, actualPlaylistCount);
        }

        [TestMethod]
        public void TestGetOnePlaylistByUserIDReturnsCorrectPlaylist()
        {
            int testID = 100001;
            int expectedPlaylistCount = 1;
            int actualPlaylistCount = 0;

            actualPlaylistCount = _playlistManager.SelectPlaylistsByUserID(testID).Count;

            Assert.AreEqual(expectedPlaylistCount, actualPlaylistCount);
        }

        [TestMethod]
        public void TestCreateNewPlaylistWorksCorrectly()
        {
            var playlist = new Playlist()
            {
                PlaylistID = 4,
                Title = "Summer",
                ImageFilePath = "summer.png",
                Description = "Sunny playlist for throughout the summer",
                UserID = 100001
            };
            bool expectedValue = true;

            bool actualValue = _playlistManager.CreatePlaylist(playlist);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestDeletePlaylistWorksCorrectly()
        {
            int testID = 1;
            bool expectedValue = true;
            bool actualValue = false; 

            actualValue = _playlistManager.DeletePlaylist(testID);

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
