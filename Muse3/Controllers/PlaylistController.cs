using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult ViewAllPlaylists()
        {
            try
            {
                playlists = _playlistManager.SelectPlaylistsByUserID(100000);
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
            PlaylistDetailsViewModel viewModel = new PlaylistDetailsViewModel();

            try
            {
                viewModel.Playlist = _playlistManager.SelectPlaylistByUserID(100000, id);
                viewModel.Songs = _songManager.SelectSongsByPlaylistID(100001, viewModel.Playlist.PlaylistID);
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
                playlist = _playlistManager.SelectPlaylistByUserID(100000, id);
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
        public ActionResult Edit(int id, Playlist playlist)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Playlist oldPlaylist = (Playlist)Session["oldPlaylist"];
                    _playlistManager.UpdatePlaylist(oldPlaylist, playlist);
                }

                return RedirectToAction("ViewAllPlaylists");
            }
            catch
            {
                return View(playlist);
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
