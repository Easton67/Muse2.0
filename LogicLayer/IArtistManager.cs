using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IArtistManager
    {
        Artist SelectArtistByArtistID(string artistID);
        List<Song> SelectSongsByArtistID(string artistID);
        List<Artist> SelectAllArtists();
    }
}
