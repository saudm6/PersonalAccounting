using System.Reflection;
using PersonalAccounting.Application.Common.Interfaces;
using PersonalAccounting.Domain.Entities;
using PersonalAccounting.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PersonalAccounting.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<ReceiptItem> ReceiptItems => Set<ReceiptItem>();

    public DbSet<Receipt> Receipts => Set<Receipt>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Receipt>()
            .HasMany(r => r.ReceiptItems)
            .WithOne(ri => ri.Receipt)
            .HasForeignKey(ri => ri.ReceiptId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Receipt>().Property(r => r.ReceiptTotal)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0M);
        builder.Entity<ReceiptItem>().Property(ri => ri.ItemPrice)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0M);
        builder.Entity<ReceiptItem>().Property(ri => ri.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0M);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
