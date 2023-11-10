using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessInterfaces
{
    public interface ISongAccessor
    {
        List<Song> SelectSongsByProfileName(string ProfileName);
        int UpdatePlaysBySongID(int SongID, int Plays);
    }
}
