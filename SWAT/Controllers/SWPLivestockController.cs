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
    public class SWPLivestockController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /SWPLivestock/

        public ActionResult Index()
        {
            var tblswatswpls = db.tblSWATSWPls.Include(t => t.lkpSWAT5rankLU).Include(t => t.tblSWATSurvey);
            return View(tblswatswpls.ToList());
        }

        //
        // GET: /SWPLivestock/Details/5

        public ActionResult Details(int id = 0)
        {
            tblSWATSWPl tblswatswpl = db.tblSWATSWPls.Find(id);
            if (tblswatswpl == null)
            {
                return HttpNotFound();
            }
            return View(tblswatswpl);
        }

        //
        // GET: /SWPLivestock/Create

        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int? lsId = db.tblSWATBackgroundinfoes.First(e => e.SurveyID == SurveyID).isEconLs;
            if (lsId != 1511)
            {
                // return RedirectToAction("Index");
                return RedirectToAction("Create", "SWPAg", new { SurveyID = SurveyID});
            }

            ViewBag.livestockEffluent = new SelectList(db.lkpSWAT5rankLU, "id", "Description");

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "livestockSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "waterFencedSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "livestockEffluentSCORE").Description;

            return View();
        }

        private void updateScores(tblSWATSWPl tblswatswpl)
        {
            if (tblswatswpl.livestock1)
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatswpl.SurveyID && e.VarName == "livestockSCORE").Value = 1;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatswpl.SurveyID && e.VarName == "livestockSCORE").Value = 0;
            }

            if (tblswatswpl.waterFenced != null)
            {
                double waterFencedScore = (double)tblswatswpl.waterFenced / 100;
                db.tblSWATScores.First(e => e.SurveyID == tblswatswpl.SurveyID && e.VarName == "waterFencedSCORE").Value = waterFencedScore;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatswpl.SurveyID && e.VarName == "waterFencedSCORE").Value = null;
            }

            if (tblswatswpl.livestockEffluent != null)
            {
                int lsOrder = (int)db.lkpSWAT5rankLU.Find(tblswatswpl.livestockEffluent).intorder;
                double lsScore = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysGood.First(e => e.intorder == lsOrder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatswpl.SurveyID && e.VarName == "livestockEffluentSCORE").Value = lsScore;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatswpl.SurveyID && e.VarName == "livestockEffluentSCORE").Value = null;
            }
            db.SaveChanges();

        }

        //
        // POST: /SWPLivestock/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATSWPl tblswatswpl)
        {
            if (ModelState.IsValid)
            {
                var lsIDs = db.tblSWATSWPls.Where(e => e.SurveyID == tblswatswpl.SurveyID).Select(e => e.ID);
                if (lsIDs.Any())
                {
                    int lsId = lsIDs.First();
                    tblswatswpl.ID = lsId;
                    db.Entry(tblswatswpl).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatswpl);

                    // return RedirectToAction("Index");
                    return RedirectToAction("Create", "SWPAg", new { SurveyID = tblswatswpl.SurveyID });
                }

                db.tblSWATSWPls.Add(tblswatswpl);
                db.SaveChanges();
                updateScores(tblswatswpl);

                // return RedirectToAction("Index");
                return RedirectToAction("Create", "SWPAg", new { SurveyID = tblswatswpl.SurveyID });
            }

            ViewBag.livestockEffluent = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatswpl.livestockEffluent);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "livestockSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "waterFencedSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "livestockEffluentSCORE").Description;

            return View(tblswatswpl);
        }

        //
        // GET: /SWPLivestock/Edit/5

        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATSWPl tblswatswpl = db.tblSWATSWPls.Find(id);
            if (tblswatswpl == null)
            {
                return HttpNotFound();
            }
            ViewBag.livestockEffluent = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatswpl.livestockEffluent);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "livestockSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "waterFencedSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "livestockEffluentSCORE").Description;

            return View(tblswatswpl);
        }

        //
        // POST: /SWPLivestock/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATSWPl tblswatswpl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatswpl).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatswpl);

                var background = db.tblSWATBackgroundinfoes.First(e => e.SurveyID == tblswatswpl.SurveyID);
                if (background != null)
                {
                    // id = 1511 is the "Yes" option in database
                        if (background.isEconAg == 1511)
                    { 
                        // TODO redirect to ag form
                        var swpag = db.tblSWATSWPags.Where(e => e.SurveyID == tblswatswpl.SurveyID);
                        if (!swpag.Any())
                        {
                            tblSWATSWPag tblswatswpag = new tblSWATSWPag();
                            tblswatswpag.SurveyID = tblswatswpl.SurveyID;
                            db.tblSWATSWPags.Add(tblswatswpag);
                            db.SaveChanges();

                            int newSWPagID = tblswatswpag.ID;
                            return RedirectToAction("Edit", "SWPAg", new { id = newSWPagID, SurveyID = tblswatswpag.SurveyID});
                        }
                        return RedirectToAction("Edit", "SWPAg", new { id = swpag.First(e => e.SurveyID == tblswatswpl.SurveyID).ID, SurveyID = tblswatswpl.SurveyID });
                    }
                    else if (background.isEconDev == 1511)
                    { 
                        // TODO redirect to dev form
                        var swpdev = db.tblSWATSWPdevs.Where(e => e.SurveyID == tblswatswpl.SurveyID);
                        if (!swpdev.Any())
                        {
                            tblSWATSWPdev tblswatswpdev = new tblSWATSWPdev();
                            tblswatswpdev.SurveyID = tblswatswpl.SurveyID;
                            db.tblSWATSWPdevs.Add(tblswatswpdev);
                            db.SaveChanges();

                            int newSWPdevID = tblswatswpdev.ID;
                            return RedirectToAction("Edit", "SWPDev", new { id = newSWPdevID, SurveyID = tblswatswpdev.SurveyID});
                        }
                        return RedirectToAction("Edit", "SWPDev", new { id = swpdev.First(e => e.SurveyID == tblswatswpl.SurveyID).ID, SurveyID = tblswatswpl.SurveyID });
                    }
                }
                // TODO redirect to health form
                var hppcom = db.tblSWATHPPcoms.Where(e => e.SurveyID == tblswatswpl.SurveyID);
                if (!hppcom.Any())
                {
                    tblSWATHPPcom tblswathppcom = new tblSWATHPPcom();
                    tblswathppcom.SurveyID = tblswatswpl.SurveyID;
                    db.tblSWATHPPcoms.Add(tblswathppcom);
                    db.SaveChanges();

                    int newHPPcomID = tblswathppcom.ID;
                    return RedirectToAction("Edit", "HPPCom", new { id = newHPPcomID, SurveyID = tblswathppcom.SurveyID });
                }
                return RedirectToAction("Edit", "HPPCom", new { id = hppcom.First(e => e.SurveyID == tblswatswpl.SurveyID).ID, SurveyID = tblswatswpl.SurveyID });
            }
            ViewBag.livestockEffluent = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatswpl.livestockEffluent);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "livestockSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "waterFencedSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "livestockEffluentSCORE").Description;

            return View(tblswatswpl);
        }

        //
        // GET: /SWPLivestock/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblSWATSWPl tblswatswpl = db.tblSWATSWPls.Find(id);
            if (tblswatswpl == null)
            {
                return HttpNotFound();
            }
            return View(tblswatswpl);
        }

        //
        // POST: /SWPLivestock/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATSWPl tblswatswpl = db.tblSWATSWPls.Find(id);
            db.tblSWATSWPls.Remove(tblswatswpl);
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