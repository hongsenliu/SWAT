﻿using System;
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
    public class SurveyController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /Survey/
        public ActionResult Index()
        {
            var tblswatsurveys = db.tblSWATSurveys.Include(t => t.tblSWATLocation).Include(t => t.Userid1);
            return View(tblswatsurveys.ToList());
        }

        // GET: /Survey/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATSurvey tblswatsurvey = db.tblSWATSurveys.Find(id);
            if (tblswatsurvey == null)
            {
                return HttpNotFound();
            }
            return View(tblswatsurvey);
        }

        // GET: /Survey/Create
        public ActionResult Create(int UserID, int LocationID)
        {
            // Create a survey record
            tblSWATSurvey tblswatsurvey = new tblSWATSurvey();
            tblswatsurvey.LocationID = LocationID;
            tblswatsurvey.UserID = UserID;
            tblswatsurvey.StartTime = DateTime.Now;
            tblswatsurvey.Status = 1;
            
            if (ModelState.IsValid)
            {
                // Add and Save the survey record to database
                db.tblSWATSurveys.Add(tblswatsurvey);
                db.SaveChanges();
                // Get the id of tblswatsurvey record (new record)
                var newSurveyID = tblswatsurvey.ID;
                var scorevars = db.lkpSWATScoreVars.ToList();
                foreach (var scorevar in scorevars)
                {
                    tblSWATScore tblswatscore = new tblSWATScore();
                    tblswatscore.SurveyID = newSurveyID;
                    tblswatscore.VariableID = scorevar.ID;

                    // For reference copy the score name from lkpSWATScoreVars table to tblSWATScore table varname field
                    tblswatscore.VarName = scorevar.VarName;

                    if (ModelState.IsValid)
                    {
                        db.tblSWATScores.Add(tblswatscore);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Details", "User", new { id = UserID});
            }

            ViewBag.LocationID = new SelectList(db.tblSWATLocations, "ID", "name");
            ViewBag.UserID = new SelectList(db.Userids.Where(user => user.type == 0), "Userid1", "Username");
            return View();
        }

        // POST: /Survey/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,UserID,Status,StartTime,EndTime,LocationID")] tblSWATSurvey tblswatsurvey)
        {
            if (ModelState.IsValid)
            {
                db.tblSWATSurveys.Add(tblswatsurvey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationID = new SelectList(db.tblSWATLocations, "ID", "name", tblswatsurvey.LocationID);
            ViewBag.UserID = new SelectList(db.Userids, "Userid1", "Username", tblswatsurvey.UserID);
            return View(tblswatsurvey);
        }

        // GET: /Survey/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATSurvey tblswatsurvey = db.tblSWATSurveys.Find(id);
            if (tblswatsurvey == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationID = new SelectList(db.tblSWATLocations, "ID", "name", tblswatsurvey.LocationID);
            ViewBag.UserID = new SelectList(db.Userids, "Userid1", "Username", tblswatsurvey.UserID);
            return View(tblswatsurvey);
        }

        // POST: /Survey/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,UserID,Status,StartTime,EndTime,LocationID")] tblSWATSurvey tblswatsurvey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatsurvey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationID = new SelectList(db.tblSWATLocations, "ID", "name", tblswatsurvey.LocationID);
            ViewBag.UserID = new SelectList(db.Userids, "Userid1", "Username", tblswatsurvey.UserID);
            return View(tblswatsurvey);
        }

        // GET: /Survey/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATSurvey tblswatsurvey = db.tblSWATSurveys.Find(id);
            if (tblswatsurvey == null)
            {
                return HttpNotFound();
            }
            return View(tblswatsurvey);
        }

        // POST: /Survey/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATSurvey tblswatsurvey = db.tblSWATSurveys.Find(id);
            db.tblSWATSurveys.Remove(tblswatsurvey);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Details", "User", new { id=tblswatsurvey.UserID });
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
