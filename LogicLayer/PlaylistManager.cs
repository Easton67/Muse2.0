using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class PlaylistManager : IPlaylistManager
    {
        private IPlaylistAccessor _playlistAccessor = null;

        public PlaylistManager()
        {
            _playlistAccessor = new PlaylistAccessor();
        }

        public PlaylistManager(IPlaylistAccessor playlistAccessor)
        {
            _playlistAccessor = playlistAccessor;
        }

        public List<Playlist> SelectPlaylistByUserID(int userId)
        {
            List<Playlist> playlists = new List<Playlist>();

            try
            {
                playlists = _playlistAccessor.SelectPlaylistByUserID(userId);
            }
            catch (Exception)
            {

            }
            return playlists;
        }
    }
}
