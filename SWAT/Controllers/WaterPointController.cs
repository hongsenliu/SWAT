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
    public class WaterPointController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /WaterPoint/

        public ActionResult Index()
        {
            var tblswatwpoverviews = db.tblSWATWPoverviews.Include(t => t.lkpSWATwpaLoc).Include(t => t.tblSWATSurvey);
            return View(tblswatwpoverviews.ToList());
        }

        //
        // GET: /WaterPoint/Details/5

        public ActionResult Details(long id = 0)
        {
            tblSWATWPoverview tblswatwpoverview = db.tblSWATWPoverviews.Find(id);
            if (tblswatwpoverview == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwpoverview);
        }

        private void getQuestion()
        {
            ViewBag.Question1 = "You can use this form to assess individual water points or groups water points within your community. For example, you may choose to assess all well within one region of your community as one water point. Please enter the name of the first water point here.";
            ViewBag.Question2 = "Where do users go to obtain water from this source?";
            ViewBag.Question3 = "What is this water source used for? (check all that apply)";
        }

        //
        // GET: /WaterPoint/Create

        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null || db.tblSWATWPoverviews.Where(e => e.SurveyID == SurveyID).Count() >= 23)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ViewBag.wpaLoc = new SelectList(db.lkpSWATwpaLocs, "id", "Description");
            getQuestion();

            return View();
        }

        private void updateScores(tblSWATWPoverview tblswatwpoverview)
        {
            if (tblswatwpoverview.wpaLoc != null)
            {
                int intorder = (int)db.lkpSWATwpaLocs.Find(tblswatwpoverview.wpaLoc).intorder;
                string wpatype = db.lkpSWATwpaTypeDescs.First(e => e.intorder == intorder).Description;
                tblswatwpoverview.wpaType = wpatype;
            }
            else
            {
                tblswatwpoverview.wpaType = null;
            }

            db.SaveChanges();
        }

        private void addScores(tblSWATWPoverview tblswatwpoverview)
        {
            // Get the id of tblswatwpoverview record (new record)
            var newWPId = tblswatwpoverview.ID;
            var scorevars = db.lkpSWATWPscoreLUs.ToList();
            foreach (var scorevar in scorevars)
            {
                tblSWATWPscore wpscore = new tblSWATWPscore();
                wpscore.wpID = newWPId;
                wpscore.ScoreID = scorevar.ID;

                // For reference copy the score name from lkpSWATWPscoreLU table to tblswatwpoverview table scorename field
                wpscore.ScoreName = scorevar.ScoreName;

                if (ModelState.IsValid)
                {
                    db.tblSWATWPscores.Add(wpscore);
                    db.SaveChanges();
                }
            }
        }

        //
        // POST: /WaterPoint/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATWPoverview tblswatwpoverview)
        {
            if (ModelState.IsValid)
            {
                var recordIDs = db.tblSWATWPoverviews.Where(e => e.SurveyID == tblswatwpoverview.SurveyID).Select(e => e.ID);
                if (recordIDs.Any())
                {
                    long recordId = recordIDs.First();
                    tblswatwpoverview.ID = recordId;
                    db.Entry(tblswatwpoverview).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatwpoverview);


                    // return RedirectToAction("WaterPoints", "Survey", new { id = tblswatwpoverview.SurveyID });
                    return RedirectToAction("Create", "WPSupply", new { wpID = tblswatwpoverview.ID });
                }

                db.tblSWATWPoverviews.Add(tblswatwpoverview);
                db.SaveChanges();
                updateScores(tblswatwpoverview);

                addScores(tblswatwpoverview);

                // return RedirectToAction("WaterPoints", "Survey", new { id = tblswatwpoverview.SurveyID });
                return RedirectToAction("Create", "WPSupply", new { wpID = tblswatwpoverview.ID });
            }

            ViewBag.wpaLoc = new SelectList(db.lkpSWATwpaLocs, "id", "Description", tblswatwpoverview.wpaLoc);
            getQuestion();
            return View(tblswatwpoverview);
        }

        //
        // GET: /WaterPoint/Edit/5

        public ActionResult Edit(long? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWPoverview tblswatwpoverview = db.tblSWATWPoverviews.Find(id);
            if (tblswatwpoverview == null)
            {
                return HttpNotFound();
            }
            ViewBag.wpaLoc = new SelectList(db.lkpSWATwpaLocs, "id", "Description", tblswatwpoverview.wpaLoc);
            getQuestion();
            return View(tblswatwpoverview);
        }

        //
        // POST: /WaterPoint/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATWPoverview tblswatwpoverview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatwpoverview).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatwpoverview);

                var records = db.tblSWATWPsupplies.Where(e => e.wpID == tblswatwpoverview.ID);
                if (!records.Any())
                {
                    tblSWATWPsupply newEntry = new tblSWATWPsupply();
                    newEntry.wpID = tblswatwpoverview.ID;
                    db.tblSWATWPsupplies.Add(newEntry);
                    db.SaveChanges();

                    long newId = newEntry.ID;
                    return RedirectToAction("Edit", "WPSupply", new { id = newId, wpID = newEntry.wpID });
                }

                return RedirectToAction("Edit", "WPSupply", new { id = records.First(e => e.wpID == tblswatwpoverview.ID).ID, wpID = tblswatwpoverview.ID });

                // return RedirectToAction("Index");
            }
            ViewBag.wpaLoc = new SelectList(db.lkpSWATwpaLocs, "id", "Description", tblswatwpoverview.wpaLoc);
            getQuestion();
            return View(tblswatwpoverview);
        }

        //
        // GET: /WaterPoint/Delete/5

        public ActionResult Delete(long id = 0)
        {
            tblSWATWPoverview tblswatwpoverview = db.tblSWATWPoverviews.Find(id);
            if (tblswatwpoverview == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwpoverview);
        }

        public void DeleteRelatedForms(long id)
        {
            var wpquality = db.tblSWATWPqualities.Where(e => e.wpID == id);
            foreach (tblSWATWPquality item in wpquality)
            {
                db.tblSWATWPqualities.Remove(item);
            }

            var wpsupply = db.tblSWATWPsupplies.Where(e => e.wpID == id);
            foreach (tblSWATWPsupply item in wpsupply)
            {
                db.tblSWATWPsupplies.Remove(item);
            }

            var wpscores = db.tblSWATWPscores.Where(e => e.wpID == id);
            foreach (tblSWATWPscore item in wpscores)
            {
                db.tblSWATWPscores.Remove(item);
            }

            db.SaveChanges();
        }

        //
        // POST: /WaterPoint/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            tblSWATWPoverview tblswatwpoverview = db.tblSWATWPoverviews.Find(id);
            db.tblSWATWPoverviews.Remove(tblswatwpoverview);
            DeleteRelatedForms(id);
            db.SaveChanges();

            return RedirectToAction("WaterPoints", "Survey", new { id = tblswatwpoverview.SurveyID });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}