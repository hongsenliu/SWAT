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
    public class WAClimateChangeController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /WAClimateChange/
        public ActionResult Index()
        {
            var tblswatwaclimatechanges = db.tblSWATWAclimateChanges.Include(t => t.tblSWATSurvey);
            return View(tblswatwaclimatechanges.ToList());
        }

        // GET: /WAClimateChange/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAclimateChange tblswatwaclimatechange = db.tblSWATWAclimateChanges.Find(id);
            if (tblswatwaclimatechange == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwaclimatechange);
        }

        // GET: /WAClimateChange/Create
        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID");
            return View();
        }

        private void updateScores(tblSWATWAclimateChange tblswatwaclimatechange)
        {
            double climateDiffScore = 0;
            double climateChangeTrues = 0;

            bool[] climateChanges = {tblswatwaclimatechange.climateDryer, tblswatwaclimatechange.climateWetter, 
                                     tblswatwaclimatechange.climateColder, tblswatwaclimatechange.climateHotter,
                                     tblswatwaclimatechange.climateSeasons};
            foreach (bool climateChange in climateChanges)
            {
                if (climateChange)
                {
                    climateChangeTrues++;
                }
            }
            
            climateDiffScore = climateChangeTrues / 5.0;
            db.tblSWATScores.Single(e => e.SurveyID == tblswatwaclimatechange.SurveyID && e.VarName == "climateDiffSCORE").Value = climateDiffScore;
            db.SaveChanges();
        }

        // POST: /WAClimateChange/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SurveyID,climateDryer,climateWetter,climateColder,climateHotter,climateSeasons")] tblSWATWAclimateChange tblswatwaclimatechange)
        {
            if (ModelState.IsValid)
            {
                var climateChangeIDs = db.tblSWATWAclimateChanges.Where(e => e.SurveyID == tblswatwaclimatechange.SurveyID).Select(e => e.ID);
                if (climateChangeIDs.Any())
                {
                    int annualPrecipId = climateChangeIDs.First();
                    tblswatwaclimatechange.ID = annualPrecipId;
                    db.Entry(tblswatwaclimatechange).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatwaclimatechange);
                    return RedirectToAction("Index");
                    //return RedirectToAction("Create", "WAClimateChange", new { SurveyID = tblswatwaclimatechange.SurveyID });
                }
                
                db.tblSWATWAclimateChanges.Add(tblswatwaclimatechange);
                db.SaveChanges();
                updateScores(tblswatwaclimatechange);
                return RedirectToAction("Index");
            }

            ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID", tblswatwaclimatechange.SurveyID);
            return View(tblswatwaclimatechange);
        }

        // GET: /WAClimateChange/Edit/5
        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAclimateChange tblswatwaclimatechange = db.tblSWATWAclimateChanges.Find(id);
            if (tblswatwaclimatechange == null)
            {
                return HttpNotFound();
            }
            ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID", tblswatwaclimatechange.SurveyID);
            return View(tblswatwaclimatechange);
        }

        // POST: /WAClimateChange/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SurveyID,climateDryer,climateWetter,climateColder,climateHotter,climateSeasons")] tblSWATWAclimateChange tblswatwaclimatechange)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatwaclimatechange).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatwaclimatechange);

                return RedirectToAction("Index");
            }
            ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID", tblswatwaclimatechange.SurveyID);
            return View(tblswatwaclimatechange);
        }

        // GET: /WAClimateChange/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAclimateChange tblswatwaclimatechange = db.tblSWATWAclimateChanges.Find(id);
            if (tblswatwaclimatechange == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwaclimatechange);
        }

        // POST: /WAClimateChange/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATWAclimateChange tblswatwaclimatechange = db.tblSWATWAclimateChanges.Find(id);
            db.tblSWATWAclimateChanges.Remove(tblswatwaclimatechange);
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
