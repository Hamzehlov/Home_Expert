using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class UserLocation
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]
    public string UserId { get; set; } = null!;

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? Area { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserLocations")]
    public virtual ApplicationUser User { get; set; } = null!;
}
