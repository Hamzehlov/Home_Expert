using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class Vendor
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]
    public string UserId { get; set; } = null!;

    [StringLength(100)]
    public string CompanyName { get; set; } = null!;

    public byte[]? Logo { get; set; }

    public string? Description { get; set; }

    public int? YearsExperience { get; set; }

    public bool? Verified { get; set; }

    public int ServiceTypeId { get; set; }

    [Column(TypeName = "decimal(3, 2)")]
    public decimal? RatingAvg { get; set; }

    public int? CompletedOrders { get; set; }

    [InverseProperty("Vendor")]
    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    [InverseProperty("Vendor")]
    public virtual ICollection<EscrowTransaction> EscrowTransactions { get; set; } = new List<EscrowTransaction>();

    [InverseProperty("Vendor")]
    public virtual ICollection<MovingOffer> MovingOffers { get; set; } = new List<MovingOffer>();

    [InverseProperty("Vendor")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    [InverseProperty("Vendor")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("Vendor")]
    public virtual ICollection<ServiceOffer> ServiceOffers { get; set; } = new List<ServiceOffer>();

    [ForeignKey("ServiceTypeId")]
    [InverseProperty("Vendors")]
    public virtual Code ServiceType { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Vendors")]
    public virtual ApplicationUser User { get; set; } = null!;

    [InverseProperty("Vendor")]
    public virtual ICollection<VendorMedium> VendorMedia { get; set; } = new List<VendorMedium>();

    [InverseProperty("Vendor")]
    public virtual ICollection<VendorSubscription> VendorSubscriptions { get; set; } = new List<VendorSubscription>();
}
