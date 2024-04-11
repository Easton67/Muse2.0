using DataAccessFakes;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LogicLayerTests
{
    [TestClass]
    public class ArtistManagerTests
    {
        IArtistManager _artistManager = null;

        [TestInitialize]
        public void TestSetUp()
        {
            _artistManager = new ArtistManager(new ArtistAccessorFakes());
        }

        [TestMethod]
        public void TestSelectAllArtistsWorksCorrectly()
        {
            int expectedArtists = 3;
            int actualArtists = 0;

            actualArtists = _artistManager.SelectAllArtists().Count;

            Assert.AreEqual(expectedArtists, actualArtists);
        }

        [TestMethod]
        public void TestSelectArtistByArtistIDWorksCorrectly()
        {
            string artistID = "Kendrick Lamar";
            int expectedValue = 1;
            int actualValue = 0;

            var artist = _artistManager.SelectArtistByArtistID(artistID);

            actualValue = (artist.ArtistID == artistID) ? 1 : 0;

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void TestSelectSongsByArtistIDWorksCorrectly()
        {
            string artistID = "Kendrick Lamar";
            int expectedSongs = 1;
            int actualSongs = 0;

            actualSongs = _artistManager.SelectSongsByArtistID(artistID).Count;

            Assert.AreEqual(expectedSongs, actualSongs);
        }
    }
}
