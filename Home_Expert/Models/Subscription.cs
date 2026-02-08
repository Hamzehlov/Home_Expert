using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class Subscription
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string NameAr { get; set; } = null!;

    [StringLength(50)]
    public string NameEn { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    public int DurationDays { get; set; }

    [InverseProperty("Subscription")]
    public virtual ICollection<SubscriptionFeature> SubscriptionFeatures { get; set; } = new List<SubscriptionFeature>();

    [InverseProperty("Subscription")]
    public virtual ICollection<VendorSubscription> VendorSubscriptions { get; set; } = new List<VendorSubscription>();
}
