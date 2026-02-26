using King_Price_Assessment.Models;
using System.ComponentModel;

namespace King_Price_Assessment.Interfaces
{
    public interface IGroupService
    {
        public Task<List<Group>> GetGroupsAsync();
        public Task<Group> GetGroupByIdAsync(Guid groupID);
        public Task<Group> AddGroupAsync(Group group);
        public Task<Group> UpdateGroupAsync(Group group);
        public Task<bool> DeleteGroupAsync(Guid groupID);
        public Task<bool> AssignUserToGroup(Guid groupID, Guid userID);
        public Task<bool> UnassignUserFromGroup(Guid groupID, Guid userID);
        public Task<bool> AssignPermissionToGroup(Guid groupID, Guid permissionID);
        public Task<bool> UnassignPermissionFromGroup(Guid groupID, Guid permissionID);

    }
}
