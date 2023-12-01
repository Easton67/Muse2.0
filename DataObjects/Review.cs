using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public int UserID { get; set; }
        public int SongID { get; set; }
        public int AlbumID { get; set; }
        public Song ReviewedSong { get; set; }


    }
}
