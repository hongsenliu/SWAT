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
    public class CCWaterManagementController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /CCWaterManagement/

        public ActionResult Index()
        {
            var tblswatccwatermanagements = db.tblSWATCCwaterManagements.Include(t => t.lkpSWAT5rankLU).Include(t => t.lkpSWAT5rankLU1).Include(t => t.lkpSWAT5rankLU2).Include(t => t.lkpSWAT5rankLU3).Include(t => t.lkpSWAT5rankLU4).Include(t => t.lkpSWATcomSatisfactionLU).Include(t => t.lkpSWATproPoorLU).Include(t => t.lkpSWATwatActionPlanLU).Include(t => t.lkpSWATwatBudgetLU).Include(t => t.lkpSWATwatClassRepLU).Include(t => t.lkpSWATwatComConcernsLU).Include(t => t.lkpSWATwatComLU).Include(t => t.lkpSWATwatTechStaffLU).Include(t => t.lkpSWATwatTechTrainingLU).Include(t => t.tblSWATSurvey);
            return View(tblswatccwatermanagements.ToList());
        }

        //
        // GET: /CCWaterManagement/Details/5

        public ActionResult Details(int id = 0)
        {
            tblSWATCCwaterManagement tblswatccwatermanagement = db.tblSWATCCwaterManagements.Find(id);
            if (tblswatccwatermanagement == null)
            {
                return HttpNotFound();
            }
            return View(tblswatccwatermanagement);
        }

        //
        // GET: /CCWaterManagement/Create

        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.watRecords1 = new SelectList(db.lkpSWAT5rankLU, "id", "Description");
            ViewBag.watRecords2 = new SelectList(db.lkpSWAT5rankLU, "id", "Description");
            ViewBag.watRecords3 = new SelectList(db.lkpSWAT5rankLU, "id", "Description");
            ViewBag.watRecords4 = new SelectList(db.lkpSWAT5rankLU, "id", "Description");
            ViewBag.watRecords5 = new SelectList(db.lkpSWAT5rankLU, "id", "Description");
            ViewBag.comSatisfaction = new SelectList(db.lkpSWATcomSatisfactionLUs, "id", "Description");
            ViewBag.proPoor = new SelectList(db.lkpSWATproPoorLUs, "id", "Description");
            ViewBag.watActionPlan = new SelectList(db.lkpSWATwatActionPlanLUs, "id", "Description");
            ViewBag.watBudget = new SelectList(db.lkpSWATwatBudgetLUs, "id", "Description");
            ViewBag.watClassRep = new SelectList(db.lkpSWATwatClassRepLUs, "id", "Description");
            ViewBag.watComConcerns = new SelectList(db.lkpSWATwatComConcernsLUs, "id", "Description");
            ViewBag.watCom = new SelectList(db.lkpSWATwatComLUs, "id", "Description");
            ViewBag.watTechStaff = new SelectList(db.lkpSWATwatTechStaffLUs, "id", "Description");
            ViewBag.watTechTraining = new SelectList(db.lkpSWATwatTechTrainingLUs, "id", "Description");

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watComSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watActionPlanSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watRecordsSCORETOTAL").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "comSatisfactionSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watClassRepSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watComConcernsSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watTechStaffSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watTechTrainingSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watBudgetSCORE").Description;
            ViewBag.Question10 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watFinPlanSCORE").Description;
            ViewBag.Question11 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "proPoorSCORE").Description;

            return View();
        }

        private void updateScores(tblSWATCCwaterManagement tblswatccwatermanagement)
        {
            if (tblswatccwatermanagement.watCom != null)
            {
                int watComOrder = (int)db.lkpSWATwatComLUs.Find(tblswatccwatermanagement.watCom).intorder;
                double watComScore = Convert.ToDouble(db.lkpSWATscores_watCom.First(e => e.intorder == watComOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watComSCORE").Value = watComScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watComSCORE").Value = null;
            }

            if (tblswatccwatermanagement.watActionPlan != null)
            {
                int watActionOrder = (int)db.lkpSWATwatActionPlanLUs.Find(tblswatccwatermanagement.watActionPlan).intorder;
                double watActionScore = Convert.ToDouble(db.lkpSWATscores_watActionPlan.First(e => e.intorder == watActionOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watActionPlanSCORE").Value = watActionScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watActionPlanSCORE").Value = null;
            }

            int recordCounter = 0;
            double recordScore = 0;

            if (tblswatccwatermanagement.watRecords1 != null)
            {
                int watRecordOrder = (int)db.lkpSWAT5rankLU.Find(tblswatccwatermanagement.watRecords1).intorder;
                double watRecordScore = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysGood.First(e => e.intorder == watRecordOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watRecordsSCORE1").Value = watRecordScore;
                recordCounter++;
                recordScore = recordScore + watRecordScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watRecordsSCORE1").Value = null;
            }

            if (tblswatccwatermanagement.watRecords2 != null)
            {
                int watRecordOrder = (int)db.lkpSWAT5rankLU.Find(tblswatccwatermanagement.watRecords2).intorder;
                double watRecordScore = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysGood.First(e => e.intorder == watRecordOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watRecordsSCORE2").Value = watRecordScore;
                recordCounter++;
                recordScore = recordScore + watRecordScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watRecordsSCORE2").Value = null;
            }

            if (tblswatccwatermanagement.watRecords3 != null)
            {
                int watRecordOrder = (int)db.lkpSWAT5rankLU.Find(tblswatccwatermanagement.watRecords3).intorder;
                double watRecordScore = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysGood.First(e => e.intorder == watRecordOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watRecordsSCORE3").Value = watRecordScore;
                recordCounter++;
                recordScore = recordScore + watRecordScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watRecordsSCORE3").Value = null;
            }

            if (tblswatccwatermanagement.watRecords4 != null)
            {
                int watRecordOrder = (int)db.lkpSWAT5rankLU.Find(tblswatccwatermanagement.watRecords4).intorder;
                double watRecordScore = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysGood.First(e => e.intorder == watRecordOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watRecordsSCORE4").Value = watRecordScore;
                recordCounter++;
                recordScore = recordScore + watRecordScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watRecordsSCORE4").Value = null;
            }

            if (tblswatccwatermanagement.watRecords5 != null)
            {
                int watRecordOrder = (int)db.lkpSWAT5rankLU.Find(tblswatccwatermanagement.watRecords5).intorder;
                double watRecordScore = Convert.ToDouble(db.lkpSWATscores_5rankAlwaysGood.First(e => e.intorder == watRecordOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watRecordsSCORE5").Value = watRecordScore;
                recordCounter++;
                recordScore = recordScore + watRecordScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watRecordsSCORE5").Value = null;
            }

            if (recordCounter > 0)
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watRecordsSCORETOTAL").Value = recordScore / recordCounter;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watRecordsSCORETOTAL").Value = null;
            }

            if (tblswatccwatermanagement.comSatisfaction != null)
            {
                int comOrder = (int)db.lkpSWATcomSatisfactionLUs.Find(tblswatccwatermanagement.comSatisfaction).intorder;
                double comScore = Convert.ToDouble(db.lkpSWATscores_comSatisfaction.First(e => e.intorder == comOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "comSatisfactionSCORE").Value = comScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "comSatisfactionSCORE").Value = null;
            }

            if (tblswatccwatermanagement.watClassRep != null)
            {
                int repOrder = (int)db.lkpSWATwatClassRepLUs.Find(tblswatccwatermanagement.watClassRep).intorder;
                double repScore = Convert.ToDouble(db.lkpSWATscores_watClassRep.First(e => e.intorder == repOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watClassRepSCORE").Value = repScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watClassRepSCORE").Value = null;
            }

            if (tblswatccwatermanagement.watComConcerns != null)
            {
                int concerOrder = (int)db.lkpSWATwatComConcernsLUs.Find(tblswatccwatermanagement.watComConcerns).intorder;
                double concerScore = Convert.ToDouble(db.lkpSWATscores_watComConcerns.First(e => e.intorder == concerOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watComConcernsSCORE").Value = concerScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watComConcernsSCORE").Value = null;
            }

            if (tblswatccwatermanagement.watTechStaff != null)
            {
                int techOrder = (int)db.lkpSWATwatTechStaffLUs.Find(tblswatccwatermanagement.watTechStaff).intorder;
                double techScore = Convert.ToDouble(db.lkpSWATscores_watTechStaff.First(e => e.intorder == techOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watTechStaffSCORE").Value = techScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watTechStaffSCORE").Value = null;
            }

            if (tblswatccwatermanagement.watTechTraining != null)
            {
                int trainOrder = (int)db.lkpSWATwatTechTrainingLUs.Find(tblswatccwatermanagement.watTechTraining).intorder;
                double trainScore = Convert.ToDouble(db.lkpSWATscores_watTechTraining.First(e => e.intorder == trainOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watTechTrainingSCORE").Value = trainScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watTechTrainingSCORE").Value = null;
            }

            if (tblswatccwatermanagement.watBudget != null)
            {
                int budgetOrder = (int)db.lkpSWATwatBudgetLUs.Find(tblswatccwatermanagement.watBudget).intorder;
                double budgetScore = Convert.ToDouble(db.lkpSWATscores_watBudget.First(e => e.intorder == budgetOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watBudgetSCORE").Value = budgetScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watBudgetSCORE").Value = null;
            }

            bool[] watFin = { tblswatccwatermanagement.watFinPlan1, tblswatccwatermanagement.watFinPlan2};
            int watFinCounter = 0;

            foreach (bool item in watFin)
            {
                if (item)
                {
                    watFinCounter++;
                }
            }
            double watFinScore = (double)watFinCounter / 2.0;
            db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "watFinPlanSCORE").Value = watFinScore;

            if (tblswatccwatermanagement.proPoor != null)
            {
                int proOrder = (int)db.lkpSWATproPoorLUs.Find(tblswatccwatermanagement.proPoor).intorder;
                double proScore = Convert.ToDouble(db.lkpSWATscores_proPoor.First(e => e.intorder == proOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "proPoorSCORE").Value = proScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccwatermanagement.SurveyID && e.VarName == "proPoorSCORE").Value = null;
            }

            db.SaveChanges();
            
        }

        //
        // POST: /CCWaterManagement/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATCCwaterManagement tblswatccwatermanagement)
        {
            if (ModelState.IsValid)
            {
                var waterManagementIDs = db.tblSWATCCwaterManagements.Where(e => e.SurveyID == tblswatccwatermanagement.SurveyID).Select(e => e.ID);
                if (waterManagementIDs.Any())
                {
                    int waterManagementID = waterManagementIDs.First();
                    tblswatccwatermanagement.ID = waterManagementID;
                    db.Entry(tblswatccwatermanagement).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatccwatermanagement);

                    return RedirectToAction("Create", "SWPLivestock", new { SurveyID = tblswatccwatermanagement.SurveyID});
                }

                db.tblSWATCCwaterManagements.Add(tblswatccwatermanagement);
                db.SaveChanges();
                updateScores(tblswatccwatermanagement);

                return RedirectToAction("Create", "SWPLivestock", new { SurveyID = tblswatccwatermanagement.SurveyID });
            }

            ViewBag.watRecords1 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords1);
            ViewBag.watRecords2 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords2);
            ViewBag.watRecords3 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords3);
            ViewBag.watRecords4 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords4);
            ViewBag.watRecords5 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords5);
            ViewBag.comSatisfaction = new SelectList(db.lkpSWATcomSatisfactionLUs, "id", "Description", tblswatccwatermanagement.comSatisfaction);
            ViewBag.proPoor = new SelectList(db.lkpSWATproPoorLUs, "id", "Description", tblswatccwatermanagement.proPoor);
            ViewBag.watActionPlan = new SelectList(db.lkpSWATwatActionPlanLUs, "id", "Description", tblswatccwatermanagement.watActionPlan);
            ViewBag.watBudget = new SelectList(db.lkpSWATwatBudgetLUs, "id", "Description", tblswatccwatermanagement.watBudget);
            ViewBag.watClassRep = new SelectList(db.lkpSWATwatClassRepLUs, "id", "Description", tblswatccwatermanagement.watClassRep);
            ViewBag.watComConcerns = new SelectList(db.lkpSWATwatComConcernsLUs, "id", "Description", tblswatccwatermanagement.watComConcerns);
            ViewBag.watCom = new SelectList(db.lkpSWATwatComLUs, "id", "Description", tblswatccwatermanagement.watCom);
            ViewBag.watTechStaff = new SelectList(db.lkpSWATwatTechStaffLUs, "id", "Description", tblswatccwatermanagement.watTechStaff);
            ViewBag.watTechTraining = new SelectList(db.lkpSWATwatTechTrainingLUs, "id", "Description", tblswatccwatermanagement.watTechTraining);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watComSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watActionPlanSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watRecordsSCORETOTAL").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "comSatisfactionSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watClassRepSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watComConcernsSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watTechStaffSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watTechTrainingSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watBudgetSCORE").Description;
            ViewBag.Question10 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watFinPlanSCORE").Description;
            ViewBag.Question11 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "proPoorSCORE").Description;

            return View(tblswatccwatermanagement);
        }

        //
        // GET: /CCWaterManagement/Edit/5

        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATCCwaterManagement tblswatccwatermanagement = db.tblSWATCCwaterManagements.Find(id);
            if (tblswatccwatermanagement == null)
            {
                return HttpNotFound();
            }
            ViewBag.watRecords1 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords1);
            ViewBag.watRecords2 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords2);
            ViewBag.watRecords3 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords3);
            ViewBag.watRecords4 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords4);
            ViewBag.watRecords5 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords5);
            ViewBag.comSatisfaction = new SelectList(db.lkpSWATcomSatisfactionLUs, "id", "Description", tblswatccwatermanagement.comSatisfaction);
            ViewBag.proPoor = new SelectList(db.lkpSWATproPoorLUs, "id", "Description", tblswatccwatermanagement.proPoor);
            ViewBag.watActionPlan = new SelectList(db.lkpSWATwatActionPlanLUs, "id", "Description", tblswatccwatermanagement.watActionPlan);
            ViewBag.watBudget = new SelectList(db.lkpSWATwatBudgetLUs, "id", "Description", tblswatccwatermanagement.watBudget);
            ViewBag.watClassRep = new SelectList(db.lkpSWATwatClassRepLUs, "id", "Description", tblswatccwatermanagement.watClassRep);
            ViewBag.watComConcerns = new SelectList(db.lkpSWATwatComConcernsLUs, "id", "Description", tblswatccwatermanagement.watComConcerns);
            ViewBag.watCom = new SelectList(db.lkpSWATwatComLUs, "id", "Description", tblswatccwatermanagement.watCom);
            ViewBag.watTechStaff = new SelectList(db.lkpSWATwatTechStaffLUs, "id", "Description", tblswatccwatermanagement.watTechStaff);
            ViewBag.watTechTraining = new SelectList(db.lkpSWATwatTechTrainingLUs, "id", "Description", tblswatccwatermanagement.watTechTraining);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watComSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watActionPlanSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watRecordsSCORETOTAL").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "comSatisfactionSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watClassRepSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watComConcernsSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watTechStaffSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watTechTrainingSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watBudgetSCORE").Description;
            ViewBag.Question10 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watFinPlanSCORE").Description;
            ViewBag.Question11 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "proPoorSCORE").Description;

            return View(tblswatccwatermanagement);
        }

        //
        // POST: /CCWaterManagement/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATCCwaterManagement tblswatccwatermanagement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatccwatermanagement).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatccwatermanagement);

                var background = db.tblSWATBackgroundinfoes.First(e => e.SurveyID == tblswatccwatermanagement.SurveyID);
                if (background != null)
                { 
                    // id = 1511 is the "Yes" option in database
                    if (background.isEconLs == 1511)
                    {
                        // TODO redirect to livestock form
                        var swpls = db.tblSWATSWPls.First(e => e.SurveyID == tblswatccwatermanagement.SurveyID);
                        if (swpls == null)
                        {
                            tblSWATSWPl tblswatswpls = new tblSWATSWPl();
                            tblswatswpls.SurveyID = tblswatccwatermanagement.SurveyID;
                            db.tblSWATSWPls.Add(tblswatswpls);
                            db.SaveChanges();

                            int newSWPlsID = tblswatswpls.ID;
                            return RedirectToAction("Edit", "SWPLivestock", new{ id = newSWPlsID, SurveyID = tblswatswpls.SurveyID});
                        }
                        return RedirectToAction("Edit", "SWPLivestock", new { id = db.tblSWATSWPls.First(e => e.SurveyID == tblswatccwatermanagement.SurveyID).ID, SurveyID = tblswatccwatermanagement.SurveyID });
                    }
                    else if (background.isEconAg == 1511)
                    { 
                        // TODO redirect to ag form
                        var swpag = db.tblSWATSWPags.First(e => e.SurveyID == tblswatccwatermanagement.SurveyID);
                        if (swpag == null)
                        {
                            tblSWATSWPag tblswatswpag = new tblSWATSWPag();
                            tblswatswpag.SurveyID = tblswatccwatermanagement.SurveyID;
                            db.tblSWATSWPags.Add(tblswatswpag);
                            db.SaveChanges();

                            int newSWPagID = tblswatswpag.ID;
                            return RedirectToAction("Edit", "SWPAg", new { id = newSWPagID, SurveyID = tblswatswpag.SurveyID});
                        }
                        return RedirectToAction("Edit", "SWPAg", new { id = db.tblSWATSWPags.First(e => e.SurveyID == tblswatccwatermanagement.SurveyID).ID, SurveyID = tblswatccwatermanagement.SurveyID});
                    }
                    else if (background.isEconDev == 1511)
                    { 
                        // TODO redirect to dev form
                        var swpdev = db.tblSWATSWPdevs.First(e => e.SurveyID == tblswatccwatermanagement.SurveyID);
                        if (swpdev == null)
                        {
                            tblSWATSWPdev tblswatswpdev = new tblSWATSWPdev();
                            tblswatswpdev.SurveyID = tblswatccwatermanagement.SurveyID;
                            db.tblSWATSWPdevs.Add(tblswatswpdev);
                            db.SaveChanges();

                            int newSWPdevID = tblswatswpdev.ID;
                            return RedirectToAction("Edit", "SWPDev", new { id = newSWPdevID, SurveyID = tblswatswpdev.SurveyID});
                        }
                        return RedirectToAction("Edit", "SWPDev", new { id = db.tblSWATSWPdevs.First(e => e.SurveyID == tblswatccwatermanagement.SurveyID).ID, SurveyID = tblswatccwatermanagement.SurveyID});
                    }
                }
                // TODO redirect to health form
                var hppcom = db.tblSWATHPPcoms.First(e => e.SurveyID == tblswatccwatermanagement.SurveyID);
                if (hppcom == null)
                {
                    tblSWATHPPcom tblswathppcom = new tblSWATHPPcom();
                    tblswathppcom.SurveyID = tblswatccwatermanagement.SurveyID;
                    db.tblSWATHPPcoms.Add(tblswathppcom);
                    db.SaveChanges();

                    int newHPPcomID = tblswathppcom.ID;
                    return RedirectToAction("Edit", "HPPCom", new { id = newHPPcomID, SurveyID = tblswathppcom.SurveyID });
                }
                return RedirectToAction("Edit", "HPPCom", new { id = db.tblSWATHPPcoms.First(e => e.SurveyID == tblswatccwatermanagement.SurveyID).ID, SurveyID = tblswatccwatermanagement.SurveyID});
            }
            ViewBag.watRecords1 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords1);
            ViewBag.watRecords2 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords2);
            ViewBag.watRecords3 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords3);
            ViewBag.watRecords4 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords4);
            ViewBag.watRecords5 = new SelectList(db.lkpSWAT5rankLU, "id", "Description", tblswatccwatermanagement.watRecords5);
            ViewBag.comSatisfaction = new SelectList(db.lkpSWATcomSatisfactionLUs, "id", "Description", tblswatccwatermanagement.comSatisfaction);
            ViewBag.proPoor = new SelectList(db.lkpSWATproPoorLUs, "id", "Description", tblswatccwatermanagement.proPoor);
            ViewBag.watActionPlan = new SelectList(db.lkpSWATwatActionPlanLUs, "id", "Description", tblswatccwatermanagement.watActionPlan);
            ViewBag.watBudget = new SelectList(db.lkpSWATwatBudgetLUs, "id", "Description", tblswatccwatermanagement.watBudget);
            ViewBag.watClassRep = new SelectList(db.lkpSWATwatClassRepLUs, "id", "Description", tblswatccwatermanagement.watClassRep);
            ViewBag.watComConcerns = new SelectList(db.lkpSWATwatComConcernsLUs, "id", "Description", tblswatccwatermanagement.watComConcerns);
            ViewBag.watCom = new SelectList(db.lkpSWATwatComLUs, "id", "Description", tblswatccwatermanagement.watCom);
            ViewBag.watTechStaff = new SelectList(db.lkpSWATwatTechStaffLUs, "id", "Description", tblswatccwatermanagement.watTechStaff);
            ViewBag.watTechTraining = new SelectList(db.lkpSWATwatTechTrainingLUs, "id", "Description", tblswatccwatermanagement.watTechTraining);

            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watComSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watActionPlanSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watRecordsSCORETOTAL").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "comSatisfactionSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watClassRepSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watComConcernsSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watTechStaffSCORE").Description;
            ViewBag.Question8 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watTechTrainingSCORE").Description;
            ViewBag.Question9 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watBudgetSCORE").Description;
            ViewBag.Question10 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "watFinPlanSCORE").Description;
            ViewBag.Question11 = db.lkpSWATScoreVarsLUs.First(e => e.VarName == "proPoorSCORE").Description;

            return View(tblswatccwatermanagement);
        }

        //
        // GET: /CCWaterManagement/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblSWATCCwaterManagement tblswatccwatermanagement = db.tblSWATCCwaterManagements.Find(id);
            if (tblswatccwatermanagement == null)
            {
                return HttpNotFound();
            }
            return View(tblswatccwatermanagement);
        }

        //
        // POST: /CCWaterManagement/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATCCwaterManagement tblswatccwatermanagement = db.tblSWATCCwaterManagements.Find(id);
            db.tblSWATCCwaterManagements.Remove(tblswatccwatermanagement);
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