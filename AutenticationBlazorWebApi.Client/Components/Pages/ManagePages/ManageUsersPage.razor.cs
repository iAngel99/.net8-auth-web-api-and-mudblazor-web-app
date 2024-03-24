using AutenticationBlazorWebApi.Client.Components.Pages.ComponentsPages;
using AutenticationBlazorWebApi.Client.Helpers;
using AutenticationBlazorWebApi.Models.DTOs;
using MudBlazor;
using System.Security.Claims;

namespace AutenticationBlazorWebApi.Client.Components.Pages.ManagePages
{
    public partial class ManageUsersPage
    {
        private List<UserDto> Users = new List<UserDto>();

        private MudDataGrid<UserDto> dataGrid;

        private MudDataGrid<UserRoleDto> roleGrid;

        private string Message = "";

        private string LoggedUserId;

        private bool adding = false;

        private List<string> rolesList = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)AuthenticationStateProvider;
            var authState = await customAuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                NavManager.NavigateTo("/identity/account/login", forceLoad: true);
            }


            LoggedUserId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var response = await UserService.GetAllUsersAsync();
            if (response.Flag)
            {
                Users = response.Items.Where(u => u.Id != LoggedUserId).ToList();
            }
            else
            {
                Message = response.Message;
            }

            var roleResponse = await RoleService.GetAllRolesAsync();
            if (roleResponse.Flag)
            {
                rolesList = roleResponse.Items.ToList();
            }
            else
            {
                Message += roleResponse.Message;
            }
        }

        private async Task SaveRow(UserDto user)
        {
            if (adding)
            {
                adding = false;
                var response = await UserService.AddUserAsync(new CreateUserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = "Abc123***",

                });
                if (!response.Flag)
                {
                    Message = response.Message;
                }
                else
                {
                    Users.Add(response.UserDto);
                }
            }
            else
            {
                // Lógica para actualizar el usuario
                var response = await UserService.UpdateUserAsync(user.Id, user);
                if (response.Flag)
                {
                    // Actualizar la lista de usuarios
                    var index = Users.FindIndex(u => u.Id == user.Id);
                    if (index != -1)
                    {
                        var olduser = Users[index];
                        olduser.FirstName = user.FirstName;
                        olduser.LastName = user.LastName;
                        olduser.Email = user.Email;
                        Users[index] = olduser;
                    }
                }
                else
                {
                    Message = response.Message;

                    var response2 = await UserService.GetAllUsersAsync();
                    if (response2.Flag)
                    {
                        Users = response2.Items.Where(u => u.Id != LoggedUserId).ToList();
                        StateHasChanged();
                    }
                    else
                    {
                        Message = response2.Message;
                    }

                }
            }

        }

        private async Task DeleteRow(UserDto user)
        {
            // Lógica para eliminar el usuario
            var response = await UserService.DeleteUserAsync(user.Id);
            if (response.Flag)
            {
                // Eliminar el usuario de la lista
                Users.Remove(user);
                StateHasChanged();
            }
            else
            {
                Message = response.Message;
            }

        }

        private void AddNewRow()
        {
            // Crear un nuevo objeto de tu modelo
            var newItem = new UserDto();
            // Añadir el nuevo objeto a la lista de elementos
            Users.Append(newItem);
            // Opcionalmente, puedes iniciar la edición del nuevo objeto
            adding = true;
            dataGrid.SetEditingItemAsync(newItem);
        }

        private async Task CambiarContraseña(UserDto user)
        {

            var parameters = new DialogParameters
            {
                { "parameters", new ChangePasswordDialogParameters { Email = user.Email } }
            };

            var dialog = await DialogService.ShowAsync<ChangePasswordDialog>("Cambiar contraseña", parameters);

            var result = await dialog.Result;

            if (!result.Canceled)
            {
                var response = await UserService.AdminChangePasswordAsync(user.Id, result.Data.ToString());

                if (!response.Flag)
                {
                    Message = response.Message;
                }

            }
        }

        private void AddNewRole(string userId)
        {
            var newItem = new UserRoleDto
            {
                UserId = userId,
                Role = new RoleDto()
            };

            var index = Users.FindIndex(u => u.Id == userId);
            if (index != -1)
            {
                Users[index].UserRole.Append(newItem);
            }
            //dataGrid.Items.FirstOrDefault(d => d.Id == userId).UserRole.Append(newItem);
            roleGrid.SetEditingItemAsync(newItem);
        }

        private async Task DeleteRole(UserRoleDto userRoleDto)
        {
            var response = await UserService.RemoveUserFromRoleAsync(userRoleDto.UserId, userRoleDto.Role.Name);
            if (response.Flag)
            {
                var index = Users.FindIndex(u => u.Id == userRoleDto.UserId);
                if (index != -1)
                {
                    var olduser = Users[index];
                    olduser.UserRole.Remove(userRoleDto);
                    Users[index] = olduser;
                    StateHasChanged();
                }
            }
            else
            {
                Message = response.Message;
            }
        }

        private async Task SaveRoleRow(UserRoleDto dto, string userId)
        {
            var role = await RoleService.GetRoleIdByNameAsync(dto.Role.Name);
            if (!role.Flag)
            {
                Message = role.Message;
                return;
            }

            var userRole = new UserRoleDto
            {
                UserId = userId,
                RoleId = role.Id,
                Role = new RoleDto
                {
                    Id = role.Id,
                    Name = dto.Role.Name,
                }

            };
            var response = await UserService.AddUserRoleAsync(userRole);
            if (!response.Flag)
            {
                Message = response.Message;
            }
            else
            {
                var index = Users.FindIndex(u => u.Id == userId);
                if (index != -1)
                {
                    var olduser = Users[index];
                    olduser.UserRole.Add(userRole);
                    Users[index] = olduser;
                    StateHasChanged();
                }

            }
        }
    }
}
