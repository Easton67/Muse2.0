using DataAccessInterfaces;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessFakes
{
    public class ArtistAccessorFakes : IArtistAccessor
    {
        public Artist SelectArtistByArtistID(string artistID)
        {
            throw new NotImplementedException();
        }
    }
}
