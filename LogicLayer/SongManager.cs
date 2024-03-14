using System;
using System.Collections.Generic;
using DataObjects;
using DataAccessInterfaces;
using DataAccessLayer;

namespace LogicLayer
{
    public class SongManager : ISongManager
    {
        private ISongAccessor _songAccessor = null;

        public SongManager()
        {
            _songAccessor = new SongAccessor();
        }
        public SongManager(ISongAccessor songAccessor)
        {
            _songAccessor = songAccessor;
        }
        public List<Song> SelectSongsByUserID(int UserID)
        {
            List<Song> songs = new List<Song>();

            try
            {
                songs = _songAccessor.SelectSongsByUserID(UserID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Library not found", ex);
            }
            return songs;
        }
        public List<Song> SelectSongsByPlaylistID(int UserID, int PlaylistID)
        {
            List<Song> songs = new List<Song>();

            try
            {
                songs = _songAccessor.SelectSongsByPlaylistID(UserID, PlaylistID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Playlist not found", ex);
            }
            return songs;
        }
        public List<string> SelectAllGenres()
        {
            List<string> genres = new List<string>();

            try
            {
                genres = _songAccessor.SelectAllGenres();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Playlist not found", ex);
            }
            return genres;
        }
        public bool InsertSong(Song newSong)
        {
            bool result = false;

            try
            {
                if (_songAccessor.InsertSong(newSong) >= 1)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Song not Added", ex);
            }
            return result;
        }
        public bool UpdateSong(Song oldSong, Song newSong)
        {
            bool result = false;

            try
            {
                result = (1 == _songAccessor.UpdateSong(oldSong, newSong));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Song not Added", ex);
            }
            return result;
        }
        public bool UpdatePlaysBySongID(int SongID, int Plays)
        {
            bool result = false;

            try
            {
                result = (1 == _songAccessor.UpdatePlaysBySongID(SongID, Plays));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Plays not updated.", ex);
            }
            return result;
        }
        public bool DeleteSong(int SongID)
        {
            bool result = false;

            try
            {
                result = (1 == _songAccessor.DeleteSong(SongID));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Song not deleted.", ex);
            }
            return result;
        }
    
    }
}
