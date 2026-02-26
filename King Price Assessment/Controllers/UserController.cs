using King_Price_Assessment.Interfaces;
using King_Price_Assessment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace King_Price_Assessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("get-users")]
        public async Task<List<User>> GetUsersAsync()
        {
            return await userService.GetUsersAsync();
        }

        [HttpGet]
        [Route("get-user-by-id")]
        public async Task<User> GetUserByIdAsync(Guid userID)
        {
            return await userService.GetUserByIdAsync(userID);
        }


        [HttpGet]
        [Route("get-users-count")]
        public async Task<int> GetUsersCountAsync()
        {
            return await userService.GetUserCountAsync();
        }

        [HttpGet]
        [Route("get-users-count-by-group")]
        public async Task<int> GetUsersCountByGroupAsync(Guid groupID)
        {
            return await userService.GetUserCountByGroupAsync(groupID);
        }

        
        [HttpPost("add-user")]
        public async Task<User> AddUserAsync(User user)
        {
            return await userService.AddUserAsync(user);
        }

        [HttpPut]
        [Route("update-user")]
        public async Task<User> UpdateUserAsync(User user)
        {
            return await userService.UpdateUserAsync(user);
        }

        [HttpDelete]
        [Route("delete-user")]
        public async Task<bool> DeleteUserAsync(Guid userID)
        {
            return await userService.DeleteUserAsync(userID);
        }
    }
}
