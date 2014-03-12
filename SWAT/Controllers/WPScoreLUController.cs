using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SWAT.Models;

namespace SWAT.Controllers
{
    public class WPScoreLUController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /WPScoreLU/

        public ActionResult Index()
        {
            var lkpswatwpscorelus = db.lkpSWATWPscoreLUs.Include(l => l.lkpSWATWPsectionLU);
            return View(lkpswatwpscorelus.ToList());
        }

        //
        // GET: /WPScoreLU/Details/5

        public ActionResult Details(int id = 0)
        {
            lkpSWATWPscoreLU lkpswatwpscorelu = db.lkpSWATWPscoreLUs.Find(id);
            if (lkpswatwpscorelu == null)
            {
                return HttpNotFound();
            }
            return View(lkpswatwpscorelu);
        }

        //
        // GET: /WPScoreLU/Create

        public ActionResult Create()
        {
            ViewBag.SectionID = new SelectList(db.lkpSWATWPsectionLUs, "ID", "SectionName");
            return View();
        }

        //
        // POST: /WPScoreLU/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(lkpSWATWPscoreLU lkpswatwpscorelu)
        {
            if (ModelState.IsValid)
            {
                db.lkpSWATWPscoreLUs.Add(lkpswatwpscorelu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SectionID = new SelectList(db.lkpSWATWPsectionLUs, "ID", "SectionName", lkpswatwpscorelu.SectionID);
            return View(lkpswatwpscorelu);
        }

        //
        // GET: /WPScoreLU/Edit/5

        public ActionResult Edit(int id = 0)
        {
            lkpSWATWPscoreLU lkpswatwpscorelu = db.lkpSWATWPscoreLUs.Find(id);
            if (lkpswatwpscorelu == null)
            {
                return HttpNotFound();
            }
            ViewBag.SectionID = new SelectList(db.lkpSWATWPsectionLUs, "ID", "SectionName", lkpswatwpscorelu.SectionID);
            return View(lkpswatwpscorelu);
        }

        //
        // POST: /WPScoreLU/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(lkpSWATWPscoreLU lkpswatwpscorelu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lkpswatwpscorelu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SectionID = new SelectList(db.lkpSWATWPsectionLUs, "ID", "SectionName", lkpswatwpscorelu.SectionID);
            return View(lkpswatwpscorelu);
        }

        //
        // GET: /WPScoreLU/Delete/5

        public ActionResult Delete(int id = 0)
        {
            lkpSWATWPscoreLU lkpswatwpscorelu = db.lkpSWATWPscoreLUs.Find(id);
            if (lkpswatwpscorelu == null)
            {
                return HttpNotFound();
            }
            return View(lkpswatwpscorelu);
        }

        //
        // POST: /WPScoreLU/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            lkpSWATWPscoreLU lkpswatwpscorelu = db.lkpSWATWPscoreLUs.Find(id);
            db.lkpSWATWPscoreLUs.Remove(lkpswatwpscorelu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}