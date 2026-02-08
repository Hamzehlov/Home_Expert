using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class Review
{
    [Key]
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int VendorId { get; set; }

    [StringLength(450)]
    public string CustomerId { get; set; } = null!;

    public int? Quality { get; set; }

    public int? Commitment { get; set; }

    public int? Communication { get; set; }

    public int? Safety { get; set; }

    [Column(TypeName = "decimal(3, 2)")]
    public decimal? FinalScore { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Reviews")]
    public virtual ApplicationUser Customer { get; set; } = null!;

    [ForeignKey("VendorId")]
    [InverseProperty("Reviews")]
    public virtual Vendor Vendor { get; set; } = null!;
}
