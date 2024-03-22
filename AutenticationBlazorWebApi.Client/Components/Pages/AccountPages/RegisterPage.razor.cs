using AutenticationBlazorWebApi.Client.Helpers;
using AutenticationBlazorWebApi.Models.DTOs;
using MudBlazor;

namespace AutenticationBlazorWebApi.Client.Components.Pages.AccountPages
{
    public partial class RegisterPage
    {
        protected RegisterDto User { get; set; } = new RegisterDto();
        bool isShow;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        bool isConfirmShow;
        InputType PasswordConfirmInput = InputType.Password;
        string PasswordConfirmInputIcon = Icons.Material.Filled.VisibilityOff;
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

        void TogglePasswordVisibility(ref bool isVisible, ref InputType inputType, ref string icon)
        {
            if (isVisible)
            {
                isVisible = false;
                icon = Icons.Material.Filled.VisibilityOff;
                inputType = InputType.Password;
            }
            else
            {
                isVisible = true;
                icon = Icons.Material.Filled.Visibility;
                inputType = InputType.Text;
            }
        }

        void ButtonTestclick()
        {
            TogglePasswordVisibility(ref isShow, ref PasswordInput, ref PasswordInputIcon);
        }

        void ButtonShowclick()
        {
            TogglePasswordVisibility(ref isConfirmShow, ref PasswordConfirmInput, ref PasswordConfirmInputIcon);
        }

    }
}
