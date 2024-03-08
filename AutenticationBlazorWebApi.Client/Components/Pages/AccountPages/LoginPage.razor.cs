using AutenticationBlazorWebApi.Client.Helpers;
using AutenticationBlazorWebApi.Models.DTOs;

namespace AutenticationBlazorWebApi.Client.Components.Pages.AccountPages
{
    public partial class LoginPage
    {
        protected LoginDto User { get; set; } = new LoginDto();
        protected bool IsLoading { get; set; } = false; // Variable para rastrear el estado de carga
        protected string Errors { get; set; } = "";


        protected override async Task OnInitializedAsync()
        {
            var customAuthStateProvider = (CustomAuthenticationStateProvider)AuthenticationStateProvider;
            var authState = await customAuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                // Redirige al usuario a la página de inicio o al panel de control, por ejemplo.
                NavManager.NavigateTo("/", forceLoad: true);
            }
        }

        async Task HandleLogin()
        {
            IsLoading = true; // Inicia el indicador de carga
            var result = await AccountService.Login(User);
            IsLoading = false; // Finaliza el indicador de carga

            if (result.Flag)
            {
                var customAuthStateProvider = (CustomAuthenticationStateProvider)AuthenticationStateProvider;
                await customAuthStateProvider.UpdateAuthenticationState(new AuthResponseDto() { Token = result.Token, RefreshToken = result.RefreshToken });
                NavManager.NavigateTo("/", forceLoad: true);
            }
            else
            {
                Errors = result.Message;
            }
        }
    }

}
