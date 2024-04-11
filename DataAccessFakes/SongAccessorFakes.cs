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
    public class SongAccessorFakes : ISongAccessor
    {
        private string defaultImg = AppDomain.CurrentDomain.BaseDirectory + "defaultAlbumImage.png";
        private List<Song> fakeSongs = new List<Song>();

        public SongAccessorFakes()
        {

            fakeSongs.Add(new Song()
            {
                SongID = 1,
                Title = "Sicko Mode",
                ImageFilePath = "sickoMode.jpg",
                Mp3FilePath = "sickoMode.mp3",
                YearReleased = 2018,
                Lyrics = "Astro, yeah, yeah",
                Explicit = true,
                Plays = 987,
                UserID = 100000,
                Artist = "Travis Scott",
                Album = "Astroworld",
                DateUploaded = DateTime.Now,
                DateAdded = DateTime.Now,
                isLiked = true,
                isPublic = true
            });

            fakeSongs.Add(new Song()
            {
                SongID = 2,
                Title = "God's Plan",
                ImageFilePath = "godsPlan.jpg",
                Mp3FilePath = "godsPlan.mp3",
                YearReleased = 2018,
                Lyrics = "Yeah, they wishin' and wishin' and wishin' and wishin'",
                Explicit = false,
                Plays = 654,
                UserID = 100001,
                Artist = "Drake",
                Album = "Scorpion",
                DateUploaded = DateTime.Now,
                DateAdded = DateTime.Now,
                isLiked = false,
                isPublic = true
            });

            fakeSongs.Add(new Song()
            {
                SongID = 3,
                Title = "Lose Yourself",
                ImageFilePath = "loseYourself.jpg",
                Mp3FilePath = "loseYourself.mp3",
                YearReleased = 2002,
                Lyrics = "Look, if you had one shot, or one opportunity",
                Explicit = true,
                Plays = 234,
                UserID = 100001,
                Artist = "Eminem",
                Album = "8 Mile",
                DateUploaded = DateTime.Now,
                DateAdded = DateTime.Now,
                isLiked = true,
                isPublic = true
            });

            fakeSongs.Add(new Song()
            {
                SongID = 4,
                Title = "Hotline Bling",
                ImageFilePath = "hotlineBling.jpg",
                Mp3FilePath = "hotlineBling.mp3",
                YearReleased = 2015,
                Lyrics = "You used to call me on my cell phone",
                Explicit = false,
                Plays = 876,
                UserID = 100001,
                Artist = "Drake",
                Album = "Views",
                DateUploaded = DateTime.Now,
                DateAdded = DateTime.Now,
                isLiked = true,
                isPublic = false
            });

            fakeSongs.Add(new Song()
            {
                SongID = 5,
                Title = "Rockstar",
                ImageFilePath = "rockstar.jpg",
                Mp3FilePath = "rockstar.mp3",
                YearReleased = 2017,
                Lyrics = "I feel just like a rockstar",
                Explicit = true,
                Plays = 789,
                UserID = 100001,
                Artist = "Post Malone",
                Album = "Beerbongs & Bentleys",
                DateUploaded = DateTime.Now,
                DateAdded = DateTime.Now,
                isLiked = false,
                isPublic = true
            });
        }
        public int InsertSong(Song song)
        {
            fakeSongs.Add(new Song()
            {
                SongID = 6,
                Title = "Added Song",
                ImageFilePath = "defaultImg.png",
                Mp3FilePath = "bruh.mp3",
                YearReleased = 2023,
                Lyrics = "fake",
                Explicit = true,
                Plays = 33526,
                UserID = 100000,
                Artist = "Liam Easton",
                Album = "No album",
                DateUploaded = DateTime.Now,
                DateAdded = DateTime.Now,
                isLiked = false,
                isPublic = true
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
                throw new ApplicationException("Song not found");
            }

            return rows;
        }
        public List<string> SelectAllGenres()
        {
            throw new NotImplementedException();
        }
        public Song SelectSongBySongID(int UserID, int SongID)
        {
            throw new NotImplementedException();
        }
        public int UpdateFavoriteStatus(int SongID, bool newIsLiked)
        {
            throw new NotImplementedException();
        }
    }
}
