using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Muse3.Controllers
{
    public class SongController : Controller
    {
        private SongManager _songManager = new SongManager();
        private UserManager _userManager = new UserManager();
        List<Song> songs = new List<Song>();

        public ActionResult Library(string searchText)
        {
            List<Song> songs = new List<Song>();

            try
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    songs = _songManager.SelectSongsByUserID(100001).Where(x => x.Title.ToLower().Contains(searchText)).ToList();
                }
                else
                {
                    songs = _songManager.SelectSongsByUserID(100001);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(songs);
        }

        // GET: Song/Details/5
        public ActionResult Details(int id)
        {
            Song song = null;

            try
            {
                song = _songManager.SelectSongBySongID(100001, id);
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(song);
        }

        // GET: Song/Create
        public ActionResult Create()
        {
            return View();
        }
        
        // POST: Song/Create
        [HttpPost]
        public ActionResult Create(Song song)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _songManager.InsertSong(song);
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.ToLower().Contains("primary"))
                {
                    ModelState.AddModelError("SongID", "Song already exists");
                }
                return View(song);
            }
        }

        // GET: Song/Edit/5
        public ActionResult Edit(int id)
        {
            Song song = null;

            try
            {
                song = _songManager.SelectSongBySongID(100001, id);
                Session["oldSong"] = song;
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(song);
        }

        // POST: Song/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Song song)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Song oldSong = (Song)Session["oldSong"];
                    _songManager.UpdateSong(oldSong, song);
                }

                return RedirectToAction("Library");
            }
            catch
            {
                return View(song);
            }
        }

        // GET: Song/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Song/Delete/5
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
