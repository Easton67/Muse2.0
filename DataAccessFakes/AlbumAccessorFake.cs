using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class AlbumAccessorFake : IAlbumAccessor
    {
        private List<Album> fakeAlbums = new List<Album>();
        public AlbumAccessorFake()
        {
            
        }
        public int CreateAlbum(Album album)
        {
            throw new NotImplementedException();
        }
        public int DeleteAlbum(int albumId)
        {
            throw new NotImplementedException();
        }
        public List<Album> SelectAlbumByAlbumID(int AlbumID)
        {
            throw new NotImplementedException();
        }
        public List<Album> SelectAllAlbums()
        {
            throw new NotImplementedException();
        }
        public int UpdateAlbum(Album oldAlbum, Album newAlbum)
        {
            throw new NotImplementedException();
        }
    }
}
