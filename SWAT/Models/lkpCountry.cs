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
    
    public partial class lkpCountry
    {
        public lkpCountry()
        {
            this.lkpSubnationals = new HashSet<lkpSubnational>();
            this.tblSWATLocations = new HashSet<tblSWATLocation>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public Nullable<int> RegionID { get; set; }
        public string CommonName { get; set; }
    
        public virtual lkpRegion lkpRegion { get; set; }
        public virtual ICollection<lkpSubnational> lkpSubnationals { get; set; }
        public virtual ICollection<tblSWATLocation> tblSWATLocations { get; set; }
    }
}
