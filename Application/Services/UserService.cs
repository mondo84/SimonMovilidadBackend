using Application.Common.Exceptions;
using Application.DTOs;
using Application.Interfaces;
using Application.Response;
using Domain.Entities;
using Domain.Interfaces;
using System.Net;

namespace Application.Services
{
    public class UserService(IUnitOfWork unitOfWork, ITokenService tokenService) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenService _tokenService = tokenService;
        
        public async Task<AppResponse> Auth(AuthDto dto)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(dto.Username);

            user = VaryfyUser(dto, user);

            var objUserDto = new UserTokenDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Role = user.Role?.Description ?? "Sin role"
            };

            var secretKey = "mi_secret_key_qwertyuiopasdfghjklñzxcvbnm";
            var issuer = "yesid_davila_editor_de_prueba_token_manual";
            var audience = "audience_simon_movilidad";
            var expireMin = 120;
            var token = _tokenService.GenerateToken(objUserDto, secretKey, issuer, audience, expireMin); // _jwtService.GenerateToken(objUserDto);

            return AppResponse<UserTokenDto>.Ok(objUserDto, "Operacion exitosa", token);
        }

        public async Task<AppResponse<Users>> CreateUserAsync(UserDto dto)
        {
            var res = await VerifyUsername(dto.UserName);
            if (res)
                throw new AppException(HttpStatusCode.BadRequest, new ErrorList
                {
                    Message = "Error al registrarse",
                    Errors = ["El usuario ya se encuentra registrado"]
                });

            var newUser = ValidateNewPassword(dto);

            await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();

            return AppResponse<Users>.Ok(newUser, "Usuario creado exitosamente");
        }

        public async Task<AppResponse> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id)
                ?? throw new AppException(HttpStatusCode.NotFound, "User not found");

            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return AppResponse.Ok("Registro eliminado exitosamente");
        }

        public async Task<AppResponse<Users>> GetUserByIdAsync(int id)
        {   
            var resp = await _unitOfWork.Users.GetByIdAsync(id) 
                ?? throw new AppException(HttpStatusCode.NotFound, "User not found");

            return AppResponse<Users>.Ok(resp, "Usuario por {id}");
        }

        public async Task<AppResponse<List<Users>>> GetUserListAsync(bool showInactive = false)
        {
            var resp = await _unitOfWork.Users.GetAllAsync(showInactive);

            return AppResponse<List<Users>>.Ok(resp, "Listado de Usuarios");
        }

        public async Task<AppResponse<Users>> UpdateUserAsync(UserDto dto)
        {
            if (dto.UserId == null)
                throw new AppException(HttpStatusCode.BadRequest, "UserId requerido");

            var user = await _unitOfWork.Users.GetByIdAsync(dto.UserId.Value) ??
                throw new AppException(HttpStatusCode.NotFound, "Usuario no encontrado");

            ValidateAndUpdatePassword(dto, user);

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.UserName;
            if (dto.Active.HasValue)
                user.Active = dto.Active.Value;

            await _unitOfWork.SaveChangesAsync();

            return AppResponse<Users>.Ok(user, "Usuario actualizado exitosamente");   
        }

        private async Task<bool> VerifyUsername (string username)
        {
            return await _unitOfWork.Users.ValidateUsernme(username);
        } 

        private static Users VaryfyUser (AuthDto dto, Users? user)
        {
            if(user == null)
               throw new AppException(HttpStatusCode.Conflict, "El correo usuario no se encuentra registrado");

            if (!user.Active)
                throw new AppException(HttpStatusCode.Conflict, "Usuario inactivo");

            var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isValid)
                throw new AppException(HttpStatusCode.Conflict, new ErrorList { Message = "Error de validacion" });

            return user;
        }

        private static Users ValidateNewPassword (UserDto dto)
        {
            var errors = new List<string>();

            if (dto.ChangePass ?? false)
            {
                if (string.IsNullOrWhiteSpace(dto.NewPassword))
                    errors.Add("Nueva contraseña requerida");

                if (string.IsNullOrWhiteSpace(dto.ConfirmPassword))
                    errors.Add("Confirmar contraseña requerido");

                if (dto.NewPassword != dto.ConfirmPassword)
                    errors.Add("Contraseñas no coinciden");

                if (errors.Count != 0)
                    throw new AppException(HttpStatusCode.BadRequest, new ErrorList
                    {
                        Message = "Error de validacion",
                        Errors = errors
                    });
            }

            var passHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            
            return new Users
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                PasswordHash = passHash,
                Active = dto.Active ?? false,
                RoleId = dto.RoleId
            };
        }

        private static void ValidateAndUpdatePassword(UserDto dto, Users user)
        {
            var errors = new List<string>();

            if (dto.ChangePass ?? false)
            {
                if (string.IsNullOrWhiteSpace(dto.PasswordHash))
                    errors.Add("Contraseña requerida");
                 
                if (string.IsNullOrWhiteSpace(dto.NewPassword))
                    errors.Add("Nueva contraseña requerida");   

                if (string.IsNullOrWhiteSpace(dto.ConfirmPassword))
                    errors.Add("Confirmar contraseña requerido");

                if (dto.ConfirmPassword != dto.NewPassword) 
                    errors.Add("Contraseñas no coinciden");

                if (errors.Count != 0)
                    throw new AppException(HttpStatusCode.BadRequest, new ErrorList { 
                        Message = "Error de validacion",
                        Errors = errors
                    });

                var isValid = BCrypt.Net.BCrypt.Verify(dto.PasswordHash, user.PasswordHash);
                if (!isValid)
                    throw new AppException(HttpStatusCode.BadRequest, new ErrorList { 
                        Message = "Error de validacion",
                        Errors = ["Contraseña actual incorrecta" ]
                    });

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            }
        }
    }
}
