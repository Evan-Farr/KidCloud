﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KidCloudProject.Models;
using Microsoft.AspNet.Identity;

namespace KidCloudProject.Controllers
{
    public class ParentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Parents
        public ActionResult Index()
        {
            return View(db.Parents.ToList());
        }

        // GET: Parents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        public ActionResult PrivateDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        public ActionResult Application(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // GET: Parents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Age,StreetAddress,City,State,ZipCode,Phone,Email,NumberOfChildren,MoneyOwed")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                var holder = User.Identity.GetUserId();
                var same = db.Users.Where(s => s.Id == holder).FirstOrDefault();
                parent.UserId = same;
                db.Parents.Add(parent);
                db.SaveChanges();
                return RedirectToAction("Create", "Children");
            }

            return View(parent);
        }

        // GET: Parents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: Parents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Age,StreetAddress,City,State,ZipCode,Phone,Email,NumberOfChildren,MoneyOwed")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Users");
            }
            return View(parent);
        }

        // GET: Parents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: Parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Parent parent = db.Parents.Find(id);
            db.Parents.Remove(parent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Parents/Remove/5
        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveConfirmed(int id)
        {
            Parent parent = db.Parents.Find(id);
            var user = User.Identity.GetUserId();
            DayCare dayCare = db.DayCares.Where(a => a.UserId.Id == user).Select(s => s).FirstOrDefault();
            foreach(var child in dayCare.Children)
            {
                if (child.Parents.Contains(parent))
                {
                    dayCare.Children.Remove(child);
                }
            }
            dayCare.Parents.Remove(parent);
            parent.DayCare = null;
            db.SaveChanges();
            TempData["Message"] = "**The parent and their children have sucessfully been removed.";
            return RedirectToAction("Index", "Users");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ViewBalance(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        public ActionResult AddAnotherChild()
        {
            return View();
        }

        public ActionResult SendApplication(int? dayCareId)
        {
            Application application = new Application();
            var user = User.Identity.GetUserId();
            Parent parent = db.Parents.Where(a => a.UserId.Id == user).Select(s => s).FirstOrDefault();
            if (parent == null)
            {
                return RedirectToAction("Register", "Account"); 
            }
            var sendTo = db.DayCares.Where(k => k.Id == dayCareId).Select(p => p).FirstOrDefault();
            application.Parent = parent;
            application.DayCare = sendTo;
            application.Date = DateTime.Today;
            application.Status = "Pending";
            sendTo.PendingApplications.Add(application);
            parent.Applications.Add(application);
            db.SaveChanges();
            TempData["Message"] = "**Your application has successfully been sent!";
            return RedirectToAction("Index", "Users");
        }

        public ActionResult ViewChildren(int? id)
        {
            var parent = db.Parents.Where(i => i.Id == id).Select(a => a).FirstOrDefault();
            var children = new List<Child>();
            foreach (var child in parent.Children)
            {
                children.Add(child);
            }
            return View(children);
        }

        public ActionResult Calendar()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();
            var user = context.Parents.Where(i => i.UserId.Id == currentUserId).First();
            var dayCareEvents = context.Events.Where(d => d.DayCareId == user.DayCare.Id).Where(e => e.EventType == EventType.Activity).ToList();
            var parentEvents = context.Events.Where(p => p.UserId.Id == user.UserId.Id).ToList();
            foreach (Event item in parentEvents)
            {
                dayCareEvents.Add(item);
            }
            return View(dayCareEvents);
        }

    }
}
