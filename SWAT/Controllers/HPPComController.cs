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
    public class HPPComController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /HPPCom/

        public ActionResult Index()
        {
            var tblswathppcoms = db.tblSWATHPPcoms.Include(t => t.lkpSWAT5rankLU).Include(t => t.lkpSWATmedicalCostLU).Include(t => t.lkpSWATmedicalTimeLU).Include(t => t.lkpSWATsurvivorshipLU).Include(t => t.tblSWATSurvey);
            return View(tblswathppcoms.ToList());
        }

        //
        // GET: /HPPCom/Details/5

        public ActionResult Details(int id = 0)
        {
            tblSWATHPPcom tblswathppcom = db.tblSWATHPPcoms.Find(id);
            if (tblswathppcom == null)
            {
                return HttpNotFound();
            }
            return View(tblswathppcom);
        }

        //
        // GET: /HPPCom/Create

        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.diarrhea = new SelectList(db.lkpSWAT5rankLU, "id", "Description");
            ViewBag.medicalCost = new SelectList(db.lkpSWATmedicalCostLUs, "id", "Description");
            ViewBag.medicalTime = new SelectList(db.lkpSWATmedicalTimeLUs, "id", "Description");
            ViewBag.survivorship = new SelectList(db.lkpSWATsurvivorshipLUs, "id", "Description");

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "diarrheaSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "survivorshipSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "medicalTimeSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "medicalCostSCORE").Description;

            return View();
        }

        private void updateScores(tblSWATHPPcom tblswathppcom)
        {
            if (tblswathppcom.diarrhea != null)
            {
                int intorder = (int)db.lkpSWAT5rankLU.Find(tblswathppcom.diarrhea).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysBad.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswathppcom.SurveyID && e.VarName == "diarrheaSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswathppcom.SurveyID && e.VarName == "diarrheaSCORE").Value = null;
            }

            if (tblswathppcom.survivorship != null)
            {
                int intorder = (int)db.lkpSWATsurvivorshipLUs.Find(tblswathppcom.survivorship).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_survivorship.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswathppcom.SurveyID && e.VarName == "survivorshipSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswathppcom.SurveyID && e.VarName == "survivorshipSCORE").Value = null;
            }

            if (tblswathppcom.medicalTime != null)
            {
                int intorder = (int)db.lkpSWATmedicalTimeLUs.Find(tblswathppcom.medicalTime).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_medicalTime.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswathppcom.SurveyID && e.VarName == "medicalTimeSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswathppcom.SurveyID && e.VarName == "medicalTimeSCORE").Value = null;
            }

            if (tblswathppcom.medicalCost != null)
            {
                int intorder = (int)db.lkpSWATmedicalCostLUs.Find(tblswathppcom.medicalCost).intorder;
                double score = Convert.ToDouble(db.lkpSWATscores_medicalCost.First(e => e.intorder == intorder).Description);
                db.tblSWATScores.First(e => e.SurveyID == tblswathppcom.SurveyID && e.VarName == "medicalCostSCORE").Value = score;
            }
            else
            {
                db.tblSWATScores.First(e => e.SurveyID == tblswathppcom.SurveyID && e.VarName == "medicalCostSCORE").Value = null;
            }

            db.SaveChanges();
        }

        //
        // POST: /HPPCom/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATHPPcom tblswathppcom)
        {
            if (ModelState.IsValid)
            {
                var recordIDs = db.tblSWATHPPcoms.Where(e => e.SurveyID == tblswathppcom.SurveyID).Select(e => e.ID);
                if (recordIDs.Any())
                {
                    int recordId = recordIDs.First();
                    tblswathppcom.ID = recordId;
                    db.Entry(tblswathppcom).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswathppcom);

                    // return RedirectToAction("Index");
                    return RedirectToAction("Create", "HPPKhp", new { SurveyID = tblswathppcom.SurveyID });
                }

                db.tblSWATHPPcoms.Add(tblswathppcom);
                db.SaveChanges();
                updateScores(tblswathppcom);

                return RedirectToAction("Create", "HPPKhp", new { SurveyID = tblswathppcom.SurveyID });
            }

            ViewBag.diarrhea = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswathppcom.diarrhea);
            ViewBag.medicalCost = new SelectList(db.lkpSWATmedicalCostLUs, "id", "Description", tblswathppcom.medicalCost);
            ViewBag.medicalTime = new SelectList(db.lkpSWATmedicalTimeLUs, "id", "Description", tblswathppcom.medicalTime);
            ViewBag.survivorship = new SelectList(db.lkpSWATsurvivorshipLUs, "id", "Description", tblswathppcom.survivorship);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "diarrheaSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "survivorshipSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "medicalTimeSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "medicalCostSCORE").Description;

            return View(tblswathppcom);
        }

        //
        // GET: /HPPCom/Edit/5

        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATHPPcom tblswathppcom = db.tblSWATHPPcoms.Find(id);
            if (tblswathppcom == null)
            {
                return HttpNotFound();
            }
            ViewBag.diarrhea = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswathppcom.diarrhea);
            ViewBag.medicalCost = new SelectList(db.lkpSWATmedicalCostLUs, "id", "Description", tblswathppcom.medicalCost);
            ViewBag.medicalTime = new SelectList(db.lkpSWATmedicalTimeLUs, "id", "Description", tblswathppcom.medicalTime);
            ViewBag.survivorship = new SelectList(db.lkpSWATsurvivorshipLUs, "id", "Description", tblswathppcom.survivorship);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "diarrheaSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "survivorshipSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "medicalTimeSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "medicalCostSCORE").Description;

            return View(tblswathppcom);
        }

        //
        // POST: /HPPCom/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATHPPcom tblswathppcom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswathppcom).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswathppcom);

                var records = db.tblSWATHPPkhps.Where(e => e.SurveyID == tblswathppcom.SurveyID);
                if (!records.Any())
                {
                    tblSWATHPPkhp newEntry = new tblSWATHPPkhp();
                    newEntry.SurveyID = tblswathppcom.SurveyID;
                    db.tblSWATHPPkhps.Add(newEntry);
                    db.SaveChanges();

                    int newId = newEntry.ID;
                    return RedirectToAction("Edit", "HPPKhp", new { id = newId, SurveyID = newEntry.SurveyID });
                }

                return RedirectToAction("Edit", "HPPKhp", new { id = records.First(e => e.SurveyID == tblswathppcom.SurveyID).ID, SurveyID = tblswathppcom.SurveyID});
            }
            ViewBag.diarrhea = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswathppcom.diarrhea);
            ViewBag.medicalCost = new SelectList(db.lkpSWATmedicalCostLUs, "id", "Description", tblswathppcom.medicalCost);
            ViewBag.medicalTime = new SelectList(db.lkpSWATmedicalTimeLUs, "id", "Description", tblswathppcom.medicalTime);
            ViewBag.survivorship = new SelectList(db.lkpSWATsurvivorshipLUs, "id", "Description", tblswathppcom.survivorship);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "diarrheaSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "survivorshipSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "medicalTimeSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "medicalCostSCORE").Description;

            return View(tblswathppcom);
        }

        //
        // GET: /HPPCom/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblSWATHPPcom tblswathppcom = db.tblSWATHPPcoms.Find(id);
            if (tblswathppcom == null)
            {
                return HttpNotFound();
            }
            return View(tblswathppcom);
        }

        //
        // POST: /HPPCom/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATHPPcom tblswathppcom = db.tblSWATHPPcoms.Find(id);
            db.tblSWATHPPcoms.Remove(tblswathppcom);
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