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
    
    public partial class lkpRegion
    {
        public lkpRegion()
        {
            this.lkpCountries = new HashSet<lkpCountry>();
            this.tblSWATLocations = new HashSet<tblSWATLocation>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<lkpCountry> lkpCountries { get; set; }
        public virtual ICollection<tblSWATLocation> tblSWATLocations { get; set; }
    }
}
