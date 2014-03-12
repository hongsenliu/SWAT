using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Attributes;
using Point = DotNet.Highcharts.Options.Point;
using SWAT.Models;
using SWAT.ViewModels;

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

        // Return subcomponent score by giving a subcomponentID and surveyID
        private double? getSubcomponentScore(int SurveyID, int SubcomponentID)
        {
            double? subcomScore = null;
           
            var scoreIDs = db.lkpSWATindicatorSubComponentLUs.Where(e => e.SubComponentID == SubcomponentID).Select(e => e.ScoreVarID);
            int scoreCounter = 0;
            foreach (int scoreVarID in scoreIDs)
            {
                double? score = db.tblSWATScores.Single(e => e.SurveyID == SurveyID && e.VariableID == scoreVarID).Value;
                if (score != null)
                {
                    subcomScore = subcomScore.GetValueOrDefault(0) + (double)score * 100;
                    scoreCounter++;
                }
            }
            if (scoreCounter > 0)
            {
                return subcomScore / scoreCounter;
            }
            return subcomScore;
        }

        // Return component score by giving a ComponentID and SurveyID
        private double? getComponentScore(int SurveyID, int ComponentID)
        {
            double? comScore = null;

            var subComIDs = db.lkpSWATsubComponentLUs.Where(e => e.ComponentID == ComponentID).Select(e => e.ID);
            int scoreCounter = 0;

            foreach (int subComID in subComIDs)
            {
                double? score = getSubcomponentScore(SurveyID, subComID);
                if (score != null)
                {
                    comScore = comScore.GetValueOrDefault(0) + score;
                    scoreCounter++;
                }
            }
            if (scoreCounter > 0)
            {
                return comScore / scoreCounter;
            }
            return comScore;

        }

        // Return Indicator group score by giving a SurveyID and GroupID
        private double? getGroupScore(int SurveyID, int GroupID)
        {
            double? groupScore = null;
            var comIDs = db.lkpSWATcomponentLUs.Where(e => e.IndicatorGroupID == GroupID).Select(e => e.ID);
            int scoreCounter = 0;

            foreach (int comID in comIDs)
            {
                double? score = getComponentScore(SurveyID, comID);
                if (score != null)
                {
                    groupScore = groupScore.GetValueOrDefault(0) + score;
                    scoreCounter++;
                }
            }
            if (scoreCounter > 0)
            {
                return groupScore / scoreCounter;
            }
            return groupScore;  
        }
        
        // Return a chart Point by giving surveyID and indicatorgroupID
        private Point getGroupScorePoint(int SurveyID, int IndicatorGroupID)
        {
            string[] colors = { "#006699", "#FFCC66", "#A5DF00" };
            double? score = getGroupScore(SurveyID, IndicatorGroupID);
            
            Point column = new Point { Y = score.GetValueOrDefault(0), Color = ColorTranslator.FromHtml(colors[IndicatorGroupID - 1]) };
            return column;
        }

        // Return a chart Point by giving surveyID and componentID
        private Point getComponentScorePoint(int SurveyID, int ComponentID)
        {
            string[] colors = { "#006699", "#FFCC66", "#A5DF00" };
            double? score = getComponentScore(SurveyID, ComponentID);
            int colorIndex = (int)db.lkpSWATcomponentLUs.Find(ComponentID).IndicatorGroupID - 1;
            Point column = new Point { Y = score.GetValueOrDefault(0), Color = ColorTranslator.FromHtml(colors[colorIndex]) };
            return column;
        }

        public ActionResult _BarColumn(int? id)
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
            

            Data data = new Data(new[]
                {
                    getGroupScorePoint((int)id, 1),
                    getGroupScorePoint((int)id, 2),
                    getGroupScorePoint((int)id, 3)
                });
            YAxisPlotBands[] ygradient = new YAxisPlotBands[100];
            const int step = 5;
            string red = "FF";
            string green = "00";
            string blue = "00";

            for (int i = 0; i < 50; i++)
            {
                int decVal = i * step;
                green = decVal.ToString("X");
                if (decVal < 16)
                {
                    green = "0" + green;
                }

                string colorCode1 = "#" + red + green + blue;
                string colorCode2 = "#" + green + red + blue;
                ygradient[i] = new YAxisPlotBands { From = i, To = i + 1, Color = ColorTranslator.FromHtml(colorCode1) };
                ygradient[99 - i] = new YAxisPlotBands { From = 99 - i, To = 100 - i, Color = ColorTranslator.FromHtml(colorCode2) };
            }
            
            Highcharts barColumn = new Highcharts("overallchart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column,
                                       
                })
                .SetTitle(new Title { Text = "Overall Result - " + tblswatsurvey.tblSWATLocation.name })
                .SetXAxis(new XAxis { Categories = new[] { "Water Resources", "Community Capacity", "Governance" } })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Max = 100,
                    Title = new YAxisTitle { Text = "Index" },
                    PlotBands = ygradient,
                    GridLineColor = ColorTranslator.FromHtml("#999999"),
                    GridLineWidth = 1
                })
                .SetLegend(new Legend { Enabled = false })
                .SetCredits(new Credits { Enabled = false })
                .SetTooltip(new Tooltip { Formatter = @"function() { return ''+ this.y; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        PointPadding = 0,
                        BorderWidth = 1,
                        BorderColor = ColorTranslator.FromHtml("#555555")
                    }
                })
                .SetSeries(
                        new Series { Name = tblswatsurvey.tblSWATLocation.name, Data = data  }
                    );

            // Details result chart
            List<Point> comPointList = new List<Point> {};
            
            foreach (var item in db.lkpSWATcomponentLUs)
            {
                comPointList.Add(getComponentScorePoint((int)id, item.ID));
            }
            Data detailsData = new Data(comPointList.ToArray());
            
            Highcharts detailsBarColumn = new Highcharts("detailschart")
                .InitChart(new Chart { DefaultSeriesType = ChartTypes.Column})
                .SetTitle(new Title { Text = "Details Result - " + tblswatsurvey.tblSWATLocation.name })
                .SetXAxis(new XAxis
                {
                    Categories = new[] { "Quantity", "Variability", "Resiliency", "Knowledge", "Financial", "Social", "Equity", "Community", "External", "Water" },
                    Labels = new XAxisLabels
                    {
                        Rotation = -90,
                        Align = HorizontalAligns.Right
                    }
                })
                .SetYAxis(new YAxis
                {
                    Min = 0,
                    Max = 100,
                    Title = new YAxisTitle { Text = "Index" },
                    PlotBands = ygradient,
                    GridLineColor = ColorTranslator.FromHtml("#999999"),
                    GridLineWidth = 1
                })
                .SetLegend(new Legend { Enabled = false })
                .SetCredits(new Credits { Enabled = false })
                .SetTooltip(new Tooltip { Formatter = @"function() { return ''+ this.y; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Column = new PlotOptionsColumn
                    {
                        PointPadding = 0,
                        BorderWidth = 1,
                        BorderColor = ColorTranslator.FromHtml("#555555")
                    }
                })
                .SetSeries(
                        new Series { Name = tblswatsurvey.tblSWATLocation.name, Data = detailsData }
                    );

            ChartsModel barCharts = new ChartsModel();
            barCharts.DetailsChart = detailsBarColumn;
            barCharts.OverallChart = barColumn;
            return PartialView(barCharts);
        }

        

        // GET: /Survey/Report/5
        public ActionResult Report(int? id)
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

        public ActionResult _PieChart(int? id)
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
            // TODO PieChart
            int sec9scores = db.tblSWATScores.Include(t => t.lkpSWATScoreVarsLU).Where(e => e.lkpSWATScoreVarsLU.SectionID == 9 && e.SurveyID == id).Count();
            int sec9ans = db.tblSWATScores.Include(t => t.lkpSWATScoreVarsLU).Where(e => e.lkpSWATScoreVarsLU.SectionID == 9 && e.SurveyID == id && e.Value != null).Count();
            
            double a = (double)sec9ans / sec9scores * 100;
            double b = 100 - a;
            Highcharts chart = new Highcharts("chart")
                .InitChart(new Chart { PlotShadow = false })
                .SetTitle(new Title { Text = string.Empty})
                .SetCredits(new Credits { Enabled = false })
                .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }" })
                .SetPlotOptions(new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        AllowPointSelect = true,
                        Cursor = Cursors.Pointer,
                        DataLabels = new PlotOptionsPieDataLabels
                        {
                            Color = ColorTranslator.FromHtml("#000000"),
                            ConnectorColor = ColorTranslator.FromHtml("#000000"),
                            Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ this.percentage +' %'; }"
                        }
                    }
                })
                .SetSeries(new Series
                {
                    Type = ChartTypes.Pie,
                    Name = "Background Information Progress",
                    Data = new Data(new object[]
                            {
                                new object[] { "Answered", a },
                                new Point
                                    {
                                        Name = "No Answer",
                                        Y = b,
                                        Sliced = true,
                                        Selected = true, 
                                        Color = ColorTranslator.FromHtml("#FFFF00")
                                    }
                            })
                });
            ChartsModel piecharts = new ChartsModel();
            piecharts.BackgroundChart = chart;
            return PartialView(piecharts);
        }

        // GET: /Survey/WaterPoints/5
        public ActionResult WaterPoints(int? id)
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
                var scorevars = db.lkpSWATScoreVarsLUs.ToList();
                foreach (var scorevar in scorevars)
                {
                    tblSWATScore tblswatscore = new tblSWATScore();
                    tblswatscore.SurveyID = newSurveyID;
                    tblswatscore.VariableID = scorevar.ID;

                    // For reference copy the score name from lkpSWATScoreVarsLU table to tblSWATScore table varname field
                    tblswatscore.VarName = scorevar.VarName;

                    if (ModelState.IsValid)
                    {
                        db.tblSWATScores.Add(tblswatscore);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Create", "Background", new { SurveyID = newSurveyID});
            }

            //ViewBag.LocationID = new SelectList(db.tblSWATLocations, "ID", "name");
            //ViewBag.UserID = new SelectList(db.Userids.Where(user => user.type == 0), "Userid1", "Username");
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

            //ViewBag.LocationID = new SelectList(db.tblSWATLocations, "ID", "name", tblswatsurvey.LocationID);
            //ViewBag.UserID = new SelectList(db.Userids, "Userid1", "Username", tblswatsurvey.UserID);
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
            //ViewBag.LocationID = new SelectList(db.tblSWATLocations, "ID", "name", tblswatsurvey.LocationID);
            //ViewBag.UserID = new SelectList(db.Userids, "Userid1", "Username", tblswatsurvey.UserID);
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
            //ViewBag.LocationID = new SelectList(db.tblSWATLocations, "ID", "name", tblswatsurvey.LocationID);
           // ViewBag.UserID = new SelectList(db.Userids, "Userid1", "Username", tblswatsurvey.UserID);
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

        private void DeleteRelatedRecords(int id)
        {
            var waterpts = db.tblSWATWPoverviews.Where(e => e.SurveyID == id);
            foreach (tblSWATWPoverview w in waterpts)
            {
                var wpCtrl = new WaterPointController();
                wpCtrl.DeleteRelatedForms(w.ID);
                db.tblSWATWPoverviews.Remove(w);
            }

            var centrals = db.tblSWATSFcentrals.Where(e => e.SurveyID == id);
            foreach (tblSWATSFcentral c in centrals)
            {
                db.tblSWATSFcentrals.Remove(c);
            }

            var septics = db.tblSWATSFseptics.Where(e => e.SurveyID == id);
            foreach (tblSWATSFseptic s in septics)
            {
                db.tblSWATSFseptics.Remove(s);
            }

            var lats = db.tblSWATSFlats.Where(e => e.SurveyID == id);
            foreach (tblSWATSFlat l in lats)
            {
                db.tblSWATSFlats.Remove(l);
            }

            var tblswatsfpoint = db.tblSWATSFpoints.Where(e => e.SurveyID == id);
            foreach (tblSWATSFpoint item in tblswatsfpoint)
            {
                db.tblSWATSFpoints.Remove(item);
            }

            var tblswatsfod = db.tblSWATSFods.Where(e => e.SurveyID == id);
            foreach (tblSWATSFod item in tblswatsfod)
            {
                db.tblSWATSFods.Remove(item);
            }

            var tblswatsfsanitation = db.tblSWATSFsanitations.Where(e => e.SurveyID == id);
            foreach (tblSWATSFsanitation item in tblswatsfsanitation)
            {
                db.tblSWATSFsanitations.Remove(item);
            }

            var tblswathppkhp = db.tblSWATHPPkhps.Where(e => e.SurveyID == id);
            foreach (tblSWATHPPkhp item in tblswathppkhp)
            {
                db.tblSWATHPPkhps.Remove(item);
            }

            var tblswathppcom = db.tblSWATHPPcoms.Where(e => e.SurveyID == id);
            foreach (tblSWATHPPcom item in tblswathppcom)
            {
                db.tblSWATHPPcoms.Remove(item);
            }

            var tblswatswpdev = db.tblSWATSWPdevs.Where(e => e.SurveyID == id);
            foreach (tblSWATSWPdev item in tblswatswpdev)
            {
                db.tblSWATSWPdevs.Remove(item);
            }

            var tblswatswpag = db.tblSWATSWPags.Where(e => e.SurveyID == id);
            foreach (tblSWATSWPag item in tblswatswpag)
            {
                db.tblSWATSWPags.Remove(item);
            }

            var tblswatswpls = db.tblSWATSWPls.Where(e => e.SurveyID == id);
            foreach (tblSWATSWPl item in tblswatswpls)
            {
                db.tblSWATSWPls.Remove(item);
            }

            var tblswatccwatermanagement = db.tblSWATCCwaterManagements.Where(e => e.SurveyID == id);
            foreach (tblSWATCCwaterManagement item in tblswatccwatermanagement)
            {
                db.tblSWATCCwaterManagements.Remove(item);
            }

            var tblswatccexternal = db.tblSWATCCexternalSupports.Where(e => e.SurveyID == id);
            foreach (tblSWATCCexternalSupport item in tblswatccexternal)
            {
                db.tblSWATCCexternalSupports.Remove(item);
            }

            var tblswatcccom = db.tblSWATCCcoms.Where(e => e.SurveyID == id);
            foreach (tblSWATCCcom item in tblswatcccom)
            {
                db.tblSWATCCcoms.Remove(item);
            }

            var tblswatccsocial = db.tblSWATCCsocials.Where(e => e.SurveyID == id);
            foreach (tblSWATCCsocial item in tblswatccsocial)
            {
                db.tblSWATCCsocials.Remove(item);
            }

            var tblswatccgender = db.tblSWATCCgenders.Where(e => e.SurveyID == id);
            foreach (tblSWATCCgender item in tblswatccgender)
            {
                db.tblSWATCCgenders.Remove(item);
            }

            var tblswatccfinancial = db.tblSWATCCfinancials.Where(e => e.SurveyID == id);
            foreach(tblSWATCCfinancial item in tblswatccfinancial)
            {
               db.tblSWATCCfinancials.Remove(item);
            }

            var tblswatccindig = db.tblSWATCCindigs.Where(e => e.SurveyID == id);
            foreach (tblSWATCCindig item in tblswatccindig)
            {
                db.tblSWATCCindigs.Remove(item);
            }

            var tblswatccschool = db.tblSWATCCschools.Where(e => e.SurveyID == id);
            foreach (tblSWATCCschool item in tblswatccschool)
            {
                db.tblSWATCCschools.Remove(item);
            }

            var tblswatcctrain = db.tblSWATCCtrains.Where(e => e.SurveyID == id);
            foreach (tblSWATCCtrain item in tblswatcctrain)
            {
                db.tblSWATCCtrains.Remove(item);
            }

            var tblswatcceduc = db.tblSWATCCedus.Where(e => e.SurveyID == id);
            foreach (tblSWATCCedu item in tblswatcceduc)
            {
                db.tblSWATCCedus.Remove(item);
            }

            var tblswatwagroundwater = db.tblSWATWAgroundWaters.Where(e => e.SurveyID == id);
            foreach (tblSWATWAgroundWater item in tblswatwagroundwater)
            {
                db.tblSWATWAgroundWaters.Remove(item);
            }

            var tblswatwasurfacewater = db.tblSWATWAsurfaceWaters.Where(e => e.SurveyID == id);
            foreach (tblSWATWAsurfaceWater item in tblswatwasurfacewater)
            {
                db.tblSWATWAsurfaceWaters.Remove(item);
            }

            var tblswatwariskprep = db.tblSWATWAriskPreps.Where(e => e.SurveyID == id);
            foreach (tblSWATWAriskPrep item in tblswatwariskprep)
            {
                db.tblSWATWAriskPreps.Remove(item);
            }

            var tblswatwaextremeevent = db.tblSWATWAextremeEvents.Where(e => e.SurveyID == id);
            foreach (tblSWATWAextremeEvent item in tblswatwaextremeevent)
            {
                db.tblSWATWAextremeEvents.Remove(item);
            }

            var tblswatwaclimatechange = db.tblSWATWAclimateChanges.Where(e => e.SurveyID == id);
            foreach (tblSWATWAclimateChange item in tblswatwaclimatechange)
            {
                db.tblSWATWAclimateChanges.Remove(item);
            }

            var tblswatwaannualprecip = db.tblSWATWAannualPrecips.Where(e => e.SurveyID == id);
            foreach (tblSWATWAannualPrecip item in tblswatwaannualprecip)
            {
                db.tblSWATWAannualPrecips.Remove(item);
            }

            var tblswatwamonthlyquantity = db.tblSWATWAMonthlyQuantities.Where(e => e.SurveyID == id);
            foreach (tblSWATWAMonthlyQuantity item in tblswatwamonthlyquantity)
            {
                db.tblSWATWAMonthlyQuantities.Remove(item);
            }

            var tblswatbackgroundinfos = db.tblSWATBackgroundinfoes.Where(e => e.SurveyID == id);
            foreach(tblSWATBackgroundinfo item in tblswatbackgroundinfos)
            {
                db.tblSWATBackgroundinfoes.Remove(item);
            }

            var tblswatwaprecipitations = db.tblSWATWAPrecipitations.Where(e => e.SurveyID == id);
            foreach(tblSWATWAPrecipitation item in tblswatwaprecipitations)
            {
                db.tblSWATWAPrecipitations.Remove(item);
            }

            var tblswatscores = db.tblSWATScores.Where(e => e.SurveyID == id);
            foreach (tblSWATScore item in tblswatscores)
            {
                db.tblSWATScores.Remove(item);
            }

            db.SaveChanges();
        }
        // POST: /Survey/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            
            
            tblSWATSurvey tblswatsurvey = db.tblSWATSurveys.Find(id);
            db.tblSWATSurveys.Remove(tblswatsurvey);
            DeleteRelatedRecords(id);
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
