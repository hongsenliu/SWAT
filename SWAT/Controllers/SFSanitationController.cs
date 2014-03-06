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
    public class SFSanitationController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /SFSanitation/

        public ActionResult Index()
        {
            var tblswatsfsanitations = db.tblSWATSFsanitations.Include(t => t.lkpSWATtoiletsAllLU).Include(t => t.tblSWATSurvey);
            return View(tblswatsfsanitations.ToList());
        }

        //
        // GET: /SFSanitation/Details/5

        public ActionResult Details(int id = 0)
        {
            tblSWATSFsanitation tblswatsfsanitation = db.tblSWATSFsanitations.Find(id);
            if (tblswatsfsanitation == null)
            {
                return HttpNotFound();
            }
            return View(tblswatsfsanitation);
        }

        //
        // GET: /SFSanitation/Create

        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.toiletsAll = new SelectList(db.lkpSWATtoiletsAllLUs, "id", "Description");

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "toiletsAllSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "toiletDistancesSCORE").Description;

            return View();
        }
        private void checkSumHHsan(tblSWATSFsanitation tblswatsfsanitation)
        {
            double?[] hhSan = { tblswatsfsanitation.toiletHome, tblswatsfsanitation.toiletYard, tblswatsfsanitation.toilet100,
                                tblswatsfsanitation.toilet500, tblswatsfsanitation.toiletFar};
            double? total = null;
            foreach (double? item in hhSan)
            {
                total = total.GetValueOrDefault(0) + item.GetValueOrDefault(0);
            }
            if (total > 100)
            {
                ModelState.AddModelError("toiletHome", "The total percentage of households cannot exceed 100.");
                ModelState.AddModelError("toiletYard", "The total percentage of households cannot exceed 100.");
                ModelState.AddModelError("toilet100", "The total percentage of households cannot exceed 100.");
                ModelState.AddModelError("toilet500", "The total percentage of households cannot exceed 100.");
                ModelState.AddModelError("toiletFar", "The total percentage of households cannot exceed 100.");
            }
        }

        private void updateScores(tblSWATSFsanitation tblswatsfsanitation)
        {
            if (tblswatsfsanitation.toiletsAll != null)
            {
                int intorder = (int)db.lkpSWATtoiletsAllLUs.Find(tblswatsfsanitation.toiletsAll).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_toiletsAll.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toiletsAllSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toiletsAllSCORE").Value = null;
            }

            double? total = null;
            if (tblswatsfsanitation.toiletHome != null)
            {
                double score = (double)tblswatsfsanitation.toiletHome / 100;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toiletHomeSCORE").Value = score;
                total = total.GetValueOrDefault(0) + score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toiletHomeSCORE").Value = null;
            }

            if (tblswatsfsanitation.toiletYard != null)
            {
                double score = (double)tblswatsfsanitation.toiletYard / 100;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toiletYardSCORE").Value = score;
                total = total.GetValueOrDefault(0) + score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toiletYardSCORE").Value = null;
            }

            if (tblswatsfsanitation.toilet100 != null)
            {
                double score = (double)tblswatsfsanitation.toilet100 / 100 * 0.75;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toilet100SCORE").Value = score;
                total = total.GetValueOrDefault(0) + score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toilet100SCORE").Value = null;
            }

            if (tblswatsfsanitation.toilet500 != null)
            {
                double score = (double)tblswatsfsanitation.toilet500 / 100 * 0.5;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toilet500SCORE").Value = score;
                total = total.GetValueOrDefault(0) + score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toilet500SCORE").Value = null;
            }

            if (tblswatsfsanitation.toiletFar != null)
            {
                double score = (double)tblswatsfsanitation.toiletFar / 100 * 0.25;
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toiletFarSCORE").Value = score;
                total = total.GetValueOrDefault(0) + score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toiletFarSCORE").Value = null;
            }

            db.tblSWATScores.First(e => e.SurveyID == tblswatsfsanitation.SurveyID && e.VarName == "toiletDistancesSCORE").Value = total;

            db.SaveChanges();
        }

        //
        // POST: /SFSanitation/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATSFsanitation tblswatsfsanitation)
        {
            checkSumHHsan(tblswatsfsanitation);
            if (ModelState.IsValid)
            {
                var recordIDs = db.tblSWATSFsanitations.Where(e => e.SurveyID == tblswatsfsanitation.SurveyID).Select(e => e.ID);
                if (recordIDs.Any())
                {
                    int recordId = recordIDs.First();
                    tblswatsfsanitation.ID = recordId;
                    db.Entry(tblswatsfsanitation).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatsfsanitation);

                    // return RedirectToAction("Index");
                    return RedirectToAction("Create", "SFOd", new { SurveyID = tblswatsfsanitation.SurveyID });
                }

                db.tblSWATSFsanitations.Add(tblswatsfsanitation);
                db.SaveChanges();
                updateScores(tblswatsfsanitation);

                // return RedirectToAction("Index");
                return RedirectToAction("Create", "SFOd", new { SurveyID = tblswatsfsanitation.SurveyID });
            }

            ViewBag.toiletsAll = new SelectList(db.lkpSWATtoiletsAllLUs, "id", "Description", tblswatsfsanitation.toiletsAll);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "toiletsAllSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "toiletDistancesSCORE").Description;

            return View(tblswatsfsanitation);
        }

        //
        // GET: /SFSanitation/Edit/5

        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATSFsanitation tblswatsfsanitation = db.tblSWATSFsanitations.Find(id);
            if (tblswatsfsanitation == null)
            {
                return HttpNotFound();
            }
            ViewBag.toiletsAll = new SelectList(db.lkpSWATtoiletsAllLUs, "id", "Description", tblswatsfsanitation.toiletsAll);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "toiletsAllSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "toiletDistancesSCORE").Description;

            return View(tblswatsfsanitation);
        }

        //
        // POST: /SFSanitation/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATSFsanitation tblswatsfsanitation)
        {
            checkSumHHsan(tblswatsfsanitation);
            if (ModelState.IsValid)
            {
                db.Entry(tblswatsfsanitation).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatsfsanitation);

                var records = db.tblSWATSFods.Where(e => e.SurveyID == tblswatsfsanitation.SurveyID);
                if (!records.Any())
                {
                    tblSWATSFod newEntry = new tblSWATSFod();
                    newEntry.SurveyID = tblswatsfsanitation.SurveyID;
                    db.tblSWATSFods.Add(newEntry);
                    db.SaveChanges();

                    int newId = newEntry.ID;
                    return RedirectToAction("Edit", "SFOd", new { id = newId, SurveyID = newEntry.SurveyID });
                }

                return RedirectToAction("Edit", "SFOd", new { id = records.First(e => e.SurveyID == tblswatsfsanitation.SurveyID).ID, SurveyID = tblswatsfsanitation.SurveyID });
            }
            ViewBag.toiletsAll = new SelectList(db.lkpSWATtoiletsAllLUs, "id", "Description", tblswatsfsanitation.toiletsAll);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "toiletsAllSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "toiletDistancesSCORE").Description;

            return View(tblswatsfsanitation);
        }

        //
        // GET: /SFSanitation/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblSWATSFsanitation tblswatsfsanitation = db.tblSWATSFsanitations.Find(id);
            if (tblswatsfsanitation == null)
            {
                return HttpNotFound();
            }
            return View(tblswatsfsanitation);
        }

        //
        // POST: /SFSanitation/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATSFsanitation tblswatsfsanitation = db.tblSWATSFsanitations.Find(id);
            db.tblSWATSFsanitations.Remove(tblswatsfsanitation);
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