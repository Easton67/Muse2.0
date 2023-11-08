using DataAccessFakes;
using DataAccessInterfaces;
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
            _playlistManager = new PlaylistManager(new PlaylistAccessorFake());
        }


        [TestMethod]
        public void TestGetPlaylistsByUserIDReturnsCorrectPlaylists()
        {
            // arrange
            int testID = 100001;
            int expectedSongCount = 2;
            int actualSongCount = 0;

            // act
            actualSongCount = _playlistManager.SelectPlaylistByUserID(testID).Count;


            // assert
            Assert.AreEqual(expectedSongCount, actualSongCount);
        }
    }
}
