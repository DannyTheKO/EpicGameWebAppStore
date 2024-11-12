using System.Text.Json;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

// The "EpicGameDBContext.cs" file acts as a bridge between the application and the database, 
// providing an abstraction layer for performing database operations and managing the entities 
// in the application.

// Domain

namespace DataAccess.EpicGame;

public partial class EpicGameDbContext : DbContext
{
    public EpicGameDbContext()
    {
    }

    public EpicGameDbContext(DbContextOptions<EpicGameDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountGame> AccountGames { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Cartdetail> Cartdetails { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Paymentmethod> Paymentmethods { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost;User=root;Password=root;Database=epicgamewebapp");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PRIMARY");

            entity.ToTable("account");

            entity.HasIndex(e => e.RoleId, "FK_Account_Role_idx");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(45);
            entity.Property(e => e.Username).HasMaxLength(45);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<AccountGame>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("accountgame");

            entity.HasIndex(e => e.AccountId, "FK_AccountGame_Account_INDEX");

            entity.HasIndex(e => e.GameId, "FK_AccountGame_Game_INDEX");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.GameId).HasColumnName("GameID");

            entity.HasOne(d => d.Account).WithMany()
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_AccountGame_Account");

            entity.HasOne(d => d.Game).WithMany()
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_AccountGame_Game");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PRIMARY");

            entity.ToTable("cart");

            entity.HasIndex(e => e.AccountId, "AccountID_INDEX");

            entity.HasIndex(e => e.PaymentMethodId, "PaymentMethodID_INDEX");

            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");
            entity.Property(e => e.TotalAmount).HasPrecision(8);

            entity.HasOne(d => d.Account).WithMany(p => p.Carts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Cart_Account");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Carts)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK_Cart_PaymentMethod");
        });

        modelBuilder.Entity<Cartdetail>(entity =>
        {
            entity.HasKey(e => e.CartDetailId).HasName("PRIMARY");

            entity.ToTable("cartdetail");

            entity.HasIndex(e => e.CartId, "CartID_INDEX");

            entity.HasIndex(e => e.GameId, "GameID_INDEX");

            entity.Property(e => e.CartDetailId).HasColumnName("CartDetailID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.Discount).HasPrecision(5);
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.Price).HasPrecision(8);

            entity.HasOne(d => d.Cart).WithMany(p => p.Cartdetails)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK_CartDetail_Cart");

            entity.HasOne(d => d.Game).WithMany(p => p.Cartdetails)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_CartDetail_Game");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.DiscountId).HasName("PRIMARY");

            entity.ToTable("discount");

            entity.HasIndex(e => e.GameId, "GameID_INDEX");

            entity.Property(e => e.DiscountId).HasColumnName("DiscountID");
            entity.Property(e => e.Code).HasMaxLength(45);
            entity.Property(e => e.EndOn).HasColumnType("datetime");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.Percent).HasPrecision(5);
            entity.Property(e => e.StartOn).HasColumnType("datetime");

            entity.HasOne(d => d.Game).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_Discount_Game");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PRIMARY");

            entity.ToTable("game");

            entity.HasIndex(e => e.GenreId, "GenreID_INDEX");

            entity.HasIndex(e => e.PublisherId, "PublisherID_INDEX");

            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.Price).HasPrecision(8);
            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Rating).HasPrecision(2, 1);
            entity.Property(e => e.Release).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Genre).WithMany(p => p.Games)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK_Game_Genre");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Games)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK_Game_Publisher");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PRIMARY");

            entity.ToTable("genre");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Paymentmethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PRIMARY");

            entity.ToTable("paymentmethod");

            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");
            entity.Property(e => e.Name).HasMaxLength(45);
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("PRIMARY");

            entity.ToTable("publisher");

            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(255);
            entity.Property(e => e.Website).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Permission)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null))
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}