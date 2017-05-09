using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KidCloudProject.Models;

namespace KidCloudProject.Controllers
{
    public class DayCaresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DayCares
        public ActionResult Index()
        {
            return View(db.DayCares.ToList());
        }

        // GET: DayCares/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DayCare dayCare = db.DayCares.Find(id);
            if (dayCare == null)
            {
                return HttpNotFound();
            }
            return View(dayCare);
        }

        // GET: DayCares/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DayCares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,OpenDate,StreetAddress,City,State,ZipCode,Phone,Email,AcceptChildrenUnderAgeTwo,AcceptDisabilites,MaxChildren,CurrentlyAcceptingApplicants")] DayCare dayCare)
        {
            if (ModelState.IsValid)
            {
                db.DayCares.Add(dayCare);
                db.SaveChanges();
                return RedirectToAction("Index", "Users");
            }

            return View(dayCare);
        }

        // GET: DayCares/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DayCare dayCare = db.DayCares.Find(id);
            if (dayCare == null)
            {
                return HttpNotFound();
            }
            return View(dayCare);
        }

        // POST: DayCares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,OpenDate,StreetAddress,City,State,ZipCode,Phone,Email,AcceptChildrenUnderAgeTwo,AcceptDisabilites,MaxChildren,CurrentlyAcceptingApplicants")] DayCare dayCare)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dayCare).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Users");
            }
            return View(dayCare);
        }

        // GET: DayCares/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DayCare dayCare = db.DayCares.Find(id);
            if (dayCare == null)
            {
                return HttpNotFound();
            }
            return View(dayCare);
        }

        // POST: DayCares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DayCare dayCare = db.DayCares.Find(id);
            db.DayCares.Remove(dayCare);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
