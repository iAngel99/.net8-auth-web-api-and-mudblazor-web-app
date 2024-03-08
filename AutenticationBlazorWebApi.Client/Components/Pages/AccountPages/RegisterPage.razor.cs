using AutenticationBlazorWebApi.Client.Helpers;
using AutenticationBlazorWebApi.Models.DTOs;

namespace AutenticationBlazorWebApi.Client.Components.Pages.AccountPages
{
    public partial class RegisterPage
    {
        protected UserDto User { get; set; } = new UserDto();

        async Task HandleRegistration()
        {
            var result = await AccountService.Register(User);
            if (result.Flag)
            {
                User = new();
                NavManager.NavigateTo("/", forceLoad: true);
            }
        }
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
    }
}
