﻿using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Muse3.Controllers
{
    public class ArtistDetailsViewModel
    {
        public Artist artist { get; set; }
        public List<Song> songs { get; set; }
    }
    public class ArtistController : Controller
    {
        private ArtistManager _artistManager = new ArtistManager();
        private SongManager _songManager = new SongManager();
        // GET: Artist
        public ActionResult Artists()
        {
            List<Artist> artists = new List<Artist>();

            try
            {
                artists = _artistManager.SelectAllArtists();
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(artists);
        }

        // GET: Artist/Details/5
        public ActionResult Details(string id)
        {
            ArtistDetailsViewModel viewModel = new ArtistDetailsViewModel();

            try
            {
                viewModel.artist = _artistManager.SelectArtistByArtistID(id);
                viewModel.songs = _songManager.SelectSongsByUserID(100001).Where(song => song.Artist == viewModel.artist.ArtistID).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return View(viewModel);
        }

        // GET: Artist/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artist/Create
        [HttpPost]
        public ActionResult Create(Artist artist)
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

        // GET: Artist/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Artist/Edit/5
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

        // GET: Artist/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Artist/Delete/5
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
