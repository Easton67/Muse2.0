using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class SongAccessorFake : ISongAccessor
    {
        private string defaultImg = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt\\defaultAlbumImage.png";
        private List<Song> fakeSongs = new List<Song>();

        public SongAccessorFake()
        {
            fakeSongs.Add(new Song()
            {
                SongID = 1,
                Title = "Wants And Needs",
                ImageFilePath = "C:\\Users\\67Eas\\Downloads\\albumart\\scaryhours.jpeg\\",
                Mp3FilePath = "C:\\Users\\67Eas\\Downloads\\songs\\WantsAndNeeds.mp3\\",
                YearReleased = 2021,
                Lyrics = "fake",
                Explicit = true,
                Plays = 372,
                UserID = 100000,
                Artist = "Drake",
                Album = "Scary Hours 2"
            });
            fakeSongs.Add(new Song()
            {
                SongID = 2,
                Title = "Rocket Man",
                ImageFilePath = "C:\\Users\\67Eas\\Downloads\\albumart\\HonkyChateau.jpg",
                Mp3FilePath = "C:\\Users\\67Eas\\Downloads\\songs\\RocketMan.mp3",
                YearReleased = 1972,
                Lyrics = "",
                Explicit = false,
                Plays = 81,
                UserID = 100000,
                Artist = "Elton John",
                Album = "HonkyChateau"
            });
            fakeSongs.Add(new Song()
            {
                SongID = 3,
                Title = "test",
                ImageFilePath = "C:\\Users\\67Eas\\Downloads\\albumart\\HonkyChateau.jpg",
                Mp3FilePath = "C:\\Users\\67Eas\\Downloads\\songs\\RocketMan.mp3",
                YearReleased = 2023,
                Lyrics = "Instrumental",
                Explicit = true,
                Plays = 0,
                UserID = 100001,
                Artist = "not here",
                Album = "none"
            });
            fakeSongs.Add(new Song()
            {
                SongID = 4,
                Title = "test",
                ImageFilePath = "C:\\Users\\67Eas\\Downloads\\albumart\\HonkyChateau.jpg",
                Mp3FilePath = "C:\\Users\\67Eas\\Downloads\\songs\\RocketMan.mp3",
                YearReleased = 2023,
                Lyrics = "Instrumental",
                Explicit = true,
                Plays = 0,
                UserID = 100001,
                Artist = "not here",
                Album = "none"
            });
            fakeSongs.Add(new Song()
            {
                SongID = 5,
                Title = "test",
                ImageFilePath = "C:\\Users\\67Eas\\Downloads\\albumart\\HonkyChateau.jpg",
                Mp3FilePath = "C:\\Users\\67Eas\\Downloads\\songs\\RocketMan.mp3",
                YearReleased = 2023,
                Lyrics = "Instrumental",
                Explicit = true,
                Plays = 0,
                UserID = 100001,
                Artist = "not here",
                Album = "none"
            });
        }
        public int InsertSong(Song song)
        {
            fakeSongs.Add(new Song()
            {
                SongID = 6,
                Title = "Added Song",
                ImageFilePath = defaultImg,
                Mp3FilePath = "C:\\Users\\67Eas\\source\\repos\\Muse2\\Muse2\\bin\\Debug\\net7.0-windows\\MuseConfig\\SongFiles\\bruh.mp3",
                YearReleased = 2023,
                Lyrics = "fake",
                Explicit = true,
                Plays = 33526,
                UserID = 100000,
                Artist = "Liam Easton",
                Album = "No album"
            });

            return 1;
        }
        public List<Song> SelectSongsByUserID(int UserID)
        {
            List<Song> songs = new List<Song>();

            foreach (Song song in fakeSongs)
            {
                if (song.UserID == UserID)
                {
                    songs.Add(song);
                }
            }
            return songs;
        }
        public List<Song> SelectSongsByPlaylistID(int UserID, int PlaylistID)
        {
            throw new NotImplementedException();
        }
        public int UpdatePlaysBySongID(int songID, int plays)
        {
            foreach (Song song in fakeSongs)
            {
                if (song.SongID == songID)
                {
                    song.Plays = plays;
                    return 1;
                }
            }
            return 0;
        }
        public int UpdateSong(Song oldSong, Song newSong)
        {
            throw new NotImplementedException();
        }
        public int DeleteSong(int SongID)
        {
            int rows = 0;

            rows = fakeSongs.RemoveAll(song => song.SongID == SongID);

            if (rows != 1) // no one found
            {
                throw new ApplicationException("User not found");
            }

            return rows;
        }
    }
}
