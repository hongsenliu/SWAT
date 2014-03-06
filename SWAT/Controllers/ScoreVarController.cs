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
    public class ScoreVarController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /ScoreVar/

        public ActionResult Index()
        {
            var lkpswatscorevarslus = db.lkpSWATScoreVarsLUs.Include(l => l.lkpSWATSectionLU);
            return View(lkpswatscorevarslus.ToList());
        }

        //
        // GET: /ScoreVar/Details/5

        public ActionResult Details(int id = 0)
        {
            lkpSWATScoreVarsLU lkpswatscorevarslu = db.lkpSWATScoreVarsLUs.Find(id);
            if (lkpswatscorevarslu == null)
            {
                return HttpNotFound();
            }
            return View(lkpswatscorevarslu);
        }

        //
        // GET: /ScoreVar/Create

        public ActionResult Create()
        {
            ViewBag.SectionID = new SelectList(db.lkpSWATSectionLUs, "ID", "SectionName");
            return View();
        }

        //
        // POST: /ScoreVar/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(lkpSWATScoreVarsLU lkpswatscorevarslu)
        {
            if (ModelState.IsValid)
            {
                db.lkpSWATScoreVarsLUs.Add(lkpswatscorevarslu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SectionID = new SelectList(db.lkpSWATSectionLUs, "ID", "SectionName", lkpswatscorevarslu.SectionID);
            return View(lkpswatscorevarslu);
        }

        //
        // GET: /ScoreVar/Edit/5

        public ActionResult Edit(int id = 0)
        {
            lkpSWATScoreVarsLU lkpswatscorevarslu = db.lkpSWATScoreVarsLUs.Find(id);
            if (lkpswatscorevarslu == null)
            {
                return HttpNotFound();
            }
            ViewBag.SectionID = new SelectList(db.lkpSWATSectionLUs, "ID", "SectionName", lkpswatscorevarslu.SectionID);
            return View(lkpswatscorevarslu);
        }

        //
        // POST: /ScoreVar/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(lkpSWATScoreVarsLU lkpswatscorevarslu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lkpswatscorevarslu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SectionID = new SelectList(db.lkpSWATSectionLUs, "ID", "SectionName", lkpswatscorevarslu.SectionID);
            return View(lkpswatscorevarslu);
        }

        //
        // GET: /ScoreVar/Delete/5

        public ActionResult Delete(int id = 0)
        {
            lkpSWATScoreVarsLU lkpswatscorevarslu = db.lkpSWATScoreVarsLUs.Find(id);
            if (lkpswatscorevarslu == null)
            {
                return HttpNotFound();
            }
            return View(lkpswatscorevarslu);
        }

        //
        // POST: /ScoreVar/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            lkpSWATScoreVarsLU lkpswatscorevarslu = db.lkpSWATScoreVarsLUs.Find(id);
            db.lkpSWATScoreVarsLUs.Remove(lkpswatscorevarslu);
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