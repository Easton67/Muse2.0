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
            catch (Exception)
            {

            }
            return songs;
        }
    }
}
