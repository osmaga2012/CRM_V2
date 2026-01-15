using CRM.App.Paginas;
using CRM.App.Shared.Interfaces;
using CRM.App.Shared.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.App.Services
{
    public class PlatformNavigationService : IPlatformNavigationService
    {
        private readonly NavigationManager navigationManager;
        private readonly IServiceProvider serviceProvider;

        public PlatformNavigationService(IServiceProvider serviceProvider)
        {
            
            this.serviceProvider = serviceProvider;
        }

        //public Task NavigateToAsync(string route)
        //{
        //    MainThread.BeginInvokeOnMainThread(async () =>
        //    {
        //        // Si usas Shell, simplemente asígnalo directamente
        //        Application.Current.MainPage = new AppShell();
        //    });
        //    return Task.CompletedTask;
        //}

        //public void NavigateToNativePage(string route)
        //{
        //    //// Esta acción DEBE ejecutarse en el hilo UI principal de MAUI
        //    //MainThread.BeginInvokeOnMainThread(async () =>
        //    //{
        //    //    var currentPage = Application.Current.MainPage;

        //    //    switch (route.ToLower())
        //    //    {
        //    //        case "empresas":
        //    //            // Empuja la página XAML en la pila de navegación MAUI
        //    //            await currentPage.Navigation.PushAsync(new ListadoTramites());
        //    //            break;
        //    //            // ... otros casos XAML ...
        //    //    }
        //    //});
        //}

        public Task NavigateToAsync(string route)
        {
            //var navigationManager = serviceProvider.GetRequiredService<NavigationManager>();

            //if (navigationManager != null)
            //{
            //    // Importante: La navegación debe ocurrir en el hilo de la UI
            //    MainThread.BeginInvokeOnMainThread(() =>
            //    {
            //        navigationManager.NavigateTo(route);
            //    });
            //}

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var nav = serviceProvider.GetRequiredService<NavigationManager>();
                // Usa "/" en lugar de "" para la home
                string finalRoute = string.IsNullOrEmpty(route) || route == "" ? "/" : route;
                nav.NavigateTo(finalRoute, forceLoad: false);
            });

            return Task.CompletedTask;
        }

        public void NavigateToNativePage(string route)
        {
            // Dejar vacío si no vas a usar páginas XAML nativas por ahora
        }
    }
}
