using Microsoft.EntityFrameworkCore;

public partial class DatabaseContext : DbContext {

    public DatabaseContext (DbContextOptions<DatabaseContext> options) : base(options) { }

    public virtual DbSet<Customer> Customer {get; set;}
    public virtual DbSet<ProductCategory> ProductCategory {get; set;}
    public virtual DbSet<Product> Product {get; set;}
    public virtual DbSet<SalesOrder> SalesOrder {get; set;}
    public virtual DbSet<SalesOrderItem> SalesOrderItem {get; set;}

    public virtual DbSet<Stock> Stock{get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasKey(e => e.CustomerId);
        modelBuilder.Entity<Customer>().Property(p => p.Name).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Customer>().Property(p => p.City).HasMaxLength(30).IsRequired();
        modelBuilder.Entity<Customer>().Property(p => p.State).HasMaxLength(30).IsRequired();
        modelBuilder.Entity<Customer>().Property(p => p.Latitude).HasPrecision(11, 3).IsRequired();
        modelBuilder.Entity<Customer>().Property(p => p.Longitude).HasPrecision(11, 3).IsRequired();
        modelBuilder.Entity<Customer>().HasMany<SalesOrder>().WithOne().HasForeignKey(e => e.CustomerId);

        modelBuilder.Entity<ProductCategory>().HasKey(e => e.ProductCategoryId);
        modelBuilder.Entity<ProductCategory>().Property(p => p.Name).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<ProductCategory>().HasMany<Product>().WithOne().HasForeignKey(e => e.ProductCategoryId);

        modelBuilder.Entity<Product>().HasKey(e => e.ProductId);
        modelBuilder.Entity<Product>().Property(p => p.ProductCategoryId).IsRequired();
        modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Product>().Property(p => p.UnitPrice).HasPrecision(11, 5).IsRequired();
        modelBuilder.Entity<Product>().HasMany<SalesOrderItem>().WithOne().HasForeignKey(e => e.ProductId);

        modelBuilder.Entity<SalesOrder>().HasKey(e => e.OrderId);
        modelBuilder.Entity<SalesOrder>().Property(p => p.CustomerId).IsRequired();
        modelBuilder.Entity<SalesOrder>().Property(p => p.OrderDate).IsRequired();
        modelBuilder.Entity<SalesOrder>().Property(p => p.EstimatedDeliveryDate);
        modelBuilder.Entity<SalesOrder>().Property(p => p.Status).HasMaxLength(20).IsRequired();
        modelBuilder.Entity<SalesOrder>().HasMany<SalesOrderItem>().WithOne().HasForeignKey(e => e.ProductId);

        modelBuilder.Entity<SalesOrderItem>().HasKey(e => new {e.OrderId, e.ProductId});
        modelBuilder.Entity<SalesOrderItem>().Property(p => p.Quantity).IsRequired();
        modelBuilder.Entity<SalesOrderItem>().Property(p => p.UnitPrice).HasPrecision(11,5).IsRequired();


        modelBuilder.Entity<Stock>().HasKey(e => e.StockId);
        modelBuilder.Entity<Stock>().Property(p => p.ProductId).IsRequired();
        modelBuilder.Entity<Stock>().Property(p => p.Quantity).IsRequired();
        modelBuilder.Entity<Stock>().HasOne(p => p.Product).WithMany().HasForeignKey(e => e.ProductId);


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}