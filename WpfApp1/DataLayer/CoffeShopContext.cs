using System;
using System.Collections.Generic;
using CoffeShopApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeShopApplication.DataLayer;

public partial class CoffeShopContext : DbContext
{
    public CoffeShopContext()
    {
    }

    public CoffeShopContext(DbContextOptions<CoffeShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderFullInfoView> OrderFullInfoViews { get; set; }

    public virtual DbSet<OrderedProduct> OrderedProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<ShopProduct> ShopProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-Matvey;Initial Catalog=CoffeMarket;User ID=dbUser;Password=dbUser;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK_Order");

            entity.Property(e => e.Cost).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Options).HasMaxLength(50);
            entity.Property(e => e.OrderDatetime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Принят");

            entity.HasOne(d => d.IdShopNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdShop)
                .HasConstraintName("FK_Order_Shop");
        });

        modelBuilder.Entity<OrderFullInfoView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("OrderFullInfoView");

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Composition).HasMaxLength(4000);
            entity.Property(e => e.Cost).HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Customer).HasMaxLength(101);
            entity.Property(e => e.Options).HasMaxLength(50);
            entity.Property(e => e.OrderDateTime).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<OrderedProduct>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable(tb =>
                {
                    tb.HasTrigger("TrUpdateOrderCost");
                    tb.HasTrigger("TrUpdateShopProductOnInsert");
                });

            entity.Property(e => e.Count).HasDefaultValue((byte)1);

            entity.HasOne(d => d.IdOrderNavigation).WithMany()
                .HasForeignKey(d => d.IdOrder)
                .HasConstraintName("FK_OrderedProduct_Order");

            entity.HasOne(d => d.IdProductNavigation).WithMany()
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK_OrderedProduct_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK_Product");

            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Image)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(6, 2)");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.IdShop).HasName("PK_Shop");

            entity.HasIndex(e => e.Address, "UQ_Address").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ShopProduct>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Count).HasDefaultValue((byte)100);

            entity.HasOne(d => d.IdProductNavigation).WithMany()
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK_ShopProduct_Product");

            entity.HasOne(d => d.IdShopNavigation).WithMany()
                .HasForeignKey(d => d.IdShop)
                .HasConstraintName("FK_ShopProduct_Shop");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
