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
        Album SelectAlbumByAlbumID(int AlbumID);
        List<Album> SelectAllAlbums();
        bool UpdateAlbum(Album oldAlbum, Album newAlbum);
        bool DeleteAlbum(int albumId);
        bool RemoveSongFromAlbum(int songID);
    }
}
