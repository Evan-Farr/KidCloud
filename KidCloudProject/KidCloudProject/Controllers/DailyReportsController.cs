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

        public ActionResult PrivateIndex()
        {
            var user = User.Identity.GetUserId();
            DayCare dayCare = db.DayCares.Where(u => u.UserId.Id == user).Select(s => s).FirstOrDefault();
            var reports = new List<DailyReport>();
            var reports1 = db.DailyReports.Where(d => d.DayCareId.Id == dayCare.Id).Select(a => a).ToList();
            foreach (var report in reports1)
            {
                reports.Add(report);
            }
            return View(reports);
        }

        public ActionResult PrivateIndexByChild(string Children)
        {
            if(Children != "")
            {
                var childId = int.Parse(Children);
                Child child = db.Children.Where(d => d.Id == childId).Select(s => s).FirstOrDefault();
                var user = User.Identity.GetUserId();
                DayCare dayCare = db.DayCares.Where(u => u.UserId.Id == user).Select(s => s).FirstOrDefault();
                var reports1 = db.DailyReports.Where(d => d.ChildId.Id == child.Id).Select(a => a).ToList();
                var reports = new List<DailyReport>();
                foreach (var report in reports1)
                {
                    if (report.DayCareId == dayCare)
                    {
                        reports.Add(report);
                    }
                }
                return View(reports);
            }
            TempData["ErrorMessage1"] = "**Error: You did not select a child.";
            return RedirectToAction("Index", "Users");
        }

        public ActionResult ParentsIndexByChild(string Children)
        {
            if(Children != "")
            {
                var childId = int.Parse(Children);
                Child child = db.Children.Where(d => d.Id == childId).Select(s => s).FirstOrDefault();
                var user = User.Identity.GetUserId();
                Parent parent = db.Parents.Where(v => v.UserId.Id == user).Select(i => i).FirstOrDefault();
                var reports1 = db.DailyReports.Where(d => d.ChildId.Id == child.Id).Select(a => a).ToList();
                var reports = new List<DailyReport>();
                foreach (var report in reports1)
                {
                    if (child.Parents.Contains(parent))
                    {
                        reports.Add(report);
                    }
                }
                return View(reports);
            }
            TempData["ErrorMessage1"] = "**Error: You did not select a child.";
            return RedirectToAction("Index", "Users");
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

        public ActionResult Create(string Children)
        {
            if(Children != "")
            {
                var childId = int.Parse(Children);
                Child child = db.Children.Where(d => d.Id == childId).Select(s => s).FirstOrDefault();
                DailyReport report = new DailyReport();
                report.ChildId = child;
                return View(report);
            }
            TempData["ErrorMessage2"] = "**Error: You did not select a child.";
            return RedirectToAction("Index", "Users");
        }

        // POST: DailyReports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ReportDate,BathroomUse,Meals,Sleep,ActivityReport,SuppliesNeeds,Mood,MiscellaneousNotes")] DailyReport dailyReport, string Children)
        {
            if (ModelState.IsValid)
            {
                dailyReport.ReportDate = DateTime.Today;
                var user = User.Identity.GetUserId();
                DayCare dayCare = db.DayCares.Where(u => u.UserId.Id == user).Select(s => s).FirstOrDefault();
                dailyReport.DayCareId = dayCare;
                var childId = int.Parse(Children);
                Child child = db.Children.Where(d => d.Id == childId).Select(s => s).FirstOrDefault();
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
