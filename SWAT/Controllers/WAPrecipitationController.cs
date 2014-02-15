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
    public class WAPrecipitationController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /WAPrecipitation/
        public ActionResult Index()
        {
            return View(db.tblSWATWAPrecipitations.ToList());
        }

        // GET: /WAPrecipitation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAPrecipitation tblswatwaprecipitation = db.tblSWATWAPrecipitations.Find(id);
            if (tblswatwaprecipitation == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwaprecipitation);
        }

        // GET: /WAPrecipitation/Create
        public ActionResult Create(int? SurveyID)
        {
            if (SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        private void updateScores(tblSWATWAPrecipitation tblswatwaprecipitation)
        {
            double? [] precips = {tblswatwaprecipitation.January, tblswatwaprecipitation.February, tblswatwaprecipitation.March,
                             tblswatwaprecipitation.April, tblswatwaprecipitation.May, tblswatwaprecipitation.June,
                             tblswatwaprecipitation.July, tblswatwaprecipitation.August, tblswatwaprecipitation.September,
                             tblswatwaprecipitation.October, tblswatwaprecipitation.November, tblswatwaprecipitation.December};
            double? precipTotal = null;
            foreach (double? precip in precips)
            {
                if (precip != null)
                {
                    precipTotal = precipTotal.GetValueOrDefault(0) + precip;
                }
            }
            db.tblSWATScores.Single(e => e.SurveyID == tblswatwaprecipitation.SurveyID && e.VarName == "precipTotal").Value = precipTotal;
            if (precipTotal != null)
            {
                int? precipScoreIntorder = null;
                foreach (var item in db.lkpSWATprecipLUs.OrderByDescending(e => e.Description))
                {
                    if (Double.Parse(item.Description) <= precipTotal)
                    {
                        precipScoreIntorder = item.intorder;
                        break;
                    }
                }
                double? precipScore = Double.Parse(db.lkpSWATscores_precip.Single(e => e.intorder == precipScoreIntorder).Description);
                var tblswatscore = db.tblSWATScores.Single(e => e.SurveyID == tblswatwaprecipitation.SurveyID && e.VarName == "precipSCORE");
                tblswatscore.Value = precipScore;
            }
            else
            {
                db.tblSWATScores.Single(e => e.SurveyID == tblswatwaprecipitation.SurveyID && e.VarName == "precipSCORE").Value = null;
            }
            db.SaveChanges();
        }

        // POST: /WAPrecipitation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SurveyID,January,February,March,April,May,June,July,August,September,October,November,December")] tblSWATWAPrecipitation tblswatwaprecipitation)
        {
            if (ModelState.IsValid)
            {
                var waprecipIDs = db.tblSWATWAPrecipitations.Where(e => e.SurveyID == tblswatwaprecipitation.SurveyID).Select(e => e.ID);
                if (waprecipIDs.Any())
                {
                    var waprecipID = waprecipIDs.First();
                    tblswatwaprecipitation.ID = waprecipID;
                    db.Entry(tblswatwaprecipitation).State = EntityState.Modified;
                    db.SaveChanges();
                    updateScores(tblswatwaprecipitation);
                    return RedirectToAction("Create", "WAMonthlyQuantity", new { SurveyID = tblswatwaprecipitation.SurveyID });
                }

                db.tblSWATWAPrecipitations.Add(tblswatwaprecipitation);
                db.SaveChanges();
                updateScores(tblswatwaprecipitation);
                return RedirectToAction("Create", "WAMonthlyQuantity", new { SurveyID = tblswatwaprecipitation.SurveyID });
            }

            return View(tblswatwaprecipitation);
        }

        // GET: /WAPrecipitation/Edit/5
        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAPrecipitation tblswatwaprecipitation = db.tblSWATWAPrecipitations.Find(id);
            if (tblswatwaprecipitation == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwaprecipitation);
        }

        // POST: /WAPrecipitation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SurveyID,January,February,March,April,May,June,July,August,September,October,November,December")] tblSWATWAPrecipitation tblswatwaprecipitation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatwaprecipitation).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatwaprecipitation);

                // If there is not any WAMonthlyQuantity with the current survey (SurveyID) then create one and redirecto to its edit link.
                var tblswatwamonthlyquantity = db.tblSWATWAMonthlyQuantities.Where(e => e.SurveyID == tblswatwaprecipitation.SurveyID);
                if (!tblswatwamonthlyquantity.Any())
                {
                    tblSWATWAMonthlyQuantity wamq = new tblSWATWAMonthlyQuantity();
                    wamq.SurveyID = tblswatwaprecipitation.SurveyID;
                    db.tblSWATWAMonthlyQuantities.Add(wamq);
                    db.SaveChanges();
                    int newWamqId = wamq.ID;
                    return RedirectToAction("Edit", "WAMonthlyQuantity", new { id = newWamqId, SurveyID = wamq.SurveyID });
                }
                else
                {
                    return RedirectToAction("Edit", "WAMonthlyQuantity", new { id = tblswatwamonthlyquantity.Single(e => e.SurveyID == tblswatwaprecipitation.SurveyID).ID, SurveyID = tblswatwaprecipitation.SurveyID });
                }

                //return RedirectToAction("Index");
            }
            return View(tblswatwaprecipitation);
        }

        // GET: /WAPrecipitation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAPrecipitation tblswatwaprecipitation = db.tblSWATWAPrecipitations.Find(id);
            if (tblswatwaprecipitation == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwaprecipitation);
        }

        // POST: /WAPrecipitation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATWAPrecipitation tblswatwaprecipitation = db.tblSWATWAPrecipitations.Find(id);
            db.tblSWATWAPrecipitations.Remove(tblswatwaprecipitation);
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
