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
        List<Album> SelectAllAlbums();
        int UpdateAlbum(Album oldAlbum, Album newAlbum);
        int DeleteAlbum(int albumId);
        int RemoveSongFromAlbum(int songID);
    }
}
