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
        List<string> SelectAllGenres();
        int UpdateSong(Song oldSong, Song newSong);
        int UpdatePlaysBySongID(int songID, int plays);
        int UpdateFavoriteStatus(int SongID, bool newIsLiked);
        int DeleteSong(int SongID);
        Song SelectSongBySongID(int UserID, int SongID);
    }
}
