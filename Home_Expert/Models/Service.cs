using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class Service
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string NameAr { get; set; } = null!;

    [StringLength(100)]
    public string NameEn { get; set; } = null!;

    public int TypeId { get; set; }

    public int? CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("ServiceCategories")]
    public virtual Code? Category { get; set; }

    [InverseProperty("Service")]
    public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();

    [ForeignKey("TypeId")]
    [InverseProperty("ServiceTypes")]
    public virtual Code Type { get; set; } = null!;
}
