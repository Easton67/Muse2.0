using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Muse3.Controllers
{
    public class PlaylistController : Controller
    {
        private PlaylistManager _playlistManager = new PlaylistManager();
        private List<Playlist> playlists = new List<Playlist>();
        public ActionResult ViewAllPlaylists()
        {
            try
            {
                playlists = _playlistManager.SelectPlaylistByUserID(100000);
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
            return View();
        }

        // GET: Playlist/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Playlist/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Playlist/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Playlist/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
