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
    
    public partial class tblSWATHPPkhp
    {
        public int ID { get; set; }
        public int SurveyID { get; set; }
        public Nullable<double> knowledgeQual { get; set; }
        public Nullable<double> trainingAccess1 { get; set; }
        public Nullable<double> trainingAccess2 { get; set; }
        public Nullable<double> trainingAccess3 { get; set; }
        public Nullable<double> trainingAccess4 { get; set; }
        public Nullable<double> trainingAccess5 { get; set; }
        public Nullable<int> promotion { get; set; }
        public Nullable<double> handWash { get; set; }
        public Nullable<double> childFaeces1 { get; set; }
        public Nullable<double> childFaeces2 { get; set; }
        public Nullable<double> childFaeces3 { get; set; }
        public Nullable<double> childFaeces4 { get; set; }
        public Nullable<double> childFaeces5 { get; set; }
        public Nullable<double> childFaeces6 { get; set; }
        public Nullable<double> childFaeces7 { get; set; }
    
        public virtual lkpSWAT5rankLU lkpSWAT5rankLU { get; set; }
        public virtual tblSWATSurvey tblSWATSurvey { get; set; }
    }
}
