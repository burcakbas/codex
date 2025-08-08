using EmployeesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApi.Data;

public class EmployeesDbContext : DbContext
{
    public EmployeesDbContext(DbContextOptions<EmployeesDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.SSN).HasMaxLength(11).IsRequired();
            entity.HasIndex(e => e.SSN).IsUnique();
            entity.Property(e => e.FirstName).HasMaxLength(64).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Salary).HasColumnType("decimal(18,2)");
        });
    }
}
