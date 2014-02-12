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
    public class BackgroundController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /Background/
        public ActionResult Index()
        {
            var tblswatbackgroundinfoes = db.tblSWATBackgroundinfoes.Include(t => t.lkpBiome).Include(t => t.lkpClimateClassification).Include(t => t.lkpSoil).Include(t => t.lkpSWATareaProtLU).Include(t => t.lkpSWATmapAridity).Include(t => t.lkpSWATurbanDistanceLU).Include(t => t.lkpSWATWatershedsLU).Include(t => t.tblSWATSurvey).Include(t => t.lkpSWATareaBMLU).Include(t => t.lkpSWATeconPrisLU).Include(t => t.lkpSWATpriorLU).Include(t => t.lkpSWATpriorLU1).Include(t => t.lkpSWATpriorLU2).Include(t => t.lkpSWATpriorLU3).Include(t => t.lkpSWATpriorLU4).Include(t => t.lkpSWATpriorLU5).Include(t => t.lkpSWATpriorLU6).Include(t => t.lkpSWATpriorLU7).Include(t => t.lkpSWATYesNoLU).Include(t => t.lkpSWATYesNoLU1).Include(t => t.lkpSWATYesNoLU2);
            return View(tblswatbackgroundinfoes.ToList());
        }

        // GET: /Background/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATBackgroundinfo tblswatbackgroundinfo = db.tblSWATBackgroundinfoes.Find(id);
            if (tblswatbackgroundinfo == null)
            {
                return HttpNotFound();
            }
            return View(tblswatbackgroundinfo);
        }

        // GET: /Background/Create
        public ActionResult Create(int? SurveyID)
        {
            if ( SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.EcoregionID = new SelectList(db.lkpBiomes, "ID", "Description");
            ViewBag.ClimateID = new SelectList(db.lkpClimateClassifications, "ID", "CCType");
            ViewBag.SoilID = new SelectList(db.lkpSoils, "ID", "Name");
            ViewBag.AreaProtID = new SelectList(db.lkpSWATareaProtLUs, "id", "Description");
            ViewBag.AridityID = new SelectList(db.lkpSWATmapAridities, "id", "Description");
            ViewBag.UrbanDistanceID = new SelectList(db.lkpSWATurbanDistanceLUs, "id", "Description");
            ViewBag.WatershedID = new SelectList(db.lkpSWATWatershedsLUs, "ID", "Description");
            //ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID");
            ViewBag.AreaBmID = new SelectList(db.lkpSWATareaBMLUs, "id", "Description");
            ViewBag.isEconPris = new SelectList(db.lkpSWATeconPrisLUs, "id", "Description");
            ViewBag.PriorQuality = new SelectList(db.lkpSWATpriorLUs, "id", "Description");
            ViewBag.PriorQuan = new SelectList(db.lkpSWATpriorLUs, "id", "Description");
            ViewBag.PriorSeasonal = new SelectList(db.lkpSWATpriorLUs, "id", "Description");
            ViewBag.PriorPolitics = new SelectList(db.lkpSWATpriorLUs, "id", "Description");
            ViewBag.PriorHealth = new SelectList(db.lkpSWATpriorLUs, "id", "Description");
            ViewBag.PriorFinances = new SelectList(db.lkpSWATpriorLUs, "id", "Description");
            ViewBag.PriorAccessible = new SelectList(db.lkpSWATpriorLUs, "id", "Description");
            ViewBag.PriorEquity = new SelectList(db.lkpSWATpriorLUs, "id", "Description");
            ViewBag.isEconAg = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");
            ViewBag.isEconLs = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");
            ViewBag.isEconDev = new SelectList(db.lkpSWATYesNoLUs, "id", "Description");
            ViewBag.SurveyID = SurveyID;
            return View();
        }

        // Helper method to update the scores that occur in the background infomation section
        private void updateScores(tblSWATBackgroundinfo tblswatbackgroundinfo)
        {
            
            
        }

        // POST: /Background/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SurveyID,ClimateID,SoilID,EcoregionID,WatershedID,AridityID,UrbanDistanceID,Population,numHouseholds,numChildren,PeoplePerHH,isEconAg,isEconLs,isEconDev,isEconPris,Area,AreaForest,AreaAg,AreaInf,AreaSw,AreaWet,AreaNat,AreaProtID,AreaBmID,PriorQuality,PriorQuan,PriorSeasonal,PriorPolitics,PriorHealth,PriorFinances,PriorAccessible,PriorEquity")] tblSWATBackgroundinfo tblswatbackgroundinfo)
        {
            // Check if the sum of Forest(%), Agriculture(%), Infrastructure(%), Source Water(%) and Wetlands(%) exceeds 100.
            var totalArea = tblswatbackgroundinfo.AreaForest.GetValueOrDefault(0) 
                            + tblswatbackgroundinfo.AreaAg.GetValueOrDefault(0) 
                            + tblswatbackgroundinfo.AreaInf.GetValueOrDefault(0)
                            + tblswatbackgroundinfo.AreaSw.GetValueOrDefault(0) 
                            + tblswatbackgroundinfo.AreaWet.GetValueOrDefault(0);
            if (totalArea > 100)
            {
                ModelState.AddModelError("AreaForest", "The sum of Forest(%), Agriculture(%), Infrastructure(%), Source Water(%) and Wetlands(%) cannot exceed 100.");
                ModelState.AddModelError("AreaAg", "The sum of Forest(%), Agriculture(%), Infrastructure(%), Source Water(%) and Wetlands(%) cannot exceed 100.");
                ModelState.AddModelError("AreaInf", "The sum of Forest(%), Agriculture(%), Infrastructure(%), Source Water(%) and Wetlands(%) cannot exceed 100.");
                ModelState.AddModelError("AreaSw", "The sum of Forest(%), Agriculture(%), Infrastructure(%), Source Water(%) and Wetlands(%) cannot exceed 100.");
                ModelState.AddModelError("AreaWet", "The sum of Forest(%), Agriculture(%), Infrastructure(%), Source Water(%) and Wetlands(%) cannot exceed 100.");
            }
            var backgrdIDs = db.tblSWATBackgroundinfoes.Where(item => item.SurveyID == tblswatbackgroundinfo.SurveyID).Select(item => item.ID);
            if (backgrdIDs.Any())
            {
                var backgrdID = backgrdIDs.First();
                tblswatbackgroundinfo.ID = backgrdID;
                db.Entry(tblswatbackgroundinfo).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatbackgroundinfo);
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                db.tblSWATBackgroundinfoes.Add(tblswatbackgroundinfo);
                db.SaveChanges();
                //var aridityOrder = db.lkpSWATmapAridities.Find(tblswatbackgroundinfo.AridityID).intorder;
                //var aridityScore = db.lkpSWATscores_Aridity.Where(item => item.intorder == aridityOrder).Select(item => item.Description).First();
                //var scoreID = db.tblSWATScores.Where(item => item.SurveyID == tblswatbackgroundinfo.SurveyID && item.VarName == "ariditySCORE");
                updateScores(tblswatbackgroundinfo);
                
                

                return RedirectToAction("Index");
            }

            ViewBag.EcoregionID = new SelectList(db.lkpBiomes, "ID", "Description", tblswatbackgroundinfo.EcoregionID);
            ViewBag.ClimateID = new SelectList(db.lkpClimateClassifications, "ID", "CCType", tblswatbackgroundinfo.ClimateID);
            ViewBag.SoilID = new SelectList(db.lkpSoils, "ID", "Name", tblswatbackgroundinfo.SoilID);
            ViewBag.AreaProtID = new SelectList(db.lkpSWATareaProtLUs, "id", "Description", tblswatbackgroundinfo.AreaProtID);
            ViewBag.AridityID = new SelectList(db.lkpSWATmapAridities, "id", "Description", tblswatbackgroundinfo.AridityID);
            ViewBag.UrbanDistanceID = new SelectList(db.lkpSWATurbanDistanceLUs, "id", "Description", tblswatbackgroundinfo.UrbanDistanceID);
            ViewBag.WatershedID = new SelectList(db.lkpSWATWatershedsLUs, "ID", "Description", tblswatbackgroundinfo.WatershedID);
            //ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID", tblswatbackgroundinfo.SurveyID);
            ViewBag.AreaBmID = new SelectList(db.lkpSWATareaBMLUs, "id", "Description", tblswatbackgroundinfo.AreaBmID);
            ViewBag.isEconPris = new SelectList(db.lkpSWATeconPrisLUs, "id", "Description", tblswatbackgroundinfo.isEconPris);
            ViewBag.PriorQuality = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorQuality);
            ViewBag.PriorQuan = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorQuan);
            ViewBag.PriorSeasonal = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorSeasonal);
            ViewBag.PriorPolitics = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorPolitics);
            ViewBag.PriorHealth = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorHealth);
            ViewBag.PriorFinances = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorFinances);
            ViewBag.PriorAccessible = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorAccessible);
            ViewBag.PriorEquity = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorEquity);
            ViewBag.isEconAg = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatbackgroundinfo.isEconAg);
            ViewBag.isEconLs = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatbackgroundinfo.isEconLs);
            ViewBag.isEconDev = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatbackgroundinfo.isEconDev);
            ViewBag.SurveyID = tblswatbackgroundinfo.SurveyID;
            return View(tblswatbackgroundinfo);
        }

        // GET: /Background/Edit/5
        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATBackgroundinfo tblswatbackgroundinfo = db.tblSWATBackgroundinfoes.Find(id);
            if (tblswatbackgroundinfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.EcoregionID = new SelectList(db.lkpBiomes, "ID", "Description", tblswatbackgroundinfo.EcoregionID);
            ViewBag.ClimateID = new SelectList(db.lkpClimateClassifications, "ID", "CCType", tblswatbackgroundinfo.ClimateID);
            ViewBag.SoilID = new SelectList(db.lkpSoils, "ID", "Name", tblswatbackgroundinfo.SoilID);
            ViewBag.AreaProtID = new SelectList(db.lkpSWATareaProtLUs, "id", "Description", tblswatbackgroundinfo.AreaProtID);
            ViewBag.AridityID = new SelectList(db.lkpSWATmapAridities, "id", "Description", tblswatbackgroundinfo.AridityID);
            ViewBag.UrbanDistanceID = new SelectList(db.lkpSWATurbanDistanceLUs, "id", "Description", tblswatbackgroundinfo.UrbanDistanceID);
            ViewBag.WatershedID = new SelectList(db.lkpSWATWatershedsLUs, "ID", "Description", tblswatbackgroundinfo.WatershedID);
            //ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID", tblswatbackgroundinfo.SurveyID);
            ViewBag.AreaBmID = new SelectList(db.lkpSWATareaBMLUs, "id", "Description", tblswatbackgroundinfo.AreaBmID);
            ViewBag.isEconPris = new SelectList(db.lkpSWATeconPrisLUs, "id", "Description", tblswatbackgroundinfo.isEconPris);
            ViewBag.PriorQuality = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorQuality);
            ViewBag.PriorQuan = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorQuan);
            ViewBag.PriorSeasonal = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorSeasonal);
            ViewBag.PriorPolitics = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorPolitics);
            ViewBag.PriorHealth = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorHealth);
            ViewBag.PriorFinances = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorFinances);
            ViewBag.PriorAccessible = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorAccessible);
            ViewBag.PriorEquity = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorEquity);
            ViewBag.isEconAg = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatbackgroundinfo.isEconAg);
            ViewBag.isEconLs = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatbackgroundinfo.isEconLs);
            ViewBag.isEconDev = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatbackgroundinfo.isEconDev);
            ViewBag.SurveyID = SurveyID;
            return View(tblswatbackgroundinfo);
        }

        // POST: /Background/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SurveyID,ClimateID,SoilID,EcoregionID,WatershedID,AridityID,UrbanDistanceID,Population,numHouseholds,numChildren,PeoplePerHH,isEconAg,isEconLs,isEconDev,isEconPris,Area,AreaForest,AreaAg,AreaInf,AreaSw,AreaWet,AreaNat,AreaProtID,AreaBmID,PriorQuality,PriorQuan,PriorSeasonal,PriorPolitics,PriorHealth,PriorFinances,PriorAccessible,PriorEquity")] tblSWATBackgroundinfo tblswatbackgroundinfo)
        {
            // Check if the sum of Forest(%), Agriculture(%), Infrastructure(%), Source Water(%) and Wetlands(%) exceeds 100.
            var totalArea = tblswatbackgroundinfo.AreaForest.GetValueOrDefault(0)
                            + tblswatbackgroundinfo.AreaAg.GetValueOrDefault(0)
                            + tblswatbackgroundinfo.AreaInf.GetValueOrDefault(0)
                            + tblswatbackgroundinfo.AreaSw.GetValueOrDefault(0)
                            + tblswatbackgroundinfo.AreaWet.GetValueOrDefault(0);
            if (totalArea > 100)
            {
                ModelState.AddModelError("AreaForest", "The sum of Forest(%), Agriculture(%), Infrastructure(%), Source Water(%) and Wetlands(%) cannot exceed 100.");
                ModelState.AddModelError("AreaAg", "The sum of Forest(%), Agriculture(%), Infrastructure(%), Source Water(%) and Wetlands(%) cannot exceed 100.");
                ModelState.AddModelError("AreaInf", "The sum of Forest(%), Agriculture(%), Infrastructure(%), Source Water(%) and Wetlands(%) cannot exceed 100.");
                ModelState.AddModelError("AreaSw", "The sum of Forest(%), Agriculture(%), Infrastructure(%), Source Water(%) and Wetlands(%) cannot exceed 100.");
                ModelState.AddModelError("AreaWet", "The sum of Forest(%), Agriculture(%), Infrastructure(%), Source Water(%) and Wetlands(%) cannot exceed 100.");
            }

            if (ModelState.IsValid)
            {
                db.Entry(tblswatbackgroundinfo).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatbackgroundinfo);
                return RedirectToAction("Index");
            }
            ViewBag.EcoregionID = new SelectList(db.lkpBiomes, "ID", "Description", tblswatbackgroundinfo.EcoregionID);
            ViewBag.ClimateID = new SelectList(db.lkpClimateClassifications, "ID", "CCType", tblswatbackgroundinfo.ClimateID);
            ViewBag.SoilID = new SelectList(db.lkpSoils, "ID", "Name", tblswatbackgroundinfo.SoilID);
            ViewBag.AreaProtID = new SelectList(db.lkpSWATareaProtLUs, "id", "Description", tblswatbackgroundinfo.AreaProtID);
            ViewBag.AridityID = new SelectList(db.lkpSWATmapAridities, "id", "Description", tblswatbackgroundinfo.AridityID);
            ViewBag.UrbanDistanceID = new SelectList(db.lkpSWATurbanDistanceLUs, "id", "Description", tblswatbackgroundinfo.UrbanDistanceID);
            ViewBag.WatershedID = new SelectList(db.lkpSWATWatershedsLUs, "ID", "Description", tblswatbackgroundinfo.WatershedID);
            //ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID", tblswatbackgroundinfo.SurveyID);
            ViewBag.AreaBmID = new SelectList(db.lkpSWATareaBMLUs, "id", "Description", tblswatbackgroundinfo.AreaBmID);
            ViewBag.isEconPris = new SelectList(db.lkpSWATeconPrisLUs, "id", "Description", tblswatbackgroundinfo.isEconPris);
            ViewBag.PriorQuality = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorQuality);
            ViewBag.PriorQuan = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorQuan);
            ViewBag.PriorSeasonal = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorSeasonal);
            ViewBag.PriorPolitics = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorPolitics);
            ViewBag.PriorHealth = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorHealth);
            ViewBag.PriorFinances = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorFinances);
            ViewBag.PriorAccessible = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorAccessible);
            ViewBag.PriorEquity = new SelectList(db.lkpSWATpriorLUs, "id", "Description", tblswatbackgroundinfo.PriorEquity);
            ViewBag.isEconAg = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatbackgroundinfo.isEconAg);
            ViewBag.isEconLs = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatbackgroundinfo.isEconLs);
            ViewBag.isEconDev = new SelectList(db.lkpSWATYesNoLUs, "id", "Description", tblswatbackgroundinfo.isEconDev);
            ViewBag.SurveyID = tblswatbackgroundinfo.SurveyID;
            return View(tblswatbackgroundinfo);
        }

        // GET: /Background/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATBackgroundinfo tblswatbackgroundinfo = db.tblSWATBackgroundinfoes.Find(id);
            if (tblswatbackgroundinfo == null)
            {
                return HttpNotFound();
            }
            return View(tblswatbackgroundinfo);
        }

        // POST: /Background/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATBackgroundinfo tblswatbackgroundinfo = db.tblSWATBackgroundinfoes.Find(id);
            db.tblSWATBackgroundinfoes.Remove(tblswatbackgroundinfo);
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
