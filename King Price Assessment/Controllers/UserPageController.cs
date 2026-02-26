using King_Price_Assessment.Models;
using King_Price_Assessment.Models.ViewModels;
using King_Price_Assessment.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace King_Price_Assessment.Controllers
{
    [Route("user")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserPageController : Controller
    {
        private ApiService apiService;
        public UserPageController(ApiService apiService)
        {
            this.apiService = apiService;
        }

        [Route("")]
        public async Task<IActionResult> Index()
        {
            var response = await this.apiService.GetJsonAsync<List<User>>("api/user/get-users");

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
        public async Task<IActionResult> Add(User user)
        {
            await apiService.PostAsync<User>("api/user/add-user", user);

            return Redirect("/user");
        }

        [Route("edit/{userID}")]
        public async Task<IActionResult> Edit(Guid userID)
        {
            var user = await apiService.GetJsonAsync<User>($"api/user/get-user-by-id?userID={userID}");

            var availableGroups = await this.apiService.GetJsonAsync<List<Group>>("api/group/get-groups");

            availableGroups = availableGroups.Where(group => !user.Groups.Select(userGroup => userGroup.GroupId).ToList().Contains(group.GroupId)).ToList();

            UserEditViewModel viewModel = new UserEditViewModel()
            {
                User = user,
                AvailableGroups = availableGroups
            };

            return View("Edit", viewModel);
        }

        [Route("edit/{userID}")]
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            await apiService.PutAsync<User>("api/user/update-user", user);

            return Redirect("/user");
        }

        [Route("delete/{userID}")]
        public async Task<IActionResult> Delete(Guid userID)
        {
            await apiService.DeleteAsync($"api/user/delete-user?userID={userID}");

            return Redirect("/user");
        }

        [Route("assign-group")]
        public async Task<IActionResult> AssignGroup(Guid groupID, Guid userID)
        {
            await apiService.GetAsync($"api/group/assign-user?groupID={groupID}&userID={userID}");

            return Redirect($"/user/edit/{userID}");
        }

        [Route("unassign-group")]
        public async Task<IActionResult> UnassignGroup(Guid groupID, Guid userID)
        {
            await apiService.GetAsync($"api/group/unassign-user?groupID={groupID}&userID={userID}");

            return Redirect($"/user/edit/{userID}");
        }
    }
}
