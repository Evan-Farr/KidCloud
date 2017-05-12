using System;
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
    public class DailyReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DailyReports
        public ActionResult Index()
        {
            return View(db.DailyReports.ToList());
        }

        // GET: DailyReports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyReport dailyReport = db.DailyReports.Find(id);
            if (dailyReport == null)
            {
                return HttpNotFound();
            }
            return View(dailyReport);
        }

        // GET: DailyReports/Create
        //public ActionResult Create(DayCare dayCare)
        //{
        //    var DayCare = db.DayCares.Where(d => d.Id == dayCare.Id).Select(s => s).FirstOrDefault();
        //    foreach(var kid in DayCare.Children)
        //    {
        //        if()
        //    }
        //    return View(child);
        //}

        // POST: DailyReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ReportDate,BathroomUse,Meals,Sleep,ActivityReport,SuppliesNeeds,Mood,MiscellaneousNotes")] DailyReport dailyReport, Child child)
        {
            if (ModelState.IsValid)
            {
                dailyReport.ReportDate = DateTime.Today;
                var user = User.Identity.GetUserId();
                DayCare dayCare = db.DayCares.Where(u => u.UserId.Id == user).Select(s => s).FirstOrDefault();
                dailyReport.DayCareId = dayCare;
                dailyReport.ChildId = child;
                dayCare.DailyReports.Add(dailyReport);
                db.DailyReports.Add(dailyReport);
                db.SaveChanges();
                TempData["Message"] = "**Daily report successfully created and saved.";
                return RedirectToAction("Index", "Users");
            }
            TempData["ErrorMessage"] = "**An error occured while saving the report.";
            return RedirectToAction("Index", "Users");
        }

        // GET: DailyReports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyReport dailyReport = db.DailyReports.Find(id);
            if (dailyReport == null)
            {
                return HttpNotFound();
            }
            return View(dailyReport);
        }

        // POST: DailyReports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ReportDate,BathroomUse,Meals,Sleep,ActivityReport,SuppliesNeeds,Mood,MiscellaneousNotes")] DailyReport dailyReport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dailyReport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Users");
            }
            return View(dailyReport);
        }

        // GET: DailyReports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DailyReport dailyReport = db.DailyReports.Find(id);
            if (dailyReport == null)
            {
                return HttpNotFound();
            }
            return View(dailyReport);
        }

        // POST: DailyReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DailyReport dailyReport = db.DailyReports.Find(id);
            db.DailyReports.Remove(dailyReport);
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
