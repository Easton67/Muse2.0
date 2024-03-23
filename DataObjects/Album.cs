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
            [DisplayName("Artist")]
            public string ArtistID { get; set; }
            public bool isExplicit { get; set; }
            [DisplayName("Album Art")]
            public string ImageFilePath { get; set; }
            public string Description { get; set; }
            public int YearReleased { get; set; }
            public DateTime? DateAdded { get; set; }
    }
}
