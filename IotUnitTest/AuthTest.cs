using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Xunit;

namespace IotUnitTest
{
    public class AuthTest
    {
        [Fact]
        public async Task Auth_debe_devolver_usuario_y_token()
        {
            var mockUow = new Mock<IUnitOfWork>();
            var mockTokenService = new Mock<ITokenService>();

            var testUser = new Users
            {
                UserId = 3,
                UserName = "mondo84",
                PasswordHash = "$2a$12$wq.NIoSIfOaVPqoBGHLUze0/KwLRiPuJPq9IojZxfjWr8aacQZZhe",
                Role = new Role { RoleId = 1, Description = "Admin" }
            };

            mockUow.Setup(x => x.Users.GetByUsernameAsync("mondo84"))
           .ReturnsAsync(testUser);

            mockTokenService.Setup(x => x.GenerateToken(
                It.IsAny<UserTokenDto>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns("TOKEN_SIMULADO");

            var authService = new UserService(mockUow.Object, mockTokenService.Object);

            var dto = new AuthDto { Username = "mondo84", Password = "123456" };

            // Servicio testeado
            var response = await authService.Auth(dto);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(3, response.Data.UserId);
            Assert.Equal("mondo84", response.Data.UserName);
            Assert.Equal("Admin", response.Data.Role);
            Assert.Equal("TOKEN_SIMULADO", response.Token);
        }
    }
}
