using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNet.Highcharts;

namespace SWAT.ViewModels
{
    public class ChartsModel
    {
        public Highcharts OverallChart { get; set; }
        public Highcharts DetailsChart { get; set; }
        public Highcharts BackgroundChart { get; set; }
        public Highcharts WAChart { get; set; }
        public Highcharts CCChart { get; set; }

    }
}