using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayer
{
    public interface ISongManager
    {
        List<Song> SelectSongsByProfileName(string ProfileName);
        List<Song> SelectSongsByUserID(int UserID);
        bool UpdatePlaysBySongID(int SongID, int Plays);
        bool UpdateTitleBySongID(int SongID, string Title);
        bool UpdateArtistBySongID(int SongID, string Artist);
        bool UpdateAlbumBySongID(int SongID, string Album);
        bool UpdateYearBySongID(int SongID, int YearReleased);
        bool UpdateExplicitBySongID(int SongID, bool Explicit);
        bool UpdateSongImageBySongID(int SongID, string ImageFilePath);
        bool UpdateLyricsBySongID(int SongID, string Lyrics);
    }
}
