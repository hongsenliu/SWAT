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
}