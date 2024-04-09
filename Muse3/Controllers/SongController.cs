﻿using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.EnterpriseServices.CompensatingResourceManager;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;

namespace Muse3.Controllers
{
    public class SongController : Controller
    {
        private SongManager _songManager = new SongManager();
        private UserManager _userManager = new UserManager();
        private string songfilesLocation = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\SongFiles";
        private string imageFilesLocation = AppDomain.CurrentDomain.BaseDirectory + "MuseConfig\\AlbumArt";
        private List<Song> songs = new List<Song>();

        public ActionResult Library(string searchText, string ascOrDesc, string sortedProperty, string isFavorite, int? songID)
        {
            List<Song> songs = new List<Song>();

            try
            {
                songs = _songManager.SelectSongsByUserID(100001);
                if (!string.IsNullOrEmpty(searchText))
                {
                    songs = songs.Where(x => x.Title.ToLower().Contains(searchText.ToLower())).ToList();
                }
                if (sortedProperty != null && ascOrDesc != null)
                {
                    if (ascOrDesc == "asc")
                    {
                        songs = songs.OrderBy(x => x.GetType().GetProperty(sortedProperty).GetValue(x, null)).ToList();
                    }
                    else
                    {
                        songs = songs.OrderByDescending(x => x.GetType().GetProperty(sortedProperty).GetValue(x, null)).ToList();
                    }
                }
                if(!string.IsNullOrEmpty(isFavorite))
                {
                    bool favoriteOrUnfavorite = (isFavorite == "favorite") ? true : false;

                    _songManager.UpdateFavoriteStatus(songID.Value, favoriteOrUnfavorite);

                    songs = _songManager.SelectSongsByUserID(100001);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View(songs);
        }

        public ActionResult LibraryGridView(string searchText)
        {
            List<Song> songs = new List<Song>();

            try
            {
                if (!string.IsNullOrEmpty(searchText))
                {
                    songs = _songManager.SelectSongsByUserID(100001);
                    songs = songs.Where(x => x.Title.ToLower().Contains(searchText.ToLower())).ToList();
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
        public ActionResult Create(Song song, HttpPostedFileBase mp3File, HttpPostedFileBase imageFile)
        {
            try
            {
                // checking if files are there
                if (mp3File != null && imageFile != null)
                {
                    // copy image to museConfig
                    var mp3 = Path.GetFileName(mp3File.FileName);
                    var mp3FilePath = Path.Combine(songfilesLocation, mp3);
                    mp3File.SaveAs(mp3FilePath);

                    var image = Path.GetFileName(imageFile.FileName);
                    var imageFilePath = Path.Combine(imageFilesLocation, image);
                    imageFile.SaveAs(imageFilePath);

                    var newSong = new Song()
                    {
                        Title = song.Title,
                        ImageFilePath = image,
                        Mp3FilePath = mp3,
                        YearReleased = song.YearReleased,
                        Lyrics = song.Lyrics,
                        Explicit = song.Explicit,
                        Genre = song.Genre,
                        Plays = song.Plays,
                        UserID = 100001,
                        Artist = song.Artist,
                        Album = song.Album,
                        DateUploaded = null,
                        DateAdded = DateTime.Now,
                        isLiked = false,
                    };
                    if (ModelState.IsValid)
                    {
                        _songManager.InsertSong(newSong);
                        return RedirectToAction("Library");
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    return RedirectToAction("Library");
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
        public ActionResult Edit(int id, Song song, HttpPostedFileBase imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Song oldSong = (Song)Session["oldSong"];
                    var newSong = new Song()
                    {
                        Title = song.Title,
                        ImageFilePath = (imageFile != null) ? Path.GetFileName(imageFile.FileName) : Path.GetFileName(oldSong.ImageFilePath),
                        Mp3FilePath = oldSong.Mp3FilePath,
                        YearReleased = song.YearReleased,
                        Lyrics = song.Lyrics,
                        Explicit = song.Explicit,
                        Genre = song.Genre,
                        Plays = song.Plays,
                        UserID = 100001,
                        Artist = song.Artist,
                        Album = song.Album,
                        DateUploaded = null,
                        DateAdded = DateTime.Now,
                        isLiked = false,
                    };
                    _songManager.UpdateSong(oldSong, newSong);
                }
                return RedirectToAction("Library");
            }
            catch
            {
                return View(song);
            }
        }

        // POST: Song/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Song song)
        {
            try
            {
                _songManager.DeleteSong(id);

                return RedirectToAction("Library");
            }
            catch
            {
                return RedirectToAction("Library");
            }
        }
    }
}
