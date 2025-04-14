using Microsoft.EntityFrameworkCore;
using Payroll.Library;

namespace Payroll.Data.Ef;

public class PayrollDbContext(DbContextOptions<PayrollDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // UUID primary key config for CockroachDB
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees");
            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id")
                .HasColumnType("uuid");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Role).HasColumnName("role");
        });

        // Add any other entity configs here
    }
}
