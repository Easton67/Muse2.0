using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class PlaylistAccessorFake : IPlaylistAccessor
    {
        private List<Playlist> fakePlaylists = new List<Playlist>();
        public PlaylistAccessorFake()
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
        public int CreatePlaylist(Playlist newPlaylist)
        {
            throw new NotImplementedException();
        }
        public int InsertSongIntoPlaylist(int songID, int playlistID)
        {
            throw new NotImplementedException();
        }
        public List<Playlist> SelectPlaylistsByUserID(int userId)
        {
            return fakePlaylists.FindAll(p => p.UserID == userId);
        }
        public int DeletePlaylist(int playlistID)
        {
            throw new NotImplementedException();
        }
        public int RemoveSongFromPlaylist(int songID)
        {
            throw new NotImplementedException();
        }
        public int UpdatePlaylist(Playlist oldPlaylist, Playlist newPlaylist)
        {
            throw new NotImplementedException();
        }
        public Playlist SelectPlaylistByUserID(int userId, int playlistID)
        {
            throw new NotImplementedException();
        }
    }
}
