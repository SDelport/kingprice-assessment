using King_Price_Assessment.Models;
using King_Price_Assessment.Models.ViewModels;
using King_Price_Assessment.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace King_Price_Assessment.Controllers
{
    [Route("group")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class GroupPageController : Controller
    {
        private ApiService apiService;
        public GroupPageController(ApiService apiService)
        {
            this.apiService = apiService;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var response = await this.apiService.GetJsonAsync<List<Group>>("api/group/get-groups");

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
        public async Task<IActionResult> Add(Group group)
        {
            await apiService.PostAsync<Group>("api/group/add-group", group);

            return Redirect("/group");
        }

        [Route("edit/{groupID}")]
        public async Task<IActionResult> Edit(Guid groupID)
        {
            var group = await apiService.GetJsonAsync<Group>($"api/group/get-group-by-id?groupID={groupID}");

            var availablePermissions = await this.apiService.GetJsonAsync<List<Permission>>("api/permission/get-permissions");

            availablePermissions = availablePermissions.Where(permission => !group.Permissions.Select(groupPermission=>groupPermission.PermissionId).ToList().Contains(permission.PermissionId)).ToList();

            GroupEditViewModel viewModel = new GroupEditViewModel()
            {
                Group = group,
                AvailablePermissions = availablePermissions
            };

            return View("Edit", viewModel);
        }

        [Route("edit/{groupID}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Group group)
        {
            await apiService.PutAsync<Group>("api/group/update-group", group);

            return Redirect("/group");
        }

        [Route("delete/{groupID}")]
        public async Task<IActionResult> Delete(Guid groupID)
        {
            await apiService.DeleteAsync($"api/group/delete-group?groupID={groupID}");

            return Redirect("/group");
        }

        [Route("assign-permission")]
        public async Task<IActionResult> AssignPermission(Guid groupID, Guid permissionID)
        {
            await apiService.GetAsync($"api/group/assign-permission?groupID={groupID}&permissionID={permissionID}");

            return Redirect($"/group/edit/{groupID}");
        }

        [Route("unassign-permission")]
        public async Task<IActionResult> UnassignPermission(Guid groupID, Guid permissionID)
        {
            await apiService.GetAsync($"api/group/unassign-permission?groupID={groupID}&permissionID={permissionID}");

            return Redirect($"/group/edit/{groupID}");
        }
    }
}
