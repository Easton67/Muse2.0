using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessInterfaces
{
    public interface ISongAccessor
    {
        int InsertSong(Song song);
        List<Song> SelectSongsByUserID(int UserID);
        List<Song> SelectSongsByPlaylistID(int UserID, int PlaylistID);
        int UpdateSong(Song oldSong, Song newSong);
        int UpdatePlaysBySongID(int songID, int plays);
        int DeleteSong(int SongID);
    }
}
