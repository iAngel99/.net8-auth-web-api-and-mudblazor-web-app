using AutenticationBlazorWebApi.Models.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Security.Claims;

namespace AutenticationBlazorWebApi.Client.Components.Pages.AccountPages
{
    public partial class ChangePasswordPage
    {
        private PasswordChange PasswordChange { get; set; } = new PasswordChange();

        private string Message { get; set; } = "";

        private string Errors { get; set; } = "";

        bool isShow;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        bool isConfirmShow;
        InputType PasswordConfirmInput = InputType.Password;
        string PasswordConfirmInputIcon = Icons.Material.Filled.VisibilityOff;

        bool isConfirmClickShow;
        InputType passwordConfirmInputType = InputType.Password;
        string PasswordConfirmIcon = Icons.Material.Filled.VisibilityOff;

        [Parameter] public EventCallback OnCancel { get; set; }


        private async Task ChangePassword()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userIdClaim = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var response = await UserService.ChangePasswordAsync(userIdClaim.Value ?? "", PasswordChange);
            if (response.Flag)
            {
                // Redirigir al usuario a una página de éxito o mostrar un mensaje de éxito
                Message = response.Message;

            }
            else
            {
                Errors = response.Message;
            }

        }

        private async Task AwaitAndCancel()
        {
            await Task.Delay(1000);
            await Cancel();
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

        void ButtonShowConfirmclick()
        {
            TogglePasswordVisibility(ref isConfirmClickShow, ref passwordConfirmInputType, ref PasswordConfirmIcon);
        }

        private async Task Cancel()
        {
            Message = "";
            Errors = "";
            PasswordChange = new PasswordChange();
            await OnCancel.InvokeAsync(null);
        }


    }
}
