using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class AlbumAccessorFakes : IAlbumAccessor
    {
        private List<Album> fakeAlbums = new List<Album>();

        public AlbumAccessorFakes()
        {
            fakeAlbums.Add(new Album()
            {
                AlbumID = 1,
                Title = "Illmatic",
                ArtistID = "Nas",
                isExplicit = true,
                ImageFilePath = "illmatic.jpg",
                Description = "Nas' debut album and arguably his best ",
                YearReleased = 1994,
                DateAdded = DateTime.Now
            });

            fakeAlbums.Add(new Album()
            {
                AlbumID = 2,
                Title = "The Miseducation of Lauryn Hill",
                ArtistID = "Lauryn Hill",
                isExplicit = false,
                ImageFilePath = "miseducationOfLaurynHill.jpg",
                Description = "Lauryn Hill's first, best, and only solo record",
                YearReleased = 1998,
                DateAdded = DateTime.Now
            });

            fakeAlbums.Add(new Album()
            {
                AlbumID = 3,
                Title = "Astroworld",
                ArtistID = "Travis Scott",
                isExplicit = true,
                ImageFilePath = "astroworld.jpg",
                Description = "Travis Scott's third album",
                YearReleased = 2018,
                DateAdded = DateTime.Now
            });

            fakeAlbums.Add(new Album()
            {
                AlbumID = 4,
                Title = "Enter the Wu-Tang (36 Chambers)",
                ArtistID = "Wu-Tang Clan",
                isExplicit = true,
                ImageFilePath = "enterTheWuTang.jpg",
                Description = "The debut studio album by the Wu-Tang Clan, and one of the best hip-hop albums of all time",
                YearReleased = 1993,
                DateAdded = DateTime.Now
            });

            fakeAlbums.Add(new Album()
            {
                AlbumID = 5,
                Title = "The Blueprint",
                ArtistID = "Jay-Z",
                isExplicit = true,
                ImageFilePath = "theBlueprint.jpg",
                Description = "Jay-Z's sixth studio album has amazing tracks and production credits from Kanye",
                YearReleased = 2001,
                DateAdded = DateTime.Now
            });
        }
        public int CreateAlbum(Album album)
        {
            fakeAlbums.Add(new Album()
            {
                AlbumID = 6,
                Title = "Graduation",
                ArtistID = "Kanye West",
                isExplicit = true,
                ImageFilePath = "graduation.jpg",
                Description = "Kanye West's third studio album",
                YearReleased = 2007,
                DateAdded = DateTime.Now
            });

            return 1;
        }
        public Album SelectAlbumByAlbumID(int AlbumID)
        {
            return fakeAlbums.FirstOrDefault(x => x.AlbumID == AlbumID);
        }
        public int SelectAlbumIDFromTitle(string albumTitle, string artistID)
        {
            var foundAlbum = fakeAlbums.FirstOrDefault(x => x.Title == albumTitle && x.ArtistID == artistID);

            return foundAlbum.AlbumID;
        }
        public List<Album> SelectAllAlbums()
        {
            return fakeAlbums;
        }
        public int InsertSongIntoAlbumID(int songID, int albumID)
        {
            throw new NotImplementedException();
        }
        public int UpdateAlbum(Album oldAlbum, Album newAlbum)
        {
            throw new NotImplementedException();
        }
        public int DeleteAlbum(int albumId)
        {
            return fakeAlbums.RemoveAll(x => x.AlbumID == albumId);
        }
        public int RemoveSongFromAlbum(int songID)
        {
            throw new NotImplementedException();
        }
        public List<Album> SelectAlbumsByUserID(int userID)
        {
            return fakeAlbums.Where(x => x.UserID == userID).ToList();
        }
    }
}
