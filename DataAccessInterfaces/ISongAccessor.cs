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
        List<Song> SelectSongsByUserID(int UserID);
        int UpdatePlaysBySongID(int SongID, int Plays);
        int UpdateTitleBySongID(int SongID, string Title);
        int UpdateArtistBySongID(int SongID, string Artist);
        int UpdateAlbumBySongID(int SongID, string Album);
        int UpdateYearBySongID(int SongID, int Year);
        int UpdateExplicitBySongID(int SongID, bool Explicit);
        int UpdateSongImageBySongID(int SongID, string ImageFilePath);
        int InsertSong(Song song);
    }
}
