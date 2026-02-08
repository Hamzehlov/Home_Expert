using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class SubscriptionFeature
{
    [Key]
    public int Id { get; set; }

    public int SubscriptionId { get; set; }

    public int FeatureKeyId { get; set; }

    public bool? IsEnabled { get; set; }

    [ForeignKey("FeatureKeyId")]
    [InverseProperty("SubscriptionFeatures")]
    public virtual Code FeatureKey { get; set; } = null!;

    [ForeignKey("SubscriptionId")]
    [InverseProperty("SubscriptionFeatures")]
    public virtual Subscription Subscription { get; set; } = null!;
}
