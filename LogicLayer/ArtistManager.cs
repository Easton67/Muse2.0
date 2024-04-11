using DataAccessInterfaces;
using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class ArtistManager : IArtistManager
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
        public List<Song> SelectSongsByArtistID(string ArtistID)
        {
            List<Song> songs = new List<Song>();

            try
            {
                songs = _artistAccessor.SelectSongsByArtistID(ArtistID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Playlist not found", ex);
            }
            return songs;
        }

        public List<Artist> SelectAllArtists()
        {
            List<Artist> artists = new List<Artist>();

            try
            {
                artists = _artistAccessor.SelectAllArtists();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Artists not found", ex);
            }
            return artists;
        }
    }
}
