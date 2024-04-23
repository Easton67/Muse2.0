using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Playlist
    {
        public int PlaylistID { get; set; }
        public string Title { get; set; }
        public string ImageFilePath { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoMimeType { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }
        public int? SongCount { get; set; }
    }
}
