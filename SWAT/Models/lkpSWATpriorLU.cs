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
    
    public partial class lkpSWATpriorLU
    {
        public lkpSWATpriorLU()
        {
            this.tblSWATBackgroundinfoes = new HashSet<tblSWATBackgroundinfo>();
            this.tblSWATBackgroundinfoes1 = new HashSet<tblSWATBackgroundinfo>();
            this.tblSWATBackgroundinfoes2 = new HashSet<tblSWATBackgroundinfo>();
            this.tblSWATBackgroundinfoes3 = new HashSet<tblSWATBackgroundinfo>();
            this.tblSWATBackgroundinfoes4 = new HashSet<tblSWATBackgroundinfo>();
            this.tblSWATBackgroundinfoes5 = new HashSet<tblSWATBackgroundinfo>();
            this.tblSWATBackgroundinfoes6 = new HashSet<tblSWATBackgroundinfo>();
            this.tblSWATBackgroundinfoes7 = new HashSet<tblSWATBackgroundinfo>();
        }
    
        public int id { get; set; }
        public string Description { get; set; }
        public Nullable<int> intorder { get; set; }
    
        public virtual ICollection<tblSWATBackgroundinfo> tblSWATBackgroundinfoes { get; set; }
        public virtual ICollection<tblSWATBackgroundinfo> tblSWATBackgroundinfoes1 { get; set; }
        public virtual ICollection<tblSWATBackgroundinfo> tblSWATBackgroundinfoes2 { get; set; }
        public virtual ICollection<tblSWATBackgroundinfo> tblSWATBackgroundinfoes3 { get; set; }
        public virtual ICollection<tblSWATBackgroundinfo> tblSWATBackgroundinfoes4 { get; set; }
        public virtual ICollection<tblSWATBackgroundinfo> tblSWATBackgroundinfoes5 { get; set; }
        public virtual ICollection<tblSWATBackgroundinfo> tblSWATBackgroundinfoes6 { get; set; }
        public virtual ICollection<tblSWATBackgroundinfo> tblSWATBackgroundinfoes7 { get; set; }
    }
}
