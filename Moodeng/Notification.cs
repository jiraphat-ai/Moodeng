//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Moodeng
{
    using System;
    using System.Collections.Generic;
    
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int RetailStoreId { get; set; }
        public string NotificationMessage { get; set; }
        public string NotificationType { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    
        public virtual RetailStore RetailStore { get; set; }
    }
}