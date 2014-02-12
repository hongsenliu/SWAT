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
    
    public partial class Userid
    {
        public Userid()
        {
            this.tblSWATSurveys = new HashSet<tblSWATSurvey>();
        }
    
        public int Userid1 { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string eMail { get; set; }
        public string Notes { get; set; }
        public string SessionVars { get; set; }
        public Nullable<int> secondaryId { get; set; }
        public int type { get; set; }
        public string properties { get; set; }
        public string targetName { get; set; }
        public string NotifStatus { get; set; }
        public string NotifCode { get; set; }
        public Nullable<System.DateTime> NotifReqCodeTime { get; set; }
        public Nullable<System.DateTime> NotifActCodeTime { get; set; }
        public bool PMEmailNotificationsEnabled { get; set; }
        public bool UseOntarioMaps { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Address { get; set; }
        public Nullable<decimal> utmX { get; set; }
        public Nullable<decimal> utmY { get; set; }
        public System.DateTime Joined { get; set; }
        public string Interests { get; set; }
        public string Workplace { get; set; }
        public string SecretQuestion { get; set; }
        public string SecretAnswer { get; set; }
        public Nullable<int> AvatarUserPhotoID { get; set; }
        public Nullable<System.DateTime> LastSuggestedContentDate { get; set; }
        public string FullName { get; set; }
    
        public virtual ICollection<tblSWATSurvey> tblSWATSurveys { get; set; }
    }
}
