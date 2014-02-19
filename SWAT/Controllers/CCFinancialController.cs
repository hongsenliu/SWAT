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
    public class CCFinancialController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /CCFinancial/
        public ActionResult Index()
        {
            var tblswatccfinancials = db.tblSWATCCfinancials.Include(t => t.lkpSWATroscaLU).Include(t => t.tblSWATSurvey);
            return View(tblswatccfinancials.ToList());
        }

        // GET: /CCFinancial/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATCCfinancial tblswatccfinancial = db.tblSWATCCfinancials.Find(id);
            if (tblswatccfinancial == null)
            {
                return HttpNotFound();
            }
            return View(tblswatccfinancial);
        }

        // GET: /CCFinancial/Create
        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.rosca = new SelectList(db.lkpSWATroscaLUs, "id", "Description");
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "incomeSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "roscaSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "assetsComSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "assetsIndSCORE").Description;
            return View();
        }

        private void updateScores(tblSWATCCfinancial tblswatccfinancial)
        {
            if (tblswatccfinancial.income != null)
            {
                double? incomeScore = tblswatccfinancial.income / 100;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccfinancial.SurveyID && e.VarName == "incomeSCORE").Value = incomeScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccfinancial.SurveyID && e.VarName == "incomeSCORE").Value = null;
            }

            if (tblswatccfinancial.rosca != null)
            {
                int? roscaOrder = db.lkpSWATroscaLUs.Find(tblswatccfinancial.rosca).intorder;
                double roscaScore = Double.Parse(db.lkpSWATscores_rosca.Single(e => e.intorder == roscaOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccfinancial.SurveyID && e.VarName == "roscaSCORE").Value = roscaScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccfinancial.SurveyID && e.VarName == "roscaSCORE").Value = null;
            }

            bool[] comArr = { tblswatccfinancial.assetsCom1, tblswatccfinancial.assetsCom2, tblswatccfinancial.assetsCom3, tblswatccfinancial.assetsCom4 };

            int numComTrue = 0;
            foreach (bool item in comArr)
            {
                if (item)
                {
                    numComTrue++;
                }
            }
            double comScore = (double)numComTrue / 4.0;
            db.tblSWATScores.Single(e => e.SurveyID == tblswatccfinancial.SurveyID && e.VarName == "assetsComSCORE").Value = comScore;

            bool[] indArr = { tblswatccfinancial.assetsInd1, tblswatccfinancial.assetsInd2, tblswatccfinancial.assetsInd3, tblswatccfinancial.assetsInd4};

            int numIndTrue = 0;
            foreach (bool item in indArr)
            {
                if (item)
                {
                    numIndTrue++;
                }
            }

            double indScore = (double)numIndTrue / 4.0;
            db.tblSWATScores.Single(e => e.SurveyID == tblswatccfinancial.SurveyID && e.VarName == "assetsIndSCORE").Value = indScore;

            db.SaveChanges();
        }

        // POST: /CCFinancial/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SurveyID,income,rosca,assetsCom1,assetsCom2,assetsCom3,assetsCom4,assetsInd1,assetsInd2,assetsInd3,assetsInd4")] tblSWATCCfinancial tblswatccfinancial)
        {
            if (ModelState.IsValid)
            {
                var financialIDs = db.tblSWATCCfinancials.Where(e => e.SurveyID == tblswatccfinancial.SurveyID).Select(e => e.ID);

                if (financialIDs.Any())
                {
                    int financialId = financialIDs.First();
                    tblswatccfinancial.ID = financialId;
                    db.Entry(tblswatccfinancial).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatccfinancial);

                    return RedirectToAction("Index");
                    // return RedirectToAction("Create", "CCFinancial", new { SurveyID = tblswatccindig.SurveyID });
                }

                db.tblSWATCCfinancials.Add(tblswatccfinancial);
                db.SaveChanges();
                updateScores(tblswatccfinancial);

                return RedirectToAction("Index");
            }

            ViewBag.rosca = new SelectList(db.lkpSWATroscaLUs, "id", "Description", tblswatccfinancial.rosca);
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "incomeSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "roscaSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "assetsComSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "assetsIndSCORE").Description;
            return View(tblswatccfinancial);
        }

        // GET: /CCFinancial/Edit/5
        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tblSWATCCfinancial tblswatccfinancial = db.tblSWATCCfinancials.Find(id);
            if (tblswatccfinancial == null)
            {
                return HttpNotFound();
            }
            ViewBag.rosca = new SelectList(db.lkpSWATroscaLUs, "id", "Description", tblswatccfinancial.rosca);
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "incomeSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "roscaSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "assetsComSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "assetsIndSCORE").Description;
            return View(tblswatccfinancial);
        }

        // POST: /CCFinancial/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SurveyID,income,rosca,assetsCom1,assetsCom2,assetsCom3,assetsCom4,assetsInd1,assetsInd2,assetsInd3,assetsInd4")] tblSWATCCfinancial tblswatccfinancial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatccfinancial).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatccfinancial);

                // TODO add redirection

                return RedirectToAction("Index");
            }
            ViewBag.rosca = new SelectList(db.lkpSWATroscaLUs, "id", "Description", tblswatccfinancial.rosca);
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "incomeSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "roscaSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "assetsComSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "assetsIndSCORE").Description;
            return View(tblswatccfinancial);
        }

        // GET: /CCFinancial/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATCCfinancial tblswatccfinancial = db.tblSWATCCfinancials.Find(id);
            if (tblswatccfinancial == null)
            {
                return HttpNotFound();
            }
            return View(tblswatccfinancial);
        }

        // POST: /CCFinancial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATCCfinancial tblswatccfinancial = db.tblSWATCCfinancials.Find(id);
            db.tblSWATCCfinancials.Remove(tblswatccfinancial);
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
