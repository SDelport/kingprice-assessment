using King_Price_Assessment.Models;
using King_Price_Assessment.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace King_Price_Assessment.Tests
{
    public class UserServiceTests
    {
        private UserContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new UserContext(options);
        }

        [Fact]
        public async Task AddUserAsync_AddsUser()
        {
            using var context = CreateContext("AddUserAddsUser");
            var service = new UserService(context);

            var user = new User { Name = "Stefan" };
            var added = await service.AddUserAsync(user);

            var fromDb = await context.Users.FirstOrDefaultAsync(u => u.Name == "Stefan");
            Assert.NotNull(fromDb);
            Assert.Equal("Stefan", fromDb.Name);
        }

        [Fact]
        public async Task AddUserAsync_Throws_WhenIdSet()
        {
            using var context = CreateContext("AddUserThrowsIdSet");
            var service = new UserService(context);

            var user = new User { UserId = Guid.NewGuid(), Name = "Martin" };
            await Assert.ThrowsAsync<Exception>(() => service.AddUserAsync(user));
        }

        [Fact]
        public async Task AddUserAsync_Throws_WhenNameEmpty()
        {
            using var context = CreateContext("AddUserThrowsNameEmpty");
            var service = new UserService(context);

            var user = new User { Name = "" };
            await Assert.ThrowsAsync<Exception>(() => service.AddUserAsync(user));
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            using var context = CreateContext("GetUserByIdReturnsUser");
            var id = Guid.NewGuid();
            context.Users.Add(new User { UserId = id, Name = "Stefan" });
            await context.SaveChangesAsync();

            var service = new UserService(context);
            var user = await service.GetUserByIdAsync(id);

            Assert.Equal("Stefan", user.Name);
        }

        [Fact]
        public async Task GetUserByIdAsync_Throws_WhenNotFound()
        {
            using var context = CreateContext("GetUserByIdThrowsNotFound");
            var service = new UserService(context);

            await Assert.ThrowsAsync<Exception>(() => service.GetUserByIdAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesName()
        {
            using var context = CreateContext("UpdateUserUpdatesName");
            var id = Guid.NewGuid();
            context.Users.Add(new User { UserId = id, Name = "Stef" });
            await context.SaveChangesAsync();

            var service = new UserService(context);
            var updated = await service.UpdateUserAsync(new User { UserId = id, Name = "Stefan" });

            var fromDb = await context.Users.FindAsync(id);
            Assert.Equal("Stefan", fromDb.Name);
        }

        [Fact]
        public async Task UpdateUserAsync_Throws_WhenIdEmpty()
        {
            using var context = CreateContext("UpdateUserThrowsIdEmpty");
            var service = new UserService(context);

            await Assert.ThrowsAsync<Exception>(() => service.UpdateUserAsync(new User { UserId = Guid.Empty, Name = "X" }));
        }

        [Fact]
        public async Task UpdateUserAsync_Throws_WhenNameEmpty()
        {
            using var context = CreateContext("UpdateUserThrowsNameEmpty");
            var id = Guid.NewGuid();
            context.Users.Add(new User { UserId = id, Name = "Stefan" });
            await context.SaveChangesAsync();

            var service = new UserService(context);
            await Assert.ThrowsAsync<Exception>(() => service.UpdateUserAsync(new User { UserId = id, Name = "" }));
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUser()
        {
            using var context = CreateContext("DeleteUserDeletesUser");
            var id = Guid.NewGuid();
            context.Users.Add(new User { UserId = id, Name = "Martin" });
            await context.SaveChangesAsync();

            var service = new UserService(context);
            var result = await service.DeleteUserAsync(id);

            Assert.True(result);
            var fromDb = await context.Users.FindAsync(id);
            Assert.Null(fromDb);
        }

        [Fact]
        public async Task DeleteUserAsync_Throws_WhenIdEmpty()
        {
            using var context = CreateContext("DeleteUserThrowsIdEmpty");
            var service = new UserService(context);

            await Assert.ThrowsAsync<Exception>(() => service.DeleteUserAsync(Guid.Empty));
        }

        [Fact]
        public async Task GetUserCountAsync_ReturnsCount()
        {
            using var context = CreateContext("GetUserCountReturnsCount");
            context.Users.Add(new User { Name = "Stefan" });
            context.Users.Add(new User { Name = "Martin" });
            await context.SaveChangesAsync();

            var service = new UserService(context);
            var count = await service.GetUserCountAsync();

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetUserCountByGroupAsync_ReturnsCount()
        {
            using var context = CreateContext("GetUserCountByGroupReturnsCount");
            var user1 = new User { UserId = Guid.NewGuid(), Name = "U1" };
            var user2 = new User { UserId = Guid.NewGuid(), Name = "U2" };
            var groupId = Guid.NewGuid();
            var group = new Group { GroupId = groupId, Name = "G1" };
            group.Users.Add(user1);
            group.Users.Add(user2);

            context.Groups.Add(group);
            await context.SaveChangesAsync();

            var service = new UserService(context);
            var count = await service.GetUserCountByGroupAsync(groupId);

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetUserCountByGroupAsync_Throws_WhenGroupNotFound()
        {
            using var context = CreateContext("GetUserCountByGroupThrowsGroupNotFound");
            var service = new UserService(context);

            await Assert.ThrowsAsync<Exception>(() => service.GetUserCountByGroupAsync(Guid.NewGuid()));
        }
    }
}
