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
    public class SFSepticController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /SFSeptic/

        public ActionResult Index()
        {
            var tblswatsfseptics = db.tblSWATSFseptics.Include(t => t.lkpSWAT5rankLU).Include(t => t.lkpSWAT5rankLU1).Include(t => t.lkpSWAThhCleanLU).Include(t => t.lkpSWATpubCleanLU).Include(t => t.lkpSWATYesNoLU).Include(t => t.lkpSWATYesNoLU1).Include(t => t.lkpSWATYesNoLU2).Include(t => t.tblSWATSurvey);
            return View(tblswatsfseptics.ToList());
        }

        //
        // GET: /SFSeptic/Details/5

        public ActionResult Details(int id = 0)
        {
            tblSWATSFseptic tblswatsfseptic = db.tblSWATSFseptics.Find(id);
            if (tblswatsfseptic == null)
            {
                return HttpNotFound();
            }
            return View(tblswatsfseptic);
        }

        private void removeOthers(int SurveyID)
        {
            var centrals = db.tblSWATSFcentrals.Where(e => e.SurveyID == SurveyID);
            foreach (tblSWATSFcentral c in centrals)
            {
                db.tblSWATSFcentrals.Remove(c);
            }

            var lats = db.tblSWATSFlats.Where(e => e.SurveyID == SurveyID);
            foreach (tblSWATSFlat l in lats)
            {
                db.tblSWATSFlats.Remove(l);
            }


            var scores = db.tblSWATScores.Where(e => e.SurveyID == SurveyID && e.lkpSWATScoreVarsLU.SectionID == 9 || e.lkpSWATScoreVarsLU.SectionID == 11);

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
        // GET: /SFSeptic/Create

        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            removeOthers((int)SurveyID);

            ViewBag.septicChemAvail = new SelectList(db.lkpSWAT5rankLU, "id", "Description");
            ViewBag.septicPumpAvail = new SelectList(db.lkpSWAT5rankLU, "id", "Description");
            ViewBag.hhSepticClean = new SelectList(db.lkpSWAThhCleanLUs, "id", "Description");
            ViewBag.septicPubClean = new SelectList(db.lkpSWATpubCleanLUs, "id", "Description");
            ViewBag.septicfeesCharged = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");
            ViewBag.septicFeesLimitAccess = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");
            ViewBag.septicFeesEnsureClean = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicToiletsPSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicUnderstandSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicDistanceSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPlugSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPumpedSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicTrashSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicChemicalsSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicChemAvailSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPumpAvailSCORE").Description;
            ViewBag.Question10 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPubCleanSCORE").Description;
            ViewBag.Question11 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPubAccessSCORE").Description;
            ViewBag.Question12 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicfeesChargedSCORE").Description;
            ViewBag.Question13 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicFeesLimitAccessSCORE").Description;
            ViewBag.Question14 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicFeesEnsureCleanSCORE").Description;
            ViewBag.Question15 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "hhSepticCleanSCORE").Description;

            showForm((int)SurveyID);

            return View();
        }

        private void updateScores(tblSWATSFseptic tblswatsfseptic)
        {
            if (tblswatsfseptic.septicToilets != null)
            {
                int toilets = (int)tblswatsfseptic.septicToilets;
                double score = (double)toilets / (int)db.tblSWATBackgroundinfoes.First(e => e.SurveyID == tblswatsfseptic.SurveyID).numHouseholds;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicToiletsPSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicToiletsPSCORE").Value = null;
            }

            if (tblswatsfseptic.septicUnderstand != null)
            {
                double score = (double)tblswatsfseptic.septicUnderstand / 100;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicUnderstandSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicUnderstandSCORE").Value = null;
            }

            if (tblswatsfseptic.septicDistance != null)
            {
                double score = (double)tblswatsfseptic.septicDistance / 100;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicDistanceSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicDistanceSCORE").Value = null;
            }

            if (tblswatsfseptic.septicPlug != null)
            {
                double score = (double)tblswatsfseptic.septicPlug / 100;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicPlugSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicPlugSCORE").Value = null;
            }

            if (tblswatsfseptic.septicPumped != null)
            {
                double score = (double)tblswatsfseptic.septicPumped / 100;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicPumpedSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicPumpedSCORE").Value = null;
            }

            if (tblswatsfseptic.septicTrash != null)
            {
                double score = (double)tblswatsfseptic.septicTrash / 100;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicTrashSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicTrashSCORE").Value = null;
            }

            if (tblswatsfseptic.septicChemicals != null)
            {
                double score = (double)tblswatsfseptic.septicChemicals / 100;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicChemicalsSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicChemicalsSCORE").Value = null;
            }

            if (tblswatsfseptic.septicChemAvail != null)
            {
                int intorder = (int)db.lkpSWAT5rankLU.Find(tblswatsfseptic.septicChemAvail).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysGood.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicChemAvailSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicChemAvailSCORE").Value = null;
            }

            if (tblswatsfseptic.septicPumpAvail != null)
            {
                int intorder = (int)db.lkpSWAT5rankLU.Find(tblswatsfseptic.septicPumpAvail).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysGood.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicPumpAvailSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicPumpAvailSCORE").Value = null;
            }

            if (tblswatsfseptic.septicPubClean != null)
            {
                int intorder = (int)db.lkpSWATpubCleanLUs.Find(tblswatsfseptic.septicPubClean).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_pubClean.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicPubCleanSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicPubCleanSCORE").Value = null;
            }

            bool[] access = { tblswatsfseptic.septicPubAccessGroup1, tblswatsfseptic.septicPubAccessGroup2, tblswatsfseptic.septicPubAccessGroup3 };
            int accessCounter = 0;
            foreach (bool item in access)
            {
                if (item)
                {
                    accessCounter++;
                }
            }
            double accessScore = (double)accessCounter / 3;
            db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicPubAccessSCORE").Value = accessScore;

            if (tblswatsfseptic.septicfeesCharged != null)
            {
                int intorder = (int)db.lkpSWATYesNoLUs.Find(tblswatsfseptic.septicfeesCharged).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_YesNoLUYesGood.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicfeesChargedSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicfeesChargedSCORE").Value = null;
            }

            if (tblswatsfseptic.septicFeesLimitAccess != null)
            {
                int intorder = (int)db.lkpSWATYesNoLUs.Find(tblswatsfseptic.septicFeesLimitAccess).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_YesNoLU_YesBad.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicFeesLimitAccessSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicFeesLimitAccessSCORE").Value = null;
            }

            if (tblswatsfseptic.septicFeesEnsureClean != null)
            {
                int intorder = (int)db.lkpSWATYesNoLUs.Find(tblswatsfseptic.septicFeesEnsureClean).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_YesNoLUYesGood.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicFeesEnsureCleanSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "septicFeesEnsureCleanSCORE").Value = null;
            }

            if (tblswatsfseptic.hhSepticClean != null)
            {
                int intorder = (int)db.lkpSWAThhCleanLUs.Find(tblswatsfseptic.hhSepticClean).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_hhClean.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "hhSepticCleanSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfseptic.SurveyID && e.VarName == "hhSepticCleanSCORE").Value = null;
            }

            db.SaveChanges();
        }

        //
        // POST: /SFSeptic/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATSFseptic tblswatsfseptic)
        {
            if (ModelState.IsValid)
            {
                var recordIDs = db.tblSWATSFseptics.Where(e => e.SurveyID == tblswatsfseptic.SurveyID).Select(e => e.ID);
                if (recordIDs.Any())
                {
                    int recordId = recordIDs.First();
                    tblswatsfseptic.ID = recordId;
                    db.Entry(tblswatsfseptic).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatsfseptic);

                    return RedirectToAction("WaterPoints", "Survey", new { id = tblswatsfseptic.SurveyID });
                }

                db.tblSWATSFseptics.Add(tblswatsfseptic);
                db.SaveChanges();
                updateScores(tblswatsfseptic);

                return RedirectToAction("WaterPoints", "Survey", new { id = tblswatsfseptic.SurveyID });
            }

            ViewBag.septicChemAvail = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatsfseptic.septicChemAvail);
            ViewBag.septicPumpAvail = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatsfseptic.septicPumpAvail);
            ViewBag.hhSepticClean = new SelectList(db.lkpSWAThhCleanLUs, "id", "Description", tblswatsfseptic.hhSepticClean);
            ViewBag.septicPubClean = new SelectList(db.lkpSWATpubCleanLUs, "id", "Description", tblswatsfseptic.septicPubClean);
            ViewBag.septicfeesCharged = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfseptic.septicfeesCharged);
            ViewBag.septicFeesLimitAccess = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfseptic.septicFeesLimitAccess);
            ViewBag.septicFeesEnsureClean = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfseptic.septicFeesEnsureClean);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicToiletsPSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicUnderstandSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicDistanceSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPlugSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPumpedSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicTrashSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicChemicalsSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicChemAvailSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPumpAvailSCORE").Description;
            ViewBag.Question10 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPubCleanSCORE").Description;
            ViewBag.Question11 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPubAccessSCORE").Description;
            ViewBag.Question12 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicfeesChargedSCORE").Description;
            ViewBag.Question13 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicFeesLimitAccessSCORE").Description;
            ViewBag.Question14 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicFeesEnsureCleanSCORE").Description;
            ViewBag.Question15 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "hhSepticCleanSCORE").Description;

            showForm(tblswatsfseptic.SurveyID);

            return View(tblswatsfseptic);
        }

        //
        // GET: /SFSeptic/Edit/5

        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATSFseptic tblswatsfseptic = db.tblSWATSFseptics.Find(id);
            if (tblswatsfseptic == null)
            {
                return HttpNotFound();
            }

            removeOthers((int)SurveyID);

            ViewBag.septicChemAvail = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatsfseptic.septicChemAvail);
            ViewBag.septicPumpAvail = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatsfseptic.septicPumpAvail);
            ViewBag.hhSepticClean = new SelectList(db.lkpSWAThhCleanLUs, "id", "Description", tblswatsfseptic.hhSepticClean);
            ViewBag.septicPubClean = new SelectList(db.lkpSWATpubCleanLUs, "id", "Description", tblswatsfseptic.septicPubClean);
            ViewBag.septicfeesCharged = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfseptic.septicfeesCharged);
            ViewBag.septicFeesLimitAccess = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfseptic.septicFeesLimitAccess);
            ViewBag.septicFeesEnsureClean = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfseptic.septicFeesEnsureClean);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicToiletsPSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicUnderstandSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicDistanceSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPlugSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPumpedSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicTrashSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicChemicalsSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicChemAvailSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPumpAvailSCORE").Description;
            ViewBag.Question10 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPubCleanSCORE").Description;
            ViewBag.Question11 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPubAccessSCORE").Description;
            ViewBag.Question12 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicfeesChargedSCORE").Description;
            ViewBag.Question13 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicFeesLimitAccessSCORE").Description;
            ViewBag.Question14 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicFeesEnsureCleanSCORE").Description;
            ViewBag.Question15 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "hhSepticCleanSCORE").Description;

            showForm((int)SurveyID);

            return View(tblswatsfseptic);
        }

        //
        // POST: /SFSeptic/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATSFseptic tblswatsfseptic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatsfseptic).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatsfseptic);

                return RedirectToAction("WaterPoints", "Survey", new { id = tblswatsfseptic.SurveyID });
            }
            ViewBag.septicChemAvail = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatsfseptic.septicChemAvail);
            ViewBag.septicPumpAvail = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatsfseptic.septicPumpAvail);
            ViewBag.hhSepticClean = new SelectList(db.lkpSWAThhCleanLUs, "id", "Description", tblswatsfseptic.hhSepticClean);
            ViewBag.septicPubClean = new SelectList(db.lkpSWATpubCleanLUs, "id", "Description", tblswatsfseptic.septicPubClean);
            ViewBag.septicfeesCharged = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfseptic.septicfeesCharged);
            ViewBag.septicFeesLimitAccess = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfseptic.septicFeesLimitAccess);
            ViewBag.septicFeesEnsureClean = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatsfseptic.septicFeesEnsureClean);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicToiletsPSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicUnderstandSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicDistanceSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPlugSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPumpedSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicTrashSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicChemicalsSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicChemAvailSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPumpAvailSCORE").Description;
            ViewBag.Question10 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPubCleanSCORE").Description;
            ViewBag.Question11 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicPubAccessSCORE").Description;
            ViewBag.Question12 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicfeesChargedSCORE").Description;
            ViewBag.Question13 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicFeesLimitAccessSCORE").Description;
            ViewBag.Question14 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "septicFeesEnsureCleanSCORE").Description;
            ViewBag.Question15 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "hhSepticCleanSCORE").Description;

            showForm(tblswatsfseptic.SurveyID);

            return View(tblswatsfseptic);
        }

        //
        // GET: /SFSeptic/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblSWATSFseptic tblswatsfseptic = db.tblSWATSFseptics.Find(id);
            if (tblswatsfseptic == null)
            {
                return HttpNotFound();
            }
            return View(tblswatsfseptic);
        }

        //
        // POST: /SFSeptic/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATSFseptic tblswatsfseptic = db.tblSWATSFseptics.Find(id);
            db.tblSWATSFseptics.Remove(tblswatsfseptic);
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