using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuestBook2.Controllers
{
    [Authorize]
    public class GuestbookController : Controller
    {
        // GET: Guestbook
        private GuestBook2.Models.GuestbookContext _db = new GuestBook2.Models.GuestbookContext();
        public ActionResult Create()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            var mostRecentEntries =
                (from entry in _db.Entries
                 orderby entry.DateAdded descending
                 select entry).Take(20);
            ViewBag.Entries = mostRecentEntries.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(GuestBook2.Models.GuestBookEntry entry)
        {
            entry.DateAdded = DateTime.Now;
            _db.Entries.Add(entry);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ViewResult Show(int id)
        {
            var entry = _db.Entries.Find(id);
            bool hasPermission = User.Identity.Name == entry.Name;
            ViewData["hasPermission"] = hasPermission;
            return View(entry);
        }

        [AllowAnonymous]
        public ActionResult CommentSummary()
        {
            var entries = from entry in _db.Entries
                          group entry by entry.Name
                          into groupedByName
                          orderby groupedByName.Count() descending
                          select new GuestBook2.Models.CommentSummary
                          {
                              NumberOfComments =
                              groupedByName.Count(),
                              UserName = groupedByName.Key
                          };
            return View(entries.ToList());
        }

        public ActionResult Edit(int id)
        {
            var entry = _db.Entries.Find(id);
            if (User.Identity.Name == entry.Name)
            {
                return View(entry);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(GuestBook2.Models.GuestBookEntry entry)
        {
            var editEntry = _db.Entries.Find(entry.Id);
            if (User.Identity.Name == editEntry.Name) 
            { 
            editEntry.Message = entry.Message;
            _db.Entry(editEntry).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Comments(string userName)
        {
            var mostRecentEntriesPerUser =
                (from entry in _db.Entries
                 where entry.Name == userName
                 orderby entry.DateAdded descending
                 select entry).Take(20);
            ViewBag.Entries = mostRecentEntriesPerUser.ToList();
            ViewBag.UserName = userName;
            return View();
        }

        [AllowAnonymous]
        public ActionResult CommentsByDate(string userDate)
        {
            DateTime myDate = new DateTime();
            DateTime myUpToDate = new DateTime();

            if (!string.IsNullOrEmpty(userDate))
            {
                myDate = DateTime.Parse(userDate.Replace("!", ":"));
                myUpToDate = myDate.AddDays(1);
            }
            var entriesPerDate =
                (from entry in _db.Entries
                 where entry.DateAdded <= myUpToDate
                 orderby entry.Name descending
                 select entry
                 ).Take(20);
            ViewBag.Entries = entriesPerDate.ToList();
            return View();
        }

        public ActionResult Delete(int? id)
        {
            var entry = _db.Entries.Find(id);
            if (User.Identity.Name == entry.Name)
            {
                return View(entry);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(GuestBook2.Models.GuestBookEntry entry)
        {
            var editEntry = _db.Entries.Find(entry.Id);
            if (User.Identity.Name == editEntry.Name)
            {
                _db.Entries.Remove(editEntry);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}