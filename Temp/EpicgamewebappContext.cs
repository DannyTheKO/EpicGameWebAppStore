using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EpicGameWebAppStore.Temp;

public partial class EpicgamewebappContext : DbContext
{
    public EpicgamewebappContext()
    {
    }

    public EpicgamewebappContext(DbContextOptions<EpicgamewebappContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Imagegame> Imagegames { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("Server=localhost;User=root;Password=root;Database=epicgamewebapp");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
        });

        modelBuilder.Entity<Imagegame>(entity =>
        {
            entity.HasKey(e => e.ImageGameId).HasName("PRIMARY");

            entity.ToTable("imagegame");

            entity.HasIndex(e => e.GameId, "GameID_INDEX");

            entity.Property(e => e.ImageGameId).HasColumnName("ImageGameID");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(255);
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.ImageType)
                .HasDefaultValueSql("'Thumbnail'")
                .HasColumnType("enum('Banner','Thumbnail','Screenshot','Background')");

            entity.HasOne(d => d.Game).WithMany(p => p.Imagegames)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_Image_Game");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
