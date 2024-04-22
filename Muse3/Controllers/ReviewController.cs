using DataObjects;
using LogicLayer;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Muse3.Controllers
{

    public class ReviewController : Controller
    {
        private ReviewManager _reviewManager = new ReviewManager();
        private SongManager _songManager = new SongManager();

        List<Review> reviews = new List<Review>();

        public int GetUserID()
        {
            var _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = _userManager.FindByEmail(User.Identity.GetUserName());
            return (int)user.UserID;
        }

        // GET: Review
        public ActionResult Reviews()
        {
            try
            {
                reviews = _reviewManager.SelectReviewsByUserID(GetUserID());
            }
            catch (Exception)
            {
                throw;
            }
            return View(reviews);
        }

        // GET: Review/Details/5
        public ActionResult Details(int id)
        {
            Review review = new Review();

            try
            {
                review = _reviewManager.SelectReviewByReviewID(GetUserID(), id);
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(review);
        }

        // GET: Review/Create
        public ActionResult CreateWithSongProvided(int songID)
        {
            Song song = new Song();
            Review review = new Review();
            
            try
            {
                song = _songManager.SelectSongBySongID(GetUserID(), songID);
                review.ReviewedSong = song;
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(review);
        }

        // POST: Review/Create
        [HttpPost]
        public ActionResult CreateWithSongProvided(Review review)
        {

            try
            {
                Song song = new Song();
                if (ModelState.IsValid)
                {
                    song = _songManager.SelectSongBySongID(GetUserID(), review.SongID);
                    review.ReviewedSong = song;
                    review.UserID = GetUserID();
                    _reviewManager.CreateReview(review);
                    return RedirectToAction("Reviews");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return View(review);
            }
        }

        // GET: Review/Edit/5
        public ActionResult Edit(int id)
        {
            Review review = null;

            try
            {
                review = _reviewManager.SelectReviewByReviewID(GetUserID(), id);
                Session["oldReview"] = review;
            }
            catch (Exception ex)
            {
                throw;
            }

            return View(review);
        }

        // POST: Review/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Review review)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Review oldReview = (Review)Session["oldReview"];
                    _reviewManager.UpdateReview(oldReview, review);
                }

                return RedirectToAction("Reviews");
            }
            catch
            {
                return View(review);
            }
        }

        // GET: Review/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Review/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Review review)
        {
            try
            {
                _reviewManager.DeleteReview(id);

                return RedirectToAction("Reviews");
            }
            catch
            {
                return RedirectToAction("Reviews");
            }
        }
    }
}
