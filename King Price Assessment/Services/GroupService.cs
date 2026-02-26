using King_Price_Assessment.Interfaces;
using King_Price_Assessment.Models;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace King_Price_Assessment.Services
{
    public class GroupService : IGroupService
    {
        private UserContext userContext;

        public GroupService(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public async Task<Group> AddGroupAsync(Group group)
        {
            if (group.GroupId != Guid.Empty)
                throw new Exception("Group cannot have primary key defined when added.");

            if (string.IsNullOrWhiteSpace(group.Name))
                throw new Exception("Group cannot have an empty name.");

            await userContext.Groups.AddAsync(group);
            await userContext.SaveChangesAsync();

            return group;
        }

        public async Task<bool> AssignPermissionToGroup(Guid groupID, Guid permissionID)
        {
            var group = await userContext.Groups.Include(group => group.Permissions).FirstOrDefaultAsync(group => group.GroupId == groupID);

            if (group == null)
                throw new Exception("Cannot find group.");

            var permission = await userContext.Permissions.FirstOrDefaultAsync(permission=>permission.PermissionId == permissionID);

            if (permission == null)
                throw new Exception("Cannot find permission.");

            if (group.Permissions.Select(permission => permission.PermissionId).Contains(permission.PermissionId))
                throw new Exception("Permission already assigned.");

            group.Permissions.Add(permission);
            await userContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnassignPermissionFromGroup(Guid groupID, Guid permissionID)
        {
            var group = await userContext.Groups.Include(group=>group.Permissions).FirstOrDefaultAsync(group => group.GroupId == groupID);

            if (group == null)
                throw new Exception("Cannot find group.");

            var permission = await userContext.Permissions.FirstOrDefaultAsync(permission => permission.PermissionId == permissionID);

            if (permission == null)
                throw new Exception("Cannot find permission.");

            if (!group.Permissions.Select(permission => permission.PermissionId).Contains(permission.PermissionId))
                throw new Exception("Permission not assigned.");

            group.Permissions.Remove(permission);
            await userContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignUserToGroup(Guid groupID, Guid userID)
        {
            var group = await userContext.Groups.Include(group => group.Users).FirstOrDefaultAsync(group => group.GroupId == groupID);

            if (group == null)
                throw new Exception("Cannot find group.");

            var user = await userContext.Users.FirstOrDefaultAsync(user => user.UserId == userID);

            if (user == null)
                throw new Exception("Cannot find user.");

            if (group.Users.Select(user => user.UserId).Contains(user.UserId))
                throw new Exception("User already assigned.");

            group.Users.Add(user);
            await userContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteGroupAsync(Guid groupID)
        {
            if (groupID == Guid.Empty)
                throw new Exception("Must specify key to be deleted.");

            var group = await userContext.Groups.FirstOrDefaultAsync(group => group.GroupId == groupID);

            if (group == null)
                throw new Exception("Group cannot be found");

            userContext.Groups.Remove(group);
            await userContext.SaveChangesAsync();

            return true;
        }

        public async Task<Group> GetGroupByIdAsync(Guid groupID)
        {
            var group = await userContext.Groups.Include(group => group.Permissions).Include(group => group.Users).FirstOrDefaultAsync(group => group.GroupId == groupID);

            if (group == null)
                throw new Exception("Group cannot be found");

            return group;
        }

        public async Task<List<Group>> GetGroupsAsync()
        {
            return await userContext.Groups.ToListAsync();
        }


        public async Task<Group> UpdateGroupAsync(Group group)
        {
            if (group.GroupId == Guid.Empty)
                throw new Exception("Group must have primary key defined when update.");

            if (string.IsNullOrWhiteSpace(group.Name))
                throw new Exception("Group cannot have an empty name.");

            var dbGroup = userContext.Groups.FirstOrDefault(dbGroup => dbGroup.GroupId == group.GroupId);

            if (dbGroup == null)
                throw new Exception("Group cannot be found.");

            dbGroup.Name = group.Name;
            await userContext.SaveChangesAsync();

            return group;
        }

        public async Task<bool> UnassignUserFromGroup(Guid groupID, Guid userID)
        {
            var group = await userContext.Groups.Include(group => group.Users).FirstOrDefaultAsync(group => group.GroupId == groupID);

            if (group == null)
                throw new Exception("Cannot find group.");

            var user = await userContext.Users.FirstOrDefaultAsync(user => user.UserId == userID);

            if (user == null)
                throw new Exception("Cannot find user.");

            if (!group.Users.Select(user => user.UserId).Contains(user.UserId))
                throw new Exception("User not assigned.");

            group.Users.Remove(user);
            await userContext.SaveChangesAsync();

            return true;
        }
    }
}
