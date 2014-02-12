using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SWAT.Models;
using PagedList;
using SWAT.ViewModels;

namespace SWAT.Controllers
{
    public class UserController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /User/
        public ActionResult Index(string sortOrder, string keywords, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.JoinedSortParm = sortOrder == "Joined" ? "Joined_desc" : "Joined";
            ViewBag.KeywordsParm = keywords;

            if (keywords != null)
            {
                page = 1;
            }
            else
            {
                keywords = currentFilter;
            }
            ViewBag.CurrentFilter = keywords;

            var users = db.Userids.Where(user => user.type == 0);

            if (!String.IsNullOrEmpty(keywords))
            {
                users = users.Where(user => user.Username.ToUpper().Contains(keywords.ToUpper()) || user.Last_Name.ToUpper().Contains(keywords.ToUpper())
                                    || user.First_Name.ToUpper().Contains(keywords.ToUpper()) || user.FullName.ToUpper().Contains(keywords.ToUpper()));
            }

            if (sortOrder == "Name_desc")
            {
                users = users.OrderByDescending(user => user.Username);
            }
            else if (sortOrder == "Joined")
            {
                users = users.OrderBy(user => user.Joined);
            }
            else if (sortOrder == "Joined_desc")
            {
                users = users.OrderByDescending(user => user.Joined);
            }
            else
            {
                users = users.OrderBy(user => user.Username);
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(users.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            var data = from country in db.lkpSubnationals
                       group country by country.lkpCountry.Name
                           into countryGroup
                           select new CountrySubnationalsGroup()
                               {
                                   CountryName = countryGroup.Key,
                                   SubNationalCount = countryGroup.Count()
                               };
            return View(data);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}

        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userid userid = db.Userids.Find(id);
            if (userid == null)
            {
                return HttpNotFound();
            }
            return View(userid);
        }

        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Userid1,Username,Password,eMail,Notes,SessionVars,secondaryId,type,properties,targetName,NotifStatus,NotifCode,NotifReqCodeTime,NotifActCodeTime,PMEmailNotificationsEnabled,UseOntarioMaps,First_Name,Last_Name,Address,utmX,utmY,Joined,Interests,Workplace,SecretQuestion,SecretAnswer,AvatarUserPhotoID,LastSuggestedContentDate,FullName")] Userid userid)
        {
            if (ModelState.IsValid)
            {
                db.Userids.Add(userid);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userid);
        }

        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userid userid = db.Userids.Find(id);
            if (userid == null)
            {
                return HttpNotFound();
            }
            return View(userid);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Userid1,Username,Password,eMail,Notes,SessionVars,secondaryId,type,properties,targetName,NotifStatus,NotifCode,NotifReqCodeTime,NotifActCodeTime,PMEmailNotificationsEnabled,UseOntarioMaps,First_Name,Last_Name,Address,utmX,utmY,Joined,Interests,Workplace,SecretQuestion,SecretAnswer,AvatarUserPhotoID,LastSuggestedContentDate,FullName")] Userid userid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userid).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userid);
        }

        // GET: /User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Userid userid = db.Userids.Find(id);
            if (userid == null)
            {
                return HttpNotFound();
            }
            return View(userid);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Userid userid = db.Userids.Find(id);
            db.Userids.Remove(userid);
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
