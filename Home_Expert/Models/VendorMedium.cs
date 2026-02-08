using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class VendorMedium
{
    [Key]
    public int Id { get; set; }

    public int VendorId { get; set; }

    public int MediaTypeId { get; set; }

    public byte[]? Media { get; set; }

    [StringLength(50)]
    public string? BeforeAfter { get; set; }

    [ForeignKey("MediaTypeId")]
    [InverseProperty("VendorMedia")]
    public virtual Code MediaType { get; set; } = null!;

    [ForeignKey("VendorId")]
    [InverseProperty("VendorMedia")]
    public virtual Vendor Vendor { get; set; } = null!;
}
