using King_Price_Assessment.Models;
using System.ComponentModel;

namespace King_Price_Assessment.Interfaces
{
    public interface IPermissionService
    {
        public Task<List<Permission>> GetPermissionsAsync();
        public Task<Permission> GetPermissionByIdAsync(Guid permissionID);
        public Task<Permission> AddPermissionAsync(Permission permission);
        public Task<Permission> UpdatePermissionAsync(Permission permission);
        public Task<bool> DeletePermissionAsync(Guid permissionID);

    }
}
