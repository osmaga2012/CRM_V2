using CRM.App.Shared.Interfaces;
using CRM.Web.Shared.Interfaces;
using CRM.Web.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace CRM.Web.Shared.Helpers
{
    public class AuthorizedHttpClientHandler : DelegatingHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationManager _navigation;

        // Inyectamos IServiceProvider en lugar de IAuthService
        public AuthorizedHttpClientHandler(
            IServiceProvider serviceProvider,
            NavigationManager navigation)
        {
            _serviceProvider = serviceProvider;
            _navigation = navigation;
        }

        //protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    var response = await base.SendAsync(request, cancellationToken);

        //    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //    {
        //        // 1. Limpiamos la sesión (Borrar token de SecureStorage, etc.)
        //        // Esto debería disparar el NotifyUserLogout de tu Provider
        //        var authService = _serviceProvider.GetRequiredService<IAuthService>();
        //        await authService.LogoutAsync();

        //        // 2. Navegación Segura en MAUI Blazor
        //        // Usamos BeginInvoke para no bloquear el hilo de la petición
        //        // y asegurar que el WebViewNavigationManager ya esté inicializado.
        //        //_navigation.NavigateTo("/login");
        //    }

        //    return response;
        //}

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 💡 Obtenemos el servicio "bajo demanda" dentro del método.
            // Esto evita que se intente crear el IAuthService antes de tiempo.
            var authService = _serviceProvider.GetRequiredService<IAuthService>();

            var storageService = _serviceProvider.GetRequiredService<ISecureStorageService>();

            // 2. Obtenemos el token directamente del storage (Web o MAUI)
            var accessToken = await storageService.GetTokenAsync();

            //var accessToken =  // await authService.GetAccessTokenAsync();


            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await authService.LogoutAsync();
                _navigation.NavigateTo("login");
                // Ojo: Retornar la respuesta original suele ser mejor para que el llamador vea el 401

                return response;
            }

            return response;
        }
    }
}
