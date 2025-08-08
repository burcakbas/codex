using EmployeesApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace EmployeesApi.Migrations
{
    [DbContext(typeof(EmployeesDbContext))]
    partial class EmployeesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("EmployeesApi.Models.Employee", b =>
            {
                b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int");
                b.Property<string>("SSN").IsRequired().HasMaxLength(11).HasColumnType("nvarchar(11)");
                b.Property<string>("FirstName").IsRequired().HasMaxLength(64).HasColumnType("nvarchar(64)");
                b.Property<string>("LastName").IsRequired().HasMaxLength(64).HasColumnType("nvarchar(64)");
                b.Property<decimal>("Salary").HasColumnType("decimal(18,2)");
                b.HasKey("Id");
                b.HasIndex("SSN").IsUnique();
                b.ToTable("Employees");
            });
        }
    }
}
