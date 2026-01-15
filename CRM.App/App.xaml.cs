using CRM.App.Shared.Pages.Login;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components.WebView.Maui;

namespace CRM.App
{
    public partial class App : Application
    {
        private readonly IAuthService authService;

        public App(IAuthService authService )
        {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new MainPage();
            //this.authService = authService;

            //authService.OnLoginSuccess += AuthService_OnLoginSuccess;   
        }
        // Método llamado cuando el login es exitoso
        //private void AuthService_OnLoginSuccess()
        //{
        //    // El cambio de UI DEBE hacerse en el hilo principal
        //    MainThread.BeginInvokeOnMainThread(() =>
        //    {
        //        // 2. Reemplazamos la ventana principal con la página principal
        //        // (La MainPage debe estar definida en el proyecto MAUI)
        //        //this.MainPage = new MainPage();
        //        this.MainPage = new AppShell();
        //        // Opcional: Desuscribirse del evento
        //        authService.OnLoginSuccess -= AuthService_OnLoginSuccess;
        //    });
        //}

        //protected override Window CreateWindow(IActivationState? activationState)
        //{

        //    return new Window(new ContentPage())
        //    {
        //        Title = "Cofradía de Pescadores de Gandia",
        //    };

        //}

        protected override void OnStart()
        {
            base.OnStart();

            //LoadLoginScreen();
        }

        // Función centralizada para cargar la pantalla de Login
        //private void LoadLoginScreen()
        //{
        //    // Aseguramos que la ejecución se realice en el hilo principal de la UI
        //    MainThread.BeginInvokeOnMainThread(() =>
        //    {
        //        var loginBlazorView = new BlazorWebView
        //        {
        //            HostPage = "wwwroot/index.html",
                    
        //        };

        //        // IMPORTANTE: Sin esto, Blazor no puede inyectar MudBlazor ni tus servicios
        //        // y la pantalla se queda blanca al fallar la DI.

        //        loginBlazorView.RootComponents.Add(new RootComponent
        //        {
        //            Selector = "#app",
        //            ComponentType = typeof(Login),
        //            Parameters = new Dictionary<string, object?>()
        //        });

        //        // 3. Lo envolvemos en un Layout para asegurar que ocupe toda la pantalla
        //        var content = new Grid();
        //        content.Children.Add(loginBlazorView);

        //        var loginPage = new ContentPage { Content = content };

        //        // 4. Asignamos la MainPage
        //        this.MainPage = loginPage;
        //    });
        //}

        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    var loginBlazorView = new BlazorWebView
        //    {
        //        // Asegúrate de que HostPage y Services estén correctos
        //        HostPage = "wwwroot/index.html",

        //        // ❌ ELIMINAR: Services = this.Handler!.MauiContext!.Services,

        //        RootComponents =
        //        {
        //            new RootComponent
        //            {
        //                Selector = "#app",
        //                ComponentType = typeof(Login),
        //                Parameters = new Dictionary<string, object?>()
        //            }
        //        }
        //    };

        //    var loginPage = new ContentPage { Content = loginBlazorView };

        //    // Retorna la ventana que contiene la página de Login
        //    return new Window(loginPage) { Title = "CRM.App" };
        //}

    }
}
