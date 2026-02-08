using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models;

public partial class Chat
{
    [Key]
    public int Id { get; set; }

    public int? OrderId { get; set; }

    [StringLength(450)]
    public string CustomerId { get; set; } = null!;

    public int VendorId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Chats")]
    public virtual ApplicationUser Customer { get; set; } = null!;

    [InverseProperty("Chat")]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    [ForeignKey("VendorId")]
    [InverseProperty("Chats")]
    public virtual Vendor Vendor { get; set; } = null!;
}
