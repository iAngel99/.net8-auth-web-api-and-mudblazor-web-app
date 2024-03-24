using AutenticationBlazorWebApi.Models.DTOs;
using AutenticationBlazorWebApi.Models.Responses;
using AutenticationBlazorWebApi.Server.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace AutenticationBlazorWebApi.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;


        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }


        /// <summary>
        /// Obtiene una lista de todos los usuarios.
        /// </summary>
        /// <returns>Una colección de usuarios.</returns>
        /// <response code="200">Devuelve la lista de usuarios.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet]
        [EnableQuery]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario.</param>
        /// <returns>Un usuario.</returns>
        /// <response code="200">Devuelve el usuario.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserResponse>> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(new UserResponse(false, "Usuario no encontrado"));
            }

            return new UserResponse(true, "Usuario encontrado con éxito", user);
        }

        /// <summary>
        /// Obtiene un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns>Un usuario.</returns>
        /// <response code="200">Devuelve el usuario.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("ByEmail/{email}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new UserResponse(false, "Usuario no encontrado"));
            }

            return Ok(new UserResponse(true, "Usuario encontrado con éxito", user));
        }

        /// <summary>
        /// Crea un nuevo usuario.
        /// </summary>
        /// <param name="user">El usuario a crear.</param>
        /// <returns>Una respuesta que indica si la operación fue exitosa y detalles del usuario creado.</returns>
        /// <response code="201">Usuario creado con éxito.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<UserResponse>> AddUser([FromBody] CreateUserDto user)
        {
            try
            {
                var createdUser = await _userRepository.AddUserAsync(user);
                return CreatedAtAction("AddUser", new UserResponse(true, "Usuario creado con éxito", createdUser));
            }
            catch (Exception)
            {
                return BadRequest("Error al crear el usuario");
            }
        }

        /// <summary>
        /// Actualiza un usuario existente.
        /// </summary>
        /// <param name="id">El ID del usuario a actualizar.</param>
        /// <param name="user">Los datos actualizados del usuario.</param>
        /// <returns>Una respuesta que indica si la operación fue exitosa.</returns>
        /// <response code="200">Usuario actualizado correctamente.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDto user)
        {
            if (id != user.Id)
            {
                return BadRequest(new GeneralResponse(true, "El ID del usuario no coincide con el ID en la URL"));
            }
            try
            {
                await _userRepository.UpdateUserAsync(user);
                return Ok(new GeneralResponse(true, "Usuario actualizado correctamente"));
            }
            catch (Exception)
            {
                return BadRequest(new GeneralResponse(true, "Error al actualizar el usuario"));
            }
        }


        /// <summary>
        /// Elimina un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario a eliminar.</param>
        /// <returns>Una respuesta que indica si la operación fue exitosa.</returns>
        /// <response code="200">Usuario eliminado correctamente.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _userRepository.DeleteUserAsync(id);
                return Ok(new GeneralResponse(true, "Usuario eliminado correctamente"));
            }
            catch (Exception)
            {
                return BadRequest(new GeneralResponse(false, "Error al eliminar el usuario"));
            }
        }


        /// <summary>
        /// Asigna un rol a un usuario.
        /// </summary>
        /// <returns>Una respuesta que indica si la operación fue exitosa.</returns>
        /// <response code="200">Rol asignado correctamente.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost]
        [Route("AddUserRole")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> AddUserRole([FromBody] UserRoleDto addUserRoleDto)
        {
            try
            {
                var result = await _userRepository.AddUserToRoleAsync(addUserRoleDto.UserId, addUserRoleDto.Role.Name);
                if (result.Succeeded)
                {
                    return Ok(new GeneralResponse(true, "Rol asignado correctamente"));
                }
                else
                {
                    return BadRequest(new GeneralResponse(false, "Error al asignar el rol"));
                }
            }
            catch (Exception)
            {
                return BadRequest(new GeneralResponse(false, "Error rol o usuario no existe"));
            }
        }


        /// <summary>
        /// Obtiene los roles asignados a un usuario.
        /// </summary>
        /// <param name="userId">El ID del usuario.</param>
        /// <returns>Una lista de roles asignados al usuario.</returns>
        /// <response code="200">Roles obtenidos correctamente.</response>
        /// <response code="400">Solicitud incorrecta.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpGet("Roles/{userId}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<string>>> GetRoles(string userId)
        {
            var roles = await _userRepository.GetRolesAsync(userId);
            return Ok(roles);
        }

        [HttpDelete("{userId}/roles/{roleName}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemoveUserFromRole(string userId, string roleName)
        {
            var result = await _userRepository.RemoveUserFromRoleAsync(userId, roleName);
            if (result.Succeeded)
            {
                return Ok("Rol eliminado correctamente.");
            }
            return BadRequest("Error al eliminar el rol.");
        }


        /// <summary>
        /// Cambia la contraseña de un usuario.
        /// </summary>
        /// <param name="userId">El ID del usuario cuya contraseña se va a cambiar.</param>
        /// <param name="currentPassword">La contraseña actual del usuario.</param>
        /// <param name="newPassword">La nueva contraseña que el usuario desea establecer.</param>
        /// <returns>Una respuesta que indica si la operación fue exitosa.</returns>
        /// <response code="200">Contraseña cambiada correctamente.</response>
        /// <response code="400">Solicitud incorrecta, por ejemplo, contraseña incorrecta.</response>
        /// <response code="500">Error interno del servidor.</response>
        [HttpPost("changepassword/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword([FromRoute] string userId, [FromBody] PasswordChangeDto passwordDto)
        {
            var result = await _userRepository.ChangePasswordAsync(userId, passwordDto.OldPassword, passwordDto.Password);
            if (result.Succeeded)
            {
                return Ok(new GeneralResponse(true, "Contraseña cambiada con éxito."));
            }
            else
            {
                return BadRequest(new GeneralResponse(false, "Error, contraseña incorrecta"));
            }
        }

        /// <summary>
        /// Cambia la contraseña de un usuario.
        /// </summary>
        /// <param name="userId">El ID del usuario cuya contraseña se va a cambiar.</param>
        /// <param name="currentPassword">La contraseña actual del usuario.</param>
        /// <param name="newPassword">La nueva contraseña que el usuario desea establecer.</param>
        /// <returns>Una respuesta que indica si la operación fue exitosa.</returns>
        /// <response code="200">Contraseña cambiada correctamente.</response>
        /// <response code="400">Solicitud incorrecta, por ejemplo, contraseña incorrecta.</response>
        /// <response code="500">Error interno del servidor.</response>
        [Authorize(Roles = "Administrator")]
        [HttpPost("adminchangepassword/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AdminChangePassword([FromRoute] string userId, [FromBody] string password)
        {
            var result = await _userRepository.AdminChangePasswordAsync(userId, password);
            if (result.Succeeded)
            {
                return Ok(new GeneralResponse(true, "Contraseña cambiada con éxito."));
            }
            else
            {
                return BadRequest(new GeneralResponse(false, "Error, al cambiar la contraseña"));
            }
        }

    }
}
