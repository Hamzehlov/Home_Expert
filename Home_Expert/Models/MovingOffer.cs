using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class MovingOffer
{
    [Key]
    public int Id { get; set; }

    public int MovingRequestId { get; set; }

    public int VendorId { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Price { get; set; }

    [StringLength(50)]
    public string? EstimatedTime { get; set; }

    public int StatusId { get; set; }

    [ForeignKey("MovingRequestId")]
    [InverseProperty("MovingOffers")]
    public virtual MovingRequest MovingRequest { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("MovingOffers")]
    public virtual Code Status { get; set; } = null!;

    [ForeignKey("VendorId")]
    [InverseProperty("MovingOffers")]
    public virtual Vendor Vendor { get; set; } = null!;
}
