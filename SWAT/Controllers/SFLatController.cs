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
    public class SFLatController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /SFLat/

        public ActionResult Index()
        {
            var tblswatsflats = db.tblSWATSFlats.Include(t => t.lkpSWAThhCleanLU).Include(t => t.lkpSWATlatrineConditionLU).Include(t => t.lkpSWATpubCleanLU).Include(t => t.lkpSWATYesNoLU).Include(t => t.lkpSWATYesNoLU1).Include(t => t.lkpSWATYesNoLU2).Include(t => t.tblSWATSurvey);
            return View(tblswatsflats.ToList());
        }

        private void removeOthers(int SurveyID)
        {
            var centrals = db.tblSWATSFcentrals.Where(e => e.SurveyID == SurveyID);
            foreach (tblSWATSFcentral c in centrals)
            {
                db.tblSWATSFcentrals.Remove(c);
            }

            var septics = db.tblSWATSFseptics.Where(e => e.SurveyID == SurveyID);
            foreach (tblSWATSFseptic s in septics)
            {
                db.tblSWATSFseptics.Remove(s);
            }


            var scores = db.tblSWATScores.Where(e => e.SurveyID == SurveyID && e.lkpSWATScoreVarsLU.SectionID == 9 || e.lkpSWATScoreVarsLU.SectionID == 10);

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
        // GET: /SFLat/Details/5

        public ActionResult Details(int id = 0)
        {
            tblSWATSFlat tblswatsflat = db.tblSWATSFlats.Find(id);
            if (tblswatsflat == null)
            {
                return HttpNotFound();
            }
            return View(tblswatsflat);
        }

        private void getQuestions()
        {
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "LatrineTypePSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "latrineProb1SCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "latrineConditionSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "latrinePubCleanSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "latrineAccessSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "latrinefeesChargedSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "latrineFeesLimitAccessSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "latrineFeesEnsureCleanSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "hhLatrineCleanSCORE").Description;
        }

        //
        // GET: /SFLat/Create

        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            removeOthers((int)SurveyID);

            ViewBag.hhLatrineClean = new SelectList(db.lkpSWAThhCleanLUs, "id", "Description");
            ViewBag.latrineCondition = new SelectList(db.lkpSWATlatrineConditionLUs, "id", "Description");
            ViewBag.latrinecentralPubClean = new SelectList(db.lkpSWATpubCleanLUs, "id", "Description");
            ViewBag.latrinefeesCharged = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");
            ViewBag.latrineFeesLimitAccess = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");
            ViewBag.latrineFeesEnsureClean = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");

            getQuestions();

            showForm((int)SurveyID);

            return View();
        }

        private void updateScores(tblSWATSFlat tblswatsflat)
        {
            int? total = null;
            if (tblswatsflat.ltype3 != null)
            {
                total = total.GetValueOrDefault(0) + tblswatsflat.ltype3;
            }

            if (tblswatsflat.ltype4 != null)
            {
                total = total.GetValueOrDefault(0) + tblswatsflat.ltype4;
            }

            db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "LatrineTypeImprovedTOTAL").Value = total;
            if (total != null)
            {
                int hh = (int)db.tblSWATBackgroundinfoes.First(e => e.SurveyID == tblswatsflat.SurveyID).numHouseholds;
                double score = (double)total / hh;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "LatrineTypePSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "LatrineTypePSCORE").Value = null;
            }

            if (tblswatsflat.latrineProb1 != null)
            {
                double score = 1 - (double)tblswatsflat.latrineProb1 / 100;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineProb1SCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineProb1SCORE").Value = null;
            }

            if (tblswatsflat.latrineProb2 != null)
            {
                double score = 1 - (double)tblswatsflat.latrineProb2 / 100;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineProb2SCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineProb2SCORE").Value = null;
            }

            if (tblswatsflat.latrineProb3 != null)
            {
                double score = 1 - (double)tblswatsflat.latrineProb3 / 100;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineProb3SCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineProb3SCORE").Value = null;
            }

            if (tblswatsflat.latrineCondition != null)
            {
                int intorder = (int)db.lkpSWATlatrineConditionLUs.Find(tblswatsflat.latrineCondition).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_latrineCondition.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineConditionSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineConditionSCORE").Value = null;
            }

            if (tblswatsflat.latrinecentralPubClean != null)
            {
                int intorder = (int)db.lkpSWATpubCleanLUs.Find(tblswatsflat.latrinecentralPubClean).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_pubClean.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrinePubCleanSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrinePubCleanSCORE").Value = null;
            }

            bool[] access = { tblswatsflat.latrineAccessGroup1, tblswatsflat.latrineAccessGroup2, tblswatsflat.latrineAccessGroup3 };
            int accessCounter = 0;
            foreach (bool item in access)
            {
                if (item)
                {
                    accessCounter++;
                }
            }
            double accessScore = (double)accessCounter / 3;
            db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineAccessSCORE").Value = accessScore;

            if (tblswatsflat.latrinefeesCharged != null)
            {
                int intorder = (int)db.lkpSWATYesNoLUs.Find(tblswatsflat.latrinefeesCharged).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_YesNoLUYesGood.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrinefeesChargedSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrinefeesChargedSCORE").Value = null;
            }

            if (tblswatsflat.latrineFeesLimitAccess != null)
            {
                int intorder = (int)db.lkpSWATYesNoLUs.Find(tblswatsflat.latrineFeesLimitAccess).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_YesNoLU_YesBad.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineFeesLimitAccessSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineFeesLimitAccessSCORE").Value = null;
            }

            if (tblswatsflat.latrineFeesEnsureClean != null)
            {
                int intorder = (int)db.lkpSWATYesNoLUs.Find(tblswatsflat.latrineFeesEnsureClean).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_YesNoLUYesGood.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineFeesEnsureCleanSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "latrineFeesEnsureCleanSCORE").Value = null;
            }

            if (tblswatsflat.hhLatrineClean != null)
            {
                int intorder = (int)db.lkpSWAThhCleanLUs.Find(tblswatsflat.hhLatrineClean).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_hhClean.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "hhLatrineCleanSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsflat.SurveyID && e.VarName == "hhLatrineCleanSCORE").Value = null;
            }

            db.SaveChanges();

        }

        //
        // POST: /SFLat/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATSFlat tblswatsflat)
        {
            if (ModelState.IsValid)
            {
                var recordIDs = db.tblSWATSFlats.Where(e => e.SurveyID == tblswatsflat.SurveyID).Select(e => e.ID);
                if (recordIDs.Any())
                {
                    int recordId = recordIDs.First();
                    tblswatsflat.ID = recordId;
                    db.Entry(tblswatsflat).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatsflat);

                    return RedirectToAction("WaterPoints", "Survey", new { id = tblswatsflat.SurveyID });
                }

                db.tblSWATSFlats.Add(tblswatsflat);
                db.SaveChanges();
                updateScores(tblswatsflat);

                return RedirectToAction("WaterPoints", "Survey", new { id = tblswatsflat.SurveyID });
            }

            ViewBag.hhLatrineClean = new SelectList(db.lkpSWAThhCleanLUs, "id", "Description", tblswatsflat.hhLatrineClean);
            ViewBag.latrineCondition = new SelectList(db.lkpSWATlatrineConditionLUs, "id", "Description", tblswatsflat.latrineCondition);
            ViewBag.latrinecentralPubClean = new SelectList(db.lkpSWATpubCleanLUs, "id", "Description", tblswatsflat.latrinecentralPubClean);
            ViewBag.latrinefeesCharged = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsflat.latrinefeesCharged);
            ViewBag.latrineFeesLimitAccess = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsflat.latrineFeesLimitAccess);
            ViewBag.latrineFeesEnsureClean = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsflat.latrineFeesEnsureClean);

            getQuestions();

            showForm(tblswatsflat.SurveyID);

            return View(tblswatsflat);
        }

        //
        // GET: /SFLat/Edit/5

        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATSFlat tblswatsflat = db.tblSWATSFlats.Find(id);
            if (tblswatsflat == null)
            {
                return HttpNotFound();
            }

            removeOthers((int)SurveyID);

            ViewBag.hhLatrineClean = new SelectList(db.lkpSWAThhCleanLUs, "id", "Description", tblswatsflat.hhLatrineClean);
            ViewBag.latrineCondition = new SelectList(db.lkpSWATlatrineConditionLUs, "id", "Description", tblswatsflat.latrineCondition);
            ViewBag.latrinecentralPubClean = new SelectList(db.lkpSWATpubCleanLUs, "id", "Description", tblswatsflat.latrinecentralPubClean);
            ViewBag.latrinefeesCharged = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsflat.latrinefeesCharged);
            ViewBag.latrineFeesLimitAccess = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsflat.latrineFeesLimitAccess);
            ViewBag.latrineFeesEnsureClean = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsflat.latrineFeesEnsureClean);

            getQuestions();

            showForm((int)SurveyID);

            return View(tblswatsflat);
        }

        //
        // POST: /SFLat/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATSFlat tblswatsflat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatsflat).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatsflat);

                return RedirectToAction("WaterPoints", "Survey", new { id = tblswatsflat.SurveyID });
            }
            ViewBag.hhLatrineClean = new SelectList(db.lkpSWAThhCleanLUs, "id", "Description", tblswatsflat.hhLatrineClean);
            ViewBag.latrineCondition = new SelectList(db.lkpSWATlatrineConditionLUs, "id", "Description", tblswatsflat.latrineCondition);
            ViewBag.latrinecentralPubClean = new SelectList(db.lkpSWATpubCleanLUs, "id", "Description", tblswatsflat.latrinecentralPubClean);
            ViewBag.latrinefeesCharged = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsflat.latrinefeesCharged);
            ViewBag.latrineFeesLimitAccess = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsflat.latrineFeesLimitAccess);
            ViewBag.latrineFeesEnsureClean = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsflat.latrineFeesEnsureClean);

            getQuestions();

            showForm(tblswatsflat.SurveyID);

            return View(tblswatsflat);
        }

        //
        // GET: /SFLat/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblSWATSFlat tblswatsflat = db.tblSWATSFlats.Find(id);
            if (tblswatsflat == null)
            {
                return HttpNotFound();
            }
            return View(tblswatsflat);
        }

        //
        // POST: /SFLat/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATSFlat tblswatsflat = db.tblSWATSFlats.Find(id);
            db.tblSWATSFlats.Remove(tblswatsflat);
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