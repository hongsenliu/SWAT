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
    public class SFCentralController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /SFCentral/

        public ActionResult Index()
        {
            var tblswatsfcentrals = db.tblSWATSFcentrals.Include(t => t.lkpSWAT5rankLU).Include(t => t.lkpSWATcentralConditionLU).Include(t => t.lkpSWATcentralTreatmentTypeLU).Include(t => t.lkpSWAThhCleanLU).Include(t => t.lkpSWATpubCleanLU).Include(t => t.lkpSWATYesNoLU).Include(t => t.lkpSWATYesNoLU1).Include(t => t.lkpSWATYesNoLU2).Include(t => t.tblSWATSurvey);
            return View(tblswatsfcentrals.ToList());
        }

        //
        // GET: /SFCentral/Details/5

        public ActionResult Details(int id = 0)
        {
            tblSWATSFcentral tblswatsfcentral = db.tblSWATSFcentrals.Find(id);
            if (tblswatsfcentral == null)
            {
                return HttpNotFound();
            }
            return View(tblswatsfcentral);
        }

        private void removeOthers(int SurveyID)
        { 
            var septics = db.tblSWATSFseptics.Where(e => e.SurveyID == SurveyID);
            foreach (tblSWATSFseptic s in septics)
            {
                db.tblSWATSFseptics.Remove(s);
            }

            var lats = db.tblSWATSFlats.Where(e => e.SurveyID == SurveyID);
            foreach (tblSWATSFlat l in lats)
            {
                db.tblSWATSFlats.Remove(l);
            }


            var scores = db.tblSWATScores.Include(t => t.lkpSWATScoreVarsLU).Where(e => e.SurveyID == SurveyID && e.lkpSWATScoreVarsLU.SectionID == 10 || e.lkpSWATScoreVarsLU.SectionID == 11);

            foreach (tblSWATScore s in scores)
            {
                    s.Value = null;
            }

            db.SaveChanges();
        }

        private void showForm(int SurveyID)
        {
            tblSWATSFpoint sfpoint = db.tblSWATSFpoints.First(e => e.SurveyID == SurveyID);
            ViewBag.pub = (sfpoint.SanUseCom || sfpoint.sanUsePub);
            ViewBag.ind = sfpoint.sanUseInd;
        }

        //
        // GET: /SFCentral/Create

        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            removeOthers((int)SurveyID);

            ViewBag.centralSludge = new SelectList(db.lkpSWAT5rankLU, "id", "Description");
            ViewBag.centralCondition = new SelectList(db.lkpSWATcentralConditionLUs, "id", "Description");
            ViewBag.centralTreatmentType = new SelectList(db.lkpSWATcentralTreatmentTypeLUs, "id", "Description");
            ViewBag.hhCentralClean = new SelectList(db.lkpSWAThhCleanLUs, "id", "Description");
            ViewBag.centralPubClean = new SelectList(db.lkpSWATpubCleanLUs, "id", "Description");
            ViewBag.centralfeesCharged = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");
            ViewBag.centralFeesLimitAccess = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");
            ViewBag.centralFeesEnsureClean = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralToiletsPSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralTreatmentTypeSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralSludgeSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralConditionSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralPubCleanSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralAccessSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralfeesChargedSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralFeesLimitAccessSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralFeesEnsureCleanSCORE").Description;
            ViewBag.Question10 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "hhCentralCleanSCORE").Description;

            showForm((int)SurveyID);

            return View();
        }

        private void updateScores(tblSWATSFcentral tblswatsfcentral)
        {
            if (tblswatsfcentral.centralToilets != null)
            {
                int toilets = (int)tblswatsfcentral.centralToilets;
                double score = (double)toilets / (int)db.tblSWATBackgroundinfoes.First(e => e.SurveyID == tblswatsfcentral.SurveyID).numHouseholds;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralToiletsPSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralToiletsPSCORE").Value = null;
            }

            if (tblswatsfcentral.centralTreatmentType != null)
            {
                int intorder = (int)db.lkpSWATcentralTreatmentTypeLUs.Find(tblswatsfcentral.centralTreatmentType).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_centralTreatment.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralTreatmentTypeSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralTreatmentTypeSCORE").Value = null;
            }

            if (tblswatsfcentral.centralSludge != null)
            {
                int intorder = (int)db.lkpSWAT5rankLU.Find(tblswatsfcentral.centralSludge).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysGood.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralSludgeSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralSludgeSCORE").Value = null;
            }

            if (tblswatsfcentral.centralCondition != null)
            {
                int intorder = (int)db.lkpSWATcentralConditionLUs.Find(tblswatsfcentral.centralCondition).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_centralCondition.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralConditionSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralConditionSCORE").Value = null;
            }

            if (tblswatsfcentral.centralPubClean != null)
            {
                int intorder = (int)db.lkpSWATpubCleanLUs.Find(tblswatsfcentral.centralPubClean).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_pubClean.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralPubCleanSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralPubCleanSCORE").Value = null;
            }

            bool[] access = { tblswatsfcentral.centralAccessGroup1, tblswatsfcentral.centralAccessGroup2, tblswatsfcentral.centralAccessGroup3};
            int accessCounter = 0;
            foreach (bool item in access)
            {
                if (item)
                {
                    accessCounter++;
                }
            }
            double accessScore = (double)accessCounter / 3;
            db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralAccessSCORE").Value = accessScore;

            if (tblswatsfcentral.centralfeesCharged != null)
            {
                int intorder = (int)db.lkpSWATYesNoLUs.Find(tblswatsfcentral.centralfeesCharged).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_YesNoLUYesGood.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralfeesChargedSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralfeesChargedSCORE").Value = null;
            }

            if (tblswatsfcentral.centralFeesLimitAccess != null)
            {
                int intorder = (int)db.lkpSWATYesNoLUs.Find(tblswatsfcentral.centralFeesLimitAccess).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_YesNoLU_YesBad.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralFeesLimitAccessSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralFeesLimitAccessSCORE").Value = null;
            }

            if (tblswatsfcentral.centralFeesEnsureClean != null)
            {
                int intorder = (int)db.lkpSWATYesNoLUs.Find(tblswatsfcentral.centralFeesEnsureClean).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_YesNoLUYesGood.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralFeesEnsureCleanSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "centralFeesEnsureCleanSCORE").Value = null;
            }

            if (tblswatsfcentral.hhCentralClean != null)
            {
                int intorder = (int)db.lkpSWAThhCleanLUs.Find(tblswatsfcentral.hhCentralClean).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_hhClean.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "hhCentralCleanSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfcentral.SurveyID && e.VarName == "hhCentralCleanSCORE").Value = null;
            }

            db.SaveChanges();
        }

        //
        // POST: /SFCentral/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATSFcentral tblswatsfcentral)
        {
            if (ModelState.IsValid)
            {
                var recordIDs = db.tblSWATSFcentrals.Where(e => e.SurveyID == tblswatsfcentral.SurveyID).Select(e => e.ID);
                if (recordIDs.Any())
                {
                    int recordId = recordIDs.First();
                    tblswatsfcentral.ID = recordId;
                    db.Entry(tblswatsfcentral).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatsfcentral);

                    return RedirectToAction("WaterPoints", "Survey", new { id = tblswatsfcentral.SurveyID });
                    // return RedirectToAction("Create", "SFPoint", new { SurveyID = tblswatsfpoint.SurveyID });
                }

                db.tblSWATSFcentrals.Add(tblswatsfcentral);
                db.SaveChanges();
                updateScores(tblswatsfcentral);

                return RedirectToAction("WaterPoints", "Survey", new { id = tblswatsfcentral.SurveyID });
            }

            ViewBag.centralSludge = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatsfcentral.centralSludge);
            ViewBag.centralCondition = new SelectList(db.lkpSWATcentralConditionLUs, "id", "Description", tblswatsfcentral.centralCondition);
            ViewBag.centralTreatmentType = new SelectList(db.lkpSWATcentralTreatmentTypeLUs, "id", "Description", tblswatsfcentral.centralTreatmentType);
            ViewBag.hhCentralClean = new SelectList(db.lkpSWAThhCleanLUs, "id", "Description", tblswatsfcentral.hhCentralClean);
            ViewBag.centralPubClean = new SelectList(db.lkpSWATpubCleanLUs, "id", "Description", tblswatsfcentral.centralPubClean);
            ViewBag.centralfeesCharged = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfcentral.centralfeesCharged);
            ViewBag.centralFeesLimitAccess = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfcentral.centralFeesLimitAccess);
            ViewBag.centralFeesEnsureClean = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfcentral.centralFeesEnsureClean);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralToiletsPSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralTreatmentTypeSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralSludgeSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralConditionSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralPubCleanSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralAccessSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralfeesChargedSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralFeesLimitAccessSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralFeesEnsureCleanSCORE").Description;
            ViewBag.Question10 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "hhCentralCleanSCORE").Description;

            showForm((int)tblswatsfcentral.SurveyID);

            return View(tblswatsfcentral);
        }

        //
        // GET: /SFCentral/Edit/5

        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATSFcentral tblswatsfcentral = db.tblSWATSFcentrals.Find(id);
            if (tblswatsfcentral == null)
            {
                return HttpNotFound();
            }

            removeOthers((int)SurveyID);

            ViewBag.centralSludge = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatsfcentral.centralSludge);
            ViewBag.centralCondition = new SelectList(db.lkpSWATcentralConditionLUs, "id", "Description", tblswatsfcentral.centralCondition);
            ViewBag.centralTreatmentType = new SelectList(db.lkpSWATcentralTreatmentTypeLUs, "id", "Description", tblswatsfcentral.centralTreatmentType);
            ViewBag.hhCentralClean = new SelectList(db.lkpSWAThhCleanLUs, "id", "Description", tblswatsfcentral.hhCentralClean);
            ViewBag.centralPubClean = new SelectList(db.lkpSWATpubCleanLUs, "id", "Description", tblswatsfcentral.centralPubClean);
            ViewBag.centralfeesCharged = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfcentral.centralfeesCharged);
            ViewBag.centralFeesLimitAccess = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfcentral.centralFeesLimitAccess);
            ViewBag.centralFeesEnsureClean = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfcentral.centralFeesEnsureClean);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralToiletsPSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralTreatmentTypeSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralSludgeSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralConditionSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralPubCleanSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralAccessSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralfeesChargedSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralFeesLimitAccessSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralFeesEnsureCleanSCORE").Description;
            ViewBag.Question10 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "hhCentralCleanSCORE").Description;

            showForm((int)tblswatsfcentral.SurveyID);
            
            return View(tblswatsfcentral);
        }

        //
        // POST: /SFCentral/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATSFcentral tblswatsfcentral)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatsfcentral).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatsfcentral);

                return RedirectToAction("WaterPoints", "Survey", new { id = tblswatsfcentral.SurveyID });
            }
            ViewBag.centralSludge = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatsfcentral.centralSludge);
            ViewBag.centralCondition = new SelectList(db.lkpSWATcentralConditionLUs, "id", "Description", tblswatsfcentral.centralCondition);
            ViewBag.centralTreatmentType = new SelectList(db.lkpSWATcentralTreatmentTypeLUs, "id", "Description", tblswatsfcentral.centralTreatmentType);
            ViewBag.hhCentralClean = new SelectList(db.lkpSWAThhCleanLUs, "id", "Description", tblswatsfcentral.hhCentralClean);
            ViewBag.centralPubClean = new SelectList(db.lkpSWATpubCleanLUs, "id", "Description", tblswatsfcentral.centralPubClean);
            ViewBag.centralfeesCharged = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfcentral.centralfeesCharged);
            ViewBag.centralFeesLimitAccess = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfcentral.centralFeesLimitAccess);
            ViewBag.centralFeesEnsureClean = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfcentral.centralFeesEnsureClean);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralToiletsPSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralTreatmentTypeSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralSludgeSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralConditionSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralPubCleanSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralAccessSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralfeesChargedSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralFeesLimitAccessSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "centralFeesEnsureCleanSCORE").Description;
            ViewBag.Question10 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "hhCentralCleanSCORE").Description;

            showForm((int)tblswatsfcentral.SurveyID);

            return View(tblswatsfcentral);
        }

        //
        // GET: /SFCentral/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblSWATSFcentral tblswatsfcentral = db.tblSWATSFcentrals.Find(id);
            if (tblswatsfcentral == null)
            {
                return HttpNotFound();
            }
            return View(tblswatsfcentral);
        }

        //
        // POST: /SFCentral/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATSFcentral tblswatsfcentral = db.tblSWATSFcentrals.Find(id);
            db.tblSWATSFcentrals.Remove(tblswatsfcentral);
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