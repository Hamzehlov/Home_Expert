using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

[Table("Code")]
public partial class Code
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string DescCodeAr { get; set; } = null!;

    [StringLength(100)]
    public string DescCodeEn { get; set; } = null!;

    [StringLength(50)]
    public string Type { get; set; } = null!;

    public int? ParentId { get; set; }

    public bool? IsActive { get; set; }


    public byte[]? Image { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<EscrowTransaction> EscrowTransactions { get; set; } = new List<EscrowTransaction>();

    [InverseProperty("Parent")]
    public virtual ICollection<Code> InverseParent { get; set; } = new List<Code>();

    [InverseProperty("Status")]
    public virtual ICollection<KitchenCostEstimate> KitchenCostEstimateStatuses { get; set; } = new List<KitchenCostEstimate>();

    [InverseProperty("WoodType")]
    public virtual ICollection<KitchenCostEstimate> KitchenCostEstimateWoodTypes { get; set; } = new List<KitchenCostEstimate>();

    [InverseProperty("Format")]
    public virtual ICollection<KitchenExport> KitchenExports { get; set; } = new List<KitchenExport>();

    [InverseProperty("InputMode")]
    public virtual ICollection<KitchenMeasurement> KitchenMeasurementInputModes { get; set; } = new List<KitchenMeasurement>();

    [InverseProperty("Unit")]
    public virtual ICollection<KitchenMeasurement> KitchenMeasurementUnits { get; set; } = new List<KitchenMeasurement>();

    [InverseProperty("Status")]
    public virtual ICollection<MovingOffer> MovingOffers { get; set; } = new List<MovingOffer>();

    [InverseProperty("Status")]
    public virtual ICollection<MovingRequest> MovingRequests { get; set; } = new List<MovingRequest>();

    [InverseProperty("Status")]
    public virtual ICollection<MovingStatusLog> MovingStatusLogs { get; set; } = new List<MovingStatusLog>();

    [ForeignKey("ParentId")]
    [InverseProperty("InverseParent")]
    public virtual Code? Parent { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Service> ServiceCategories { get; set; } = new List<Service>();

    [InverseProperty("Status")]
    public virtual ICollection<ServiceOffer> ServiceOffers { get; set; } = new List<ServiceOffer>();

    [InverseProperty("IssueType")]
    public virtual ICollection<ServiceRequestIssue> ServiceRequestIssues { get; set; } = new List<ServiceRequestIssue>();

    [InverseProperty("Status")]
    public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();

    [InverseProperty("Type")]
    public virtual ICollection<Service> ServiceTypes { get; set; } = new List<Service>();

    [InverseProperty("FeatureKey")]
    public virtual ICollection<SubscriptionFeature> SubscriptionFeatures { get; set; } = new List<SubscriptionFeature>();

    [InverseProperty("MediaType")]
    public virtual ICollection<VendorMedium> VendorMedia { get; set; } = new List<VendorMedium>();

    [InverseProperty("ServiceType")]
    public virtual ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();
}
