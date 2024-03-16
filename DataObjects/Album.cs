using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Album
    {
        public int AlbumID { get; set; }
        public string Title { get; set; }
        [DisplayName("Album Art")]
        public string ImageFilePath { get; set; }
        public string Description { get; set; }
    }
    public class AlbumVM : Album
    {
        public Artist Artist { get; set; }
        [DisplayName("Track List")]
        public List<Song> AlbumSongs { get; set; }
    }
}
