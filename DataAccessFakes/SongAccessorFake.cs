﻿using DataAccessInterfaces;
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
                Private = true,
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
                Private = true,
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
                Private = true,
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
                Private = true,
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
                Private = true,
                Explicit = true,
                Plays = 0,
                UserID = 100001,
                Artist = "not here",
                Album = "none"
            });
        }
        public int InsertSong(Song song)
        {
            throw new NotImplementedException();
        }
        public List<Song> SelectSongsByPlaylistID(int UserID, int PlaylistID)
        {
            throw new NotImplementedException();
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
        public int UpdateAlbumBySongID(int SongID, string Album)
        {
            throw new NotImplementedException();
        }
        public int UpdateArtistBySongID(int SongID, string Artist)
        {
            throw new NotImplementedException();
        }
        public int UpdateExplicitBySongID(int SongID, bool Explicit)
        {
            throw new NotImplementedException();
        }
        public int UpdateLyricsBySongID(int SongID, string Lyrics)
        {
            throw new NotImplementedException();
        }
        public int UpdatePlaysBySongID(int SongID, int Plays)
        {
            throw new NotImplementedException();
        }
        public int UpdateSongImageBySongID(int SongID, string ImageFilePath)
        {
            throw new NotImplementedException();
        }
        public int UpdateTitleBySongID(int SongID, string Title)
        {
            throw new NotImplementedException();
        }
        public int UpdateYearBySongID(int SongID, int Year)
        {
            throw new NotImplementedException();
        }
    }
}
