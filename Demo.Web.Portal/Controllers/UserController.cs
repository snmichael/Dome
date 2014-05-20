﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demo.Service;
using Demo.Services.Models;
using Demo.Web.Utility;

namespace Demo.Web.Portal.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            var cmd = new UserService();
            var list = cmd.Get();
            return View(list);
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(string name,string job)
        {
            try
            {
                var model=new User {Name=name,Job = new Job {Position = job}};
                var cmd = new UserService();
                cmd.Add(model);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /User/Edit/5

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

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /User/Delete/5

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