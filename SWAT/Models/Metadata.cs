using System;
using System.ComponentModel.DataAnnotations;

namespace SWAT.Models
{
    public class CCFinancialMetadata
    {
        [Display(Name = "% of Households")]
        [Range(0, 100, ErrorMessage = "The value is out of range")]
        public Nullable<double> income { get; set; }

        [Display(Name = "Land or real estate")]
        public bool assetsCom1 { get; set; }

        [Display(Name = "Equipment")]
        public bool assetsCom2 { get; set; }

        [Display(Name = "Cash")]
        public bool assetsCom3 { get; set; }

        [Display(Name = "Other investments (stocks, bonds, etc.)")]
        public bool assetsCom4 { get; set; }

        [Display(Name = "Land or real estate")]
        public bool assetsInd1 { get; set; }

        [Display(Name = "Equipment")]
        public bool assetsInd2 { get; set; }

        [Display(Name = "Cash")]
        public bool assetsInd3 { get; set; }

        [Display(Name = "Other investments (stocks, bonds, etc.)")]
        public bool assetsInd4 { get; set; }
    }

    public class CCIndigMetadata
    {
        [Display(Name = "Indigenous Popluation")]
        [Range(0, int.MaxValue, ErrorMessage = "The value is out of range")]
        public Nullable<int> indigPop { get; set; }

        [Display(Name = "Longterm Popluation")]
        [Range(0, int.MaxValue, ErrorMessage = "The value is out of range")]
        public Nullable<int> longtermPop { get; set; }
    }

    public class CCSchoolMetadata
    {
        [Display(Name = "% of children")]
        [Range(0, 100, ErrorMessage = "The value is out of range.")]
        public Nullable<double> schoolAttend { get; set; }

        [Display(Name = "Elementary School")]
        public bool schoolInstitution1 { get; set; }

        [Display(Name = "Secondary School")]
        public bool schoolInstitution2 { get; set; }

        [Display(Name = "Technical Institute")]
        public bool schoolInstitution3 { get; set; }

        [Display(Name = "College/University")]
        public bool schoolInstitution4 { get; set; }

        [Display(Name = "Access to Health and Hygiene")]
        public Nullable<int> schoolHHAccess { get; set; }
    }

    public class CCTrainMetadata
    {
        [Display(Name = "Administrative supervisor")]
        public bool trainProf1 { get; set; }

        [Display(Name = "Health scientist (nurse, doctor, researcher)")]
        public bool trainProf2 { get; set; }

        [Display(Name = "Engineer")]
        public bool trainProf3 { get; set; }

        [Display(Name = "Lawyer")]
        public bool trainProf4 { get; set; }

        [Display(Name = "Accountant")]
        public bool trainProf5 { get; set; }

        [Display(Name = "Mechanic")]
        public bool trainTech1 { get; set; }

        [Display(Name = "Laboratory technician")]
        public bool trainTech2 { get; set; }

        [Display(Name = "Water system operator")]
        public bool trainTech3 { get; set; }

        [Display(Name = "Administrative Assistant")]
        public bool trainTech4 { get; set; }

        [Display(Name = "IT Technician")]
        public bool trainTech5 { get; set; }
    }

    public class CCEducationMetadata
    {
        [Display(Name = "% of Primary School")]
        [Range(0, 100, ErrorMessage="The value is out of range")]
        public Nullable<double> eduPrim { get; set; }

        [Display(Name = "% in Secondary School")]
        [Range(0, 100, ErrorMessage = "The value is out of range")]
        public Nullable<double> eduSec { get; set; }

        [Display(Name = "% of Women Finished Secondary School")]
        [Range(0, 100, ErrorMessage = "The value is out of range")]
        public Nullable<double> eduGradWomen { get; set; }

        [Display(Name = "% of Men Finished Secondary School")]
        [Range(0, 100, ErrorMessage = "The value is out of range")]
        public Nullable<double> eduGradMen { get; set; }
    }

    public class WAGroundWaterMetadata
    {
        [Display(Name = "Ground Water Availability")]
        public Nullable<int> gwAvailability { get; set; }

        [Display(Name = "Ground Water Reliability")]
        public Nullable<int> gwReliability { get; set; }
    }

    public class WASurfaceWaterMetadata
    {
        [Display(Name = "Runoff")]
        public Nullable<int> runoff { get; set; }

        [Display(Name = "Suface Water Level")]
        public Nullable<int> surfaceVar { get; set; }
    }

    public class WARiskPrepMetadata
    {
        [Display(Name = "Risk of Fire")]
        public Nullable<int> riskFire { get; set; }

        [Display(Name = "Risk of Flood")]
        public Nullable<int> riskFlood { get; set; }

        [Display(Name = "Risk of Drought")]
        public Nullable<int> riskDrought { get; set; }

