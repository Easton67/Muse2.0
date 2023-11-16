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
        public bool UpdateAlbumBySongID(int SongID, string Album)
        {
            bool result = false;

            try
            {
                result = (1 == _songAccessor.UpdateAlbumBySongID(SongID, Album));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Album not accepted ", ex);
            }
            return result;
        }
        public bool UpdateArtistBySongID(int SongID, string Artist)
        {
            bool result = false;

            try
            {
                result = (1 == _songAccessor.UpdateArtistBySongID(SongID, Artist));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Artist not accepted ", ex);
            }
            return result;
        }
        public bool UpdateExplicitBySongID(int SongID, bool Explicit)
        {
            bool result = false;

            try
            {
                result = (1 == _songAccessor.UpdateExplicitBySongID(SongID, Explicit));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Status change not accepted ", ex);
            }
            return result;
        }
        public bool UpdateLyricsBySongID(int SongID, string Lyrics)
        {
            bool result = false;

            try
            {
                result = (1 == _songAccessor.UpdateLyricsBySongID(SongID, Lyrics));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Lyrics not accepted ", ex);
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
                throw new ApplicationException("Song's plays not found.", ex);
            }

            return result;
        }
        public bool UpdateSongImageBySongID(int SongID, string ImageFilePath)
        {
            bool result = false;

            try
            {
                result = (1 == _songAccessor.UpdateSongImageBySongID(SongID, ImageFilePath));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Song's image not found.", ex);
            }

            return result;
        }
        public bool UpdateTitleBySongID(int SongID, string Title)
        {
            bool result = false;

            try
            {
                result = (1 == _songAccessor.UpdateTitleBySongID(SongID, Title));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Song's title not found.", ex);
            }

            return result;
        }
        public bool UpdateYearBySongID(int SongID, int YearReleased)
        {
            bool result = false;

            try
            {
                result = (1 == _songAccessor.UpdateYearBySongID(SongID, YearReleased));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Song's title not found.", ex);
            }
            return result;
        }
    }
}
