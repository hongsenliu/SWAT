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
    public class CCExternalController : Controller
    {
        private SWATEntities db = new SWATEntities();

        //
        // GET: /CCExternal/

        public ActionResult Index()
        {
            var tblswatccexternalsupports = db.tblSWATCCexternalSupports.Include(t => t.lkpSWATextVisitLU).Include(t => t.lkpSWATextVisitLU1).Include(t => t.lkpSWATfundAppLU).Include(t => t.lkpSWATfundAppSuccessLU).Include(t => t.lkpSWATgovRightsLU).Include(t => t.lkpSWATgovWatAnalLU).Include(t => t.lkpSWATgovWatPolLU).Include(t => t.tblSWATSurvey);
            return View(tblswatccexternalsupports.ToList());
        }

        //
        // GET: /CCExternal/Details/5

        public ActionResult Details(int id = 0)
        {
            tblSWATCCexternalSupport tblswatccexternalsupport = db.tblSWATCCexternalSupports.Find(id);
            if (tblswatccexternalsupport == null)
            {
                return HttpNotFound();
            }
            return View(tblswatccexternalsupport);
        }

        //
        // GET: /CCExternal/Create

        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.extVisitTech = new SelectList(db.lkpSWATextVisitLUs, "id", "Description");
            ViewBag.extVisitAdmin = new SelectList(db.lkpSWATextVisitLUs, "id", "Description");
            ViewBag.fundApp = new SelectList(db.lkpSWATfundAppLUs, "id", "Description");
            ViewBag.fundAppSuccess = new SelectList(db.lkpSWATfundAppSuccessLUs, "id", "Description");
            ViewBag.govRights = new SelectList(db.lkpSWATgovRightsLUs, "id", "Description");
            ViewBag.govWatAnal = new SelectList(db.lkpSWATgovWatAnalLUs, "id", "Description");
            ViewBag.govWatPol = new SelectList(db.lkpSWATgovWatPolLUs, "ID", "Description");
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "fundApp_FUNDSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "fundAppSuccessSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "govRightsSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "govWatPolSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "govWatAnalSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "extVisitTechSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "expertAccessSCORE").Description;
            return View();
        }

        private void updateScores(tblSWATCCexternalSupport tblswatccexternalsupport)
        {
            if (tblswatccexternalsupport.fundApp != null)
            {
                int fundAppOrder = (int)db.lkpSWATfundAppLUs.Find(tblswatccexternalsupport.fundApp).intorder;
                double fundAppScore = Double.Parse(db.lkpSWATscores_fundAppFUND.Single(e => e.intorder == fundAppOrder).Description);
                double fundAppLinkScore = Double.Parse(db.lkpSWATscores_fundAppLINK.Single(e => e.intorder == fundAppOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "fundApp_FUNDSCORE").Value = fundAppScore;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "fundApp_LINKSCORE").Value = fundAppLinkScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "fundApp_FUNDSCORE").Value = null;
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "fundApp_LINKSCORE").Value = null;
            }

            if (tblswatccexternalsupport.fundAppSuccess != null)
            {
                int fundAppSuccessOrder = (int)db.lkpSWATfundAppSuccessLUs.Find(tblswatccexternalsupport.fundAppSuccess).intorder;
                double fundAppSuccessScore = Double.Parse(db.lkpSWATscores_fundAppSuccess.Single(e => e.intorder == fundAppSuccessOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "fundAppSuccessSCORE").Value = fundAppSuccessScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "fundAppSuccessSCORE").Value = null;
            }

            if (tblswatccexternalsupport.govRights != null)
            {
                int govRightOrder = (int)db.lkpSWATgovRightsLUs.Find(tblswatccexternalsupport.govRights).intorder;
                if (govRightOrder == 3)
                {
                    db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "govRightsSCORE").Value = null;
                }
                else
                {
                    double govRightScore = Double.Parse(db.lkpSWATscores_govRights.Single(e => e.intorder == govRightOrder).Description);
                    db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "govRightsSCORE").Value = govRightScore;
                }
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "govRightsSCORE").Value = null;
            }

            if (tblswatccexternalsupport.govWatPol != null)
            {
                int govWatPolOrder = (int)db.lkpSWATgovWatPolLUs.Find(tblswatccexternalsupport.govWatPol).intorder;
                double govWatPolScore = Double.Parse(db.lkpSWATscores_govWatPol.Single(e => e.intorder == govWatPolOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "govWatPolSCORE").Value = govWatPolScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "govWatPolSCORE").Value = null;
            }

            if (tblswatccexternalsupport.govWatAnal != null)
            {
                int govWatAnalOrder = (int)db.lkpSWATgovWatAnalLUs.Find(tblswatccexternalsupport.govWatAnal).intorder;
                if (govWatAnalOrder == 4)
                {
                    db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "govWatAnalSCORE").Value = null;
                }
                else
                {
                    double govWatAnalScore = Double.Parse(db.lkpSWATscores_govWatAnal.Single(e => e.intorder == govWatAnalOrder).Description);
                    db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "govWatAnalSCORE").Value = govWatAnalScore;
                }
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "govWatAnalSCORE").Value = null;
            }

            if (tblswatccexternalsupport.extVisitTech != null)
            {
                int extVisitTechOrder = (int)db.lkpSWATextVisitLUs.Find(tblswatccexternalsupport.extVisitTech).intorder;
                double extVTScore = Double.Parse(db.lkpSWATscores_extVisit.Single(e => e.intorder == extVisitTechOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "extVisitTechSCORE").Value = extVTScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "extVisitTechSCORE").Value = null;
            }

            if (tblswatccexternalsupport.extVisitAdmin != null)
            {
                int extVAOrder = (int)db.lkpSWATextVisitLUs.Find(tblswatccexternalsupport.extVisitAdmin).intorder;
                double extVAScore = Double.Parse(db.lkpSWATscores_extVisit.Single(e => e.intorder == extVAOrder).Description);
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "extVisitAdminSCORE").Value = extVAScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "extVisitAdminSCORE").Value = null;
            }

            bool[] expertAccess = {tblswatccexternalsupport.expertAccess1, tblswatccexternalsupport.expertAccess2, tblswatccexternalsupport.expertAccess3,
                                     tblswatccexternalsupport.expertAccess4, tblswatccexternalsupport.expertAccess5};
            double checkedCounter = 0;
            foreach (bool item in expertAccess)
            {
                if (item)
                {
                    checkedCounter++;
                }
            }
            double expertAccessScore = checkedCounter / 5;
            db.tblSWATScores.Single(e => e.SurveyID == tblswatccexternalsupport.SurveyID && e.VarName == "expertAccessSCORE").Value = expertAccessScore;
            db.SaveChanges();
        }
       
        //
        // POST: /CCExternal/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblSWATCCexternalSupport tblswatccexternalsupport)
        {
            if (ModelState.IsValid)
            {
                var externalIDs = db.tblSWATCCexternalSupports.Where(e => e.SurveyID == tblswatccexternalsupport.SurveyID).Select(e => e.ID);
                if (externalIDs.Any())
                {
                    int externalID = externalIDs.First();
                    tblswatccexternalsupport.ID = externalID;
                    db.Entry(tblswatccexternalsupport).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatccexternalsupport);

                    return RedirectToAction("Create", "CCWaterManagement", new { SurveyID = tblswatccexternalsupport.SurveyID});
                }

                db.tblSWATCCexternalSupports.Add(tblswatccexternalsupport);
                db.SaveChanges();
                updateScores(tblswatccexternalsupport);

                return RedirectToAction("Create", "CCWaterManagement", new { SurveyID = tblswatccexternalsupport.SurveyID});
            }

            ViewBag.extVisitTech = new SelectList(db.lkpSWATextVisitLUs, "id", "Description", tblswatccexternalsupport.extVisitTech);
            ViewBag.extVisitAdmin = new SelectList(db.lkpSWATextVisitLUs, "id", "Description", tblswatccexternalsupport.extVisitAdmin);
            ViewBag.fundApp = new SelectList(db.lkpSWATfundAppLUs, "id", "Description", tblswatccexternalsupport.fundApp);
            ViewBag.fundAppSuccess = new SelectList(db.lkpSWATfundAppSuccessLUs, "id", "Description", tblswatccexternalsupport.fundAppSuccess);
            ViewBag.govRights = new SelectList(db.lkpSWATgovRightsLUs, "id", "Description", tblswatccexternalsupport.govRights);
            ViewBag.govWatAnal = new SelectList(db.lkpSWATgovWatAnalLUs, "id", "Description", tblswatccexternalsupport.govWatAnal);
            ViewBag.govWatPol = new SelectList(db.lkpSWATgovWatPolLUs, "ID", "Description", tblswatccexternalsupport.govWatPol);
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "fundApp_FUNDSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "fundAppSuccessSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "govRightsSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "govWatPolSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "govWatAnalSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "extVisitTechSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "expertAccessSCORE").Description;
            return View(tblswatccexternalsupport);
        }

        //
        // GET: /CCExternal/Edit/5

        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATCCexternalSupport tblswatccexternalsupport = db.tblSWATCCexternalSupports.Find(id);
            if (tblswatccexternalsupport == null)
            {
                return HttpNotFound();
            }
            ViewBag.extVisitTech = new SelectList(db.lkpSWATextVisitLUs, "id", "Description", tblswatccexternalsupport.extVisitTech);
            ViewBag.extVisitAdmin = new SelectList(db.lkpSWATextVisitLUs, "id", "Description", tblswatccexternalsupport.extVisitAdmin);
            ViewBag.fundApp = new SelectList(db.lkpSWATfundAppLUs, "id", "Description", tblswatccexternalsupport.fundApp);
            ViewBag.fundAppSuccess = new SelectList(db.lkpSWATfundAppSuccessLUs, "id", "Description", tblswatccexternalsupport.fundAppSuccess);
            ViewBag.govRights = new SelectList(db.lkpSWATgovRightsLUs, "id", "Description", tblswatccexternalsupport.govRights);
            ViewBag.govWatAnal = new SelectList(db.lkpSWATgovWatAnalLUs, "id", "Description", tblswatccexternalsupport.govWatAnal);
            ViewBag.govWatPol = new SelectList(db.lkpSWATgovWatPolLUs, "ID", "Description", tblswatccexternalsupport.govWatPol);
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "fundApp_FUNDSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "fundAppSuccessSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "govRightsSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "govWatPolSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "govWatAnalSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "extVisitTechSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "expertAccessSCORE").Description;
            return View(tblswatccexternalsupport);
        }

        //
        // POST: /CCExternal/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblSWATCCexternalSupport tblswatccexternalsupport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatccexternalsupport).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatccexternalsupport);

                var ccwatermanagement = db.tblSWATCCwaterManagements.First(e => e.SurveyID == tblswatccexternalsupport.SurveyID);
                if (ccwatermanagement == null)
                {
                    tblSWATCCwaterManagement tblswatccwatermanagement = new tblSWATCCwaterManagement();
                    tblswatccwatermanagement.SurveyID = tblswatccexternalsupport.SurveyID;
                    db.tblSWATCCwaterManagements.Add(tblswatccwatermanagement);
                    db.SaveChanges();

                    int newExternalID = tblswatccwatermanagement.ID;

                    return RedirectToAction("Edit", "CCWaterManagement", new { id = newExternalID, SurveyID = tblswatccwatermanagement.SurveyID });
                }

                return RedirectToAction("Edit", "CCWaterManagement", new { id = db.tblSWATCCwaterManagements.First(e => e.SurveyID == tblswatccexternalsupport.SurveyID).ID, SurveyID = tblswatccexternalsupport.SurveyID});
            }
            ViewBag.extVisitTech = new SelectList(db.lkpSWATextVisitLUs, "id", "Description", tblswatccexternalsupport.extVisitTech);
            ViewBag.extVisitAdmin = new SelectList(db.lkpSWATextVisitLUs, "id", "Description", tblswatccexternalsupport.extVisitAdmin);
            ViewBag.fundApp = new SelectList(db.lkpSWATfundAppLUs, "id", "Description", tblswatccexternalsupport.fundApp);
            ViewBag.fundAppSuccess = new SelectList(db.lkpSWATfundAppSuccessLUs, "id", "Description", tblswatccexternalsupport.fundAppSuccess);
            ViewBag.govRights = new SelectList(db.lkpSWATgovRightsLUs, "id", "Description", tblswatccexternalsupport.govRights);
            ViewBag.govWatAnal = new SelectList(db.lkpSWATgovWatAnalLUs, "id", "Description", tblswatccexternalsupport.govWatAnal);
            ViewBag.govWatPol = new SelectList(db.lkpSWATgovWatPolLUs, "ID", "Description", tblswatccexternalsupport.govWatPol);
            ViewBag.Question1 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "fundApp_FUNDSCORE").Description;
            ViewBag.Question2 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "fundAppSuccessSCORE").Description;
            ViewBag.Question3 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "govRightsSCORE").Description;
            ViewBag.Question4 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "govWatPolSCORE").Description;
            ViewBag.Question5 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "govWatAnalSCORE").Description;
            ViewBag.Question6 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "extVisitTechSCORE").Description;
            ViewBag.Question7 = db.lkpSWATScoreVarsLUs.Single(e => e.VarName == "expertAccessSCORE").Description;
            return View(tblswatccexternalsupport);
        }

        //
        // GET: /CCExternal/Delete/5

        public ActionResult Delete(int id = 0)
        {
            tblSWATCCexternalSupport tblswatccexternalsupport = db.tblSWATCCexternalSupports.Find(id);
            if (tblswatccexternalsupport == null)
            {
                return HttpNotFound();
            }
            return View(tblswatccexternalsupport);
        }

        //
        // POST: /CCExternal/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATCCexternalSupport tblswatccexternalsupport = db.tblSWATCCexternalSupports.Find(id);
            db.tblSWATCCexternalSupports.Remove(tblswatccexternalsupport);
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