using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IAlbumAccessor
    {
        int CreateAlbum(Album album); 
        int InsertSongIntoAlbumID(int songID, int albumID);
        Album SelectAlbumByAlbumID(int AlbumID);
        int SelectAlbumIDFromTitle(string albumTitle, string artistID);
        List<Album> SelectAllAlbums();
        List<Album> SelectAlbumsByUserID(int userID);
        int UpdateAlbum(Album oldAlbum, Album newAlbum);
        int DeleteAlbum(int albumId);
        int RemoveSongFromAlbum(int songID);
    }
}
