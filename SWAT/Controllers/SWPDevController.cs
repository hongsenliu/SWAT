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
    public class SWPDevController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /SWPDev/

        public ActionResult Index()
        {
            var tblswatswpdevs = db.tblSWATSWPdevs.Include(t => t.lkpSWAT5rankLU).Include(t => t.lkpSWATbestManIndLU).Include(t => t.tblSWATSurvey);
            return View(tblswatswpdevs.ToList());
        }

        //
        // GET: /SWPDev/Details/5

        public ActionResult Details(int id = 0)
        {
            tblSWATSWPdev tblswatswpdev = db.tblSWATSWPdevs.Find(id);
            if (tblswatswpdev == null)
            {
                return HttpNotFound();
            }
            return View(tblswatswpdev);
        }

        //
        // GET: /SWPDev/Create

        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int? devId = db.tblSWATBackgroundinfoes.First(e => e.SurveyID == SurveyID).isEconDev;
            if (devId != 1511)
            {
                // return RedirectToAction("Index");
                return RedirectToAction("Create", "HPPCom", new { SurveyID = SurveyID });
            }

            ViewBag.wwTreatInd = new SelectList(db.lkpSWAT5rankLU, "id", "Description");
            ViewBag.bestManInd = new SelectList(db.lkpSWATbestManIndLUs, "id", "Description");

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "devSiteSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "bestManIndSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "wwTreatIndSCORE").Description;

            return View();
        }

        private void updateScores(tblSWATSWPdev tblswatswpdev)
        {
            int? devSiteTotal = null;
            int?[] devSites = { tblswatswpdev.devSite1, tblswatswpdev.devSite2, tblswatswpdev.devSite3, tblswatswpdev.devSite4 };

            foreach (int? item in devSites)
            {
                if (item != null)
                {
                    devSiteTotal = devSiteTotal.GetValueOrDefault(0) + item;
                }
            }
            tblswatswpdev.devSiteTOTAL = devSiteTotal;

            if (devSiteTotal != null)
            {
                int devSiteOrder = 0;
                foreach (var item in db.lkpSWATdevSiteLUs.OrderByDescending(e => e.Description))
                {
                    if (item.Description <= devSiteTotal)
                    {
                        devSiteOrder = item.intorder;
                        break;
                    }
                }
                if (devSiteOrder > 0)
                {
                    double score = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysGood.First(e => e.intorder == devSiteOrder).Description);
                    db.tblSWATScores.First(e => e.SurveyID == tblswatswpdev.SurveyID && e.VarName == "devSiteSCORE").Value = score;
                }
                else
                {
                    db.tblSWATScores.First(e => e.SurveyID == tblswatswpdev.SurveyID && e.VarName == "devSiteSCORE").Value = null;
                }
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatswpdev.SurveyID && e.VarName == "devSiteSCORE").Value = null;
            }

            if (tblswatswpdev.bestManInd != null)
            {
                int intorder = (int)db.lkpSWATbestManIndLUs.Find(tblswatswpdev.bestManInd).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_bestManInd.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatswpdev.SurveyID && e.VarName == "bestManIndSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatswpdev.SurveyID && e.VarName == "bestManIndSCORE").Value = null;
            }

            if (tblswatswpdev.wwTreatInd != null)
            {
                int intorder = (int)db.lkpSWAT5rankLU.Find(tblswatswpdev.wwTreatInd).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysGood.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswatswpdev.SurveyID && e.VarName == "wwTreatIndSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswatswpdev.SurveyID && e.VarName == "wwTreatIndSCORE").Value = null;
            }

            db.SaveChanges();
        }

        //
        // POST: /SWPDev/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATSWPdev tblswatswpdev)
        {
            if (ModelState.IsValid)
            {
                var devIDs = db.tblSWATSWPdevs.Where(e => e.SurveyID == tblswatswpdev.SurveyID).Select(e => e.ID);
                if (devIDs.Any())
                {
                    int devId = devIDs.First();
                    tblswatswpdev.ID = devId;
                    db.Entry(tblswatswpdev).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatswpdev);

                    // return RedirectToAction("Index");
                    return RedirectToAction("Create", "HPPCom", new { SurveyID = tblswatswpdev.SurveyID });
                }

                db.tblSWATSWPdevs.Add(tblswatswpdev);
                db.SaveChanges();
                updateScores(tblswatswpdev);

                // return RedirectToAction("Index");
                return RedirectToAction("Create", "HPPCom", new { SurveyID = tblswatswpdev.SurveyID });
            }

            ViewBag.wwTreatInd = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatswpdev.wwTreatInd);
            ViewBag.bestManInd = new SelectList(db.lkpSWATbestManIndLUs, "id", "Description", tblswatswpdev.bestManInd);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "devSiteSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "bestManIndSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "wwTreatIndSCORE").Description;

            return View(tblswatswpdev);
        }

        //
        // GET: /SWPDev/Edit/5

        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATSWPdev tblswatswpdev = db.tblSWATSWPdevs.Find(id);
            if (tblswatswpdev == null)
            {
                return HttpNotFound();
            }
            ViewBag.wwTreatInd = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatswpdev.wwTreatInd);
            ViewBag.bestManInd = new SelectList(db.lkpSWATbestManIndLUs, "id", "Description", tblswatswpdev.bestManInd);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "devSiteSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "bestManIndSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "wwTreatIndSCORE").Description;

            return View(tblswatswpdev);
        }

        //
        // POST: /SWPDev/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATSWPdev tblswatswpdev)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatswpdev).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatswpdev);

                var hppcom = db.tblSWATHPPcoms.Where(e => e.SurveyID == tblswatswpdev.SurveyID);
                if (!hppcom.Any())
                {
                    tblSWATHPPcom tblswathppcom = new tblSWATHPPcom();
                    tblswathppcom.SurveyID = tblswatswpdev.SurveyID;
                    db.tblSWATHPPcoms.Add(tblswathppcom);
                    db.SaveChanges();

                    int newHPPcomID = tblswathppcom.ID;
                    return RedirectToAction("Edit", "HPPCom", new { id = newHPPcomID, SurveyID = tblswathppcom.SurveyID });
                }
                return RedirectToAction("Edit", "HPPCom", new { id = hppcom.First(e => e.SurveyID == tblswatswpdev.SurveyID).ID, SurveyID = tblswatswpdev.SurveyID });

                // return RedirectToAction("Index");
            }
            ViewBag.wwTreatInd = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatswpdev.wwTreatInd);
            ViewBag.bestManInd = new SelectList(db.lkpSWATbestManIndLUs, "id", "Description", tblswatswpdev.bestManInd);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "devSiteSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "bestManIndSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "wwTreatIndSCORE").Description;

            return View(tblswatswpdev);
        }

        //
        // GET: /SWPDev/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblSWATSWPdev tblswatswpdev = db.tblSWATSWPdevs.Find(id);
            if (tblswatswpdev == null)
            {
                return HttpNotFound();
            }
            return View(tblswatswpdev);
        }

        //
        // POST: /SWPDev/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATSWPdev tblswatswpdev = db.tblSWATSWPdevs.Find(id);
            db.tblSWATSWPdevs.Remove(tblswatswpdev);
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