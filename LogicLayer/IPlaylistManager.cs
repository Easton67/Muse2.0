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
        List<Playlist> SelectPlaylistByUserID(int userId);
        bool InsertSongIntoPlaylist(int songID, int playlistID);
    }
}
