using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class ArtistManager
    {
        private IArtistAccessor _artistAccessor = null;
        public ArtistManager()
        {
            _artistAccessor = new ArtistAccessor();
        }
        public ArtistManager(IArtistAccessor artistAccessor)
        {
            _artistAccessor = artistAccessor;
        }

        public Artist SelectArtistByArtistID(string artistID)
        {
            Artist artist = new Artist();

            try
            {
                artist = _artistAccessor.SelectArtistByArtistID(artistID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Artist not found", ex);
            }
            return artist;
        }
        List<Song> SelectSongsByArtistID(int SongID, int ArtistID)
        {
            List<Song> songs = new List<Song>();

            try
            {
                songs = _artistAccessor.SelectSongsByArtistID(SongID, ArtistID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Playlist not found", ex);
            }
            return songs;
        }
    }
}
