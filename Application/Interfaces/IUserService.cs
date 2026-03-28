using Application.DTOs;
using Application.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<AppResponse<Users>> CreateUserAsync(UserDto dto);
        Task<AppResponse<Users>> GetUserByIdAsync(int id);
        Task<AppResponse<List<Users>>> GetUserListAsync(bool showInactive);
        Task<AppResponse<Users>> UpdateUserAsync(UserDto dto);
        Task<AppResponse> DeleteUserAsync(int id);
        Task<AppResponse> Auth(AuthDto dto);
    }
}
