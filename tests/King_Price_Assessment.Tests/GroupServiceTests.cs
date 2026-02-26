using King_Price_Assessment.Models;
using King_Price_Assessment.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace King_Price_Assessment.Tests
{
    public class GroupServiceTests
    {
        private UserContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new UserContext(options);
        }

        [Fact]
        public async Task AddGroupAsync_AddsGroup()
        {
            using var context = CreateContext("AddGroupAddsGroup");
            var service = new GroupService(context);

            var group = new Group { Name = "Admins" };
            var added = await service.AddGroupAsync(group);

            var fromDb = await context.Groups.FirstOrDefaultAsync(g => g.Name == "Admins");
            Assert.NotNull(fromDb);
            Assert.Equal("Admins", fromDb.Name);
        }

        [Fact]
        public async Task AddGroupAsync_Throws_WhenIdSet()
        {
            using var context = CreateContext("AddGroupThrowsIdSet");
            var service = new GroupService(context);

            var group = new Group { GroupId = Guid.NewGuid(), Name = "G" };
            await Assert.ThrowsAsync<Exception>(() => service.AddGroupAsync(group));
        }

        [Fact]
        public async Task AddGroupAsync_Throws_WhenNameEmpty()
        {
            using var context = CreateContext("AddGroupThrowsNameEmpty");
            var service = new GroupService(context);

            var group = new Group { Name = "" };
            await Assert.ThrowsAsync<Exception>(() => service.AddGroupAsync(group));
        }

        [Fact]
        public async Task AssignUserToGroup_AssignsUser()
        {
            using var context = CreateContext("AssignUserToGroupAssignsUser");
            var user = new User { UserId = Guid.NewGuid(), Name = "Stefan" };
            var group = new Group { GroupId = Guid.NewGuid(), Name = "G1" };
            context.Users.Add(user);
            context.Groups.Add(group);
            await context.SaveChangesAsync();

            var service = new GroupService(context);
            var result = await service.AssignUserToGroup(group.GroupId, user.UserId);

            Assert.True(result);
            var fromDb = await context.Groups.Include(g => g.Users).FirstOrDefaultAsync(g => g.GroupId == group.GroupId);
            Assert.Contains(fromDb.Users, u => u.UserId == user.UserId);
        }

        [Fact]
        public async Task AssignUserToGroup_Throws_WhenAlreadyAssigned()
        {
            using var context = CreateContext("AssignUserToGroupThrowsAlreadyAssigned");
            var user = new User { UserId = Guid.NewGuid(), Name = "Martin" };
            var group = new Group { GroupId = Guid.NewGuid(), Name = "G2" };
            group.Users.Add(user);
            context.Users.Add(user);
            context.Groups.Add(group);
            await context.SaveChangesAsync();

            var service = new GroupService(context);
            await Assert.ThrowsAsync<Exception>(() => service.AssignUserToGroup(group.GroupId, user.UserId));
        }

        [Fact]
        public async Task UnassignUserFromGroup_RemovesUser()
        {
            using var context = CreateContext("UnassignUserFromGroupRemovesUser");
            var user = new User { UserId = Guid.NewGuid(), Name = "Stefan" };
            var group = new Group { GroupId = Guid.NewGuid(), Name = "G3" };
            group.Users.Add(user);
            context.Users.Add(user);
            context.Groups.Add(group);
            await context.SaveChangesAsync();

            var service = new GroupService(context);
            var result = await service.UnassignUserFromGroup(group.GroupId, user.UserId);

            Assert.True(result);
            var fromDb = await context.Groups.Include(g => g.Users).FirstOrDefaultAsync(g => g.GroupId == group.GroupId);
            Assert.DoesNotContain(fromDb.Users, u => u.UserId == user.UserId);
        }

        [Fact]
        public async Task UnassignUserFromGroup_Throws_WhenNotAssigned()
        {
            using var context = CreateContext("UnassignUserFromGroupThrowsNotAssigned");
            var user = new User { UserId = Guid.NewGuid(), Name = "Martin" };
            var group = new Group { GroupId = Guid.NewGuid(), Name = "G4" };
            context.Users.Add(user);
            context.Groups.Add(group);
            await context.SaveChangesAsync();

            var service = new GroupService(context);
            await Assert.ThrowsAsync<Exception>(() => service.UnassignUserFromGroup(group.GroupId, user.UserId));
        }

        [Fact]
        public async Task AssignPermissionToGroup_AssignsPermission()
        {
            using var context = CreateContext("AssignPermissionToGroupAssignsPermission");
            var permission = new Permission { PermissionId = Guid.NewGuid(), Name = "P1" };
            var group = new Group { GroupId = Guid.NewGuid(), Name = "G5" };
            context.Permissions.Add(permission);
            context.Groups.Add(group);
            await context.SaveChangesAsync();

            var service = new GroupService(context);
            var result = await service.AssignPermissionToGroup(group.GroupId, permission.PermissionId);

            Assert.True(result);
            var fromDb = await context.Groups.Include(g => g.Permissions).FirstOrDefaultAsync(g => g.GroupId == group.GroupId);
            Assert.Contains(fromDb.Permissions, p => p.PermissionId == permission.PermissionId);
        }

        [Fact]
        public async Task UnassignPermissionFromGroup_RemovesPermission()
        {
            using var context = CreateContext("UnassignPermissionFromGroupRemovesPermission");
            var permission = new Permission { PermissionId = Guid.NewGuid(), Name = "P2" };
            var group = new Group { GroupId = Guid.NewGuid(), Name = "G6" };
            group.Permissions.Add(permission);
            context.Permissions.Add(permission);
            context.Groups.Add(group);
            await context.SaveChangesAsync();

            var service = new GroupService(context);
            var result = await service.UnassignPermissionFromGroup(group.GroupId, permission.PermissionId);

            Assert.True(result);
            var fromDb = await context.Groups.Include(g => g.Permissions).FirstOrDefaultAsync(g => g.GroupId == group.GroupId);
            Assert.DoesNotContain(fromDb.Permissions, p => p.PermissionId == permission.PermissionId);
        }
    }
}
