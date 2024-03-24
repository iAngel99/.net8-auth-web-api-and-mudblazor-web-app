using AutenticationBlazorWebApi.Client.Helpers;
using AutenticationBlazorWebApi.Client.States;
using AutenticationBlazorWebApi.Models.DTOs;
using System.Security.Claims;



namespace AutenticationBlazorWebApi.Client.Components.Pages.AccountPages
{
    public partial class UserManagePage
    {
        private UserDto User { get; set; } = new UserDto();

        private string UserID { get; set; }

        protected string Errors { get; set; } = "";
        private string Message { get; set; } = "";

        protected bool Show { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var claimsPrincipal = authState.User;
            var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var nameClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var lastNameClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var emailClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            //var roleClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            User.Email = emailClaim;
            User.FirstName = nameClaim;
            User.LastName = lastNameClaim;
            User.Id = userIdClaim;
            UserID = userIdClaim;
        }

        async Task HandleUserChange()
        {

            var result = await UserService.UpdateUserAsync(UserID, User);
            if (result.Flag)
            {
                Message = result.Message;

                // Obtener el token
                var stringToken = MainStates.JwtToken;


                // Deserializar el token en un objeto UserSession
                var deserializeToken = Serializations.DeserializeJsonString<AuthResponseDto>(stringToken);

                var newTokenResult = await AccountService.RefreshToken(deserializeToken);
                if (newTokenResult.Flag)
                {
                    // Asumiendo que tienes un método para actualizar el token en el cliente
                    await CustomAuthenticationStateProvider.UpdateAuthenticationState(new AuthResponseDto
                    {
                        RefreshToken = newTokenResult.RefreshToken,
                        Token = newTokenResult.Token
                    });

                    Message += " y el token ha sido actualizado.";
                }
                else
                {
                    Errors = "Error al actualizar el token: " + newTokenResult.Message;
                }
            }
            else
            {
                Errors = result.Message;
            }
        }

        private void Cancel()
        {
            NavManager.NavigateTo("/");
            Message = "";
            Errors = "";
        }

        private void ChangePasswordAction()
        {

            Show = !Show;
        }
    }
}
