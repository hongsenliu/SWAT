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
    public class WAMonthlyQuantityController : Controller
    {
        private SWATEntities db = new SWATEntities();

        // GET: /WAMonthlyQuantity/
        public ActionResult Index()
        {
            var tblswatwamonthlyquantities = db.tblSWATWAMonthlyQuantities.Include(t => t.lkpSWATwaterMonthLU).Include(t => t.lkpSWATwaterMonthLU1).Include(t => t.lkpSWATwaterMonthLU2).Include(t => t.lkpSWATwaterMonthLU3).Include(t => t.lkpSWATwaterMonthLU4).Include(t => t.lkpSWATwaterMonthLU5).Include(t => t.lkpSWATwaterMonthLU6).Include(t => t.lkpSWATwaterMonthLU7).Include(t => t.lkpSWATwaterMonthLU8).Include(t => t.lkpSWATwaterMonthLU9).Include(t => t.lkpSWATwaterMonthLU10).Include(t => t.lkpSWATwaterMonthLU11).Include(t => t.tblSWATSurvey);
            return View(tblswatwamonthlyquantities.ToList());
        }

        // GET: /WAMonthlyQuantity/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAMonthlyQuantity tblswatwamonthlyquantity = db.tblSWATWAMonthlyQuantities.Find(id);
            if (tblswatwamonthlyquantity == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwamonthlyquantity);
        }

        // GET: /WAMonthlyQuantity/Create
        public ActionResult Create()
        {
            ViewBag.August = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description");
            ViewBag.September = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description");
            ViewBag.October = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description");
            ViewBag.November = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description");
            ViewBag.December = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description");
            ViewBag.January = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description");
            ViewBag.February = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description");
            ViewBag.March = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description");
            ViewBag.April = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description");
            ViewBag.May = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description");
            ViewBag.June = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description");
            ViewBag.July = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description");
            ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID");
            return View();
        }

        private void updateScores(tblSWATWAMonthlyQuantity tblswatwamonthlyquantity)
        {
            int wamqTot = 0;
            if (tblswatwamonthlyquantity.January != null)
            {
                if (db.lkpSWATwaterMonthLUs.Find(tblswatwamonthlyquantity.January).intorder < 3)
                {
                    wamqTot++;
                }
            }
            if (tblswatwamonthlyquantity.February != null)
            {
                if (db.lkpSWATwaterMonthLUs.Find(tblswatwamonthlyquantity.February).intorder < 3)
                {
                    wamqTot++;
                }
            }
            if (tblswatwamonthlyquantity.March != null)
            {
                if (db.lkpSWATwaterMonthLUs.Find(tblswatwamonthlyquantity.March).intorder < 3)
                {
                    wamqTot++;
                }
            }
            if (tblswatwamonthlyquantity.April != null)
            {
                if (db.lkpSWATwaterMonthLUs.Find(tblswatwamonthlyquantity.April).intorder < 3)
                {
                    wamqTot++;
                }
            }
            if (tblswatwamonthlyquantity.May != null)
            {
                if (db.lkpSWATwaterMonthLUs.Find(tblswatwamonthlyquantity.May).intorder < 3)
                {
                    wamqTot++;
                }
            }
            if (tblswatwamonthlyquantity.June != null)
            {
                if (db.lkpSWATwaterMonthLUs.Find(tblswatwamonthlyquantity.June).intorder < 3)
                {
                    wamqTot++;
                }
            }
            if (tblswatwamonthlyquantity.July != null)
            {
                if (db.lkpSWATwaterMonthLUs.Find(tblswatwamonthlyquantity.July).intorder < 3)
                {
                    wamqTot++;
                }
            }
            if (tblswatwamonthlyquantity.August != null)
            {
                if (db.lkpSWATwaterMonthLUs.Find(tblswatwamonthlyquantity.August).intorder < 3)
                {
                    wamqTot++;
                }
            }
            if (tblswatwamonthlyquantity.September != null)
            {
                if (db.lkpSWATwaterMonthLUs.Find(tblswatwamonthlyquantity.September).intorder < 3)
                {
                    wamqTot++;
                }
            }
            if (tblswatwamonthlyquantity.October != null)
            {
                if (db.lkpSWATwaterMonthLUs.Find(tblswatwamonthlyquantity.October).intorder < 3)
                {
                    wamqTot++;
                }
            }
            if (tblswatwamonthlyquantity.November != null)
            {
                if (db.lkpSWATwaterMonthLUs.Find(tblswatwamonthlyquantity.November).intorder < 3)
                {
                    wamqTot++;
                }
            }
            if (tblswatwamonthlyquantity.December != null)
            {
                if (db.lkpSWATwaterMonthLUs.Find(tblswatwamonthlyquantity.December).intorder < 3)
                {
                    wamqTot++;
                }
            }

            double waterScore = wamqTot / 12.0;
            db.tblSWATScores.Single(e => e.SurveyID == tblswatwamonthlyquantity.SurveyID && e.VarName == "waterTot").Value = wamqTot;
            db.tblSWATScores.Single(e => e.SurveyID == tblswatwamonthlyquantity.SurveyID && e.VarName == "waterSCORE").Value = waterScore;
            db.SaveChanges();
        }

        // POST: /WAMonthlyQuantity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,SurveyID,January,February,March,April,May,June,July,August,September,October,November,December")] tblSWATWAMonthlyQuantity tblswatwamonthlyquantity)
        {
            var wamqIDs = db.tblSWATWAMonthlyQuantities.Where(e => e.SurveyID == tblswatwamonthlyquantity.SurveyID).Select(e => e.ID);
            if (wamqIDs.Any())
            {
                var wamqID = wamqIDs.First();
                tblswatwamonthlyquantity.ID = wamqID;
                db.Entry(tblswatwamonthlyquantity).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatwamonthlyquantity);
                return RedirectToAction("Index");
                //return RedirectToAction("Create", "WAMonthlyQuantity", new { SurveyID = tblswatwaprecipitation.SurveyID });
            }
            if (ModelState.IsValid)
            {
                db.tblSWATWAMonthlyQuantities.Add(tblswatwamonthlyquantity);
                db.SaveChanges();
                updateScores(tblswatwamonthlyquantity);
                return RedirectToAction("Index");
            }

            ViewBag.August = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.August);
            ViewBag.September = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.September);
            ViewBag.October = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.October);
            ViewBag.November = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.November);
            ViewBag.December = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.December);
            ViewBag.January = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.January);
            ViewBag.February = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.February);
            ViewBag.March = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.March);
            ViewBag.April = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.April);
            ViewBag.May = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.May);
            ViewBag.June = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.June);
            ViewBag.July = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.July);
            ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID", tblswatwamonthlyquantity.SurveyID);
            return View(tblswatwamonthlyquantity);
        }

        // GET: /WAMonthlyQuantity/Edit/5
        public ActionResult Edit(int? id, int? SurveyID)
        {
            if (id == null || SurveyID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAMonthlyQuantity tblswatwamonthlyquantity = db.tblSWATWAMonthlyQuantities.Find(id);
            if (tblswatwamonthlyquantity == null)
            {
                return HttpNotFound();
            }
            ViewBag.August = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.August);
            ViewBag.September = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.September);
            ViewBag.October = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.October);
            ViewBag.November = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.November);
            ViewBag.December = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.December);
            ViewBag.January = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.January);
            ViewBag.February = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.February);
            ViewBag.March = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.March);
            ViewBag.April = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.April);
            ViewBag.May = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.May);
            ViewBag.June = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.June);
            ViewBag.July = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.July);
            ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID", tblswatwamonthlyquantity.SurveyID);
            return View(tblswatwamonthlyquantity);
        }

        // POST: /WAMonthlyQuantity/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,SurveyID,January,February,March,April,May,June,July,August,September,October,November,December")] tblSWATWAMonthlyQuantity tblswatwamonthlyquantity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblswatwamonthlyquantity).State = EntityState.Modified;
                db.SaveChanges();
                updateScores(tblswatwamonthlyquantity);
                return RedirectToAction("Index");
            }
            ViewBag.August = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.August);
            ViewBag.September = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.September);
            ViewBag.October = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.October);
            ViewBag.November = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.November);
            ViewBag.December = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.December);
            ViewBag.January = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.January);
            ViewBag.February = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.February);
            ViewBag.March = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.March);
            ViewBag.April = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.April);
            ViewBag.May = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.May);
            ViewBag.June = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.June);
            ViewBag.July = new SelectList(db.lkpSWATwaterMonthLUs, "id", "Description", tblswatwamonthlyquantity.July);
            ViewBag.SurveyID = new SelectList(db.tblSWATSurveys, "ID", "ID", tblswatwamonthlyquantity.SurveyID);
            return View(tblswatwamonthlyquantity);
        }

        // GET: /WAMonthlyQuantity/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSWATWAMonthlyQuantity tblswatwamonthlyquantity = db.tblSWATWAMonthlyQuantities.Find(id);
            if (tblswatwamonthlyquantity == null)
            {
                return HttpNotFound();
            }
            return View(tblswatwamonthlyquantity);
        }

        // POST: /WAMonthlyQuantity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblSWATWAMonthlyQuantity tblswatwamonthlyquantity = db.tblSWATWAMonthlyQuantities.Find(id);
            db.tblSWATWAMonthlyQuantities.Remove(tblswatwamonthlyquantity);
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
