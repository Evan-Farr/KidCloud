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
using Twilio;
using Twilio.Rest.Chat.V2.Service;
using Twilio.Rest.Chat.V2.Service.Channel;
using KidCloudProject._APIs;

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

        public ActionResult IndexParents(int? id)
        {
            var dayCare = db.DayCares.Where(d => d.Id == id).Select(s => s).FirstOrDefault();
            var parents = new List<Parent>();
            foreach(var parent in dayCare.Parents)
            {
                parents.Add(parent);
            }
            return View(parents);
        }

        public ActionResult IndexChildren(int? id)
        {
            var dayCare = db.DayCares.Where(d => d.Id == id).Select(s => s).FirstOrDefault();
            var children = new List<Child>();
            foreach (var child in dayCare.Children)
            {
                children.Add(child);
            }
            return View(children);
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

        //This details is meant for the page where you can see details and apply 
        public ActionResult ApplyDetails(int? id)
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
        public ActionResult Create([Bind(Include = "Id,Name,OpenDate,StreetAddress,City,State,ZipCode,Phone,Email,AcceptChildrenUnderAgeTwo,AcceptDisabilites,MaxChildren,CurrentlyAcceptingApplicants,ChannelId,DailyRatePerChild")] DayCare dayCare)
        {
            if (ModelState.IsValid)
            {
                const string accountSid = "AC383fc67c15e32582075da33d541fd388";
                const string authToken = "7915107c0ef796ad3d5a51fe2cdd0552";
                const string serviceSid = "IS9acc1cbb2d874347a95cacebfc5cd5aa";

                var holder = User.Identity.GetUserId();
                var same = db.Users.Where(s => s.Id == holder).FirstOrDefault();
                dayCare.UserId = same;

                TwilioClient.Init(accountSid, authToken);
                var channel = ChannelResource.Create(serviceSid, friendlyName: dayCare.Name, type: ChannelResource.ChannelTypeEnum.Private);
                dayCare.ChannelId = channel.Sid;
                MemberResource.Create(serviceSid, channel.Sid, "Admin");
                MemberResource.Create(serviceSid, channel.Sid, dayCare.UserId.UserName);

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
        public ActionResult Edit([Bind(Include = "Id,Name,OpenDate,StreetAddress,City,State,ZipCode,Phone,Email,AcceptChildrenUnderAgeTwo,AcceptDisabilites,MaxChildren,CurrentlyAcceptingApplicants,ChannelId,DailyRatePerChild")] DayCare dayCare)
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

        public ActionResult FindDayCares(string zipCode)
        {
            if (zipCode == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(db.DayCares.Where(p => p.ZipCode == zipCode).ToList());
        }

        public ActionResult Calendar()
        {

            return View();
        }

        public ActionResult ViewPendingApplications()
        {
            var user = User.Identity.GetUserId();
            return View(db.DayCares.Where(u => u.UserId.Id == user).Select(s => s.PendingApplications).First().ToList());
        }

        public ActionResult AcceptApplication(int? applicationId)
        {
            var user = User.Identity.GetUserId();
            var application = db.Applications.Where(a => a.Id == applicationId).Select(p => p).FirstOrDefault();
            DayCare dayCare = db.DayCares.Where(u => u.UserId.Id == user).Select(s => s).FirstOrDefault();
            dayCare.Parents.Add(application.Parent);
            var kids = new List<Child>();
            foreach (var kid in application.Parent.Children)
            {
                dayCare.Children.Add(kid);
            }
            application.Status = "Approved";
            dayCare.PendingApplications.Remove(application);

            // Add parent to daycare group chat channel
            TwilioClient.Init(TwilioApiKeys.accountSid, TwilioApiKeys.authToken);
            MemberResource.Create(TwilioApiKeys.serviceSid, dayCare.ChannelId, application.Parent.UserId.UserName);

            db.SaveChanges();
            TempData["Message"] = "**Successfully added a new family to your day care!";
            return RedirectToAction("ViewPendingApplications");
        }

        public ActionResult DenyApplication(int? applicationId)
        {
            var user = User.Identity.GetUserId();
            var application = db.Applications.Where(a => a.Id == applicationId).Select(p => p).FirstOrDefault();
            DayCare dayCare = db.DayCares.Where(u => u.UserId.Id == user).Select(s => s).FirstOrDefault();
            dayCare.PendingApplications.Remove(application);
            application.Status = "Denied";
            db.SaveChanges();
            TempData["Message"] = "**Application has been removed from your pending applications.";
            return RedirectToAction("ViewPendingApplications");
        }

        public ActionResult ChooseChild()
        {
            return RedirectToAction("Create", "DailyReports", new { id = });
        }
    }
}