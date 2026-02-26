using King_Price_Assessment.Models;
using King_Price_Assessment.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security;

namespace King_Price_Assessment.Controllers
{
    [Route("permission")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PermissionPageController : Controller
    {
        private ApiService apiService;
        public PermissionPageController(ApiService apiService)
        {
            this.apiService = apiService;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var response = await this.apiService.GetJsonAsync<List<Permission>>("api/permission/get-permissions");

            if (response == null)
                return Redirect("/error");

            return View(response);
        }

        [Route("add")]
        public async Task<IActionResult> Add()
        {
            return View("Edit");
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> Add(Permission permission)
        {
            await apiService.PostAsync<Permission>("api/permission/add-permission", permission);

            return Redirect("/permission");
        }

        [Route("edit/{permissionID}")]
        public async Task<IActionResult> Edit(Guid permissionID)
        {
            var permission = await apiService.GetJsonAsync<Permission>($"api/permission/get-permission-by-id?permissionID={permissionID}");

            return View("Edit", permission);
        }

        [Route("edit/{permissionID}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Permission permission)
        {
            await apiService.PutAsync<Permission>("api/permission/update-permission", permission);

            return Redirect("/permission");
        }

        [Route("delete/{permissionID}")]
        public async Task<IActionResult> Delete(Guid permissionID)
        {
            await apiService.DeleteAsync($"api/permission/delete-permission?permissionID={permissionID}");

            return Redirect("/permission");
        }
    }
}
