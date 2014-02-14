﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SWATEntities : DbContext
    {
        public SWATEntities()
            : base("name=SWATEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<lkpBiome> lkpBiomes { get; set; }
        public DbSet<lkpClimateClassification> lkpClimateClassifications { get; set; }
        public DbSet<lkpCountry> lkpCountries { get; set; }
        public DbSet<lkpRegion> lkpRegions { get; set; }
        public DbSet<lkpSoil> lkpSoils { get; set; }
        public DbSet<lkpSubnational> lkpSubnationals { get; set; }
        public DbSet<lkpSWATareaBMLU> lkpSWATareaBMLUs { get; set; }
        public DbSet<lkpSWATareaProtLU> lkpSWATareaProtLUs { get; set; }
        public DbSet<lkpSWATeconPrisLU> lkpSWATeconPrisLUs { get; set; }
        public DbSet<lkpSWATmapAridity> lkpSWATmapAridities { get; set; }
        public DbSet<lkpSWATpriorLU> lkpSWATpriorLUs { get; set; }
        public DbSet<lkpSWATscores_areaBMLU> lkpSWATscores_areaBMLU { get; set; }
        public DbSet<lkpSWATscores_areaProt> lkpSWATscores_areaProt { get; set; }
        public DbSet<lkpSWATscores_Aridity> lkpSWATscores_Aridity { get; set; }
        public DbSet<lkpSWATscores_econPris> lkpSWATscores_econPris { get; set; }
        public DbSet<lkpSWATscores_urbanDistance> lkpSWATscores_urbanDistance { get; set; }
        public DbSet<lkpSWATscores_YesNoLUYesGood> lkpSWATscores_YesNoLUYesGood { get; set; }
        public DbSet<lkpSWATScoreVarsLU> lkpSWATScoreVarsLUs { get; set; }
        public DbSet<lkpSWATSectionLU> lkpSWATSectionLUs { get; set; }
        public DbSet<lkpSWATurbanDistanceLU> lkpSWATurbanDistanceLUs { get; set; }
        public DbSet<lkpSWATWatershedsLU> lkpSWATWatershedsLUs { get; set; }
        public DbSet<lkpSWATYesNoLU> lkpSWATYesNoLUs { get; set; }
        public DbSet<tblSWATBackgroundinfo> tblSWATBackgroundinfoes { get; set; }
        public DbSet<tblSWATLocation> tblSWATLocations { get; set; }
        public DbSet<tblSWATScore> tblSWATScores { get; set; }
        public DbSet<tblSWATSurvey> tblSWATSurveys { get; set; }
        public DbSet<tblSWATVisualGroup> tblSWATVisualGroups { get; set; }
        public DbSet<tblSWATVisualGrpScoreMap> tblSWATVisualGrpScoreMaps { get; set; }
        public DbSet<Userid> Userids { get; set; }
        public DbSet<tblSWATWAPrecipitation> tblSWATWAPrecipitations { get; set; }
        public DbSet<lkpSWATprecipLU> lkpSWATprecipLUs { get; set; }
        public DbSet<lkpSWATscores_precip> lkpSWATscores_precip { get; set; }
        public DbSet<lkpSWATwaterMonthLU> lkpSWATwaterMonthLUs { get; set; }
        public DbSet<tblSWATWAMonthlyQuantity> tblSWATWAMonthlyQuantities { get; set; }
    }
}
