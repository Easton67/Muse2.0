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
            throw new NotImplementedException();
        }
    }
}
