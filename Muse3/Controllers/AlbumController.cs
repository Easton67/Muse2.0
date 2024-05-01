using DataObjects;
using LogicLayer;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

    [Authorize]
    public class AlbumController : Controller
    {
        private AlbumManager _albumManager = new AlbumManager();
        private SongManager _songManager = new SongManager();

        public int GetUserID()
        {
            var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = _userManager.FindByEmail(User.Identity.GetUserName());
            return (int)user.UserID;
        }

        // GET: Album
        public ActionResult Albums(string searchText)
        {
            List<Album> albums = new List<Album>();

            try
            {
                albums = _albumManager.SelectAlbumsByUserID(GetUserID()).Where(x => !x.Title.Equals("None") && !x.Title.Equals("Unknown")).ToList();
                if (!string.IsNullOrEmpty(searchText))
                {
                    albums = albums.Where(x => x.Title.ToLower().Contains(searchText.ToLower())).ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View(albums);
        }

        // GET: Album/Details/
        public ActionResult Details(int? id, string albumTitle, string artistID)
        {
            AlbumDetailsViewModel viewModel = new AlbumDetailsViewModel();

            try
            {
                if (!string.IsNullOrEmpty(albumTitle) && !string.IsNullOrEmpty(artistID))
                {
                    int albumID = _albumManager.SelectAlbumIDFromTitle(albumTitle, artistID);
                    viewModel.album = _albumManager.SelectAlbumByAlbumID(albumID);
                    viewModel.songs = _songManager.SelectSongsByUserID(GetUserID()).Where(x => x.Album.ToLower() == viewModel.album.Title.ToLower()).ToList();
                }
                if (id != null)
                {
                    viewModel.album = _albumManager.SelectAlbumByAlbumID((int)id);
                    viewModel.songs = _songManager.SelectSongsByUserID(GetUserID()).Where(x => x.Album.ToLower() == viewModel.album.Title.ToLower()).ToList();
                }
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
                Session["oldAlbum"] = album;
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(album);
        }

        // POST: Album/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Album album, HttpPostedFileBase imageFile)
        {
            try
             {
                HttpPostedFileBase photo = imageFile;
                string mimeType = null;
                byte[] photoOut = null;

                if (ModelState.IsValid)
                {
                    var oldAlbum = (Album)Session["oldAlbum"];
                    var newAlbum = new Album()
                    {
                        AlbumID = oldAlbum.AlbumID,
                        Title = album.Title,
                        Photo = oldAlbum.Photo,
                        PhotoMimeType = oldAlbum.PhotoMimeType,
                        ArtistID = album.ArtistID,
                        isExplicit = album.isExplicit,
                        ImageFilePath = (imageFile != null) ? Path.GetFileName(imageFile.FileName) : Path.GetFileName(oldAlbum.ImageFilePath),
                        Description = album.Description,
                        YearReleased = album.YearReleased,
                        UserID = album.UserID,
                        DateAdded = album.DateAdded
                    };

                    if (photo != null)
                    {
                        newAlbum.PhotoMimeType = photo.ContentType;
                        newAlbum.Photo = new byte[photo.ContentLength];
                        imageFile.InputStream.Read(newAlbum.Photo, 0, photo.ContentLength);
                    }

                    _albumManager.UpdateAlbum(oldAlbum, newAlbum);
                }
                return RedirectToAction("Albums");
            }
            catch
            {
                return View(album);
            }
        }

        // POST: Album/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Album album)
        {
            try
            {
                _albumManager.DeleteAlbum(id);

                return RedirectToAction("Albums");
            }
            catch
            {
                return RedirectToAction("Albums");
            }
        }
    }
}
