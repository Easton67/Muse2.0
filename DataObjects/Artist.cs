using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Artist
    {
        public string ArtistID { get; set; }
        public string ImageFilePath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public bool isLiked { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int UserID { get; set; }
    }
}