        [Display(Name = "Preparedness for Fire")]
        public Nullable<int> prepFire { get; set; }

        [Display(Name = "Preparedness for Flood")]
        public Nullable<int> prepFlood { get; set; }

        [Display(Name = "Preparedness for Drought")]
        public Nullable<int> prepDrought { get; set; }
    }

    public class LocationMetadata
    {
        [Required(ErrorMessage="Settlement Name is required.")]
        [StringLength(50)]
        public string name;
        [Range(Double.MinValue, Double.MaxValue, ErrorMessage="Latitude value is invalid.")]
        public double latitude;
        [Range(Double.MinValue, Double.MaxValue, ErrorMessage="Longitude value is invalid.")]
        public double longitude;
        [Required(ErrorMessage = "Country is required.")]
        public int countryID;
        [Required(ErrorMessage = "Region is required.")]
        public int regionID;
        [Required(ErrorMessage = "Subnation is required.")]
        public int subnationalID;
    }

    public class BackgroundMetadata
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The population has to be an integer and >0.")]
        public Nullable<long> Population;

        [Required]
        [Display(Name = "Number of Households")]
        [Range(1, int.MaxValue, ErrorMessage = "The number of households has to be an integer and >0.")]
        public Nullable<int> numHouseholds;

        [Display(Name = "Number of Children")]
        [Range(1, int.MaxValue, ErrorMessage = "The number of Children has to be an integer and >0.")]
        public Nullable<int> numChildren;

        [Display(Name = "People Per Household")]
        [Range(0.0001, Double.MaxValue, ErrorMessage = "The people per household has to be >0.")]
        public Nullable<double> PeoplePerHH;

        [Display(Name = "Forest (%)")]
        [Range(0, 100, ErrorMessage="The value is out of range.")]
        public Nullable<double> AreaForest;

        [Display(Name = "Agriculture (%)")]
        [Range(0, 100, ErrorMessage = "The value is out of range.")]
        public Nullable<double> AreaAg;

        [Display(Name = "Infrastructure (%)")]
        [Range(0, 100, ErrorMessage = "The value is out of range.")]
        public Nullable<double> AreaInf;

        [Display(Name = "Source Water (%)")]
        [Range(0, 100, ErrorMessage = "The value is out of range.")]
        public Nullable<double> AreaSw;

        [Display(Name = "Wetlands (%)")]
        [Range(0, 100, ErrorMessage = "The value is out of range.")]
        public Nullable<double> AreaWet;

        [Display(Name = "Native or Natural Lands (%)")]
        [Range(0, 100, ErrorMessage = "The value is out of range.")]
        public Nullable<double> AreaNat;
    }

    public class WAPrecipitationMetadata
    {
        [Display(Name = "Average precipitation of January")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> January { get; set; }

        [Display(Name = "Average precipitation of February")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> February { get; set; }

        [Display(Name = "Average precipitation of March")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> March { get; set; }

        [Display(Name = "Average precipitation of April")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> April { get; set; }

        [Display(Name = "Average precipitation of May")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> May { get; set; }

        [Display(Name = "Average precipitation of June")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> June { get; set; }

        [Display(Name = "Average precipitation of July")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> July { get; set; }

        [Display(Name = "Average precipitation of August")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> August { get; set; }

        [Display(Name = "Average precipitation of September")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> September { get; set; }

        [Display(Name = "Average precipitation of October")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> October { get; set; }

        [Display(Name = "Average precipitation of November")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> November { get; set; }

        [Display(Name = "Average precipitation of December")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> December { get; set; }
    
    }

    public class WAannualPrecipMetadata
    {
        [Display(Name = "Standard Deviation of annual precipitation")]
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> precipVar { get; set; }

        [Display(Name = "Annual Precipitation")]
        public Nullable<int> precipVarALT { get; set; }
    }

    public class WAClimateChangeMetadata
    {
        [Display(Name = "Dryer")]
        public bool climateDryer { get; set; }

        [Display(Name = "Wetter")]
        public bool climateWetter { get; set; }

        [Display(Name = "Colder")]
        public bool climateColder { get; set; }

        [Display(Name = "Hotter")]
        public bool climateHotter { get; set; }

        [Display(Name = "Seasons")]
        public bool climateSeasons { get; set; }

    }

    public class WAExtremeEventMetadata
    {
        [Display(Name="Extreme Dry")]
        public Nullable<int> extremeDry { get; set; }

        [Display(Name = "Extreme Flood")]
        public Nullable<int> extremeFlood { get; set; }

        [Display(Name = "Extreme Other Events")]
        public Nullable<int> extremeOther { get; set; }

        [Display(Name = "Other? (Please specify)")]
        public string extremeOtherComment { get; set; }
    }
}