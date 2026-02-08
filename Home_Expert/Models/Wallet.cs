using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class Wallet
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]
    public string UserId { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Balance { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Wallets")]
    public virtual ApplicationUser User { get; set; } = null!;
}
