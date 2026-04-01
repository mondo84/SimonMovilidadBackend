using Application.DTOs;
using Application.Interfaces;
using Application.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService service) : ControllerBase
    {
        private readonly IUserService _service = service;

        [Authorize(Roles = "Admin, User, Viewer")]
        [HttpGet()]
        public Task<AppResponse<List<Users>>> GetUserList([FromQuery] bool showInactive = false) {
            return _service.GetUserListAsync(showInactive);
        }

        [Authorize(Roles = "Admin, User, Viewer")]
        [HttpGet("{id}")]
        public async Task<AppResponse<Users>> GetUserById(int id)
        {
            return await _service.GetUserByIdAsync(id);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost()]
        public async Task<AppResponse<Users>> Create(UserDto user)
        {
            return await _service.CreateUserAsync(user);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut()]
        public async Task<AppResponse<Users>> Update(UserDto dto)
        {
            return await _service.UpdateUserAsync(dto); ;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}", Name = "DeleteUser")]
        public async Task<AppResponse> Delete(int userId)
        {
            return await _service.DeleteUserAsync(userId);
        }

    }
}
