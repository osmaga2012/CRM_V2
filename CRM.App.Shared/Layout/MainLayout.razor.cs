using CRM.App.Shared.Interfaces;
using CRM.Dominio.Entidades;
using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.App.Shared.Layout
{
    public partial class MainLayout: LayoutComponentBase
    {
        //[Parameter] public string Title { get; set; } = "Dashboard"; // Propiedad de título, si la usas
        [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; } // Acceso al estado de autenticación
        //[Inject] private IPlatformNavigationService _navigationManager { get; set; } // Para redirecciones
        [Inject] private IPlatformNavigationService _navigationManager { get; set; } // Para redirecciones
        [Inject] private IAuthService _authService { get; set; }
        [Inject] private ICurrentUserService currentUserService { get; set; }   

        private bool _drawerOpen = true;
        private UsuarioDto? _loggedInUser; // El perfil del usuario, puede ser null
        private bool _isLoading = true; // Indica si se está cargando el perfil del usuario

        // Método para alternar el estado del drawer (barra lateral)
        protected void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen; 
        }

        private async Task CambiaModoOscuro()
        {
            //isDarkMode = !isDarkMode;
            //SetDarkModeClass();
            //StateHasChanged();
            //await ThemeService.ToggleThemeAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                //await ThemeService.InitializeAsync();
                //ThemeService.OnChange += StateHasChanged;
                await LoadUserProfile();
                if (_loggedInUser?.TipoUsuario == CRM.Dtos.Enums.TipoUsuarioDto.Empresa)
                {
                    _drawerOpen = false; // No hacer nada si el usuario no está cargado
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error crítico al inicializar MainLayout. {ex.Message}");
                throw;
            }

        }

        private async Task LoadUserProfile()
        {

            _loggedInUser = new();

            _isLoading = true; // Empieza a cargar
            try
            {
                // Primero, verifica si el usuario está realmente autenticado según AuthStateProvider
                var authState = await authenticationStateTask;
                var user = authState.User;

                if (user.Identity?.IsAuthenticated == true)
                {
                    // Si está autenticado, intenta obtener el perfil.
                    // Esta llamada usará el HttpClient con TokenRefreshHandler.
                    _loggedInUser = await currentUserService.GetCurrentUserAsync(user);

                    if (_loggedInUser == null)
                    {
                        // Si GetCurrentUserAsync devuelve null (ej. API de perfil inaccesible o 401 que no se pudo refrescar)
                        // Informa al usuario y redirige al login.
                        //Snackbar.Add("No se pudo cargar el perfil de usuario. Intente iniciar sesión de nuevo.", Severity.Error);
                        _navigationManager.NavigateToAsync("login");
                    }
                    else
                    {
                        //_navigationManager.NavigateToAsync("");
                    }
                }
                else
                {
                    // Si el usuario no está autenticado según AuthStateProvider, redirige.
                    // Esto es un fallback, App.razor ya debería haberlo manejado, pero es buena práctica.
                    //Snackbar.Add("No autenticado. Por favor, inicie sesión.", Severity.Info);
                    await _navigationManager.NavigateToAsync("login");
                    //_navigationManager.NavigateTo("login");
                    //_navigationManager.NavigateToAsync("/");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el perfil de usuario en MainLayout: {ex.Message}");
                //Snackbar.Add($"Error al cargar el perfil: {ex.Message}. Redirigiendo a login.", Severity.Error);
                _loggedInUser = null; // Asegura que el usuario sea null para mostrar el mensaje de error en la UI
                //_navigationManager.NavigateTo("login"); // Redirige en caso de error grave
            }
            finally
            {
                _isLoading = false; // Termina la carga, independientemente del resultado
            }
        }

        // Método para cerrar sesión (si lo añades en el AppBar, por ejemplo)
        //[Inject] private IAuthService _authService { get; set; }
        private async Task Logout()
        {
            //await _authService.LogoutAsync();
            //_loggedInUser = null; // Limpiar el usuario en el layout
            //Snackbar.Add("Has cerrado sesión exitosamente.", Severity.Info);
            await _authService.LogoutAsync();
            await _navigationManager.NavigateToAsync("login");
            StateHasChanged();
        }



        private async Task Configuracion()
        {

        }

        public void Dispose()
        {
            //ThemeService.OnChange -= StateHasChanged;
        }

    }
}
