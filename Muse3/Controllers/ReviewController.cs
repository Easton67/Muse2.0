using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Muse3.Controllers
{
    public class ReviewController : Controller
    {
        private ReviewManager _reviewManager = new ReviewManager();

        List<Review> reviews = new List<Review>();

        // GET: Review
        public ActionResult ViewAllReviews()
        {
            try
            {
                reviews = _reviewManager.SelectReviewsByUserID(100001);
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
                review = _reviewManager.SelectReviewByReviewID(100000, id);
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(review);
        }

        // GET: Review/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Review/Create
        [HttpPost]
        public ActionResult Create(Review review)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _reviewManager.CreateReview(review);
                    return RedirectToAction("ViewAllReviews");
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
                review = _reviewManager.SelectReviewByReviewID(100000, id);
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

                return RedirectToAction("ViewAllReviews");
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
