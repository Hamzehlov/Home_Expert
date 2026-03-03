using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class ProductImage
{
    [Key]
    public int Id { get; set; }

    public int ProductId { get; set; }

    public byte[] ImageData { get; set; } = null!;

    public bool IsMain { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductImages")]
    public virtual Product Product { get; set; } = null!;
}
