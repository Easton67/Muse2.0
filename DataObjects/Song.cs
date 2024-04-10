using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Song
    {
        [DisplayName("Song ID")]
        public int SongID { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Album Art")]
        public string ImageFilePath { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoMimeType { get; set; }
        [DisplayName("Song File Location")]
        public string Mp3FilePath { get; set; }
        [DisplayName("Year Released")]
        public int YearReleased { get; set; }
        [DisplayName("Lyrics")]
        public string Lyrics { get; set; }
        [DisplayName("Explicit")]
        public bool Explicit { get; set; }
        [DisplayName("Genre")]
        public string Genre { get; set; }
        [DisplayName("Plays")]
        public int Plays { get; set; }
        [DisplayName("User ID")]
        public int UserID { get; set; }
        [DisplayName("Artist")]
        public string Artist { get; set; }
        [DisplayName("Album")]
        public string Album { get; set; }
        [DisplayName("Date Uploaded")]
        public DateTime? DateUploaded { get; set; }
        [DisplayName("Date Added")]
        public DateTime DateAdded { get; set; }
        [DisplayName("Favorite")]
        public bool isLiked { get; set; }
        [DisplayName("Public")]
        public bool isPublic { get; set; }

    }
}
