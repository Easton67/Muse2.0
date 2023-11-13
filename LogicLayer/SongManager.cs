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

        public List<Song> SelectSongsByProfileName(string ProfileName)
        {
            List<Song> songs = new List<Song>();

            try
            {
                songs = _songAccessor.SelectSongsByProfileName(ProfileName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Library not found", ex);
            }
            return songs;
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

        public bool UpdatePlaysBySongID(int SongID, int Plays)
        {
            bool result = false;

            try
            {
                result = (1 == _songAccessor.UpdatePlaysBySongID(SongID, Plays));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Song's plays not found.", ex);
            }

            return result;
        }
    }
}
