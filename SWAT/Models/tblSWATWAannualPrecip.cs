//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SWAT.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblSWATWAannualPrecip
    {
        public int ID { get; set; }
        public int SurveyID { get; set; }
        public Nullable<double> precipVar { get; set; }
        public Nullable<int> precipVarALT { get; set; }
    
        public virtual lkpSWATprecipVarAltLU lkpSWATprecipVarAltLU { get; set; }
        public virtual tblSWATSurvey tblSWATSurvey { get; set; }
    }
}
