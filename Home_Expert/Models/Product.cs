using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class Product
{
    [Key]
    public int Id { get; set; }

    public int VendorId { get; set; }

    public int CategoryId { get; set; }

    [StringLength(100)]
    public string NameAr { get; set; } = null!;

    [StringLength(100)]
    public string NameEn { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PriceRangeMin { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? PriceRangeMax { get; set; }

    public bool? IsActive { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("VendorId")]
    [InverseProperty("Products")]
    public virtual Vendor Vendor { get; set; } = null!;
}
