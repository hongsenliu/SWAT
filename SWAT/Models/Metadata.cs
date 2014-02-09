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
}