using King_Price_Assessment.Interfaces;
using King_Price_Assessment.Models;
using King_Price_Assessment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace King_Price_Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private IPermissionService permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            this.permissionService = permissionService;
        }

        [HttpGet]
        [Route("get-permissions")]
        public async Task<List<Permission>> GetPermissionsAsync()
        {
            return await permissionService.GetPermissionsAsync();
        }

        [HttpGet]
        [Route("get-permission-by-id")]
        public async Task<Permission> GetPermissionByIdAsync(Guid permissionID)
        {
            return await permissionService.GetPermissionByIdAsync(permissionID);
        }


        [HttpPost]
        [Route("add-permission")]
        public async Task<Permission> AddPermissionAsync(Permission permission)
        {
            return await permissionService.AddPermissionAsync(permission);
        }

        [HttpPut]
        [Route("update-permission")]
        public async Task<Permission> UpdatePermissionAsync(Permission permission)
        {
            return await permissionService.UpdatePermissionAsync(permission);
        }

        [HttpDelete]
        [Route("delete-permission")]
        public async Task<bool> DeletePermissionAsync(Guid permissionID)
        {
            return await permissionService.DeletePermissionAsync(permissionID);
        }
    }
}
