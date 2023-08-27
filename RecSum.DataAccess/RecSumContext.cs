using Microsoft.EntityFrameworkCore;
using RecSum.DataAccess.EntityConfigurations;
using RecSum.Domain.Entities;

namespace RecSum.DataAccess;

public sealed class RecSumContext : DbContext
{
    public RecSumContext(DbContextOptions<RecSumContext> options) : base(options)
    {
    }
    
    public DbSet<Invoice> Invoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}