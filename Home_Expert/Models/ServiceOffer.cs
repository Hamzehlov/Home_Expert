using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class ServiceOffer
{
    [Key]
    public int Id { get; set; }

    public int RequestId { get; set; }

    public int VendorId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PriceEstimate { get; set; }

    public bool? InspectionRequired { get; set; }

    public int StatusId { get; set; }

    [ForeignKey("RequestId")]
    [InverseProperty("ServiceOffers")]
    public virtual ServiceRequest Request { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("ServiceOffers")]
    public virtual Code Status { get; set; } = null!;

    [ForeignKey("VendorId")]
    [InverseProperty("ServiceOffers")]
    public virtual Vendor Vendor { get; set; } = null!;
}
