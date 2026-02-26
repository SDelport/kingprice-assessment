using King_Price_Assessment.Interfaces;
using King_Price_Assessment.Models;
using King_Price_Assessment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace King_Price_Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IGroupService groupService;

        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        [HttpGet]
        [Route("get-groups")]
        public async Task<List<Group>> GetGroupsAsync()
        {
            return await groupService.GetGroupsAsync();
        }

        [HttpGet]
        [Route("get-group-by-id")]
        public async Task<Group> GetGroupByIdAsync(Guid groupID)
        {
            return await groupService.GetGroupByIdAsync(groupID);
        }

        [HttpPost]
        [Route("add-group")]
        public async Task<Group> AddGroupAsync(Group group)
        {
            return await groupService.AddGroupAsync(group);
        }

        [HttpPut]
        [Route("update-group")]
        public async Task<Group> UpdatePermissionAsync(Group group)
        {
            return await groupService.UpdateGroupAsync(group);
        }

        [HttpDelete]
        [Route("delete-group")]
        public async Task<bool> DeleteGroupAsync(Guid groupID)
        {
            return await groupService.DeleteGroupAsync(groupID);
        }

        [HttpGet]
        [Route("assign-permission")]
        public async Task<bool> AssignPermission(Guid groupID, Guid permissionID)
        {
            return await groupService.AssignPermissionToGroup(groupID, permissionID);
        }

        [HttpGet]
        [Route("unassign-permission")]
        public async Task<bool> UnassignPermission(Guid groupID, Guid permissionID)
        {
            return await groupService.UnassignPermissionFromGroup(groupID, permissionID);
        }
        [HttpGet]
        [Route("assign-user")]
        public async Task<bool> AssignUser(Guid groupID, Guid userID)
        {
            return await groupService.AssignUserToGroup(groupID, userID);
        }

        [HttpGet]
        [Route("unassign-user")]
        public async Task<bool> UnassignUser(Guid groupID, Guid userID)
        {
            return await groupService.UnassignUserFromGroup(groupID, userID);
        }
    }
}
