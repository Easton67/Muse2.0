using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IPlaylistAccessor
    {
        int CreatePlaylist(Playlist newPlaylist);
        int InsertSongIntoPlaylist(int songID, int playlistID);
        Playlist SelectPlaylistByUserID(int userId, int playlistID);
        List<Playlist> SelectPlaylistsByUserID(int userId);
        int UpdatePlaylist(Playlist oldPlaylist, Playlist newPlaylist);
        int DeletePlaylist(int playlistID);
        int RemoveSongFromPlaylist(int songID);
    }
}
