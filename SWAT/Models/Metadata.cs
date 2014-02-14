using System;
using System.ComponentModel.DataAnnotations;

namespace SWAT.Models
{
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
        [Range(1, int.MaxValue, ErrorMessage = "The population has to be an integer and >0.")]
        public Nullable<long> Population;

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
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> January { get; set; }

        [Display(Name = "Average precipitation of February")]
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> February { get; set; }

        [Display(Name = "Average precipitation of March")]
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> March { get; set; }

        [Display(Name = "Average precipitation of April")]
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> April { get; set; }

        [Display(Name = "Average precipitation of May")]
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> May { get; set; }

        [Display(Name = "Average precipitation of June")]
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> June { get; set; }

        [Display(Name = "Average precipitation of July")]
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> July { get; set; }

        [Display(Name = "Average precipitation of August")]
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> August { get; set; }

        [Display(Name = "Average precipitation of September")]
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> September { get; set; }

        [Display(Name = "Average precipitation of October")]
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> October { get; set; }

        [Display(Name = "Average precipitation of November")]
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> November { get; set; }

        [Display(Name = "Average precipitation of December")]
        [Range(0, double.MaxValue, ErrorMessage = "The value is out of range.")]
        public Nullable<double> December { get; set; }
    
    }
}