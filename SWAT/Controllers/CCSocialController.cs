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
    public class CCSocialController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /CCSocial/

        public ActionResult Index()
        {
            var tblswatccsocials = db.tblSWATCCsocials.Include(t => t.lkpSWAT5rankLU).Include(t => t.lkpSWAT5rankLU1).Include(t => t.tblSWATSurvey);
            return View(tblswatccsocials.ToList());
        }

        //
        // GET: /CCSocial/Details/5

        public ActionResult Details(int id = 0)
        {
            tblSWATCCsocial tblswatccsocial = db.tblSWATCCsocials.Find(id);
            if (tblswatccsocial == null)
            {
                return HttpNotFound();
            }
            return View(tblswatccsocial);
        }

        //
        // GET: /CCSocial/Create

        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.socHelp = new SelectList(db.lkpSWAT5rankLU, "id", "Description");
            ViewBag.socClique = new SelectList(db.lkpSWAT5rankLU, "id", "Description");
            ViewBag.socCBO = new SelectList(db.lkpSWATsocCBOLUs, "id", "Description");
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socHelpSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socCliqueSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socCBOSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socAttendSCORE").Description;
            return View();
        }

        private void updateScores(tblSWATCCsocial tblswatccsocial)
        {
            if (tblswatccsocial.socHelp != null)
            {
                int socHelpOrder = Convert.ToInt32(db.lkpSWAT5rankLU.Find(tblswatccsocial.socHelp).intorder);
                double socHelpScore = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysGood.Single(e => e.intorder == socHelpOrder).Description);
                db.tblSWATScores.Single(e => e.VarName == "socHelpSCORE" && e.SurveyID == tblswatccsocial.SurveyID).Value = socHelpScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.VarName == "socHelpSCORE" && e.SurveyID == tblswatccsocial.SurveyID).Value = null;
            }

            if (tblswatccsocial.socClique != null)
            {
                int socCliqueOrder = Convert.ToInt32(db.lkpSWAT5rankLU.Find(tblswatccsocial.socClique).intorder);
                double socCliqueScore = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysBad.Single(e => e.intorder == socCliqueOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccsocial.SurveyID && e.VarName == "socCliqueSCORE").Value = socCliqueScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccsocial.SurveyID && e.VarName == "socCliqueSCORE").Value = null;
            }

            if (tblswatccsocial.socCBO != null)
            {
                int socCboOrder = Convert.ToInt32(db.lkpSWATsocCBOLUs.Find(tblswatccsocial.socCBO).intorder);
                double socCboScore = Convert.ToDouble(db.lkpSWATscores_socCBO.Single(e => e.intorder == socCboOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccsocial.SurveyID && e.VarName == "socCBOSCORE").Value = socCboScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccsocial.SurveyID && e.VarName == "socCBOSCORE").Value = null;
            }

            if (tblswatccsocial.socAttend != null)
            {
                double socAttendScore = (double)tblswatccsocial.socAttend / 100;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccsocial.SurveyID && e.VarName == "socAttendSCORE").Value = socAttendScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccsocial.SurveyID && e.VarName == "socAttendSCORE").Value = null;
            }

            db.SaveChanges();
        }

        //
        // POST: /CCSocial/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATCCsocial tblswatccsocial)
        {
            if (ModelState.IsValid)
            {
                var socialIDs = db.tblSWATCCsocials.Where(e => e.SurveyID == tblswatccsocial.SurveyID).Select(e => e.ID);
                if (socialIDs.Any())
                {
                    int socialID = socialIDs.First();
                    tblswatccsocial.ID = socialID;
                    db.Entry(tblswatccsocial).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatccsocial);

                    // return RedirectToAction("Index");
                    // return RedirectToAction("Report", "Survey", new { id = tblswatccsocial.SurveyID });
                    return RedirectToAction("Create", "CCCom", new { SurveyID = tblswatccsocial.SurveyID });
                }

                db.tblSWATCCsocials.Add(tblswatccsocial);
                db.SaveChanges();
                updateScores(tblswatccsocial);

                // return RedirectToAction("Index");
                // return RedirectToAction("Report", "Survey", new { id = tblswatccsocial.SurveyID });
                return RedirectToAction("Create", "CCCom", new { SurveyID = tblswatccsocial.SurveyID });
            }

            ViewBag.socHelp = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccsocial.socHelp);
            ViewBag.socClique = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccsocial.socClique);
            ViewBag.socCBO = new SelectList(db.lkpSWATsocCBOLUs, "id", "Description", tblswatccsocial.socCBO);
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socHelpSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socCliqueSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socCBOSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socAttendSCORE").Description;
            return View(tblswatccsocial);
        }

        //
        // GET: /CCSocial/Edit/5

        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATCCsocial tblswatccsocial = db.tblSWATCCsocials.Find(id);
            if (tblswatccsocial == null)
            {
                return HttpNotFound();
            }
            ViewBag.socHelp = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccsocial.socHelp);
            ViewBag.socClique = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccsocial.socClique);
            ViewBag.socCBO = new SelectList(db.lkpSWATsocCBOLUs, "id", "Description", tblswatccsocial.socCBO);
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socHelpSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socCliqueSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socCBOSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socAttendSCORE").Description;
            return View(tblswatccsocial);
        }

        //
        // POST: /CCSocial/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATCCsocial tblswatccsocial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatccsocial).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatccsocial);

                // If there is not any CCCom with the current survey (SurveyID) then create one and redirect to its edit link.
                var coms = db.tblSWATCCcoms.Where(e => e.SurveyID == tblswatccsocial.SurveyID);
                if (!coms.Any())
                {
                    tblSWATCCcom tblswatcccom = new tblSWATCCcom();
                    tblswatcccom.SurveyID = tblswatccsocial.SurveyID;
                    db.tblSWATCCcoms.Add(tblswatcccom);
                    db.SaveChanges();

                    int newCCcomId = tblswatcccom.ID;
                    return RedirectToAction("Edit", "CCCom", new { id = newCCcomId, SurveyID = tblswatcccom.SurveyID});
                }

                return RedirectToAction("Edit", "CCCom", new { id = coms.Single(e => e.SurveyID == tblswatccsocial.SurveyID).ID, SurveyID = tblswatccsocial.SurveyID});
            }
            ViewBag.socHelp = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccsocial.socHelp);
            ViewBag.socClique = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccsocial.socClique);
            ViewBag.socCBO = new SelectList(db.lkpSWATsocCBOLUs, "id", "Description", tblswatccsocial.socCBO);
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socHelpSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socCliqueSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socCBOSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "socAttendSCORE").Description;
            return View(tblswatccsocial);
        }

        //
        // GET: /CCSocial/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblSWATCCsocial tblswatccsocial = db.tblSWATCCsocials.Find(id);
            if (tblswatccsocial == null)
            {
                return HttpNotFound();
            }
            return View(tblswatccsocial);
        }

        //
        // POST: /CCSocial/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATCCsocial tblswatccsocial = db.tblSWATCCsocials.Find(id);
            db.tblSWATCCsocials.Remove(tblswatccsocial);
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