using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class EscrowTransaction
{
    [Key]
    public int Id { get; set; }

    public int? OrderId { get; set; }

    [StringLength(450)]
    public string CustomerId { get; set; } = null!;

    public int VendorId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    public int StatusId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("EscrowTransactions")]
    public virtual ApplicationUser Customer { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("EscrowTransactions")]
    public virtual Code Status { get; set; } = null!;

    [ForeignKey("VendorId")]
    [InverseProperty("EscrowTransactions")]
    public virtual Vendor Vendor { get; set; } = null!;
}
