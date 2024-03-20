﻿using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Muse3.Controllers
{
    public class AdminController : Controller
    {
        private UserManager _userManager = new UserManager();
        private List<User> users = new List<User>();
        public ActionResult ViewAllUsers()
        {
            try
            {
                users = _userManager.SelectAllUsers();
            }
            catch (Exception)
            {   
                throw;
            }
            return View(users);
        }

        // GET: Admin/Details/5
        public ActionResult Details(string id)
        {
            User user = new User();

            try
            {
                user = _userManager.GetUserVMByEmail("67Easton@gmail.com");
            }
            catch (Exception ex)
            {

                throw;
            }

            return View(user);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
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

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
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

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
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
