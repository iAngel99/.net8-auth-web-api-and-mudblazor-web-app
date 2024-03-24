using AutenticationBlazorWebApi.Models;
using AutenticationBlazorWebApi.Models.DTOs;
using AutenticationBlazorWebApi.Server.Data;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutenticationBlazorWebApi.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserRepository> _logger;
        private readonly IMapper _mapper;


        public UserRepository(UserManager<User> userManager, ILogger<UserRepository> logger, IMapper mapper, AppDbContext db)
        {
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByEmailAsync(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var userList = await _userManager.Users
                .Include(u => u.UserRole)
                .ThenInclude(ur => ur.Role).ToListAsync();
            return _mapper.Map<List<UserDto>>(userList);

        }

        public async Task<UserDto> AddUserAsync(CreateUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
            {
                return await GetUserByEmailAsync(user.Email);
            }
            string errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError(errorMessage);
            throw new Exception("Error al crear el usuario");
        }

        public async Task<UserDto> UpdateUserAsync(UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id);
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            var emailCambio = userDto.Email != user.Email;
            user.Email = userDto.Email;
            user.UserName = userDto.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return _mapper.Map<UserDto>(user);
            }
            string errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError(errorMessage);
            throw new Exception("Error al actualizar el usuario" + (emailCambio ? ", nuevo Email ya está siendo utilizado " : ""));
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            else
            {
                _logger.LogError("Usuario no encontrado");
                throw new Exception("Error al eliminar, usuario no encontrado");
            }
        }
        public async Task<IdentityResult> AddUserToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Usuario no encontrado");
                throw new Exception("Usuario no encontrado.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                _logger.LogInformation("Rol eliminado correctamente para el usuario con ID: " + userId + " .");
            }
            else
            {
                _logger.LogError("Error al eliminar el rol para el usuario con ID: " + userId);
                _logger.LogError(result.Errors.ToString());
            }
            return result;
        }


        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Usuario no encontrado");
                throw new Exception("Usuario no encontrado.");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Cambio de contraseña correcto para usuario con ID: " + userId + " .");
            }
            _logger.LogError("Cambio de contraseña incorrecto para usuario con ID: " + userId);
            _logger.LogError(result.Errors.ToString());
            return result;
        }

        public async Task<IdentityResult> AdminChangePasswordAsync(string userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError("Usuario no encontrado");
                throw new Exception("Usuario no encontrado.");
            }
            // Elimina la contraseña actual (si es necesario)
            var resultadoEliminar = await _userManager.RemovePasswordAsync(user);
            if (!resultadoEliminar.Succeeded)
            {
                _logger.LogError("Error al eliminar la contraseña actual.");
                _logger.LogError(resultadoEliminar.Errors.ToString());
            }


            var result = await _userManager.AddPasswordAsync(user, newPassword);
            if (!result.Succeeded)
            {
                _logger.LogError("Cambio de contraseña incorrecto para usuario con ID: " + userId);
                _logger.LogError(result.Errors.ToString());
            }
            _logger.LogInformation("Cambio de contraseña correcto para usuario con ID: " + userId + " .");

            return result;
        }
    }

}
