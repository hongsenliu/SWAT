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
    
    public partial class lkpSWATYesNoLU
    {
        public lkpSWATYesNoLU()
        {
            this.tblSWATBackgroundinfoes = new HashSet<tblSWATBackgroundinfo>();
            this.tblSWATBackgroundinfoes1 = new HashSet<tblSWATBackgroundinfo>();
            this.tblSWATBackgroundinfoes2 = new HashSet<tblSWATBackgroundinfo>();
        }
    
        public int id { get; set; }
        public string Description { get; set; }
        public Nullable<int> intorder { get; set; }
    
        public virtual ICollection<tblSWATBackgroundinfo> tblSWATBackgroundinfoes { get; set; }
        public virtual ICollection<tblSWATBackgroundinfo> tblSWATBackgroundinfoes1 { get; set; }
        public virtual ICollection<tblSWATBackgroundinfo> tblSWATBackgroundinfoes2 { get; set; }
    }
}
