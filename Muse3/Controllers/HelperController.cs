using DataObjects;
using LogicLayer;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Muse3.Controllers
{
    public class HelperController : Controller
    {
        private ArtistManager _artistManager = new ArtistManager();
        private SongManager _songManager = new SongManager();
        private AlbumManager _albumManager = new AlbumManager();
        private PlaylistManager _playlistManager = new PlaylistManager();
        private UserManager _userManager = new UserManager();

        // GET: Helper
        public ActionResult Index()
        {
            return View();
        }

        public int GetUserID()
        {
            var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = _userManager.FindByEmail(User.Identity.GetUserName());
            return (int)user.UserID;
        }

        public FileContentResult GetArtistPhoto(string artistID)
        {
            try
            {
                Artist artist = _artistManager.SelectArtistByArtistID(artistID);
                if (artist?.Photo != null && artist?.PhotoMimeType != null)
                {
                    return File(artist.Photo, artist.PhotoMimeType);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public FileContentResult GetSongPhoto(int songID)
        {
            try
            {
                Song song = _songManager.SelectSongBySongID(GetUserID(), songID);
                if (song?.Photo != null && song?.PhotoMimeType != null)
                {
                    return File(song.Photo, song.PhotoMimeType);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public FileContentResult GetAlbumPhoto(int albumID)
        {
            try
            {
                Album album = _albumManager.SelectAlbumByAlbumID(albumID);
                if (album.Photo != null && album.PhotoMimeType != null)
                {
                    return File(album.Photo, album.PhotoMimeType);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public FileContentResult GetPlaylistPhoto(int playlistID)
        {
            try
            {
                Playlist playlist = _playlistManager.SelectPlaylistByUserID(GetUserID(), playlistID);
                if (playlist.Photo != null && playlist.PhotoMimeType != null)
                {
                    return File(playlist.Photo, playlist.PhotoMimeType);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public FileContentResult GetFriendPhoto(int FriendID)
        {
            try
            {
                User friend = _userManager.SelectAllUsers().FirstOrDefault(x => x.UserID == FriendID);

                string imagePath = Path.Combine(friend.ImageFilePath);

                if (System.IO.File.Exists(friend.ImageFilePath))
                {
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                    friend.Photo = imageBytes;
                    friend.PhotoMimeType = Path.GetExtension(friend.ImageFilePath);
                }

                if (friend?.Photo != null && friend?.PhotoMimeType != null)
                {
                    return File(friend.Photo, friend.PhotoMimeType);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}