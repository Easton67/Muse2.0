﻿using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessInterfaces
{
    public interface IPlaylistAccessor
    {
        int CreatePlaylist(Playlist newPlaylist);
        int InsertSongIntoPlaylist(int songID, int playlistID);
        List<Playlist> SelectPlaylistByUserID(int userId);
        int DeletePlaylist(int playlistID);
    }
}
