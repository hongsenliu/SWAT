using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Reflection;

namespace SWAT.Models
{
    [MetadataType(typeof(WPSupplyMetadata))]
    public partial class tblSWATWPsupply
    {
    }

    [MetadataType(typeof(WaterPointMetadata))]
    public partial class tblSWATWPoverview
    {
    }

    [MetadataType(typeof(SFLatMetadata))]
    public partial class tblSWATSFlat
    {
    }

    [MetadataType(typeof(SFSepticMetadata))]
    public partial class tblSWATSFseptic
    {
    }

    [MetadataType(typeof(SFCentralMetadata))]
    public partial class tblSWATSFcentral
    {
    }

    [MetadataType(typeof(SFPointMetadata))]
    public partial class tblSWATSFpoint
    {
    }

    [MetadataType(typeof(SFOdMetadata))]
    public partial class tblSWATSFod
    {
    }

    [MetadataType(typeof(SFSanitationMetadata))]
    public partial class tblSWATSFsanitation
    { 
    }

    [MetadataType(typeof(HPPKhpMetadata))]
    public partial class tblSWATHPPkhp
    { 
    }

    [MetadataType(typeof(SWPDevMetadata))]
    public partial class tblSWATSWPdev
    {
    }

    [MetadataType(typeof(SWPAgMetadata))]
    public partial class tblSWATSWPag
    { 
    }

    [MetadataType(typeof(SWPLivestockMetadata))]
    public partial class tblSWATSWPl
    { 
    }

    [MetadataType(typeof(CCWaterManagementMetadata))]
    public partial class tblSWATCCwaterManagement
    { 
    }

    [MetadataType(typeof(CCExternalMetadata))]
    public partial class tblSWATCCexternalSupport
    { 
    }

    [MetadataType(typeof(CCComMetada))]
    public partial class tblSWATCCcom
    { 
    }

    [MetadataType(typeof(CCSocialMetadata))]
    public partial class tblSWATCCsocial
    { 
    }

    [MetadataType(typeof(CCGenderMetadata))]
    public partial class tblSWATCCgender
    { 
    }

    [MetadataType(typeof(CCFinancialMetadata))]
    public partial class tblSWATCCfinancial
    { 
    }

    [MetadataType(typeof(CCIndigMetadata))]
    public partial class tblSWATCCindig
    {
    }

    [MetadataType(typeof(CCSchoolMetadata))]
    public partial class tblSWATCCschool
    { 
    }

    [MetadataType(typeof(CCTrainMetadata))]
    public partial class tblSWATCCtrain
    {
    }

    [MetadataType(typeof(CCEducationMetadata))]
    public partial class tblSWATCCedu
    { 
    }

    [MetadataType(typeof(WAGroundWaterMetadata))]
    public partial class tblSWATWAgroundWater
    {
    }

    [MetadataType(typeof(WASurfaceWaterMetadata))]
    public partial class tblSWATWAsurfaceWater
    { 
    }

    [MetadataType(typeof(WARiskPrepMetadata))]
    public partial class tblSWATWAriskPrep
    { 
    }

    [MetadataType(typeof(LocationMetadata))]
    public partial class tblSWATLocation
    {
    }

    [MetadataType(typeof(BackgroundMetadata))]
    public partial class tblSWATBackgroundinfo
    { 
    }

    [MetadataType(typeof(WAPrecipitationMetadata))]
    public partial class tblSWATWAPrecipitation
    {
    }

    [MetadataType(typeof(WAannualPrecipMetadata))]
    public partial class tblSWATWAannualPrecip
    { 
    }

    [MetadataType(typeof(WAClimateChangeMetadata))]
    public partial class tblSWATWAclimateChange
    { 
    }

    [MetadataType(typeof(WAExtremeEventMetadata))]
    public partial class tblSWATWAextremeEvent
    { 
    }

    
    //public partial class SWATEntities
    //{
    //    // TODO uncomment this part to deploy
    //    //private void setDBpw(string password)
    //    //{
    //    //    var settings = System.Configuration.ConfigurationManager.ConnectionStrings[2];
    //    //    var fi = typeof(System.Configuration.ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
    //    //    fi.SetValue(settings, false);
    //    //    string connStr = settings.ConnectionString;
    //    //    int insertIndex = connStr.IndexOf("databasename");
    //    //    connStr = connStr.Insert(insertIndex, "password=" + password + ";");
    //    //    settings.ConnectionString = connStr;
    //    //    fi.SetValue(settings, true);
    //    //}
    //}
}