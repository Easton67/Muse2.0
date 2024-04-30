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

    [Authorize]
    public class PlaylistController : Controller
    {
        private PlaylistManager _playlistManager = new PlaylistManager();
        private SongManager _songManager = new SongManager();
        private List<Playlist> playlists = new List<Playlist>();
        private string imageFilesLocation = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\PlaylistImages";

        public int GetUserID()
        {
            var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = _userManager.FindByEmail(User.Identity.GetUserName());
            return (int)user.UserID;
        }

        public ActionResult ViewAllPlaylists()
        {
            try
            {
                playlists = _playlistManager.SelectPlaylistsByUserID(GetUserID());
                foreach (var playlist in playlists) 
                {
                    var playlistId = playlist.PlaylistID; 
                    playlist.SongCount = _songManager.SelectSongsByPlaylistID(GetUserID(), playlistId).Count();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(playlists);
        }

        public ActionResult AddSongToPlaylist(int playlistID)
        {
            List<Song> songs = new List<Song>();

            try
            {
                songs = _songManager.SelectSongsByUserID(GetUserID());
            }
            catch (Exception)
            {
                throw;
            }

            ViewBag.PlaylistID = playlistID;
            return View(songs);
        }

        [HttpPost]
        public ActionResult AddSongToPlaylist(int playlistID, int songID)
        {
            try
            {
                _playlistManager.InsertSongIntoPlaylist(songID, playlistID);
                return RedirectToAction("Details", "Playlist", new { id = playlistID });
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Playlist", "AddSongToPlaylist"));
            }

            return View();
        }
        
        [HttpPost]
        public ActionResult RemoveFromPlaylist(int songID, int playlistID)
        {
            List<Song> songs = new List<Song>();
            try
            {
                _playlistManager.RemoveSongFromPlaylist(songID);
                return RedirectToAction("Details", "Playlist", new { id = playlistID });
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Playlist", "RemoveFromPlaylist"));
            }

            return View();
        }

        // GET: Playlist/Details/5
        public ActionResult Details(int id)
        {
            PlaylistDetailsViewModel viewModel = new PlaylistDetailsViewModel();

            try
            {
                viewModel.Playlist = _playlistManager.SelectPlaylistByUserID(GetUserID(), id);
                viewModel.Songs = _songManager.SelectSongsByPlaylistID(GetUserID(), viewModel.Playlist.PlaylistID);
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
        public ActionResult Create(Playlist playlist, HttpPostedFileBase imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var image = Path.GetFileName(imageFile.FileName);
                    var imageFilePath = Path.Combine(imageFilesLocation, image);
                    imageFile.SaveAs(imageFilePath);

                    playlist.UserID = GetUserID();
                    playlist.SongCount = 0;
                    playlist.ImageFilePath = image;

                    // get the photo and the mime type from the file
                    HttpPostedFileBase photo = imageFile;
                    string mimeType = null;
                    byte[] photoOut = null;

                    if (photo != null)
                    {
                        playlist.PhotoMimeType = photo.ContentType;
                        playlist.Photo = new byte[photo.ContentLength];
                        imageFile.InputStream.Read(playlist.Photo, 0, photo.ContentLength);
                    }

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
