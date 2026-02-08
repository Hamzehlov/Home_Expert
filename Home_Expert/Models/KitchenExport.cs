using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class KitchenExport
{
    [Key]
    public int Id { get; set; }

    public int MeasurementId { get; set; }

    public int FormatId { get; set; }

    public byte[]? File { get; set; }

    [ForeignKey("FormatId")]
    [InverseProperty("KitchenExports")]
    public virtual Code Format { get; set; } = null!;

    [ForeignKey("MeasurementId")]
    [InverseProperty("KitchenExports")]
    public virtual KitchenMeasurement Measurement { get; set; } = null!;
}
