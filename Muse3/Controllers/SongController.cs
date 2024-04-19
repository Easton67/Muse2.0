using Antlr.Runtime.Misc;
using DataObjects;
using LogicLayer;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
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

        public int GetUserID()
        {
            var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = _userManager.FindByEmail(User.Identity.GetUserName());
            return (int)user.UserID;
        }

        public FileContentResult GetPhoto(int SongID)
        {
            try
            {
                Song song = _songManager.SelectSongBySongID(GetUserID(), SongID);
                if (song.Photo != null && song.PhotoMimeType != null)
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

        public ActionResult Library(string searchText, string ascOrDesc, string sortedProperty, string isFavorite, int? songID)
        {
            List<Song> songs = new List<Song>();

            try
            {
                songs = _songManager.SelectSongsByUserID(GetUserID());

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

                    songs = _songManager.SelectSongsByUserID(GetUserID());
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
                    songs = _songManager.SelectSongsByUserID(GetUserID());
                    songs = songs.Where(x => x.Title.ToLower().Contains(searchText.ToLower())).ToList();
                }
                else
                {
                    songs = _songManager.SelectSongsByUserID(GetUserID());
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
                song = _songManager.SelectSongBySongID(GetUserID(), id);
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

        public ActionResult CreateFromFolder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFromFolder(HttpPostedFileBase file)
        {
            //string[] subfolders = Directory.GetDirectories(file);
            string Muse3SongFilesDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent + "\\Muse3\\MuseConfig\\SongFiles";
            string Muse3AlbumArtDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent + "\\Muse3\\MuseConfig\\AlbumArt";
            
            int numberOfSongsAdded = 0;

            //foreach (string subfolder in subfolders)
            //{
                try
                {
                    //string[] songFiles = Directory.GetFiles(subfolder);

                    string album = "Unknown";
                    string genre = "Unknown"; 
                    int yearReleased = 0;
                    string artist = "Unknown";
                    string songTitle = "Unknown";
                    string imageFilePath = "defaultAlbumImage.png";
                    string lyrics = "No Lyrics Provided.";
                    bool exp = false;

                    string mp3FilePath = null;
                    string txtFilePath = null;

                    //foreach (string file in songFiles)
                    //{
                        string extension = Path.GetExtension(file.FileName).ToLower();
                        switch (extension)
                        {
                            case ".mp3":
                                mp3FilePath = file.FileName;
                                break;
                            case ".png":
                                imageFilePath = file.FileName;
                                break;
                            case ".txt":
                                if (file.FileName.Contains("information.txt"))
                                {
                                    try
                                    {
                                        string[] lines = System.IO.File.ReadAllLines(file.FileName);

                                        foreach (string line in lines)
                                        {
                                            if (line.StartsWith("Title:"))
                                            {
                                                songTitle = line.Substring("Title:".Length).Trim();
                                            }
                                            else if (line.StartsWith("Artist:"))
                                            {
                                                artist = line.Substring("Artist:".Length).Trim();
                                            }
                                            else if (line.StartsWith("Release Year:"))
                                            {
                                                if (line.Substring("Release Year:".Length).Trim().Equals("Release year not found"))
                                                {
                                                    yearReleased = 2023;
                                                }
                                                else
                                                {
                                                    yearReleased = (DateTime.ParseExact(line.Substring("Release Year:".Length).Trim(), "dd MMM yyyy, HH:mm", System.Globalization.CultureInfo.InvariantCulture)).Year;
                                                }
                                            }
                                            else if (line.StartsWith("Album Name:"))
                                            {
                                                album = line.Substring("Album Name:".Length).Trim();
                                            }
                                            else if (line.StartsWith("Genre:"))
                                            {
                                                genre = line.Substring("Genre:".Length).Trim();
                                            }
                                            else if (line.StartsWith("Explicit:"))
                                            {
                                                exp = bool.Parse(line.Substring("Explicit:".Length).Trim().ToLower());
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // Handle exception
                                    }
                                }
                                else
                                {
                                    if (new FileInfo(file.FileName).Length > 0)
                                    {
                                        lyrics = System.IO.File.ReadAllText(file.FileName);
                                    }
                                }
                                break;
                        }
                    //}

                    if (!string.IsNullOrEmpty(mp3FilePath))
                    {
                        try
                        {
                            System.IO.File.Copy(mp3FilePath, Path.Combine(songfilesLocation, Path.GetFileName(mp3FilePath)), true);
                            System.IO.File.Copy(mp3FilePath, Path.Combine(Muse3SongFilesDirectory, Path.GetFileName(mp3FilePath)), true);

                            string unsplitSongTitle = Path.GetFileNameWithoutExtension(mp3FilePath);
                            if (unsplitSongTitle.Contains(" - "))
                            {
                                string[] parts = unsplitSongTitle.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                                if (parts.Length == 2)
                                {
                                    songTitle = parts[1].Trim();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle exception
                        }
                    }

                    if (!string.IsNullOrEmpty(imageFilePath))
                    {
                        try
                        {
                            System.IO.File.Copy(imageFilePath, Path.Combine(Muse3AlbumArtDirectory, Path.GetFileName(imageFilePath)), true);
                        }
                        catch (Exception ex)
                        {
                            // Handle exception
                        }
                    }

                    try
                    {
                        var newSong = new Song()
                        {
                            DateAdded = DateTime.Now.Date,
                            Title = songTitle,
                            ImageFilePath = Path.GetFileName(imageFilePath),
                            Mp3FilePath = Path.GetFileName(mp3FilePath),
                            YearReleased = yearReleased,
                            Lyrics = lyrics,
                            Explicit = exp,
                            Genre = genre,
                            Plays = 0,
                            UserID = GetUserID(),
                            Album = album,
                            Artist = artist,
                        };

                        bool result = _songManager.InsertSong(newSong);
                        if (result == true)
                        {
                            numberOfSongsAdded++;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                catch (Exception)
                {

                }
            //}

            ViewBag.SuccessMessage = $"{numberOfSongsAdded} songs were added to your library.";
            ViewBag.ErrorMessage = "An error occurred while processing your request.";

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
                        UserID = GetUserID(),
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
                song = _songManager.SelectSongBySongID(GetUserID(), id);
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
                        UserID = GetUserID(),
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
