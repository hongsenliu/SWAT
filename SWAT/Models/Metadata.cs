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
}