using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

[Table("VendorService")]
public partial class VendorService
{
    [Key]
    public int Id { get; set; }

    public int VendorId { get; set; }

    public int ServiceId { get; set; }

    [ForeignKey("ServiceId")]
    [InverseProperty("VendorServices")]
    public virtual Service Service { get; set; } = null!;

    [ForeignKey("VendorId")]
    [InverseProperty("VendorServices")]
    public virtual Vendor Vendor { get; set; } = null!;
}
