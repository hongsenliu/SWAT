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
    public class CCTrainController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /CCTrain/
        public ActionResult Index()
        {
            var tblswatcctrains = db.tblSWATCCtrains.Include(t => t.tblSWATSurvey);
            return View(tblswatcctrains.ToList());
        }

        // GET: /CCTrain/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATCCtrain tblswatcctrain = db.tblSWATCCtrains.Find(id);
            if (tblswatcctrain == null)
            {
                return HttpNotFound();
            }
            return View(tblswatcctrain);
        }

        // GET: /CCTrain/Create
        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "trainProfSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "trainTechSCORE").Description;
            return View();
        }

        private void updateScores(tblSWATCCtrain tblswatcctrain)
        {
            bool[] trainProfs = {tblswatcctrain.trainProf1, tblswatcctrain.trainProf2, tblswatcctrain.trainProf3,
                                 tblswatcctrain.trainProf4, tblswatcctrain.trainProf5};
            int numTrainProf = 0;
            foreach (bool item in trainProfs)
            {
                if (item)
                {
                    numTrainProf++;
                }
            }
            double trainProfScore = numTrainProf / 5.0;
            db.tblSWATScores.Single(e => e.SurveyID == tblswatcctrain.SurveyID && e.VarName == "trainProfSCORE").Value = trainProfScore;

            bool[] trainTechs = {tblswatcctrain.trainTech1, tblswatcctrain.trainTech2, tblswatcctrain.trainTech3,
                                 tblswatcctrain.trainTech4, tblswatcctrain.trainTech5};
            int numTrainTech = 0;
            foreach (bool item in trainTechs)
            {
                if (item)
                {
                    numTrainTech++;
                }
            }
            double trainTechScore = numTrainTech / 5.0;
            db.tblSWATScores.Single(e => e.SurveyID == tblswatcctrain.SurveyID && e.VarName == "trainTechSCORE").Value = trainTechScore;

            db.SaveChanges();
        }

        // POST: /CCTrain/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SurveyID,trainProf1,trainProf2,trainProf3,trainProf4,trainProf5,trainTech1,trainTech2,trainTech3,trainTech4,trainTech5")] tblSWATCCtrain tblswatcctrain)
        {
            if (ModelState.IsValid)
            {
                var trainIDs = db.tblSWATCCtrains.Where(e => e.SurveyID == tblswatcctrain.SurveyID).Select(e => e.ID);

                if (trainIDs.Any())
                {
                    int trainId = trainIDs.First();
                    tblswatcctrain.ID = trainId;
                    db.Entry(tblswatcctrain).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatcctrain);
                    //return RedirectToAction("Index");
                    return RedirectToAction("Create", "CCSchool", new { SurveyID = tblswatcctrain.SurveyID });
                }

                db.tblSWATCCtrains.Add(tblswatcctrain);
                db.SaveChanges();
                updateScores(tblswatcctrain);

                ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "trainProfSCORE").Description;
                ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "trainTechSCORE").Description;
                // return RedirectToAction("Index");
                return RedirectToAction("Create", "CCSchool", new { SurveyID = tblswatcctrain.SurveyID });
            }

            return View(tblswatcctrain);
        }

        // GET: /CCTrain/Edit/5
        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATCCtrain tblswatcctrain = db.tblSWATCCtrains.Find(id);
            if (tblswatcctrain == null)
            {
                return HttpNotFound();
            }

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "trainProfSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "trainTechSCORE").Description;
            return View(tblswatcctrain);
        }

        // POST: /CCTrain/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SurveyID,trainProf1,trainProf2,trainProf3,trainProf4,trainProf5,trainTech1,trainTech2,trainTech3,trainTech4,trainTech5")] tblSWATCCtrain tblswatcctrain)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatcctrain).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatcctrain);

                // If there is not any CCSchool with the current survey (SurveyID) then create one and redirect to its edit link.
                var schools = db.tblSWATCCschools.Where(e => e.SurveyID == tblswatcctrain.SurveyID);
                if (!schools.Any())
                {
                    tblSWATCCschool tblswatccschool = new tblSWATCCschool();
                    tblswatccschool.SurveyID = tblswatcctrain.SurveyID;
                    db.tblSWATCCschools.Add(tblswatccschool);
                    db.SaveChanges();
                    int newSchoolID = tblswatccschool.ID;

                    return RedirectToAction("Edit", "CCSchool", new { id = newSchoolID, SurveyID = tblswatccschool.SurveyID });
                }
                else
                {
                    return RedirectToAction("Edit", "CCSchool", new { id = schools.Single(e => e.SurveyID == tblswatcctrain.SurveyID).ID, SurveyID = tblswatcctrain.SurveyID });
                }
                // return RedirectToAction("Index");
            }

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "trainProfSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "trainTechSCORE").Description;
            return View(tblswatcctrain);
        }

        // GET: /CCTrain/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATCCtrain tblswatcctrain = db.tblSWATCCtrains.Find(id);
            if (tblswatcctrain == null)
            {
                return HttpNotFound();
            }
            return View(tblswatcctrain);
        }

        // POST: /CCTrain/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATCCtrain tblswatcctrain = db.tblSWATCCtrains.Find(id);
            db.tblSWATCCtrains.Remove(tblswatcctrain);
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
