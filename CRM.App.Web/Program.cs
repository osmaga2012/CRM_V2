using Blazored.LocalStorage;
using CRM.App.Shared.Interfaces;
using CRM.App.Shared.Services;
using CRM.App.Sharted.Services;
using CRM.App.Web.Client.Services;
using CRM.App.Web.Components;
using CRM.App.Web.Services;
using CRM.Web.Shared.Helpers;
using CRM.Web.Shared.Interfaces;
using CRM.Web.Shared.Providers;
using CRM.Web.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.FluentUI.AspNetCore.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();
builder.Services.AddFluentUIComponents();

// Add device-specific services used by the CRM.App.Shared project
builder.Services.AddSingleton<IFormFactor, CRM.App.Web.Services.FormFactor>();
builder.Services.AddBlazoredLocalStorage(); // Asegúrate de registrar Blazored.LocalStorage

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IPlatformNavigationService, WebNavigationService>();

builder.Services.AddScoped<CofradiaState>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ISecureStorageService, WebStorageService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddTransient<AuthorizedHttpClientHandler>();

builder.Services.AddScoped(typeof(IApiClient<>), typeof(ApiClient<>));

// 5. Configuración de Autenticación
// Registra tu implementación personalizada de AuthenticationStateProvider

// 6. Configura el HttpClient principal para tu API con el AuthorizedHttpClientHandler
// Este cliente con nombre "ApiClient" será usado por servicios que necesiten autenticación (ej. AuthService y ApiClient<T>).
builder.Services.AddHttpClient("ApiClient", client =>
{
    // Asegúrate de que esta URL sea correcta y apunte a tu API de backend
    // Lee la BaseUrl desde la configuración (ej. appsettings.json)
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7254/");
})
.AddHttpMessageHandler<AuthorizedHttpClientHandler>(); // Añade tu handler autorizado

// 7. Configura un HttpClient adicional *sin* el handler de autorización
//    Este cliente con nombre "NoAuthClient" será usado internamente por AuthService para refrescar tokens
//    sin entrar en un bucle de intentar refrescar el token para la propia solicitud de refresco.
builder.Services.AddHttpClient("NoAuthClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7254/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapStaticAssets(); // <--- AÑADE ESTA LÍNEA AQUÍ (Crucial para .NET 10)
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(CRM.App.Shared._Imports).Assembly,
        typeof(CRM.App.Web.Client._Imports).Assembly);

app.Run();
