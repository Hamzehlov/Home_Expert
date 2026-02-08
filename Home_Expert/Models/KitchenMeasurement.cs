using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class KitchenMeasurement
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]
    public string CustomerId { get; set; } = null!;

    public int InputModeId { get; set; }

    public int UnitId { get; set; }

    public string? RawData { get; set; }

    [Column("Generated2DPlan")]
    public byte[]? Generated2Dplan { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("KitchenMeasurements")]
    public virtual ApplicationUser Customer { get; set; } = null!;

    [ForeignKey("InputModeId")]
    [InverseProperty("KitchenMeasurementInputModes")]
    public virtual Code InputMode { get; set; } = null!;

    [InverseProperty("Measurement")]
    public virtual ICollection<KitchenCostEstimate> KitchenCostEstimates { get; set; } = new List<KitchenCostEstimate>();

    [InverseProperty("Measurement")]
    public virtual ICollection<KitchenExport> KitchenExports { get; set; } = new List<KitchenExport>();

    [ForeignKey("UnitId")]
    [InverseProperty("KitchenMeasurementUnits")]
    public virtual Code Unit { get; set; } = null!;
}
