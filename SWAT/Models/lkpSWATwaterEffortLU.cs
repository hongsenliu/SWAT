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
    
    public partial class lkpSWATwaterEffortLU
    {
        public lkpSWATwaterEffortLU()
        {
            this.tblSWATWPsupplies = new HashSet<tblSWATWPsupply>();
            this.tblSWATWPsupplies1 = new HashSet<tblSWATWPsupply>();
        }
    
        public int id { get; set; }
        public string Description { get; set; }
        public Nullable<int> intorder { get; set; }
    
        public virtual ICollection<tblSWATWPsupply> tblSWATWPsupplies { get; set; }
        public virtual ICollection<tblSWATWPsupply> tblSWATWPsupplies1 { get; set; }
    }
}
