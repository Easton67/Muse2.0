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
        Album SelectAlbumByAlbumID(int AlbumID);
        int UpdateAlbum(Album oldAlbum, Album newAlbum);
        int DeleteAlbum(int albumId);
        List<Album> SelectAllAlbums();
    }
}
