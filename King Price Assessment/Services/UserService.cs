using King_Price_Assessment.Interfaces;
using King_Price_Assessment.Models;
using Microsoft.EntityFrameworkCore;

namespace King_Price_Assessment.Services
{
    public class UserService : IUserService
    {
        private UserContext userContext;

        public UserService(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public async Task<User> AddUserAsync(User user)
        {
            if (user.UserId != Guid.Empty)
                throw new Exception("User cannot have primary key defined when added.");

            if (string.IsNullOrWhiteSpace(user.Name))
                throw new Exception("User cannot have an empty name.");

            await userContext.Users.AddAsync(user);
            await userContext.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUserAsync(Guid userID)
        {
            if (userID == Guid.Empty)
                throw new Exception("Must specify key to be deleted.");

            var user = await userContext.Users.FirstOrDefaultAsync(user => user.UserId == userID);

            if (user == null)
                throw new Exception("User cannot be found");

            userContext.Users.Remove(user);
            await userContext.SaveChangesAsync();

            return true;
        }

        public async Task<User> GetUserByIdAsync(Guid guid)
        {
            var user = await userContext.Users.Include(x=> x.Groups).FirstOrDefaultAsync(user => user.UserId == guid);

            if (user == null)
                throw new Exception("User cannot be found");

            return user;
        }

        public async Task<int> GetUserCountAsync()
        {
            return await userContext.Users.CountAsync();
        }

        public async Task<int> GetUserCountByGroupAsync(Guid groupID)
        {
            var group = await userContext.Groups.Include(group => group.Users).FirstOrDefaultAsync(group => group.GroupId == groupID);

            if (group == null)
                throw new Exception("Group Cannot be found.");

            return group.Users.Count();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await userContext.Users.ToListAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            if (user.UserId == Guid.Empty)
                throw new Exception("User must have primary key defined when update.");

            if (string.IsNullOrWhiteSpace(user.Name))
                throw new Exception("User cannot have an empty name.");

            var dbUser = userContext.Users.FirstOrDefault(dbUser => dbUser.UserId == user.UserId);

            if (dbUser == null)
                throw new Exception("User cannot be found.");

            dbUser.Name = user.Name;
            await userContext.SaveChangesAsync();

            return user;
        }
    }
}
