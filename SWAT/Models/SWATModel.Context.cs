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
        public DbSet<lkpSWAT5rankLU> lkpSWAT5rankLU { get; set; }
        public DbSet<lkpSWATareaBMLU> lkpSWATareaBMLUs { get; set; }
        public DbSet<lkpSWATareaProtLU> lkpSWATareaProtLUs { get; set; }
        public DbSet<lkpSWATcomponentLU> lkpSWATcomponentLUs { get; set; }
        public DbSet<lkpSWATeconPrisLU> lkpSWATeconPrisLUs { get; set; }
        public DbSet<lkpSWATeduGradDiffLU> lkpSWATeduGradDiffLUs { get; set; }
        public DbSet<lkpSWATextremeEventsLU> lkpSWATextremeEventsLUs { get; set; }
        public DbSet<lkpSWATextremePrepLU> lkpSWATextremePrepLUs { get; set; }
        public DbSet<lkpSWATextremeRiskLU> lkpSWATextremeRiskLUs { get; set; }
        public DbSet<lkpSWATgenderLU> lkpSWATgenderLUs { get; set; }
        public DbSet<lkpSWATgRoleWomenLU> lkpSWATgRoleWomenLUs { get; set; }
        public DbSet<lkpSWATgwAvailabilityLU> lkpSWATgwAvailabilityLUs { get; set; }
        public DbSet<lkpSWATgWomenEngagedLU> lkpSWATgWomenEngagedLUs { get; set; }
        public DbSet<lkpSWATindicatorGroupLU> lkpSWATindicatorGroupLUs { get; set; }
        public DbSet<lkpSWATindicatorSubComponentLU> lkpSWATindicatorSubComponentLUs { get; set; }
        public DbSet<lkpSWATindigPopLU> lkpSWATindigPopLUs { get; set; }
        public DbSet<lkpSWATlongtermPopLU> lkpSWATlongtermPopLUs { get; set; }
        public DbSet<lkpSWATmapAridity> lkpSWATmapAridities { get; set; }
        public DbSet<lkpSWATprecipLU> lkpSWATprecipLUs { get; set; }
        public DbSet<lkpSWATprecipVarAltLU> lkpSWATprecipVarAltLUs { get; set; }
        public DbSet<lkpSWATprecipVardivMeanLU> lkpSWATprecipVardivMeanLUs { get; set; }
        public DbSet<lkpSWATpriorLU> lkpSWATpriorLUs { get; set; }
        public DbSet<lkpSWATroscaLU> lkpSWATroscaLUs { get; set; }
        public DbSet<lkpSWATrunoffLU> lkpSWATrunoffLUs { get; set; }
        public DbSet<lkpSWATscores_5rankAlwaysGood> lkpSWATscores_5rankAlwaysGood { get; set; }
        public DbSet<lkpSWATscores_areaBMLU> lkpSWATscores_areaBMLU { get; set; }
        public DbSet<lkpSWATscores_areaProt> lkpSWATscores_areaProt { get; set; }
        public DbSet<lkpSWATscores_Aridity> lkpSWATscores_Aridity { get; set; }
        public DbSet<lkpSWATscores_econPris> lkpSWATscores_econPris { get; set; }
        public DbSet<lkpSWATscores_eduGradDiff> lkpSWATscores_eduGradDiff { get; set; }
        public DbSet<lkpSWATscores_extremeEvents> lkpSWATscores_extremeEvents { get; set; }
        public DbSet<lkpSWATscores_extremePrep> lkpSWATscores_extremePrep { get; set; }
        public DbSet<lkpSWATscores_extremeRISK> lkpSWATscores_extremeRISK { get; set; }
        public DbSet<lkpSWATscores_gender> lkpSWATscores_gender { get; set; }
        public DbSet<lkpSWATscores_gRoleWomen> lkpSWATscores_gRoleWomen { get; set; }
        public DbSet<lkpSWATscores_gwAvailability> lkpSWATscores_gwAvailability { get; set; }
        public DbSet<lkpSWATscores_gWomenEngaged> lkpSWATscores_gWomenEngaged { get; set; }
        public DbSet<lkpSWATscores_indigPop> lkpSWATscores_indigPop { get; set; }
        public DbSet<lkpSWATscores_longtermPop> lkpSWATscores_longtermPop { get; set; }
        public DbSet<lkpSWATscores_precip> lkpSWATscores_precip { get; set; }
        public DbSet<lkpSWATscores_precipVar> lkpSWATscores_precipVar { get; set; }
        public DbSet<lkpSWATscores_rosca> lkpSWATscores_rosca { get; set; }
        public DbSet<lkpSWATscores_runoff> lkpSWATscores_runoff { get; set; }
        public DbSet<lkpSWATscores_urbanDistance> lkpSWATscores_urbanDistance { get; set; }
        public DbSet<lkpSWATscores_YesNoLUYesGood> lkpSWATscores_YesNoLUYesGood { get; set; }
        public DbSet<lkpSWATScoreVarsLU> lkpSWATScoreVarsLUs { get; set; }
        public DbSet<lkpSWATSectionLU> lkpSWATSectionLUs { get; set; }
        public DbSet<lkpSWATsubComponentLU> lkpSWATsubComponentLUs { get; set; }
        public DbSet<lkpSWATurbanDistanceLU> lkpSWATurbanDistanceLUs { get; set; }
        public DbSet<lkpSWATwaterMonthLU> lkpSWATwaterMonthLUs { get; set; }
        public DbSet<lkpSWATWatershedsLU> lkpSWATWatershedsLUs { get; set; }
        public DbSet<lkpSWATYesNoLU> lkpSWATYesNoLUs { get; set; }
        public DbSet<tblSWATBackgroundinfo> tblSWATBackgroundinfoes { get; set; }
        public DbSet<tblSWATCCedu> tblSWATCCedus { get; set; }
        public DbSet<tblSWATCCfinancial> tblSWATCCfinancials { get; set; }
        public DbSet<tblSWATCCgender> tblSWATCCgenders { get; set; }
        public DbSet<tblSWATCCindig> tblSWATCCindigs { get; set; }
        public DbSet<tblSWATCCschool> tblSWATCCschools { get; set; }
        public DbSet<tblSWATCCsocial> tblSWATCCsocials { get; set; }
        public DbSet<tblSWATCCtrain> tblSWATCCtrains { get; set; }
        public DbSet<tblSWATLocation> tblSWATLocations { get; set; }
        public DbSet<tblSWATScore> tblSWATScores { get; set; }
        public DbSet<tblSWATSurvey> tblSWATSurveys { get; set; }
        public DbSet<tblSWATVisualGroup> tblSWATVisualGroups { get; set; }
        public DbSet<tblSWATVisualGrpScoreMap> tblSWATVisualGrpScoreMaps { get; set; }
        public DbSet<tblSWATWAannualPrecip> tblSWATWAannualPrecips { get; set; }
        public DbSet<tblSWATWAclimateChange> tblSWATWAclimateChanges { get; set; }
        public DbSet<tblSWATWAextremeEvent> tblSWATWAextremeEvents { get; set; }
        public DbSet<tblSWATWAgroundWater> tblSWATWAgroundWaters { get; set; }
        public DbSet<tblSWATWAMonthlyQuantity> tblSWATWAMonthlyQuantities { get; set; }
        public DbSet<tblSWATWAPrecipitation> tblSWATWAPrecipitations { get; set; }
        public DbSet<tblSWATWAriskPrep> tblSWATWAriskPreps { get; set; }
        public DbSet<tblSWATWAsurfaceWater> tblSWATWAsurfaceWaters { get; set; }
        public DbSet<Userid> Userids { get; set; }
        public DbSet<lkpSWATscores_surfaceVar> lkpSWATscores_surfaceVar { get; set; }
        public DbSet<lkpSWATsurfaceVarLU> lkpSWATsurfaceVarLUs { get; set; }
        public DbSet<lkpSWATscores_socCBO> lkpSWATscores_socCBO { get; set; }
        public DbSet<lkpSWATsocCBOLU> lkpSWATsocCBOLUs { get; set; }
        public DbSet<lkpSWATscores_5rankAlwaysBad> lkpSWATscores_5rankAlwaysBad { get; set; }
        public DbSet<tblSWATCCcom> tblSWATCCcoms { get; set; }
        public DbSet<lkpSWATextVisitLU> lkpSWATextVisitLUs { get; set; }
        public DbSet<lkpSWATfundAppLU> lkpSWATfundAppLUs { get; set; }
        public DbSet<lkpSWATfundAppSuccessLU> lkpSWATfundAppSuccessLUs { get; set; }
        public DbSet<lkpSWATgovRightsLU> lkpSWATgovRightsLUs { get; set; }
        public DbSet<lkpSWATgovWatAnalLU> lkpSWATgovWatAnalLUs { get; set; }
        public DbSet<lkpSWATgovWatPolLU> lkpSWATgovWatPolLUs { get; set; }
        public DbSet<lkpSWATscores_extVisit> lkpSWATscores_extVisit { get; set; }
        public DbSet<lkpSWATscores_fundAppFUND> lkpSWATscores_fundAppFUND { get; set; }
        public DbSet<lkpSWATscores_fundAppLINK> lkpSWATscores_fundAppLINK { get; set; }
        public DbSet<lkpSWATscores_fundAppSuccess> lkpSWATscores_fundAppSuccess { get; set; }
        public DbSet<lkpSWATscores_govRights> lkpSWATscores_govRights { get; set; }
        public DbSet<lkpSWATscores_govWatAnal> lkpSWATscores_govWatAnal { get; set; }
        public DbSet<lkpSWATscores_govWatPol> lkpSWATscores_govWatPol { get; set; }
        public DbSet<tblSWATCCexternalSupport> tblSWATCCexternalSupports { get; set; }
        public DbSet<lkpSWATcomSatisfactionLU> lkpSWATcomSatisfactionLUs { get; set; }
        public DbSet<lkpSWATproPoorLU> lkpSWATproPoorLUs { get; set; }
        public DbSet<lkpSWATscores_comSatisfaction> lkpSWATscores_comSatisfaction { get; set; }
        public DbSet<lkpSWATscores_proPoor> lkpSWATscores_proPoor { get; set; }
        public DbSet<lkpSWATscores_watActionPlan> lkpSWATscores_watActionPlan { get; set; }
        public DbSet<lkpSWATscores_watBudget> lkpSWATscores_watBudget { get; set; }
        public DbSet<lkpSWATscores_watClassRep> lkpSWATscores_watClassRep { get; set; }
        public DbSet<lkpSWATscores_watCom> lkpSWATscores_watCom { get; set; }
        public DbSet<lkpSWATscores_watComConcerns> lkpSWATscores_watComConcerns { get; set; }
        public DbSet<lkpSWATscores_watTechStaff> lkpSWATscores_watTechStaff { get; set; }
        public DbSet<lkpSWATscores_watTechTraining> lkpSWATscores_watTechTraining { get; set; }
        public DbSet<lkpSWATwatActionPlanLU> lkpSWATwatActionPlanLUs { get; set; }
        public DbSet<lkpSWATwatBudgetLU> lkpSWATwatBudgetLUs { get; set; }
        public DbSet<lkpSWATwatClassRepLU> lkpSWATwatClassRepLUs { get; set; }
        public DbSet<lkpSWATwatComConcernsLU> lkpSWATwatComConcernsLUs { get; set; }
        public DbSet<lkpSWATwatComLU> lkpSWATwatComLUs { get; set; }
        public DbSet<lkpSWATwatTechStaffLU> lkpSWATwatTechStaffLUs { get; set; }
        public DbSet<lkpSWATwatTechTrainingLU> lkpSWATwatTechTrainingLUs { get; set; }
        public DbSet<tblSWATCCwaterManagement> tblSWATCCwaterManagements { get; set; }
        public DbSet<lkpSWATbestManagementLU> lkpSWATbestManagementLUs { get; set; }
        public DbSet<lkpSWATbestManIndLU> lkpSWATbestManIndLUs { get; set; }
        public DbSet<lkpSWATcropLossLU> lkpSWATcropLossLUs { get; set; }
        public DbSet<lkpSWATerosionLU> lkpSWATerosionLUs { get; set; }
        public DbSet<lkpSWATscores_bestManagement> lkpSWATscores_bestManagement { get; set; }
        public DbSet<lkpSWATscores_bestManInd> lkpSWATscores_bestManInd { get; set; }
        public DbSet<lkpSWATscores_cropLoss> lkpSWATscores_cropLoss { get; set; }
        public DbSet<lkpSWATscores_erosion> lkpSWATscores_erosion { get; set; }
        public DbSet<lkpSWATscores_YesNoLU_YesBad> lkpSWATscores_YesNoLU_YesBad { get; set; }
        public DbSet<tblSWATSWPag> tblSWATSWPags { get; set; }
        public DbSet<tblSWATSWPdev> tblSWATSWPdevs { get; set; }
        public DbSet<lkpSWATmedicalCostLU> lkpSWATmedicalCostLUs { get; set; }
        public DbSet<lkpSWATmedicalTimeLU> lkpSWATmedicalTimeLUs { get; set; }
        public DbSet<lkpSWATscores_medicalCost> lkpSWATscores_medicalCost { get; set; }
        public DbSet<lkpSWATscores_medicalTime> lkpSWATscores_medicalTime { get; set; }
        public DbSet<lkpSWATscores_survivorship> lkpSWATscores_survivorship { get; set; }
        public DbSet<lkpSWATsurvivorshipLU> lkpSWATsurvivorshipLUs { get; set; }
        public DbSet<tblSWATHPPcom> tblSWATHPPcoms { get; set; }
        public DbSet<tblSWATSWPl> tblSWATSWPls { get; set; }
        public DbSet<lkpSWATdevSiteLU> lkpSWATdevSiteLUs { get; set; }
        public DbSet<tblSWATHPPkhp> tblSWATHPPkhps { get; set; }
        public DbSet<lkpSWATscores_toiletsAll> lkpSWATscores_toiletsAll { get; set; }
        public DbSet<lkpSWATtoiletsAllLU> lkpSWATtoiletsAllLUs { get; set; }
        public DbSet<tblSWATSFsanitation> tblSWATSFsanitations { get; set; }
        public DbSet<tblSWATSFod> tblSWATSFods { get; set; }
        public DbSet<lkpSWATsanTypeLU> lkpSWATsanTypeLUs { get; set; }
        public DbSet<lkpSWATscores_sanType> lkpSWATscores_sanType { get; set; }
        public DbSet<tblSWATSFpoint> tblSWATSFpoints { get; set; }
        public DbSet<lkpSWATcentralConditionLU> lkpSWATcentralConditionLUs { get; set; }
        public DbSet<lkpSWATcentralTreatmentTypeLU> lkpSWATcentralTreatmentTypeLUs { get; set; }
        public DbSet<lkpSWAThhCleanLU> lkpSWAThhCleanLUs { get; set; }
        public DbSet<lkpSWATlatrineConditionLU> lkpSWATlatrineConditionLUs { get; set; }
        public DbSet<lkpSWATpubCleanLU> lkpSWATpubCleanLUs { get; set; }
        public DbSet<lkpSWATscores_centralCondition> lkpSWATscores_centralCondition { get; set; }
        public DbSet<lkpSWATscores_centralTreatment> lkpSWATscores_centralTreatment { get; set; }
        public DbSet<lkpSWATscores_hhClean> lkpSWATscores_hhClean { get; set; }
        public DbSet<lkpSWATscores_latrineCondition> lkpSWATscores_latrineCondition { get; set; }
        public DbSet<lkpSWATscores_pubClean> lkpSWATscores_pubClean { get; set; }
        public DbSet<tblSWATSFcentral> tblSWATSFcentrals { get; set; }
        public DbSet<tblSWATSFlat> tblSWATSFlats { get; set; }
        public DbSet<tblSWATSFseptic> tblSWATSFseptics { get; set; }
        public DbSet<lkpSWATwpaLoc> lkpSWATwpaLocs { get; set; }
        public DbSet<lkpSWATwpaTypeDesc> lkpSWATwpaTypeDescs { get; set; }
        public DbSet<tblSWATWPoverview> tblSWATWPoverviews { get; set; }
        public DbSet<tblSWATWPscore> tblSWATWPscores { get; set; }
        public DbSet<lkpSWATWPscoreLU> lkpSWATWPscoreLUs { get; set; }
        public DbSet<lkpSWATWPsectionLU> lkpSWATWPsectionLUs { get; set; }
        public DbSet<lkpSWATcollectDangerLU> lkpSWATcollectDangerLUs { get; set; }
        public DbSet<lkpSWATdomWaterUsesLU> lkpSWATdomWaterUsesLUs { get; set; }
        public DbSet<lkpSWATeaseUseLU> lkpSWATeaseUseLUs { get; set; }
        public DbSet<lkpSWATscores_collectDanger> lkpSWATscores_collectDanger { get; set; }
        public DbSet<lkpSWATscores_domWaterUses> lkpSWATscores_domWaterUses { get; set; }
        public DbSet<lkpSWATscores_easeUse> lkpSWATscores_easeUse { get; set; }
        public DbSet<lkpSWATscores_waterCollectTime> lkpSWATscores_waterCollectTime { get; set; }
        public DbSet<lkpSWATscores_waterEffort> lkpSWATscores_waterEffort { get; set; }
        public DbSet<lkpSWATscores_wpaReliabilityMonth> lkpSWATscores_wpaReliabilityMonth { get; set; }
        public DbSet<lkpSWATwaterCollectTimeLU> lkpSWATwaterCollectTimeLUs { get; set; }
        public DbSet<lkpSWATwaterEffortLU> lkpSWATwaterEffortLUs { get; set; }
        public DbSet<lkpSWATwpaReliabilityMonthLU> lkpSWATwpaReliabilityMonthLUs { get; set; }
        public DbSet<tblSWATWPsupply> tblSWATWPsupplies { get; set; }
        public DbSet<lkpSWATfaecalPathogensLU> lkpSWATfaecalPathogensLUs { get; set; }
        public DbSet<lkpSWATqualTreatedLU> lkpSWATqualTreatedLUs { get; set; }
        public DbSet<lkpSWATscores_faecalPathogens> lkpSWATscores_faecalPathogens { get; set; }
        public DbSet<lkpSWATscores_qualTreated> lkpSWATscores_qualTreated { get; set; }
        public DbSet<lkpSWATscores_userTreatedifTreatedbefore> lkpSWATscores_userTreatedifTreatedbefore { get; set; }
        public DbSet<lkpSWATscores_userTreatedifUntreatedBefore> lkpSWATscores_userTreatedifUntreatedBefore { get; set; }
        public DbSet<lkpSWATscores_userTreatmentMethod> lkpSWATscores_userTreatmentMethod { get; set; }
        public DbSet<lkpSWATscores_waterTasteOdour> lkpSWATscores_waterTasteOdour { get; set; }
        public DbSet<lkpSWATscores_waterTurbidity> lkpSWATscores_waterTurbidity { get; set; }
        public DbSet<lkpSWATuserTreatedLU> lkpSWATuserTreatedLUs { get; set; }
        public DbSet<lkpSWATuserTreatmentMethodLU> lkpSWATuserTreatmentMethodLUs { get; set; }
        public DbSet<lkpSWATwaterTasteOdourLU> lkpSWATwaterTasteOdourLUs { get; set; }
        public DbSet<lkpSWATwaterTurbidityLU> lkpSWATwaterTurbidityLUs { get; set; }
        public DbSet<tblSWATWPquality> tblSWATWPqualities { get; set; }
    }
}
