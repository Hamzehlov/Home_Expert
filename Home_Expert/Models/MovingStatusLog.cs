using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class MovingStatusLog
{
    [Key]
    public int Id { get; set; }

    public int MovingRequestId { get; set; }

    public int StatusId { get; set; }

    public byte[]? Proof { get; set; }

    [ForeignKey("MovingRequestId")]
    [InverseProperty("MovingStatusLogs")]
    public virtual MovingRequest MovingRequest { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("MovingStatusLogs")]
    public virtual Code Status { get; set; } = null!;
}
