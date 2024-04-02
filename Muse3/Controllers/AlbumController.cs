using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Muse3.Controllers
{
    public class AlbumDetailsViewModel
    {
        public Album album { get; set; }
        public List<Song> songs { get; set; }
    }
    public class AlbumController : Controller
    {
        private AlbumManager _albumManager = new AlbumManager();
        private SongManager _songManager = new SongManager();
        List<Album> albums = new List<Album>();

        // GET: Album
        public ActionResult Albums()
        {
            try
            {
                albums = _albumManager.SelectAllAlbums().Where(x => !x.Title.Equals("None") && !x.Title.Equals("Unknown")).ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return View(albums);
        }

        // GET: Album/Details/
        public ActionResult Details(int id)
        {
            AlbumDetailsViewModel viewModel = new AlbumDetailsViewModel();

            try
            {
                viewModel.album = _albumManager.SelectAlbumByAlbumID(id);
                viewModel.songs = _songManager.SelectSongsByUserID(100001).Where(x => x.Album.ToLower() == viewModel.album.Title.ToLower()).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(viewModel);
        }

        // GET: Album/Details
        public ActionResult DetailsFromArtistAndTitle(string albumTitle, string artistID)
        {
            AlbumDetailsViewModel viewModel = new AlbumDetailsViewModel();

            try
            {
                int albumID = _albumManager.SelectAlbumIDFromTitle(albumTitle, artistID);
                viewModel.album = _albumManager.SelectAlbumByAlbumID(albumID);
                viewModel.songs = _songManager.SelectSongsByUserID(100001).Where(x => x.Album.ToLower() == viewModel.album.Title.ToLower()).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(viewModel);
        }

        // GET: Album/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Album/Create
        [HttpPost]
        public ActionResult Create(Album album)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _albumManager.CreateAlbum(album);
                    return RedirectToAction("Albums");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return View(album);
            }
        }

        // GET: Album/Edit/5
        public ActionResult Edit(int id)
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
