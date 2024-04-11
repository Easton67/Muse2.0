using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class PlaylistAccessorFakes : IPlaylistAccessor
    {
        private List<Playlist> fakePlaylists = new List<Playlist>();
        private List<Song> fakePlaylistSongs = new List<Song>();

        public PlaylistAccessorFakes()
        {
            fakePlaylists.Add(new Playlist()
            {
                PlaylistID = 1,
                Title = "Night",
                ImageFilePath = "night.png",
                Description = "Perfect playlist for once it gets dark out",
                UserID = 100001
            });
            fakePlaylists.Add(new Playlist()
            {
                PlaylistID = 2,
                Title = "Day",
                ImageFilePath = "day.png",
                Description = "Great for when its nice and sunny outside",
                UserID = 100001
            });
            fakePlaylists.Add(new Playlist()
            {
                PlaylistID = 3,
                Title = "Gym",
                ImageFilePath = "gym.png",
                Description = "Playlist for pushing weight",
                UserID = 100002
            });
        }
        public Playlist SelectPlaylistByUserID(int userId, int playlistID)
        {
            return fakePlaylists.FirstOrDefault(p => p.UserID == userId);
        }
        public List<Playlist> SelectPlaylistsByUserID(int userId)
        {
            return fakePlaylists.FindAll(p => p.UserID == userId);
        }
        public int CreatePlaylist(Playlist newPlaylist)
        {
            fakePlaylists.Add(new Playlist()
            {
                PlaylistID = 4,
                Title = "Summer",
                ImageFilePath = "summer.png",
                Description = "Sunny playlist for throughout the summer",
                UserID = 100001
            });

            return 1;
        }
        public int InsertSongIntoPlaylist(int songID, int playlistID)
        {
            throw new NotImplementedException();
        }
        public int UpdatePlaylist(Playlist oldPlaylist, Playlist newPlaylist)
        {
            throw new NotImplementedException();
        }
        public int RemoveSongFromPlaylist(int songID)
        {
            throw new NotImplementedException();
        }
        public int DeletePlaylist(int playlistID)
        {
            int result = 0;

            if (fakePlaylists.RemoveAll(p => p.PlaylistID == playlistID) == 1)
            {
                result = 1;
            }

            return result;
        }
    }
}
