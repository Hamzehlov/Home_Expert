using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class Message
{
    [Key]
    public int Id { get; set; }

    public int ChatId { get; set; }

    [StringLength(450)]
    public string SenderId { get; set; } = null!;

    [Column("Message")]
    public string? Message1 { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SentAt { get; set; }

    [ForeignKey("ChatId")]
    [InverseProperty("Messages")]
    public virtual Chat Chat { get; set; } = null!;

    [ForeignKey("SenderId")]
    [InverseProperty("Messages")]
    public virtual ApplicationUser Sender { get; set; } = null!;
}
