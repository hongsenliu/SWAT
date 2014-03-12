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
    public class WPQualityController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /WPQuality/

        public ActionResult Index()
        {
            var tblswatwpqualities = db.tblSWATWPqualities.Include(t => t.lkpSWATfaecalPathogensLU).Include(t => t.lkpSWATqualTreatedLU).Include(t => t.lkpSWATuserTreatedLU).Include(t => t.lkpSWATuserTreatmentMethodLU).Include(t => t.lkpSWATwaterTasteOdourLU).Include(t => t.lkpSWATwaterTasteOdourLU1).Include(t => t.lkpSWATwaterTurbidityLU).Include(t => t.tblSWATWPoverview);
            return View(tblswatwpqualities.ToList());
        }

        //
        // GET: /WPQuality/Details/5

        public ActionResult Details(long id = 0)
        {
            tblSWATWPquality tblswatwpquality = db.tblSWATWPqualities.Find(id);
            if (tblswatwpquality == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwpquality);
        }

        //
        // GET: /WPQuality/Create

        public ActionResult Create()
        {
            ViewBag.faecalPathogens = new SelectList(db.lkpSWATfaecalPathogensLUs, "id", "Description");
            ViewBag.qualTreated = new SelectList(db.lkpSWATqualTreatedLUs, "id", "Description");
            ViewBag.userTreated = new SelectList(db.lkpSWATuserTreatedLUs, "id", "Description");
            ViewBag.userTreatmentMethod = new SelectList(db.lkpSWATuserTreatmentMethodLUs, "id", "Description");
            ViewBag.waterOdour = new SelectList(db.lkpSWATwaterTasteOdourLUs, "id", "Description");
            ViewBag.waterTaste = new SelectList(db.lkpSWATwaterTasteOdourLUs, "id", "Description");
            ViewBag.waterTurbidity = new SelectList(db.lkpSWATwaterTurbidityLUs, "id", "Description");
            ViewBag.wpID = new SelectList(db.tblSWATWPoverviews, "ID", "wpname");
            return View();
        }

        //
        // POST: /WPQuality/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATWPquality tblswatwpquality)
        {
            if (ModelState.IsValid)
            {
                db.tblSWATWPqualities.Add(tblswatwpquality);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.faecalPathogens = new SelectList(db.lkpSWATfaecalPathogensLUs, "id", "Description", tblswatwpquality.faecalPathogens);
            ViewBag.qualTreated = new SelectList(db.lkpSWATqualTreatedLUs, "id", "Description", tblswatwpquality.qualTreated);
            ViewBag.userTreated = new SelectList(db.lkpSWATuserTreatedLUs, "id", "Description", tblswatwpquality.userTreated);
            ViewBag.userTreatmentMethod = new SelectList(db.lkpSWATuserTreatmentMethodLUs, "id", "Description", tblswatwpquality.userTreatmentMethod);
            ViewBag.waterOdour = new SelectList(db.lkpSWATwaterTasteOdourLUs, "id", "Description", tblswatwpquality.waterOdour);
            ViewBag.waterTaste = new SelectList(db.lkpSWATwaterTasteOdourLUs, "id", "Description", tblswatwpquality.waterTaste);
            ViewBag.waterTurbidity = new SelectList(db.lkpSWATwaterTurbidityLUs, "id", "Description", tblswatwpquality.waterTurbidity);
            ViewBag.wpID = new SelectList(db.tblSWATWPoverviews, "ID", "wpname", tblswatwpquality.wpID);
            return View(tblswatwpquality);
        }

        //
        // GET: /WPQuality/Edit/5

        public ActionResult Edit(long id = 0)
        {
            tblSWATWPquality tblswatwpquality = db.tblSWATWPqualities.Find(id);
            if (tblswatwpquality == null)
            {
                return HttpNotFound();
            }
            ViewBag.faecalPathogens = new SelectList(db.lkpSWATfaecalPathogensLUs, "id", "Description", tblswatwpquality.faecalPathogens);
            ViewBag.qualTreated = new SelectList(db.lkpSWATqualTreatedLUs, "id", "Description", tblswatwpquality.qualTreated);
            ViewBag.userTreated = new SelectList(db.lkpSWATuserTreatedLUs, "id", "Description", tblswatwpquality.userTreated);
            ViewBag.userTreatmentMethod = new SelectList(db.lkpSWATuserTreatmentMethodLUs, "id", "Description", tblswatwpquality.userTreatmentMethod);
            ViewBag.waterOdour = new SelectList(db.lkpSWATwaterTasteOdourLUs, "id", "Description", tblswatwpquality.waterOdour);
            ViewBag.waterTaste = new SelectList(db.lkpSWATwaterTasteOdourLUs, "id", "Description", tblswatwpquality.waterTaste);
            ViewBag.waterTurbidity = new SelectList(db.lkpSWATwaterTurbidityLUs, "id", "Description", tblswatwpquality.waterTurbidity);
            ViewBag.wpID = new SelectList(db.tblSWATWPoverviews, "ID", "wpname", tblswatwpquality.wpID);
            return View(tblswatwpquality);
        }

        //
        // POST: /WPQuality/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATWPquality tblswatwpquality)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatwpquality).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.faecalPathogens = new SelectList(db.lkpSWATfaecalPathogensLUs, "id", "Description", tblswatwpquality.faecalPathogens);
            ViewBag.qualTreated = new SelectList(db.lkpSWATqualTreatedLUs, "id", "Description", tblswatwpquality.qualTreated);
            ViewBag.userTreated = new SelectList(db.lkpSWATuserTreatedLUs, "id", "Description", tblswatwpquality.userTreated);
            ViewBag.userTreatmentMethod = new SelectList(db.lkpSWATuserTreatmentMethodLUs, "id", "Description", tblswatwpquality.userTreatmentMethod);
            ViewBag.waterOdour = new SelectList(db.lkpSWATwaterTasteOdourLUs, "id", "Description", tblswatwpquality.waterOdour);
            ViewBag.waterTaste = new SelectList(db.lkpSWATwaterTasteOdourLUs, "id", "Description", tblswatwpquality.waterTaste);
            ViewBag.waterTurbidity = new SelectList(db.lkpSWATwaterTurbidityLUs, "id", "Description", tblswatwpquality.waterTurbidity);
            ViewBag.wpID = new SelectList(db.tblSWATWPoverviews, "ID", "wpname", tblswatwpquality.wpID);
            return View(tblswatwpquality);
        }

        //
        // GET: /WPQuality/Delete/5

        public ActionResult Delete(long id = 0)
        {
            tblSWATWPquality tblswatwpquality = db.tblSWATWPqualities.Find(id);
            if (tblswatwpquality == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwpquality);
        }

        //
        // POST: /WPQuality/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tblSWATWPquality tblswatwpquality = db.tblSWATWPqualities.Find(id);
            db.tblSWATWPqualities.Remove(tblswatwpquality);
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