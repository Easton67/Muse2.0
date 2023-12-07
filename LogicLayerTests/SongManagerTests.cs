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
        [TestMethod]
        public void TestInsertSongWorksCorrectly()
        {
            // arrange
            Song insertedSong = new Song()
            {
                SongID = 7,
                Title = "Added Song",
                ImageFilePath = "C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\net7.0-windows\\MuseConfig\\PlaylistImages\\defaultAlbumImage",
                Mp3FilePath = "C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\net7.0-windows\\MuseConfig\\SongFiles\\bruh.mp3",
                YearReleased = 2023,
                Lyrics = "fake",
                Explicit = true,
                Plays = 33526,
                UserID = 100000,
                Artist = "Liam Easton",
                Album = "No album"
            };

            bool expectedResult = true;
            bool actualResult = false;

            // act
            actualResult = _songManager.InsertSong(insertedSong);


            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestUpdatePlaysBySongIDWorksCorrectly() 
        {
            // arrange
            int songID = 2;
            int plays = 0;

            bool expectedResult = true;
            bool actualResult = false;

            // act
            actualResult = _songManager.UpdatePlaysBySongID(songID, plays);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestDeleteSongWorks()
        {
            // arrange
            int songID = 1;

            bool expectedResult = true;
            bool actualResult = false;

            // act
            actualResult = _songManager.DeleteSong(songID);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestDeleteSongFailsWithSongIDNotFound()
        {
            // arrange
            int songID = 20;

            bool expectedResult = false;
            bool actualResult = true;

            // act
            actualResult = _songManager.DeleteSong(songID);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
