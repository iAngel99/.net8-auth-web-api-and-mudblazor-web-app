using AutenticationBlazorWebApi.Client.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AutenticationBlazorWebApi.Client.Components.Pages.ComponentsPages
{

    public partial class ChangePasswordDialog
    {

        [Parameter]
        public ChangePasswordDialogParameters parameters { get; set; } = new ChangePasswordDialogParameters();


        [CascadingParameter] private MudDialogInstance mudDialog { get; set; }

        private void Cancelar()
        {
            mudDialog.Cancel();
        }

        private void GuardarContraseña()
        {
            mudDialog.Close(DialogResult.Ok(parameters.NewPassword));
        }
    }
}
