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

        modelBuilder.Entity<PermissionDB>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.HasOne(p => p.Type)
                  .WithMany(pt => pt.Permissions)
                  .HasForeignKey(p => p.PermissionTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PermissionTypeDB>(entity =>
        {
            entity.HasKey(pt => pt.Id);
        });

        modelBuilder.Entity<PermissionTypeDB>().HasData(
        new PermissionTypeDB { Id = 1, Description = "Permiso 1" },
        new PermissionTypeDB { Id = 2, Description = "Permiso 2" },
        new PermissionTypeDB { Id = 3, Description = "Permiso 3" });

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
                EmployeeLastName = "Adam",
                EmployeeFirstName = "Tolosa",
                PermissionTypeId = 2
            });
    }
}
