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
    public class WAannualPrecipController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /WAannualPrecip/
        public ActionResult Index()
        {
            var tblswatwaannualprecips = db.tblSWATWAannualPrecips.Include(t => t.lkpSWATprecipVarAltLU).Include(t => t.tblSWATSurvey);
            return View(tblswatwaannualprecips.ToList());
        }

        // GET: /WAannualPrecip/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAannualPrecip tblswatwaannualprecip = db.tblSWATWAannualPrecips.Find(id);
            if (tblswatwaannualprecip == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwaannualprecip);
        }

        // GET: /WAannualPrecip/Create
        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.precipVarALT = new SelectList(db.lkpSWATprecipVarAltLUs, "id", "Description");
            return View();
        }

        private void updateScores(tblSWATWAannualPrecip tblswatwaannualprecip)
        {
            // Update precipVarSCORE1 by giving standard deviation of annual precipitation.
            if (tblswatwaannualprecip.precipVar != null)
            {
                double? precipMean = db.tblSWATScores.Single(e => e.SurveyID == tblswatwaannualprecip.SurveyID && e.VarName == "precipTotal").Value;
                if (precipMean > 0)
                {
                    double? precipStdDivMean = tblswatwaannualprecip.precipVar.Value / precipMean;
                    var precipVardivMean = db.tblSWATScores.Single(e => e.SurveyID == tblswatwaannualprecip.SurveyID && e.VarName == "precipVardivMean");
                    precipVardivMean.Value = precipStdDivMean;
                    db.SaveChanges();
                    int? precipVardivMeanIntorder = null;

                    foreach (var item in db.lkpSWATprecipVardivMeanLUs.OrderByDescending(e => e.Description))
                    {
                        if (Double.Parse(item.Description) <= precipStdDivMean)
                        {
                            precipVardivMeanIntorder = item.intorder;
                            break;
                        }
                    }
                    if (precipVardivMeanIntorder != null)
                    {
                        double precipVarScore1Value = Double.Parse(db.lkpSWATscores_precipVar.Single(e => e.intorder == precipVardivMeanIntorder).Description);
                        var precipVarScore1 = db.tblSWATScores.Single(e => e.SurveyID == tblswatwaannualprecip.SurveyID && e.VarName == "precipVarSCORE1");
                        precipVarScore1.Value = precipVarScore1Value;
                    }
                }
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwaannualprecip.SurveyID && e.VarName == "precipVarALTSCORE").Value = null;
                db.SaveChanges();
            }
            // Update precipVarALTSCORE by selecting from select list.
            else if (tblswatwaannualprecip.precipVarALT != null)
            {
                int? precipVarALT = tblswatwaannualprecip.precipVarALT;
                int? precipVarALTintorder = db.lkpSWATprecipVarAltLUs.Find(precipVarALT).intorder;
                double precipVarALTSCORE = Double.Parse(db.lkpSWATscores_precipVar.Single(e => e.intorder == precipVarALTintorder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwaannualprecip.SurveyID && e.VarName == "precipVarALTSCORE").Value = precipVarALTSCORE;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwaannualprecip.SurveyID && e.VarName == "precipVarSCORE1").Value = null;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwaannualprecip.SurveyID && e.VarName == "precipVardivMean").Value = null;
                db.SaveChanges();
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwaannualprecip.SurveyID && e.VarName == "precipVarSCORE1").Value = null;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwaannualprecip.SurveyID && e.VarName == "precipVardivMean").Value = null;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwaannualprecip.SurveyID && e.VarName == "precipVarALTSCORE").Value = null;
                db.SaveChanges();
            }

        }

        // POST: /WAannualPrecip/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SurveyID,precipVar,precipVarALT")] tblSWATWAannualPrecip tblswatwaannualprecip)
        {
            
            if (ModelState.IsValid)
            {
                var annualPrecipIds = db.tblSWATWAannualPrecips.Where(e => e.SurveyID == tblswatwaannualprecip.SurveyID).Select(e => e.ID);
                if (annualPrecipIds.Any())
                {
                    int annualPrecipId = annualPrecipIds.First();
                    tblswatwaannualprecip.ID = annualPrecipId;
                    db.Entry(tblswatwaannualprecip).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatwaannualprecip);
                    return RedirectToAction("Index");
                }
                db.tblSWATWAannualPrecips.Add(tblswatwaannualprecip);
                db.SaveChanges();
                updateScores(tblswatwaannualprecip);
                return RedirectToAction("Index");
            }

            ViewBag.precipVarALT = new SelectList(db.lkpSWATprecipVarAltLUs, "id", "Description", tblswatwaannualprecip.precipVarALT);
            //ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID", tblswatwaannualprecip.SurveyID);
            return View(tblswatwaannualprecip);
        }

        // GET: /WAannualPrecip/Edit/5
        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null | SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAannualPrecip tblswatwaannualprecip = db.tblSWATWAannualPrecips.Find(id);
            if (tblswatwaannualprecip == null)
            {
                return HttpNotFound();
            }
            ViewBag.precipVarALT = new SelectList(db.lkpSWATprecipVarAltLUs, "id", "Description", tblswatwaannualprecip.precipVarALT);
           // ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID", tblswatwaannualprecip.SurveyID);
            return View(tblswatwaannualprecip);
        }

        // POST: /WAannualPrecip/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SurveyID,precipVar,precipVarALT")] tblSWATWAannualPrecip tblswatwaannualprecip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatwaannualprecip).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatwaannualprecip);
                return RedirectToAction("Index");
            }
            ViewBag.precipVarALT = new SelectList(db.lkpSWATprecipVarAltLUs, "id", "Description", tblswatwaannualprecip.precipVarALT);
            ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID", tblswatwaannualprecip.SurveyID);
            return View(tblswatwaannualprecip);
        }

        // GET: /WAannualPrecip/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAannualPrecip tblswatwaannualprecip = db.tblSWATWAannualPrecips.Find(id);
            if (tblswatwaannualprecip == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwaannualprecip);
        }

        // POST: /WAannualPrecip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATWAannualPrecip tblswatwaannualprecip = db.tblSWATWAannualPrecips.Find(id);
            db.tblSWATWAannualPrecips.Remove(tblswatwaannualprecip);
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
