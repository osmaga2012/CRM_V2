using Blazored.LocalStorage;
using CRM.App.Paginas;
using CRM.App.Services;
using CRM.App.Shared.Interfaces;
using CRM.App.Shared.Services;
using CRM.App.Sharted.Services;
using CRM.Web.Services;
using CRM.Web.Shared.Helpers;
using CRM.Web.Shared.Interfaces;
using CRM.Web.Shared.Providers;
using CRM.Web.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using MudBlazor.Services;
namespace CRM.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMudServices();
            builder.Services.AddSingleton<Microsoft.FluentUI.AspNetCore.Components.GlobalState>();
            builder.Services.AddSingleton<LibraryConfiguration>(new LibraryConfiguration { });
            builder.Services.AddBlazoredLocalStorage();
            // **Asegúrate de que estos estén presentes para MAUI:**
            //builder.Services.AddAuthorizationCore();
            //builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddTransient<AuthorizedHttpClientHandler>();
            builder.Services.AddScoped(typeof(IApiClient<>), typeof(ApiClient<>));
            builder.Services.AddScoped<ISecureStorageService, MauiStorageService>();

            // 1. CONFIGURAR LA URL SEGÚN LA PLATAFORMA

            string baseUrl = "https://10.0.2.2:7254/"; //string.Empty;
            if (DeviceInfo.DeviceType == DeviceType.Virtual)
            {
                baseUrl = "http://10.0.2.2:5245/";
            }
            else
            {
                baseUrl = baseUrl = "http://192.168.1.14:5245/";
            }

            // 2. REGISTRAR HTTPCLIENT (Esto soluciona el error de IHttpClientFactory)
            builder.Services.AddHttpClient("NoAuthClient", client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

            builder.Services.AddHttpClient("ApiClient", client =>
            {
                // Asegúrate de que esta URL sea correcta y apunte a tu API de backend
                // Lee la BaseUrl desde la configuración (ej. appsettings.json)
                client.BaseAddress = new Uri(baseUrl); //new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7254/");
            })
            .AddHttpMessageHandler<AuthorizedHttpClientHandler>(); // Añade tu handler autorizado

            // 1. Habilitar el sistema de autorización de Blazor
            builder.Services.AddAuthorizationCore();

            // 2. Registrar tu CustomAuthStateProvider
            // Usamos la misma instancia para AuthenticationStateProvider y para la clase específica
            builder.Services.AddScoped<CustomAuthStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthStateProvider>());

            // Add device-specific services used by the CRM.App.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddScoped<IPlatformNavigationService, PlatformNavigationService>();

            // Registrar el servicio de flujo de Login como Singleton
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<CofradiaState>();

            // Registrar la interfaz para que Blazor pueda inyectarla

            builder.Services.AddTransient<ListadoTramites>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
