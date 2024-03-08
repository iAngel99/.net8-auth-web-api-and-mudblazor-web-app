using Microsoft.AspNetCore.Http.Features;
using Microsoft.JSInterop;
using MudBlazor;

namespace AutenticationBlazorWebApi.Client.Components.Pages.ComponentsPages
{
    public partial class ConsentCookies
    {
        private MudDialog dialogRef;
        private bool IsOpen = true;
        string cookieString;
        ITrackingConsentFeature consentFeature;


        protected override void OnInitialized()
        {
            consentFeature = HttpContextAccessor.HttpContext.Features.Get<ITrackingConsentFeature>();
            IsOpen = !consentFeature?.CanTrack ?? false;
        }

        private async Task AcceptCookies()
        {
            var cookieString = consentFeature?.CreateConsentCookie();

            // Añade la propiedad 'expires' a la cadena de la cookie.
            var expiresDate = DateTime.MaxValue.ToString("R"); // Formato RFC 1123.
            cookieString += $"; expires={expiresDate}";

            // Cerrar el diálogo.
            dialogRef.Close();
            IsOpen = false;

            await JSRuntime.InvokeVoidAsync("JsFunction.acceptCookieMessage", cookieString).ConfigureAwait(false);

        }



    }
}
