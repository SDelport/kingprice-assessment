using King_Price_Assessment.Models;
using King_Price_Assessment.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace King_Price_Assessment.Tests
{
    public class PermissionServiceTests
    {
        private UserContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new UserContext(options);
        }

        [Fact]
        public async Task AddPermissionAsync_AddsPermission()
        {
            using var context = CreateContext("AddPermissionAddsPermission");
            var service = new PermissionService(context);

            var permission = new Permission { Name = "CanEdit" };
            var added = await service.AddPermissionAsync(permission);

            var fromDb = await context.Permissions.FirstOrDefaultAsync(p => p.Name == "CanEdit");
            Assert.NotNull(fromDb);
            Assert.Equal("CanEdit", fromDb.Name);
        }

        [Fact]
        public async Task AddPermissionAsync_Throws_WhenIdSet()
        {
            using var context = CreateContext("AddPermissionThrowsIdSet");
            var service = new PermissionService(context);

            var permission = new Permission { PermissionId = Guid.NewGuid(), Name = "P" };
            await Assert.ThrowsAsync<Exception>(() => service.AddPermissionAsync(permission));
        }

        [Fact]
        public async Task AddPermissionAsync_Throws_WhenNameEmpty()
        {
            using var context = CreateContext("AddPermissionThrowsNameEmpty");
            var service = new PermissionService(context);

            var permission = new Permission { Name = "" };
            await Assert.ThrowsAsync<Exception>(() => service.AddPermissionAsync(permission));
        }

        [Fact]
        public async Task UpdatePermissionAsync_UpdatesName()
        {
            using var context = CreateContext("UpdatePermissionUpdatesName");
            var id = Guid.NewGuid();
            context.Permissions.Add(new Permission { PermissionId = id, Name = "Old" });
            await context.SaveChangesAsync();

            var service = new PermissionService(context);
            var updated = await service.UpdatePermissionAsync(new Permission { PermissionId = id, Name = "New" });

            var fromDb = await context.Permissions.FindAsync(id);
            Assert.Equal("New", fromDb.Name);
        }

        [Fact]
        public async Task UpdatePermissionAsync_Throws_WhenIdEmpty()
        {
            using var context = CreateContext("UpdatePermissionThrowsIdEmpty");
            var service = new PermissionService(context);

            await Assert.ThrowsAsync<Exception>(() => service.UpdatePermissionAsync(new Permission { PermissionId = Guid.Empty, Name = "X" }));
        }

        [Fact]
        public async Task UpdatePermissionAsync_Throws_WhenNameEmpty()
        {
            using var context = CreateContext("UpdatePermissionThrowsNameEmpty");
            var id = Guid.NewGuid();
            context.Permissions.Add(new Permission { PermissionId = id, Name = "A" });
            await context.SaveChangesAsync();

            var service = new PermissionService(context);
            await Assert.ThrowsAsync<Exception>(() => service.UpdatePermissionAsync(new Permission { PermissionId = id, Name = "" }));
        }

        [Fact]
        public async Task DeletePermissionAsync_DeletesPermission()
        {
            using var context = CreateContext("DeletePermissionDeletesPermission");
            var id = Guid.NewGuid();
            context.Permissions.Add(new Permission { PermissionId = id, Name = "ToDelete" });
            await context.SaveChangesAsync();

            var service = new PermissionService(context);
            var result = await service.DeletePermissionAsync(id);

            Assert.True(result);
            var fromDb = await context.Permissions.FindAsync(id);
            Assert.Null(fromDb);
        }

        [Fact]
        public async Task DeletePermissionAsync_Throws_WhenIdEmpty()
        {
            using var context = CreateContext("DeletePermissionThrowsIdEmpty");
            var service = new PermissionService(context);

            await Assert.ThrowsAsync<Exception>(() => service.DeletePermissionAsync(Guid.Empty));
        }

        [Fact]
        public async Task GetPermissionByIdAsync_ReturnsPermission()
        {
            using var context = CreateContext("GetPermissionByIdReturnsPermission");
            var id = Guid.NewGuid();
            context.Permissions.Add(new Permission { PermissionId = id, Name = "PFound" });
            await context.SaveChangesAsync();

            var service = new PermissionService(context);
            var perm = await service.GetPermissionByIdAsync(id);

            Assert.Equal("PFound", perm.Name);
        }

        [Fact]
        public async Task GetPermissionByIdAsync_Throws_WhenNotFound()
        {
            using var context = CreateContext("GetPermissionByIdThrowsNotFound");
            var service = new PermissionService(context);

            await Assert.ThrowsAsync<Exception>(() => service.GetPermissionByIdAsync(Guid.NewGuid()));
        }
    }
}
