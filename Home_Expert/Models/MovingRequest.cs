using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class MovingRequest
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]
    public string CustomerId { get; set; } = null!;

    [StringLength(255)]
    public string? FromAddress { get; set; }

    [StringLength(255)]
    public string? ToAddress { get; set; }

    [StringLength(50)]
    public string? HouseType { get; set; }

    public int? RoomsCount { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? MoveDate { get; set; }

    public int StatusId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("MovingRequests")]
    public virtual ApplicationUser Customer { get; set; } = null!;

    [InverseProperty("MovingRequest")]
    public virtual ICollection<MovingOffer> MovingOffers { get; set; } = new List<MovingOffer>();

    [InverseProperty("MovingRequest")]
    public virtual ICollection<MovingStatusLog> MovingStatusLogs { get; set; } = new List<MovingStatusLog>();

    [ForeignKey("StatusId")]
    [InverseProperty("MovingRequests")]
    public virtual Code Status { get; set; } = null!;
}
