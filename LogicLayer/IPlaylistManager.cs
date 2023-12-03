using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IPlaylistManager
    {
        bool InsertSongIntoPlaylist(int songID, int playlistID);
        bool CreatePlaylist(Playlist newPlaylist);
        List<Playlist> SelectPlaylistByUserID(int userId);
        bool UpdatePlaylist(Playlist oldPlaylist, Playlist newPlaylist);
        bool DeletePlaylist(int playlistID);
    }
}
