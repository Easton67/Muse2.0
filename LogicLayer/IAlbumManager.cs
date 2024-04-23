using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IAlbumManager
    {
        bool CreateAlbum(Album album);
        bool InsertSongIntoAlbumID(int songID, int albumID);
        int SelectAlbumIDFromTitle(string albumTitle, string artistID);
        Album SelectAlbumByAlbumID(int AlbumID);
        List<Album> SelectAlbumsByUserID(int userID);
        List<Album> SelectAllAlbums();
        bool UpdateAlbum(Album oldAlbum, Album newAlbum);
        bool DeleteAlbum(int albumId);
        bool RemoveSongFromAlbum(int songID);
    }
}
