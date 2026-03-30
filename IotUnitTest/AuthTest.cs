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
            // Arrange
            var mockUow = new Mock<IUnitOfWork>();
            var mockTokenService = new Mock<ITokenService>();

            var testUser = new Users
            {
                UserId = 1,
                UserName = "mondo84",
                Role = new Role { RoleId = 1, Description = "Admin" }
            };

            // Simulamos que el repositorio devuelve el usuario
            mockUow.Setup(x => x.Users.GetByUsernameAsync("mondo84"))
                   .ReturnsAsync(testUser);

            // Simulamos que el tokenService genera un token
            mockTokenService.Setup(x => x.GenerateToken(
                It.IsAny<UserTokenDto>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int>()))
                .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtb25kbzg0IiwidXNlcklkIjozLCJyb2xlIjoiQWRtaW4iLCJpc3MiOiJ5ZXNpZF9kYXZpbGFfZWRpdG9yX2RlX3BydWViYV90b2tlbl9tYW51YWwiLCJhdWQiOiJhdWRpZW5jZV9zaW1vbl9tb3ZpbGlkYWQiLCJleHAiOjE3NzQ4OTAxMTAsImlhdCI6MTc3NDg4MjkxMH0.eIcDsaqdTPie4sZ2HJKFS7Bv3VhLI4wiVwVfg8UxAGY");

            var authService = new UserService(mockUow.Object, mockTokenService.Object);

            var dto = new AuthDto
            {
                Username = "admin",
                Password = "cualquierpass"
            };

            // Act
            var response = await authService.Auth(dto);
            
            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(1, response.Data.UserId);
            Assert.Equal("mondo84", response.Data.UserName);
            Assert.Equal("Admin", response.Data.Role);
            Assert.Equal("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtb25kbzg0IiwidXNlcklkIjozLCJyb2xlIjoiQWRtaW4iLCJpc3MiOiJ5ZXNpZF9kYXZpbGFfZWRpdG9yX2RlX3BydWViYV90b2tlbl9tYW51YWwiLCJhdWQiOiJhdWRpZW5jZV9zaW1vbl9tb3ZpbGlkYWQiLCJleHAiOjE3NzQ4OTAxMTAsImlhdCI6MTc3NDg4MjkxMH0.eIcDsaqdTPie4sZ2HJKFS7Bv3VhLI4wiVwVfg8UxAGY", response.Token);
        }
    }
}
