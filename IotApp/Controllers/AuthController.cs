
using Application.Response;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace AppPortal.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IUserService service) : ControllerBase
    {
        private readonly IUserService _service = service;

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<AppResponse<UserTokenDto>> Auth(AuthDto authDto)
        {
            return await _service.Auth(authDto);
        }

    }
}
