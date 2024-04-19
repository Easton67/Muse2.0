using DataObjects;
using LogicLayer;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Muse3.Controllers
{
    public class HelperController : Controller
    {
        private ArtistManager _artistManager = new ArtistManager();
        private SongManager _songManager = new SongManager();
        private AlbumManager _albumManager = new AlbumManager();

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
    }
}