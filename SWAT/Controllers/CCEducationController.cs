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
    public class CCEducationController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /CCEducation/
        public ActionResult Index()
        {
            var tblswatccedus = db.tblSWATCCedus.Include(t => t.tblSWATSurvey);
            return View(tblswatccedus.ToList());
        }

        // GET: /CCEducation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATCCedu tblswatccedu = db.tblSWATCCedus.Find(id);
            if (tblswatccedu == null)
            {
                return HttpNotFound();
            }
            return View(tblswatccedu);
        }

        // GET: /CCEducation/Create
        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduPrimeSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduSecSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduGradWomenSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduGradMenSCORE").Description;
            return View();
        }

        private void updateScores(tblSWATCCedu tblswatccedu)
        {
            if (tblswatccedu.eduPrim != null)
            {
                double? eduPrimeScore = tblswatccedu.eduPrim / 100;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccedu.SurveyID && e.VarName == "eduPrimeSCORE").Value = eduPrimeScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccedu.SurveyID && e.VarName == "eduPrimeSCORE").Value = null;
            }

            if (tblswatccedu.eduSec != null)
            {
                double? eduSecScore = tblswatccedu.eduSec / 100;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccedu.SurveyID && e.VarName == "eduSecSCORE").Value = eduSecScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccedu.SurveyID && e.VarName == "eduSecSCORE").Value = null;
            }

            if (tblswatccedu.eduGradWomen != null)
            {
                double? eduGradWomenScore = tblswatccedu.eduGradWomen / 100;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccedu.SurveyID && e.VarName == "eduGradWomenSCORE").Value = eduGradWomenScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccedu.SurveyID && e.VarName == "eduGradWomenSCORE").Value = null;
            }

            if (tblswatccedu.eduGradMen != null)
            {
                double? eduGradMenScore = tblswatccedu.eduGradMen / 100;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccedu.SurveyID && e.VarName == "eduGradMenSCORE").Value = eduGradMenScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccedu.SurveyID && e.VarName == "eduGradMenSCORE").Value = null;
            }

            if (tblswatccedu.eduGradMen != null && tblswatccedu.eduGradWomen != null)
            {
                double? eduGradDiffMenWomen = 2 * (tblswatccedu.eduGradMen - tblswatccedu.eduGradWomen) / (tblswatccedu.eduGradMen + tblswatccedu.eduGradWomen);
                int? eduGradDiffOrder = null;
                foreach (var item in db.lkpSWATeduGradDiffLUs.OrderByDescending(e => e.Description))
                {
                    if (Double.Parse(item.Description) <= eduGradDiffMenWomen)
                    {
                        eduGradDiffOrder = item.intorder;
                        break;
                    }
                }
                double? eduGradDiffScore = Double.Parse(db.lkpSWATscores_eduGradDiff.Single(e => e.intorder == eduGradDiffOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccedu.SurveyID && e.VarName == "eduGradDiffMenWomen").Value = eduGradDiffMenWomen;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccedu.SurveyID && e.VarName == "eduGradDiffMenWomenSCORE").Value = eduGradDiffScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccedu.SurveyID && e.VarName == "eduGradDiffMenWomen").Value = null;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccedu.SurveyID && e.VarName == "eduGradDiffMenWomenSCORE").Value = null;
            }

            db.SaveChanges();

        }

        // POST: /CCEducation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SurveyID,eduPrim,eduSec,eduGradWomen,eduGradMen")] tblSWATCCedu tblswatccedu)
        {
            if (ModelState.IsValid)
            {
                var educationIDs = db.tblSWATCCedus.Where(e => e.SurveyID == tblswatccedu.SurveyID).Select(e => e.ID);
                if (educationIDs.Any())
                {
                    int educationId = educationIDs.First();
                    tblswatccedu.ID = educationId;
                    db.Entry(tblswatccedu).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatccedu);
                    // return RedirectToAction("Index");
                    return RedirectToAction("Create", "CCTrain", new { SurveyID = tblswatccedu.SurveyID });
                }

                db.tblSWATCCedus.Add(tblswatccedu);
                db.SaveChanges();
                updateScores(tblswatccedu);
                //return RedirectToAction("Index");
                return RedirectToAction("Create", "CCTrain", new { SurveyID = tblswatccedu.SurveyID });
            }

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduPrimeSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduSecSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduGradWomenSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduGradMenSCORE").Description;
            return View(tblswatccedu);
        }

        // GET: /CCEducation/Edit/5
        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATCCedu tblswatccedu = db.tblSWATCCedus.Find(id);
            if (tblswatccedu == null)
            {
                return HttpNotFound();
            }

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduPrimeSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduSecSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduGradWomenSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduGradMenSCORE").Description;
            return View(tblswatccedu);
        }

        // POST: /CCEducation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SurveyID,eduPrim,eduSec,eduGradWomen,eduGradMen")] tblSWATCCedu tblswatccedu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatccedu).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatccedu);

                // If there is not any CCTrain with the current survey (SurveyID) then create one and redirect to its edit link.
                var trains = db.tblSWATCCtrains.Where(e => e.SurveyID == tblswatccedu.SurveyID);
                if (!trains.Any())
                {
                    tblSWATCCtrain tblswatcctrain = new tblSWATCCtrain();
                    tblswatcctrain.SurveyID = tblswatccedu.SurveyID;
                    db.tblSWATCCtrains.Add(tblswatcctrain);
                    db.SaveChanges();

                    int newTrainID = tblswatcctrain.ID;
                    return RedirectToAction("Edit", "CCTrain", new { id = newTrainID, SurveyID = tblswatcctrain.SurveyID });
                }
                else
                {
                    return RedirectToAction("Edit", "CCTrain", new { id = trains.Single(e => e.SurveyID == tblswatccedu.SurveyID).ID, SurveyID = tblswatccedu.SurveyID });
                }

                //return RedirectToAction("Index");
            }

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduPrimeSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduSecSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduGradWomenSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "eduGradMenSCORE").Description;
            return View(tblswatccedu);
        }

        // GET: /CCEducation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATCCedu tblswatccedu = db.tblSWATCCedus.Find(id);
            if (tblswatccedu == null)
            {
                return HttpNotFound();
            }
            return View(tblswatccedu);
        }

        // POST: /CCEducation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATCCedu tblswatccedu = db.tblSWATCCedus.Find(id);
            db.tblSWATCCedus.Remove(tblswatccedu);
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
