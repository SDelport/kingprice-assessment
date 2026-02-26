using King_Price_Assessment.Models;
using System.ComponentModel;

namespace King_Price_Assessment.Interfaces
{
    public interface IUserService
    {
        public Task<List<User>> GetUsersAsync();
        public Task<User> GetUserByIdAsync(Guid guid);
        public Task<User> AddUserAsync(User user);
        public Task<User> UpdateUserAsync(User user);
        public Task<bool> DeleteUserAsync(Guid userID);
        public Task<int> GetUserCountAsync();
        public Task<int> GetUserCountByGroupAsync(Guid groupID);

    }
}
