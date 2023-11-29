using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
        public bool InsertSongIntoPlaylist(int songID, int playlistID)
        {
            bool result = false;

            try
            {
                result = (1 == _playlistAccessor.InsertSongIntoPlaylist(songID, playlistID));
                if(result == false)
                {
                    throw new ArgumentException("Song may already exist");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Song not Added", ex);
            }
            return result;
        }
        public List<Playlist> SelectPlaylistByUserID(int userId)
        {
            List<Playlist> playlists = new List<Playlist>();

            try
            {
                playlists = _playlistAccessor.SelectPlaylistByUserID(userId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Playlists not found", ex);
            }
            return playlists;
        }
        public bool CreatePlaylist(Playlist newPlaylist)
        {
            bool result = false;

            try
            {
                result = (1 == _playlistAccessor.CreatePlaylist(newPlaylist));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Song not Added", ex);
            }
            return result;
        }
    }
}
