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
using System.Web.Routing;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KidCloudProject.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Event @event, DateTime StartDate, DateTime EndDate, DateTime StartTime, DateTime EndTime)
        {
            @event.start = StartDate.ToString("yyyy-MM-dd ") + StartTime.ToString("HH:mm:ss");
            @event.end = EndDate.ToString("yyyy-MM-dd ") + EndTime.ToString("HH:mm:ss");
            @event.editable = true;

            if (ModelState.IsValid)
            {
                var holder = User.Identity.GetUserId();
                var same = db.Users.Where(s => s.Id == holder).FirstOrDefault();
                @event.UserId = same;
                if (isUser("Parent"))
                {
                    var parentControl = db.Parents.Where(u => u.UserId.Id == holder).First();
                    @event.DayCareId = parentControl.DayCare.Id;
                    db.Events.Add(@event);
                    db.SaveChanges();
                    return RedirectToAction("Calendar", new RouteValueDictionary(new { controller = "Parents", action = "Calendar" }));
                }
                else if (isUser("DayCare"))
                {
                    var dayCareControl = db.DayCares.Where(u => u.UserId.Id == holder).First();
                    @event.DayCareId = dayCareControl.Id;
                    db.Events.Add(@event);
                    db.SaveChanges();
                    return RedirectToAction("Calendar", new RouteValueDictionary(
                        new { controller = "DayCares", action = "Calendar" }));
                }
            }

            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventId,title,allDay,start,end,editable,EventType")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
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
        private bool isUser(string role)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == role)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
