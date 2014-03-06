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
    public class SFPointController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /SFPoint/

        public ActionResult Index()
        {
            var tblswatsfpoints = db.tblSWATSFpoints.Include(t => t.lkpSWATsanTypeLU).Include(t => t.tblSWATSurvey);
            return View(tblswatsfpoints.ToList());
        }

        //
        // GET: /SFPoint/Details/5

        public ActionResult Details(int id = 0)
        {
            tblSWATSFpoint tblswatsfpoint = db.tblSWATSFpoints.Find(id);
            if (tblswatsfpoint == null)
            {
                return HttpNotFound();
            }
            return View(tblswatsfpoint);
        }

        //
        // GET: /SFPoint/Create

        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.sanType = new SelectList(db.lkpSWATsanTypeLUs, "id", "Description");

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "sanTypeVAL").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "SanUSeSUMMARY").Description;

            return View();
        }

        private void removeCSL(tblSWATSFpoint tblswatsfpoint)
        {
            // TODO remove CSL SF
            // type = [1001, 1002, 1003]
            
            var centrals = db.tblSWATSFcentrals.Where(e => e.SurveyID == tblswatsfpoint.SurveyID);
            foreach (tblSWATSFcentral c in centrals)
            {
                db.tblSWATSFcentrals.Remove(c);
            }

            var septics = db.tblSWATSFseptics.Where(e => e.SurveyID == tblswatsfpoint.SurveyID);
            foreach (tblSWATSFseptic s in septics)
            {
                db.tblSWATSFseptics.Remove(s);
            }

            var lats = db.tblSWATSFlats.Where(e => e.SurveyID == tblswatsfpoint.SurveyID);
            foreach (tblSWATSFlat l in lats)
            {
                db.tblSWATSFlats.Remove(l);
            }

            var scores = db.tblSWATScores.Include(t => t.lkpSWATScoreVarsLU).Where(e => e.SurveyID == tblswatsfpoint.SurveyID && (e.lkpSWATScoreVarsLU.SectionID == 9 || e.lkpSWATScoreVarsLU.SectionID == 10 || e.lkpSWATScoreVarsLU.SectionID == 11));
            
            foreach (tblSWATScore s in scores)
            {
                s.Value = null;
            }
            db.SaveChanges();
        }

        private void updateScores(tblSWATSFpoint tblswatsfpoint)
        {
            if (tblswatsfpoint.sanType != null)
            {
                int intorder = (int)db.lkpSWATsanTypeLUs.Find(tblswatsfpoint.sanType).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_sanType.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfpoint.SurveyID && e.VarName == "sanTypeVAL").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatsfpoint.SurveyID && e.VarName == "sanTypeVAL").Value = null;
                removeCSL(tblswatsfpoint);
            }

            bool[] sanUse = { tblswatsfpoint.sanUseInd, tblswatsfpoint.SanUseCom, tblswatsfpoint.sanUsePub};
            int sanUseCounter = 0;

            foreach (bool item in sanUse)
            {
                if (item)
                {
                    sanUseCounter++;
                }
            }
            double sanUseScore = (double)sanUseCounter / 3;
            db.tblSWATScores.First(e => e.SurveyID == tblswatsfpoint.SurveyID && e.VarName == "SanUSeSUMMARY").Value = sanUseScore;
            db.SaveChanges();
            
        }

        //
        // POST: /SFPoint/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATSFpoint tblswatsfpoint)
        {
            if (ModelState.IsValid)
            {
                var recordIDs = db.tblSWATSFpoints.Where(e => e.SurveyID == tblswatsfpoint.SurveyID).Select(e => e.ID);
                if (recordIDs.Any())
                {
                    int recordId = recordIDs.First();
                    tblswatsfpoint.ID = recordId;
                    db.Entry(tblswatsfpoint).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatsfpoint);

                    // sanType = [1001, 1002, 1003]
                    if (tblswatsfpoint.sanType == 1001)
                    {
                        return RedirectToAction("Create", "SFCentral", new { SurveyID = tblswatsfpoint.SurveyID });
                    }
                    else if (tblswatsfpoint.sanType == 1002)
                    {
                        return RedirectToAction("Create", "SFSeptic", new { SurveyID = tblswatsfpoint.SurveyID });
                    }
                    else if (tblswatsfpoint.sanType == 1003)
                    {
                        return RedirectToAction("Create", "SFLat", new { SurveyID = tblswatsfpoint.SurveyID });
                    }
                    return RedirectToAction("WaterPoints", "Survey", new { id = tblswatsfpoint.SurveyID });
                    // return RedirectToAction("Create", "SFPoint", new { SurveyID = tblswatsfpoint.SurveyID });
                }

                db.tblSWATSFpoints.Add(tblswatsfpoint);
                db.SaveChanges();
                updateScores(tblswatsfpoint);

                // sanType = [1001, 1002, 1003]
                if (tblswatsfpoint.sanType == 1001)
                {
                    return RedirectToAction("Create", "SFCentral", new { SurveyID = tblswatsfpoint.SurveyID });
                }
                else if (tblswatsfpoint.sanType == 1002)
                {
                    return RedirectToAction("Create", "SFSeptic", new { SurveyID = tblswatsfpoint.SurveyID });
                }
                else if (tblswatsfpoint.sanType == 1003)
                {
                    return RedirectToAction("Create", "SFLat", new { SurveyID = tblswatsfpoint.SurveyID });
                }
                return RedirectToAction("WaterPoints", "Survey", new { id = tblswatsfpoint.SurveyID});
                // return RedirectToAction("Create", "SFPoint", new { SurveyID = tblswatsfpoint.SurveyID });
            }

            ViewBag.sanType = new SelectList(db.lkpSWATsanTypeLUs, "id", "Description", tblswatsfpoint.sanType);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "sanTypeVAL").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "SanUSeSUMMARY").Description;

            return View(tblswatsfpoint);
        }

        //
        // GET: /SFPoint/Edit/5

        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATSFpoint tblswatsfpoint = db.tblSWATSFpoints.Find(id);
            if (tblswatsfpoint == null)
            {
                return HttpNotFound();
            }
            ViewBag.sanType = new SelectList(db.lkpSWATsanTypeLUs, "id", "Description", tblswatsfpoint.sanType);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "sanTypeVAL").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "SanUSeSUMMARY").Description;

            return View(tblswatsfpoint);
        }

        //
        // POST: /SFPoint/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATSFpoint tblswatsfpoint)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatsfpoint).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatsfpoint);

                // sanType = [1001, 1002, 1003]
                if (tblswatsfpoint.sanType == 1001)
                {
                    var records = db.tblSWATSFcentrals.Where(e => e.SurveyID == tblswatsfpoint.SurveyID);
                    if (!records.Any())
                    {
                        tblSWATSFcentral newEntry = new tblSWATSFcentral();
                        newEntry.SurveyID = tblswatsfpoint.SurveyID;
                        db.tblSWATSFcentrals.Add(newEntry);
                        db.SaveChanges();

                        int newId = newEntry.ID;
                        return RedirectToAction("Edit", "SFCentral", new { id = newId, SurveyID = newEntry.SurveyID });
                    }

                    return RedirectToAction("Edit", "SFCentral", new { id = records.First(e => e.SurveyID == tblswatsfpoint.SurveyID).ID, SurveyID = tblswatsfpoint.SurveyID });

                    
                }
                else if (tblswatsfpoint.sanType == 1002)
                {
                    var records = db.tblSWATSFseptics.Where(e => e.SurveyID == tblswatsfpoint.SurveyID);
                    if (!records.Any())
                    {
                        tblSWATSFseptic newEntry = new tblSWATSFseptic();
                        newEntry.SurveyID = tblswatsfpoint.SurveyID;
                        db.tblSWATSFseptics.Add(newEntry);
                        db.SaveChanges();

                        int newId = newEntry.ID;
                        return RedirectToAction("Edit", "SFSeptic", new { id = newId, SurveyID = newEntry.SurveyID });
                    }

                    return RedirectToAction("Edit", "SFSeptic", new { id = records.First(e => e.SurveyID == tblswatsfpoint.SurveyID).ID, SurveyID = tblswatsfpoint.SurveyID });
                }
                else if (tblswatsfpoint.sanType == 1003)
                {
                    var records = db.tblSWATSFlats.Where(e => e.SurveyID == tblswatsfpoint.SurveyID);
                    if (!records.Any())
                    {
                        tblSWATSFlat newEntry = new tblSWATSFlat();
                        newEntry.SurveyID = tblswatsfpoint.SurveyID;
                        db.tblSWATSFlats.Add(newEntry);
                        db.SaveChanges();

                        int newId = newEntry.ID;
                        return RedirectToAction("Edit", "SFLat", new { id = newId, SurveyID = newEntry.SurveyID });
                    }

                    return RedirectToAction("Edit", "SFLat", new { id = records.First(e => e.SurveyID == tblswatsfpoint.SurveyID).ID, SurveyID = tblswatsfpoint.SurveyID });
                }

                return RedirectToAction("WaterPoints", "Survey", new { id = tblswatsfpoint.SurveyID});
            }
            ViewBag.sanType = new SelectList(db.lkpSWATsanTypeLUs, "id", "Description", tblswatsfpoint.sanType);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "sanTypeVAL").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "SanUSeSUMMARY").Description;

            return View(tblswatsfpoint);
        }

        //
        // GET: /SFPoint/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblSWATSFpoint tblswatsfpoint = db.tblSWATSFpoints.Find(id);
            if (tblswatsfpoint == null)
            {
                return HttpNotFound();
            }
            return View(tblswatsfpoint);
        }

        //
        // POST: /SFPoint/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATSFpoint tblswatsfpoint = db.tblSWATSFpoints.Find(id);
            db.tblSWATSFpoints.Remove(tblswatsfpoint);
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