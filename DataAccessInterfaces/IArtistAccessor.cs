using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IArtistAccessor
    {
        Artist SelectArtistByArtistID(string artistID);
        List<Song> SelectSongsByArtistID(string ArtistID);
        List<Artist> SelectAllArtists();
    }
}
