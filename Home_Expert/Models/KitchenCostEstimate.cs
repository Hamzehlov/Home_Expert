using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class KitchenCostEstimate
{
    [Key]
    public int Id { get; set; }

    public int MeasurementId { get; set; }

    public int WoodTypeId { get; set; }

    public string? Accessories { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? EstimatedCost { get; set; }

    public int StatusId { get; set; }

    [ForeignKey("MeasurementId")]
    [InverseProperty("KitchenCostEstimates")]
    public virtual KitchenMeasurement Measurement { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("KitchenCostEstimateStatuses")]
    public virtual Code Status { get; set; } = null!;

    [ForeignKey("WoodTypeId")]
    [InverseProperty("KitchenCostEstimateWoodTypes")]
    public virtual Code WoodType { get; set; } = null!;
}
