using N5Challenge.Api.Infraestructure.Entities;
using N5Challenge.Api.Infraestructure.Indexables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.Constants;

public static class SeedDataConstants
{
    public static readonly List<PermissionTypeDB> PermissionTypesDB =
    [
        new PermissionTypeDB { Id = 1, Description = "Permiso 1" },
        new PermissionTypeDB { Id = 2, Description = "Permiso 2" },
        new PermissionTypeDB { Id = 3, Description = "Permiso 3" }
    ];

    public static readonly List<PermissionDB> PermissionsDB =
    [
        new PermissionDB
        {
            Id = 1,
            EmployeeLastName = "Tolosa",
            EmployeeFirstName = "Adam",
            PermissionTypeId = 1,
            Date = new DateTime(2025, 6, 10)
        },
        new PermissionDB
        {
            Id = 2,
            EmployeeLastName = "Adam",
            EmployeeFirstName = "Tolosa",
            PermissionTypeId = 2,
            Date = new DateTime(2025, 6, 10)
        }
    ];

    public static readonly List<IndexablePermissionType> IndexablePermissionTypes =
    [
        new IndexablePermissionType { Id = "1", Description = "Permiso 1" },
        new IndexablePermissionType { Id = "2", Description = "Permiso 2" },
        new IndexablePermissionType { Id = "3", Description = "Permiso 3" }
    ];

    public static readonly List<IndexablePermission> IndexablePermissions =
    [
        new IndexablePermission
        {
            Id = "1",
            EmployeeLastName = "Tolosa",
            EmployeeFirstName = "Adam",
            PermissionTypeId = 1,
            Date = new DateTime(2025, 6, 10)
        },
        new IndexablePermission
        {
            Id = "2",
            EmployeeLastName = "Adam",
            EmployeeFirstName = "Tolosa",
            PermissionTypeId = 2,
            Date = new DateTime(2025, 6, 10)
        }
    ];
}