using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Song
    {
        public int SongID { get; set; }
        public string Title { get; set; }
        public string ImageFilePath { get; set; }
        public string Mp3FilePath { get; set; }
        public int YearReleased { get; set; }
        public string Lyrics { get; set; }
        public bool Private { get; set; }
        public bool Explicit { get; set; }
        public int Plays { get; set; }
        public int CreatedBy { get; set; }
    }
}
