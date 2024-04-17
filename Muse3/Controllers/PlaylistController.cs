using DataObjects;
using LogicLayer;
using Microsoft.AspNet.Identity;
using Muse3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;

namespace Muse3.Controllers
{
    public class PlaylistDetailsViewModel
    {
        public Playlist Playlist { get; set; }
        public List<Song> Songs { get; set; }
    }

    public class PlaylistController : Controller
    {
        private PlaylistManager _playlistManager = new PlaylistManager();
        private SongManager _songManager = new SongManager();
        private List<Playlist> playlists = new List<Playlist>();

        public int GetUserID()
        {
            var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var test = User.Identity.GetUserName();
            var user = _userManager.FindByEmail(test);
            return (int)user.UserID;
        }

        public FileContentResult GetPhoto(int PlaylistID)
        {
            try
            {
                Playlist playlist = _playlistManager.SelectPlaylistByUserID(GetUserID(), PlaylistID);
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

        public ActionResult ViewAllPlaylists()
        {
            try
            {
                playlists = _playlistManager.SelectPlaylistsByUserID(GetUserID());
            }
            catch (Exception)
            {

                throw;
            }
            return View(playlists);
        }

        // GET: Playlist/Details/5
        public ActionResult Details(int id)
        {
            int userID = GetUserID();
            PlaylistDetailsViewModel viewModel = new PlaylistDetailsViewModel();

            try
            {
                viewModel.Playlist = _playlistManager.SelectPlaylistByUserID(userID, id);
                viewModel.Songs = _songManager.SelectSongsByPlaylistID(userID, viewModel.Playlist.PlaylistID);
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(viewModel);
        }

        // GET: Playlist/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Playlist/Create
        [HttpPost]
        public ActionResult Create(Playlist playlist)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _playlistManager.CreatePlaylist(playlist);
                    return RedirectToAction("ViewAllPlaylists");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return View(playlist);
            }
        }

        // GET: Playlist/Edit/5
        public ActionResult Edit(int id)
        {
            Playlist playlist = null;

            try
            {
                playlist = _playlistManager.SelectPlaylistByUserID(GetUserID(), id);
                Session["oldPlaylist"] = playlist;
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(playlist);
        }

        // POST: Playlist/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Playlist playlist, HttpPostedFileBase image = null)
        {
            playlist.UserID = GetUserID();
            playlist.ImageFilePath = "Not implemented";

            if (ModelState.IsValid)
            {
                try
                {
                    HttpPostedFileBase photo = image;
                    string mimeType = null;
                    byte[] photoOut = null;

                    if (photo != null)
                    {
                        playlist.PhotoMimeType = photo.ContentType;
                        playlist.Photo = new byte[photo.ContentLength];
                        image.InputStream.Read(playlist.Photo, 0, photo.ContentLength);
                    }

                    Playlist oldPlaylist = (Playlist)Session["oldPlaylist"];
                    _playlistManager.UpdatePlaylist(oldPlaylist, playlist);

                    return RedirectToAction("ViewAllPlaylists");
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("ViewAllPlaylists");
            }
        }

        // GET: Playlist/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Playlist/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
