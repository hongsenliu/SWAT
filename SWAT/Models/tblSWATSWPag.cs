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
    
    public partial class tblSWATSWPag
    {
        public int ID { get; set; }
        public int SurveyID { get; set; }
        public bool agType1 { get; set; }
        public bool agType2 { get; set; }
        public Nullable<int> fertilizer { get; set; }
        public Nullable<int> pesticide { get; set; }
        public Nullable<int> erosion { get; set; }
        public Nullable<int> bestManagement { get; set; }
        public Nullable<int> cropLoss { get; set; }
    
        public virtual lkpSWAT5rankLU lkpSWAT5rankLU { get; set; }
        public virtual lkpSWAT5rankLU lkpSWAT5rankLU1 { get; set; }
        public virtual lkpSWATbestManagementLU lkpSWATbestManagementLU { get; set; }
        public virtual lkpSWATcropLossLU lkpSWATcropLossLU { get; set; }
        public virtual lkpSWATerosionLU lkpSWATerosionLU { get; set; }
        public virtual tblSWATSurvey tblSWATSurvey { get; set; }
    }
}