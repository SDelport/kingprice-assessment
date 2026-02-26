using King_Price_Assessment.Interfaces;
using King_Price_Assessment.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace King_Price_Assessment.Services
{
    public class PermissionService : IPermissionService
    {
        private UserContext userContext;

        public PermissionService(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public async Task<Permission> AddPermissionAsync(Permission permission)
        {
            if (permission.PermissionId != Guid.Empty)
                throw new Exception("Permission cannot have primary key defined when added.");

            if (string.IsNullOrWhiteSpace(permission.Name))
                throw new Exception("Permission cannot have an empty name.");

            await userContext.Permissions.AddAsync(permission);
            await userContext.SaveChangesAsync();

            return permission;
        }

        public async Task<bool> DeletePermissionAsync(Guid permissionID)
        {
            if (permissionID == Guid.Empty)
                throw new Exception("Must specify key to be deleted.");

            var permission = await userContext.Permissions.FirstOrDefaultAsync(permission => permission.PermissionId == permissionID);

            if (permission == null)
                throw new Exception("Permission cannot be found");

            userContext.Permissions.Remove(permission);
            await userContext.SaveChangesAsync();

            return true;
        }

        public async Task<Permission> GetPermissionByIdAsync(Guid permissionID)
        {
            var permission = await userContext.Permissions.Include(permission => permission.Groups).FirstOrDefaultAsync(permission => permission.PermissionId == permissionID);

            if (permission == null)
                throw new Exception("Group cannot be found");

            return permission;
        }

        public async Task<List<Permission>> GetPermissionsAsync()
        {
            return await userContext.Permissions.ToListAsync();
        }

        public async Task<Permission> UpdatePermissionAsync(Permission permission)
        {

            if (permission.PermissionId == Guid.Empty)
                throw new Exception("Permission must have primary key defined when update.");

            if (string.IsNullOrWhiteSpace(permission.Name))
                throw new Exception("Permission cannot have an empty name.");

            var dbPermission = userContext.Permissions.FirstOrDefault(dbPermission => dbPermission.PermissionId == permission.PermissionId);

            if (dbPermission == null)
                throw new Exception("Permission cannot be found.");

            dbPermission.Name = permission.Name;
            await userContext.SaveChangesAsync();

            return permission;
        }

    }
}
