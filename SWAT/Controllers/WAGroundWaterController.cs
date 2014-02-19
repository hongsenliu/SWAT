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
    public class WAGroundWaterController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /WAGroundWater/
        public ActionResult Index()
        {
            var tblswatwagroundwaters = db.tblSWATWAgroundWaters.Include(t => t.lkpSWATgwAvailabilityLU).Include(t => t.lkpSWATYesNoLU).Include(t => t.tblSWATSurvey);
            return View(tblswatwagroundwaters.ToList());
        }

        // GET: /WAGroundWater/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAgroundWater tblswatwagroundwater = db.tblSWATWAgroundWaters.Find(id);
            if (tblswatwagroundwater == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwagroundwater);
        }

        // GET: /WAGroundWater/Create
        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.gwAvailability = new SelectList(db.lkpSWATgwAvailabilityLUs, "id", "Description");
            ViewBag.gwReliability = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");
            return View();
        }

        private void updateScores(tblSWATWAgroundWater tblswatwagroundwater)
        {
            if (tblswatwagroundwater.gwAvailability != null)
            {
                int? availabilityOrder = db.lkpSWATgwAvailabilityLUs.Find(tblswatwagroundwater.gwAvailability).intorder;
                double? availabilityScore = Double.Parse(db.lkpSWATscores_gwAvailability.Single(e => e.intorder == availabilityOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwagroundwater.SurveyID && e.VarName == "gwAvailabilitySCORE").Value = availabilityScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwagroundwater.SurveyID && e.VarName == "gwAvailabilitySCORE").Value = null;
            }

            if (tblswatwagroundwater.gwReliability != null)
            {
                int? reliabilityOrder = db.lkpSWATYesNoLUs.Find(tblswatwagroundwater.gwReliability).intorder;
                double? reliabilityScore = Double.Parse(db.lkpSWATscores_YesNoLUYesGood.Single(e => e.intorder == reliabilityOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwagroundwater.SurveyID && e.VarName == "gwReliabilitySCORE").Value = reliabilityScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwagroundwater.SurveyID && e.VarName == "gwReliabilitySCORE").Value = null;
            }
            db.SaveChanges();

        }

        // POST: /WAGroundWater/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SurveyID,gwAvailability,gwReliability")] tblSWATWAgroundWater tblswatwagroundwater)
        {
            if (ModelState.IsValid)
            {
                var groundWaterIDs = db.tblSWATWAgroundWaters.Where(e => e.SurveyID == tblswatwagroundwater.SurveyID).Select(e => e.ID);
                if (groundWaterIDs.Any())
                {
                    int groundWaterId = groundWaterIDs.First();
                    tblswatwagroundwater.ID = groundWaterId;
                    db.Entry(tblswatwagroundwater).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatwagroundwater);
                    // return RedirectToAction("Index");
                    return RedirectToAction("Create", "CCEducation", new { SurveyID = tblswatwagroundwater.SurveyID });
                }

                db.tblSWATWAgroundWaters.Add(tblswatwagroundwater);
                db.SaveChanges();
                updateScores(tblswatwagroundwater);
                return RedirectToAction("Create", "CCEducation", new { SurveyID = tblswatwagroundwater.SurveyID });
                // return RedirectToAction("Index");
            }

            ViewBag.gwAvailability = new SelectList(db.lkpSWATgwAvailabilityLUs, "id", "Description", tblswatwagroundwater.gwAvailability);
            ViewBag.gwReliability = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatwagroundwater.gwReliability);
            return View(tblswatwagroundwater);
        }

        // GET: /WAGroundWater/Edit/5
        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAgroundWater tblswatwagroundwater = db.tblSWATWAgroundWaters.Find(id);
            if (tblswatwagroundwater == null)
            {
                return HttpNotFound();
            }
            ViewBag.gwAvailability = new SelectList(db.lkpSWATgwAvailabilityLUs, "id", "Description", tblswatwagroundwater.gwAvailability);
            ViewBag.gwReliability = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatwagroundwater.gwReliability);
            return View(tblswatwagroundwater);
        }

        // POST: /WAGroundWater/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SurveyID,gwAvailability,gwReliability")] tblSWATWAgroundWater tblswatwagroundwater)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatwagroundwater).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatwagroundwater);

                // If there is not any CCEducation with the current survey (SurveyID) then create one and redirect to its edit link.
                var educations = db.tblSWATCCedus.Where(e => e.SurveyID == tblswatwagroundwater.SurveyID);
                if (!educations.Any())
                {
                    tblSWATCCedu tblswatccedu = new tblSWATCCedu();
                    tblswatccedu.SurveyID = tblswatwagroundwater.SurveyID;
                    db.tblSWATCCedus.Add(tblswatccedu);
                    db.SaveChanges();

                    int newEducationID = tblswatccedu.ID;
                    return RedirectToAction("Edit", "CCEducation", new { id = newEducationID, SurveyID = tblswatccedu.SurveyID });
                }
                else
                {
                    return RedirectToAction("Edit", "CCEducation", new { id = educations.Single(e => e.SurveyID == tblswatwagroundwater.SurveyID).ID, SurveyID = tblswatwagroundwater.SurveyID });
                }

                //return RedirectToAction("Index");
            }
            ViewBag.gwAvailability = new SelectList(db.lkpSWATgwAvailabilityLUs, "id", "Description", tblswatwagroundwater.gwAvailability);
            ViewBag.gwReliability = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatwagroundwater.gwReliability);
            return View(tblswatwagroundwater);
        }

        // GET: /WAGroundWater/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAgroundWater tblswatwagroundwater = db.tblSWATWAgroundWaters.Find(id);
            if (tblswatwagroundwater == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwagroundwater);
        }

        // POST: /WAGroundWater/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATWAgroundWater tblswatwagroundwater = db.tblSWATWAgroundWaters.Find(id);
            db.tblSWATWAgroundWaters.Remove(tblswatwagroundwater);
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
