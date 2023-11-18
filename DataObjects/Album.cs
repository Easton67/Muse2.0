﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Album
    {
        public int AlbumID { get; set; }
        public string Title { get; set; }
        public string ImageFilePath { get; set; }
        public string Description { get; set; }
        public bool Explicit { get; set; }
        public bool Private { get; set; }
        public int Plays { get; set; }
    }
    public class AlbumVM : Album
    {
        public List<Song> AlbumSongs { get; set; }
    }
}
