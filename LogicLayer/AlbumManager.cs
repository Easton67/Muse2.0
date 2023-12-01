using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;

namespace LogicLayer
{
    public class AlbumManager
    {
        private IAlbumAccessor _albumAccessor = null;
        public AlbumManager()
        {
            _albumAccessor = new AlbumAccessor();
        }
        public AlbumManager(IAlbumAccessor albumAccessor)
        {
            _albumAccessor = albumAccessor;
        }
        public bool CreateAlbum(Album album)
        {
            bool result = false;

            try
            { 
                result = (1 == _albumAccessor.CreateAlbum(album));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Album not Added", ex);
            }
            return result;
        }
        public List<Album> SelectAlbumByAlbumID(int AlbumID)
        {
            List<Album> albums = new List<Album>();

            try
            {
                _albumAccessor.SelectAlbumByAlbumID(AlbumID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Album not found", ex);
            }
            return albums;
        }
        public bool UpdateAlbum(Album oldAlbum, Album newAlbum)
        {
            bool result = false;

            try
            {
                result = (1 == _albumAccessor.UpdateAlbum(oldAlbum, newAlbum));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Album not updated", ex);
            }
            return result;
        }
        public bool DeleteAlbum(int albumId)
        {
            bool result = false;

            try
            {
                result = (1 == _albumAccessor.DeleteAlbum(albumId));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Album not deleted", ex);
            }
            return result;
        }
    }
}
