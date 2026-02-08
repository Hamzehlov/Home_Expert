using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class ServiceRequest
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]
    public string CustomerId { get; set; } = null!;

    public int ServiceId { get; set; }

    public int? LocationId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PreferredTime { get; set; }

    public int StatusId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("ServiceRequests")]
    public virtual ApplicationUser Customer { get; set; } = null!;

    [ForeignKey("ServiceId")]
    [InverseProperty("ServiceRequests")]
    public virtual Service Service { get; set; } = null!;

    [InverseProperty("Request")]
    public virtual ICollection<ServiceOffer> ServiceOffers { get; set; } = new List<ServiceOffer>();

    [InverseProperty("Request")]
    public virtual ICollection<ServiceRequestIssue> ServiceRequestIssues { get; set; } = new List<ServiceRequestIssue>();

    [ForeignKey("StatusId")]
    [InverseProperty("ServiceRequests")]
    public virtual Code Status { get; set; } = null!;
}
