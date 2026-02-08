using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class VendorSubscription
{
    [Key]
    public int Id { get; set; }

    public int VendorId { get; set; }

    public int SubscriptionId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    public bool? IsActive { get; set; }

    [ForeignKey("SubscriptionId")]
    [InverseProperty("VendorSubscriptions")]
    public virtual Subscription Subscription { get; set; } = null!;

    [ForeignKey("VendorId")]
    [InverseProperty("VendorSubscriptions")]
    public virtual Vendor Vendor { get; set; } = null!;
}
