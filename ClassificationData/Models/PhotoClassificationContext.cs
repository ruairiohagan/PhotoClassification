using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClassificationData.Models;

public partial class PhotoClassificationContext : DbContext
{
    public PhotoClassificationContext()
    {
    }

    public PhotoClassificationContext(DbContextOptions<PhotoClassificationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Image>? Images { get; set; }

    public virtual DbSet<ImageFolder>? ImageFolders { get; set; }

    public virtual DbSet<Setting>? Settings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite($"Data Source={DBController.DBFile}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImagePath);

            entity.ToTable("images");

            entity.Property(e => e.ImagePath).HasColumnName("imagePath");
            entity.Property(e => e.ClassificationMs)
                .HasColumnType("INT")
                .HasColumnName("classificationMS");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DescriptionDate).HasColumnName("descriptionDate");
            entity.Property(e => e.ImageDate).HasColumnName("imageDate");
            entity.Property(e => e.ModelUsed).HasColumnName("modelUsed");
        });

        modelBuilder.Entity<ImageFolder>(entity =>
        {
            entity.HasKey(e => e.FolderPath);

            entity.ToTable("imageFolders");

            entity.Property(e => e.FolderPath).HasColumnName("folderPath");
            entity.Property(e => e.Recursive)
                .HasColumnType("INT")
                .HasColumnName("recursive");
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.ToTable("settings");

            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
