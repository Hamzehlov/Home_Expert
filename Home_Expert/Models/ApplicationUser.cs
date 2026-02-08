using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Home_Expert.Models; 

    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // حقول OTP الجديدة
        [MaxLength(6)]
        public string? OTPCode { get; set; }

        public DateTime? OTPExpiry { get; set; }

        public int OTPAttempts { get; set; }

        public DateTime? OTPGeneratedAt { get; set; }

        // خاصية محسوبة
        public string FullName => $"{FirstName} {LastName}";





    [InverseProperty("Customer")]
    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    [InverseProperty("Customer")]
    public virtual ICollection<EscrowTransaction> EscrowTransactions { get; set; } = new List<EscrowTransaction>();

    [InverseProperty("Customer")]
    public virtual ICollection<KitchenMeasurement> KitchenMeasurements { get; set; } = new List<KitchenMeasurement>();

    [InverseProperty("Sender")]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    [InverseProperty("Customer")]
    public virtual ICollection<MovingRequest> MovingRequests { get; set; } = new List<MovingRequest>();

    [InverseProperty("Customer")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("Customer")]
    public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();

    [InverseProperty("User")]
    public virtual ICollection<UserLocation> UserLocations { get; set; } = new List<UserLocation>();

    [InverseProperty("User")]
    public virtual ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();

    [InverseProperty("User")]
    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();


}


