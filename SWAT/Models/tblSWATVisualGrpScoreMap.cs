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
    
    public partial class tblSWATVisualGrpScoreMap
    {
        public int ID { get; set; }
        public int VisualGrpID { get; set; }
        public long ScoreID { get; set; }
    
        public virtual tblSWATVisualGroup tblSWATVisualGroup { get; set; }
        public virtual tblSWATScore tblSWATScore { get; set; }
    }
}
