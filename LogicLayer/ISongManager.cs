﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace LogicLayer
{
    public interface ISongManager
    {
        List<Song> SelectSongsByUserID(int UserID);
        List<Song> SelectSongsByPlaylistID(int UserID, int PlaylistID);
        bool InsertSong(Song song);
        bool UpdateSong(Song oldSong, Song newSong);
        bool UpdatePlaysBySongID(int SongID, int Plays);
        bool UpdateFavoriteStatus(int SongID, bool newIsLiked);
        bool DeleteSong(int SongID);
        List<string> SelectAllGenres();
        Song SelectSongBySongID(int UserID, int SongID);
    }
}
