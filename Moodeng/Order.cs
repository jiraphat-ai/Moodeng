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
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public int RetailStoreId { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string Payment { get; set; }
        public byte PaymentStatus { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual RetailStore RetailStore { get; set; }
        public object Customer { get; internal set; }
    }
}
