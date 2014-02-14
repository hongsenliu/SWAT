using System;
using System.ComponentModel.DataAnnotations;

namespace SWAT.Models
{
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
}