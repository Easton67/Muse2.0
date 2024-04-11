using DataAccessFakes;
using DataObjects;
using LogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;

namespace LogicLayerTests
{
    [TestClass]
    public class AlbumManagerTests
    {
        IAlbumManager _albumManager = null;

        [TestInitialize]
        public void TestSetUp()
        {
            _albumManager = new AlbumManager(new AlbumAccessorFakes());
        }
        [TestMethod]
        public void TestCreateAlbumWorksCorrectly()
        {
            Album album = new Album()
            {
                AlbumID = 100000,
                Title = "Fake Album",
                ArtistID = "Fakio",
                isExplicit = false,
                ImageFilePath = "fake.png",
                Description = "Realest music you'll ever hear",
                YearReleased = 2024,
                DateAdded = DateTime.Now,
            };

            bool expectedValue = true;
            bool actualValue;

            actualValue = _albumManager.CreateAlbum(album);

            Assert.AreEqual(expectedValue, actualValue);
        }
        [TestMethod]
        public void TestSelectAllAlbumsWorksCorrectly()
        {
            int expectedCount = 5;
            int actualCount;

            actualCount = _albumManager.SelectAllAlbums().Count;

            Assert.AreEqual(expectedCount, actualCount);
        }
        [TestMethod]
        public void TestSelectAlbumIDFromTitleWorksCorrectly()
        {
            string title = "The Blueprint";
            string artistID = "Jay-Z";
            int expectedAlbumID = 5;
            int actualAlbumID;

            actualAlbumID = _albumManager.SelectAlbumIDFromTitle(title, artistID);

            Assert.AreEqual(expectedAlbumID, actualAlbumID);
        }
        [TestMethod]
        public void TestDeleteAlbumWorksCorrectly()
        {
            int testAlbumID = 4;
            bool expectedValue = true;
            bool actualValue;

            actualValue = _albumManager.DeleteAlbum(testAlbumID);

            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
