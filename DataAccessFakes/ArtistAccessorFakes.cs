using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class ArtistAccessorFakes : IArtistAccessor
    {
        private List<Artist> fakeArtists = new List<Artist>();
        private List<Song> fakeSongs = new List<Song>();

        public ArtistAccessorFakes()
        {
            fakeSongs.Add(new Song()
            {
                SongID = 1,
                Title = "HUMBLE.",
                ImageFilePath = "humble.png",
                Mp3FilePath = "HUMBLE.mp3",
                YearReleased = 2017,
                Lyrics = "fake",
                Explicit = true,
                Plays = 500,
                UserID = 100000,
                Artist = "Kendrick Lamar",
                Album = "DAMN."
            });
            fakeSongs.Add(new Song()
            {
                SongID = 2,
                Title = "Piano Man",
                ImageFilePath = "pianoman.jpg",
                Mp3FilePath = "PianoMan.mp3",
                YearReleased = 1973,
                Lyrics = "fake lyrics",
                Explicit = false,
                Plays = 150,
                UserID = 100000,
                Artist = "Billy Joel",
                Album = "Piano Man"
            });

            fakeArtists.Add(new Artist()
            {
                ArtistID = "Kendrick Lamar",
                ImageFilePath = "kendrickLamar.png",
                FirstName = "Kendrick",
                LastName = "Lamar",
                Description = "Compton California native, and first rapper to be awarded a nobel prize.",
                isLiked = true,      
                DateOfBirth = new DateTime(1987, 6, 17)
            });

            fakeArtists.Add(new Artist()
            {
                ArtistID = "Ye",
                ImageFilePath = "kanyeWest.png",
                FirstName = "Kanye",
                LastName = "West",
                Description = "Chicago producer turned rapper, Kanye West is a multifaceted musician.",
                isLiked = true,
                DateOfBirth = new DateTime(1977, 6, 8)
            });

            fakeArtists.Add(new Artist()
            {
                ArtistID = "Billy Joel",
                ImageFilePath = "billyJoel.png",
                FirstName = "Billy",
                LastName = "Joel",
                Description = "Iconic American singer-songwriter known for his timeless hits and captivating performances.",
                isLiked = true,
                DateOfBirth = new DateTime(1949, 5, 9)
            });
        }
        public List<Artist> SelectAllArtists()
        {
            return fakeArtists;
        }
        public Artist SelectArtistByArtistID(string artistID)
        {
            return fakeArtists.Find(x => x.ArtistID == artistID);
        }
        public List<Song> SelectSongsByArtistID(string ArtistID)
        {
            return fakeSongs.FindAll(x => x.Artist == ArtistID);
        }
    }
}
