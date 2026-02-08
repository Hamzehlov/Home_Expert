using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Home_Expert.Models;

public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


  

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Code> Codes { get; set; }

    public virtual DbSet<EscrowTransaction> EscrowTransactions { get; set; }

    public virtual DbSet<KitchenCostEstimate> KitchenCostEstimates { get; set; }

    public virtual DbSet<KitchenExport> KitchenExports { get; set; }

    public virtual DbSet<KitchenMeasurement> KitchenMeasurements { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MovingOffer> MovingOffers { get; set; }

    public virtual DbSet<MovingRequest> MovingRequests { get; set; }

    public virtual DbSet<MovingStatusLog> MovingStatusLogs { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceOffer> ServiceOffers { get; set; }

    public virtual DbSet<ServiceRequest> ServiceRequests { get; set; }

    public virtual DbSet<ServiceRequestIssue> ServiceRequestIssues { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<SubscriptionFeature> SubscriptionFeatures { get; set; }

    public virtual DbSet<UserLocation> UserLocations { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }

    public virtual DbSet<VendorMedium> VendorMedia { get; set; }

    public virtual DbSet<VendorSubscription> VendorSubscriptions { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07F6C92F4F");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK__Categorie__Paren__6FE99F9F");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chats__3214EC07BD2A9C65");

            entity.HasOne(d => d.Customer).WithMany(p => p.Chats)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chats_AspNetUsers");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Chats)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Chats__VendorId__2B0A656D");
        });

        modelBuilder.Entity<Code>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Code__3214EC076A41649B");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("FK__Code__ParentId__4BAC3F29");
        });

        modelBuilder.Entity<EscrowTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EscrowTr__3214EC07A113965E");

            entity.HasOne(d => d.Customer).WithMany(p => p.EscrowTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EscrowTransactions_AspNetUsers");

            entity.HasOne(d => d.Status).WithMany(p => p.EscrowTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EscrowTra__Statu__282DF8C2");

            entity.HasOne(d => d.Vendor).WithMany(p => p.EscrowTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EscrowTra__Vendo__2739D489");
        });

        modelBuilder.Entity<KitchenCostEstimate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KitchenC__3214EC076898FC0C");

            entity.HasOne(d => d.Measurement).WithMany(p => p.KitchenCostEstimates)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KitchenCo__Measu__1F98B2C1");

            entity.HasOne(d => d.Status).WithMany(p => p.KitchenCostEstimateStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KitchenCo__Statu__2180FB33");

            entity.HasOne(d => d.WoodType).WithMany(p => p.KitchenCostEstimateWoodTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KitchenCo__WoodT__208CD6FA");
        });

        modelBuilder.Entity<KitchenExport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KitchenE__3214EC078FDF1D81");

            entity.HasOne(d => d.Format).WithMany(p => p.KitchenExports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KitchenEx__Forma__1CBC4616");

            entity.HasOne(d => d.Measurement).WithMany(p => p.KitchenExports)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KitchenEx__Measu__1BC821DD");
        });

        modelBuilder.Entity<KitchenMeasurement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__KitchenM__3214EC07EFD06F71");

            entity.HasOne(d => d.Customer).WithMany(p => p.KitchenMeasurements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KitchenMeasurements_AspNetUsers");

            entity.HasOne(d => d.InputMode).WithMany(p => p.KitchenMeasurementInputModes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KitchenMe__Input__17F790F9");

            entity.HasOne(d => d.Unit).WithMany(p => p.KitchenMeasurementUnits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__KitchenMe__UnitI__18EBB532");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC07F75BAFDA");

            entity.Property(e => e.SentAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages).HasConstraintName("FK__Messages__ChatId__2DE6D218");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_AspNetUsers");
        });

        modelBuilder.Entity<MovingOffer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MovingOf__3214EC07945FABC7");

            entity.HasOne(d => d.MovingRequest).WithMany(p => p.MovingOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MovingOff__Movin__0B91BA14");

            entity.HasOne(d => d.Status).WithMany(p => p.MovingOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MovingOff__Statu__0D7A0286");

            entity.HasOne(d => d.Vendor).WithMany(p => p.MovingOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MovingOff__Vendo__0C85DE4D");
        });

        modelBuilder.Entity<MovingRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MovingRe__3214EC0754227BD5");

            entity.HasOne(d => d.Customer).WithMany(p => p.MovingRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovingRequests_AspNetUsers");

            entity.HasOne(d => d.Status).WithMany(p => p.MovingRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MovingReq__Statu__08B54D69");
        });

        modelBuilder.Entity<MovingStatusLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MovingSt__3214EC07FCBCDFA3");

            entity.HasOne(d => d.MovingRequest).WithMany(p => p.MovingStatusLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MovingSta__Movin__10566F31");

            entity.HasOne(d => d.Status).WithMany(p => p.MovingStatusLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MovingSta__Statu__114A936A");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07FFC3FE8C");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__73BA3083");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Products)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Vendor__72C60C4A");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reviews__3214EC07D3DC7560");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reviews_AspNetUsers");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__VendorI__14270015");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Services__3214EC072FE6C0E7");

            entity.HasOne(d => d.Category).WithMany(p => p.ServiceCategories).HasConstraintName("FK__Services__Catego__787EE5A0");

            entity.HasOne(d => d.Type).WithMany(p => p.ServiceTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Services__TypeId__778AC167");
        });

        modelBuilder.Entity<ServiceOffer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceO__3214EC0785FEA22C");

            entity.Property(e => e.InspectionRequired).HasDefaultValue(false);

            entity.HasOne(d => d.Request).WithMany(p => p.ServiceOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceOf__Reque__02FC7413");

            entity.HasOne(d => d.Status).WithMany(p => p.ServiceOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceOf__Statu__05D8E0BE");

            entity.HasOne(d => d.Vendor).WithMany(p => p.ServiceOffers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceOf__Vendo__03F0984C");
        });

        modelBuilder.Entity<ServiceRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceR__3214EC07F384800B");

            entity.HasOne(d => d.Customer).WithMany(p => p.ServiceRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceRequests_AspNetUsers");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceRe__Servi__7B5B524B");

            entity.HasOne(d => d.Status).WithMany(p => p.ServiceRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceRe__Statu__7C4F7684");
        });

        modelBuilder.Entity<ServiceRequestIssue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceR__3214EC0779D24B5D");

            entity.HasOne(d => d.IssueType).WithMany(p => p.ServiceRequestIssues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceRe__Issue__00200768");

            entity.HasOne(d => d.Request).WithMany(p => p.ServiceRequestIssues)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ServiceRe__Reque__7F2BE32F");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC07AD0B687E");
        });

        modelBuilder.Entity<SubscriptionFeature>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC07D62F0262");

            entity.Property(e => e.IsEnabled).HasDefaultValue(false);

            entity.HasOne(d => d.FeatureKey).WithMany(p => p.SubscriptionFeatures)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subscript__Featu__6C190EBB");

            entity.HasOne(d => d.Subscription).WithMany(p => p.SubscriptionFeatures)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subscript__Subsc__6B24EA82");
        });

        modelBuilder.Entity<UserLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserLoca__3214EC07ECF92CA2");

            entity.HasOne(d => d.User).WithMany(p => p.UserLocations).HasConstraintName("FK_UserLocations_AspNetUsers");
        });

        modelBuilder.Entity<Vendor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vendors__3214EC07395B6452");

            entity.Property(e => e.CompletedOrders).HasDefaultValue(0);
            entity.Property(e => e.RatingAvg).HasDefaultValue(0m);
            entity.Property(e => e.Verified).HasDefaultValue(false);
            entity.Property(e => e.YearsExperience).HasDefaultValue(0);

            entity.HasOne(d => d.ServiceType).WithMany(p => p.Vendors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Vendors__Service__5165187F");

            entity.HasOne(d => d.User).WithMany(p => p.Vendors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vendors_AspNetUsers");
        });

        modelBuilder.Entity<VendorMedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VendorMe__3214EC07524F27F1");

            entity.HasOne(d => d.MediaType).WithMany(p => p.VendorMedia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VendorMed__Media__571DF1D5");

            entity.HasOne(d => d.Vendor).WithMany(p => p.VendorMedia).HasConstraintName("FK__VendorMed__Vendo__5629CD9C");
        });

        modelBuilder.Entity<VendorSubscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VendorSu__3214EC0737DCCF91");

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Subscription).WithMany(p => p.VendorSubscriptions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VendorSub__Subsc__5CD6CB2B");

            entity.HasOne(d => d.Vendor).WithMany(p => p.VendorSubscriptions).HasConstraintName("FK__VendorSub__Vendo__5BE2A6F2");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Wallets__3214EC0779C4F957");

            entity.Property(e => e.Balance).HasDefaultValue(0m);

            entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wallets_AspNetUsers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
