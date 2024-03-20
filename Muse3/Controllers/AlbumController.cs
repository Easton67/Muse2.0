using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Muse3.Controllers
{
    public class AlbumController : Controller
    {
        private AlbumManager _albumManager = new AlbumManager();
        List<Album> albums = new List<Album>();
        // GET: Album
        public ActionResult Albums()
        {
            try
            {
                albums = _albumManager.SelectAllAlbums();
            }
            catch (Exception)
            {

                throw;
            }
            return View(albums);
        }

        // GET: Album/Details/5
        public ActionResult Details(int id)
        {
            Album album = new Album();

            try
            {
                album = _albumManager.SelectAlbumByAlbumID(id);
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(album);
        }

        // GET: Album/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Album/Create
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

        // GET: Album/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Album/Edit/5
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

        // GET: Album/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Album/Delete/5
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
