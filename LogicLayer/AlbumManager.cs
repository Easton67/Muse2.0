using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;

namespace LogicLayer
{
    public class AlbumManager : IAlbumManager
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
        public bool InsertSongIntoAlbumID(int songID, int albumID)
        {
            bool result = false;

            try
            {
                result = (1 == _albumAccessor.InsertSongIntoAlbumID(songID, albumID));
                if (result == false)
                {
                    throw new ArgumentException("Song may already exist");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Song not Added", ex);
            }
            return result;
        }
        public Album SelectAlbumByAlbumID(int AlbumID)
        {
            Album album = new Album();

            try
            {
                album = _albumAccessor.SelectAlbumByAlbumID(AlbumID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Album not found", ex);
            }
            return album;
        }
        public int SelectAlbumIDFromTitle(string albumTitle, string artistID)
        {
            int albumID;

            try
            {
                albumID = _albumAccessor.SelectAlbumIDFromTitle(albumTitle, artistID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("AlbumID not found", ex);
            }
            return albumID;
        }
        public List<Album> SelectAllAlbums()
        {
            List<Album> albums = new List<Album>();

            try
            {
                albums = _albumAccessor.SelectAllAlbums();
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
        public bool RemoveSongFromAlbum(int songID)
        {
            bool result = false;

            try
            {
                result = (1 == _albumAccessor.RemoveSongFromAlbum(songID));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Song not removed.", ex);
            }
            return result;
        }
    }
}
