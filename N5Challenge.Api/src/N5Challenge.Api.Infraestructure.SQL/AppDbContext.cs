using Microsoft.EntityFrameworkCore;
using N5Challenge.Api.Infraestructure.SQL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.SQL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<PermissionDB> Permissions => Set<PermissionDB>();
    public DbSet<PermissionTypeDB> PermissionTypes => Set<PermissionTypeDB>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<PermissionTypeDB>().HasData(
        new PermissionTypeDB { Id = 1, Description = "unknown 1" },
        new PermissionTypeDB { Id = 2, Description = "unknown 2" });

        modelBuilder.Entity<PermissionDB>().HasData(
            new PermissionDB
            {
                Id = 1,
                EmployeeLastName = "Tolosa",
                EmployeeFirstName = "Adam",
                PermissionTypeId = 1
            },
            new PermissionDB
            {
                Id = 2,
                EmployeeLastName = "Spinelli",
                EmployeeFirstName = "Berenice",
                PermissionTypeId = 2
            });
    }
}
