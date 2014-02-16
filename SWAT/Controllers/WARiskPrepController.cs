using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SWAT.Models;

namespace SWAT.Controllers
{
    public class WARiskPrepController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /WARiskPrep/
        public ActionResult Index()
        {
            var tblswatwariskpreps = db.tblSWATWAriskPreps.Include(t => t.lkpSWATextremePrepLU).Include(t => t.lkpSWATextremePrepLU1).Include(t => t.lkpSWATextremePrepLU2).Include(t => t.lkpSWATextremeRiskLU).Include(t => t.lkpSWATextremeRiskLU1).Include(t => t.lkpSWATextremeRiskLU2).Include(t => t.tblSWATSurvey);
            return View(tblswatwariskpreps.ToList());
        }

        // GET: /WARiskPrep/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAriskPrep tblswatwariskprep = db.tblSWATWAriskPreps.Find(id);
            if (tblswatwariskprep == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwariskprep);
        }

        // GET: /WARiskPrep/Create
        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.prepFire = new SelectList(db.lkpSWATextremePrepLUs, "id", "Description");
            ViewBag.prepFlood = new SelectList(db.lkpSWATextremePrepLUs, "id", "Description");
            ViewBag.prepDrought = new SelectList(db.lkpSWATextremePrepLUs, "id", "Description");
            ViewBag.riskFire = new SelectList(db.lkpSWATextremeRiskLUs, "id", "Description");
            ViewBag.riskFlood = new SelectList(db.lkpSWATextremeRiskLUs, "id", "Description");
            ViewBag.riskDrought = new SelectList(db.lkpSWATextremeRiskLUs, "id", "Description");
            return View();
        }

        private void updateScores(tblSWATWAriskPrep tblswatwariskprep)
        {
            if (tblswatwariskprep.riskFire != null)
            {
                int? riskFireOrder = db.lkpSWATextremeRiskLUs.Find(tblswatwariskprep.riskFire).intorder;
                double? riskFireScore = Double.Parse(db.lkpSWATscores_extremeRISK.Single(e => e.intorder == riskFireOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwariskprep.SurveyID && e.VarName == "riskFireSCORE").Value = riskFireScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwariskprep.SurveyID && e.VarName == "riskFireSCORE").Value = null;
            }

            if (tblswatwariskprep.riskFlood != null)
            {
                int? riskFloodOrder = db.lkpSWATextremeRiskLUs.Find(tblswatwariskprep.riskFlood).intorder;
                double? riskFloodScore = Double.Parse(db.lkpSWATscores_extremeRISK.Single(e => e.intorder == riskFloodOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwariskprep.SurveyID && e.VarName == "riskFloodSCORE").Value = riskFloodScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwariskprep.SurveyID && e.VarName == "riskFloodSCORE").Value = null;
            }

            if (tblswatwariskprep.riskDrought != null)
            {
                int? riskDroughtOrder = db.lkpSWATextremeRiskLUs.Find(tblswatwariskprep.riskDrought).intorder;
                double? riskDroughtScore = Double.Parse(db.lkpSWATscores_extremeRISK.Single(e => e.intorder == riskDroughtOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwariskprep.SurveyID && e.VarName == "riskDroughtSCORE").Value = riskDroughtScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwariskprep.SurveyID && e.VarName == "riskDroughtSCORE").Value = null;
            }

            if (tblswatwariskprep.prepFire != null)
            {
                int? prepFireOrder = db.lkpSWATextremePrepLUs.Find(tblswatwariskprep.prepFire).intorder;
                double? prepFireScore = Double.Parse(db.lkpSWATscores_extremePrep.Single(e => e.intorder == prepFireOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwariskprep.SurveyID && e.VarName == "prepFireSCORE").Value = prepFireScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwariskprep.SurveyID && e.VarName == "prepFireSCORE").Value = null;
            }

            if (tblswatwariskprep.prepFlood!= null)
            {
                int? prepFloodOrder = db.lkpSWATextremePrepLUs.Find(tblswatwariskprep.prepFlood).intorder;
                double? prepFloodScore = Double.Parse(db.lkpSWATscores_extremePrep.Single(e => e.intorder == prepFloodOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwariskprep.SurveyID && e.VarName == "prepFloodSCORE").Value = prepFloodScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwariskprep.SurveyID && e.VarName == "prepFloodSCORE").Value = null;
            }

            if (tblswatwariskprep.prepDrought != null)
            {
                int? prepDroughtOrder = db.lkpSWATextremePrepLUs.Find(tblswatwariskprep.prepDrought).intorder;
                double? prepDroughtScore = Double.Parse(db.lkpSWATscores_extremePrep.Single(e => e.intorder == prepDroughtOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwariskprep.SurveyID && e.VarName == "prepDroughtSCORE").Value = prepDroughtScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwariskprep.SurveyID && e.VarName == "prepDroughtSCORE").Value = null;
            }

            db.SaveChanges();
        }

        // POST: /WARiskPrep/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SurveyID,riskFire,riskFlood,riskDrought,prepFire,prepFlood,prepDrought")] tblSWATWAriskPrep tblswatwariskprep)
        {
            if (ModelState.IsValid)
            {
                var riskPrepIDs = db.tblSWATWAriskPreps.Where(e => e.SurveyID == tblswatwariskprep.SurveyID).Select(e => e.ID);
                if (riskPrepIDs.Any())
                {
                    int riskPrepId = riskPrepIDs.First();
                    tblswatwariskprep.ID = riskPrepId;
                    db.Entry(tblswatwariskprep).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatwariskprep);
                    return RedirectToAction("Index");
                    //return RedirectToAction("Create", "WARiskPrep", new { SurveyID = tblswatwaextremeevent.SurveyID });
                }

                db.tblSWATWAriskPreps.Add(tblswatwariskprep);
                updateScores(tblswatwariskprep);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.prepFire = new SelectList(db.lkpSWATextremePrepLUs, "id", "Description", tblswatwariskprep.prepFire);
            ViewBag.prepFlood = new SelectList(db.lkpSWATextremePrepLUs, "id", "Description", tblswatwariskprep.prepFlood);
            ViewBag.prepDrought = new SelectList(db.lkpSWATextremePrepLUs, "id", "Description", tblswatwariskprep.prepDrought);
            ViewBag.riskFire = new SelectList(db.lkpSWATextremeRiskLUs, "id", "Description", tblswatwariskprep.riskFire);
            ViewBag.riskFlood = new SelectList(db.lkpSWATextremeRiskLUs, "id", "Description", tblswatwariskprep.riskFlood);
            ViewBag.riskDrought = new SelectList(db.lkpSWATextremeRiskLUs, "id", "Description", tblswatwariskprep.riskDrought);
            return View(tblswatwariskprep);
        }

        // GET: /WARiskPrep/Edit/5
        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAriskPrep tblswatwariskprep = db.tblSWATWAriskPreps.Find(id);
            if (tblswatwariskprep == null)
            {
                return HttpNotFound();
            }
            ViewBag.prepFire = new SelectList(db.lkpSWATextremePrepLUs, "id", "Description", tblswatwariskprep.prepFire);
            ViewBag.prepFlood = new SelectList(db.lkpSWATextremePrepLUs, "id", "Description", tblswatwariskprep.prepFlood);
            ViewBag.prepDrought = new SelectList(db.lkpSWATextremePrepLUs, "id", "Description", tblswatwariskprep.prepDrought);
            ViewBag.riskFire = new SelectList(db.lkpSWATextremeRiskLUs, "id", "Description", tblswatwariskprep.riskFire);
            ViewBag.riskFlood = new SelectList(db.lkpSWATextremeRiskLUs, "id", "Description", tblswatwariskprep.riskFlood);
            ViewBag.riskDrought = new SelectList(db.lkpSWATextremeRiskLUs, "id", "Description", tblswatwariskprep.riskDrought);
            return View(tblswatwariskprep);
        }

        // POST: /WARiskPrep/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SurveyID,riskFire,riskFlood,riskDrought,prepFire,prepFlood,prepDrought")] tblSWATWAriskPrep tblswatwariskprep)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatwariskprep).State = EntityState.Modified;
                updateScores(tblswatwariskprep);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.prepFire = new SelectList(db.lkpSWATextremePrepLUs, "id", "Description", tblswatwariskprep.prepFire);
            ViewBag.prepFlood = new SelectList(db.lkpSWATextremePrepLUs, "id", "Description", tblswatwariskprep.prepFlood);
            ViewBag.prepDrought = new SelectList(db.lkpSWATextremePrepLUs, "id", "Description", tblswatwariskprep.prepDrought);
            ViewBag.riskFire = new SelectList(db.lkpSWATextremeRiskLUs, "id", "Description", tblswatwariskprep.riskFire);
            ViewBag.riskFlood = new SelectList(db.lkpSWATextremeRiskLUs, "id", "Description", tblswatwariskprep.riskFlood);
            ViewBag.riskDrought = new SelectList(db.lkpSWATextremeRiskLUs, "id", "Description", tblswatwariskprep.riskDrought);
            return View(tblswatwariskprep);
        }

        // GET: /WARiskPrep/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAriskPrep tblswatwariskprep = db.tblSWATWAriskPreps.Find(id);
            if (tblswatwariskprep == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwariskprep);
        }

        // POST: /WARiskPrep/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATWAriskPrep tblswatwariskprep = db.tblSWATWAriskPreps.Find(id);
            db.tblSWATWAriskPreps.Remove(tblswatwariskprep);
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
