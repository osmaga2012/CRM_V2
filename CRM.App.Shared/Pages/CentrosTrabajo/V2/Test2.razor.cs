using CRM.App.Shared.Interfaces;
using CRM.App.Shared.Pages.CentrosTrabajo.Usuarios;
using CRM.App.Shared.Services;
using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.JSInterop;
using MudBlazor;
using System.Linq;

namespace CRM.App.Shared.Pages.CentrosTrabajo.V2
{
    public partial class Test2: ComponentBase
    {
        [Inject] private IApiClient<BarcosDto> servicioBarco { get; set; }
        [Inject] private IApiClient<EmpresasDto> servicioEmpresas { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IFormFactor PlatformService { get; set; }
        [Inject] private IPlatformNavigationService NavigationService { get; set; }
        [Inject] private IJSRuntime js { get; set; }

        private ICollection<BarcosDto> barcos = new List<BarcosDto>();
        private ICollection<EmpresasDto> empresas = new List<EmpresasDto>();

        private BarcosDto barcoSeleccionado;
        private string _filtroBarco = string.Empty;

        private string _emailInput = string.Empty;
        private string _emailError = string.Empty;
        private List<string> EmailsAviso = new();
        private bool IsMobile = true;
        // Simulating data


        // Estado de paginación (15 elementos para barcos suele ser ideal)
        PaginationState paginacion = new PaginationState { ItemsPerPage = 15 };

        // Propiedad que QuickGrid usará para mostrar los datos filtrados (siempre devuelve IQueryable no nulo)
        protected IQueryable<EmpresasDto> FiltroEmpresas
        {
            get
            {
                var source = empresas ?? Enumerable.Empty<EmpresasDto>();
                if (string.IsNullOrWhiteSpace(_filtroBarco))
                {
                    return source.AsQueryable();
                }
                return source.Where(filtroEmpresa).AsQueryable();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await CargarBarcos();


            }
            catch (OperationCanceledException)
            {
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var dimensions = await js.InvokeAsync<WindowDimensions>("getWindowDimensions");
                IsMobile = dimensions.Width < 768;
                StateHasChanged();
            }
        }
        public class WindowDimensions
        {
            public int Width { get; set; }
        }

        private async Task CargarBarcos()
        {
            string[] includesEmpresas = new string[] { "Barco" };
            var result = await servicioEmpresas.GetAllAsync("api/Empresa", null, includesEmpresas);
            empresas = result?.ToList() ?? new List<EmpresasDto>();
        }

        // quick filter - filter globally across multiple columns with the same input
        private Func<EmpresasDto, bool> filtroEmpresa => x =>
        {
            if (string.IsNullOrWhiteSpace(_filtroBarco))
            {
                return true; // Si no hay texto de búsqueda, mostrar todas las empresas
            }

            var textoBusquedaLower = _filtroBarco.ToLowerInvariant();

            // Revisa nulos para evitar NullReferenceException
            if (x.CodigoBarco.ToString().Contains(textoBusquedaLower, StringComparison.OrdinalIgnoreCase))
                return true;


            if (!string.IsNullOrEmpty(x.NombreArmador) && x.NombreArmador.Contains(textoBusquedaLower, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Barco != null && !string.IsNullOrEmpty(x.Barco!.NombreB) && x.Barco!.NombreB.Contains(textoBusquedaLower, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Barco != null && !string.IsNullOrEmpty(x.Barco.NombreA) && x.Barco.NombreA.Contains(textoBusquedaLower, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Barco != null && !string.IsNullOrEmpty(x.Barco.CapitanNombre) && x.Barco.CapitanNombre.Contains(textoBusquedaLower, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.CodigoEmpresa != null && !string.IsNullOrEmpty(x.CodigoEmpresa) && x.CodigoEmpresa.Contains(textoBusquedaLower, StringComparison.OrdinalIgnoreCase))
                return true;

            if ($"{x.CodigoBanco} {x.Barco!.NombreB} {x.NombreArmador} {x.Barco?.CapitanNombre} {x.CodigoEmpresa} ".Contains(textoBusquedaLower, StringComparison.OrdinalIgnoreCase))
                return true;



            return false;
        };

        // Navegación: usa NavigationService en MAUI (hilo UI) y NavigationManager en web
        public void OpenTramites(string codigoEmpresa, BarcosDto barco)
        {
            if (PlatformService != null && PlatformService.IsNativeApp && NavigationService != null)
            {
                NavigationService.NavigateToAsync($"barco/empresa/{codigoEmpresa}/tramites/{barco.CodigoBarco}");
            }
            else
            {
                NavigationManager.NavigateTo($"barco/empresa/{codigoEmpresa}/tramites/{barco.CodigoBarco}");
            }
        }

        public async Task OpenTramitesAsync(string codigoEmpresa, BarcosDto barco)
        {
            if (PlatformService != null && PlatformService.IsNativeApp && NavigationService != null)
            {
                await NavigationService.NavigateToAsync($"barco/empresa/{codigoEmpresa}/tramites/{barco.CodigoBarco}");
            }
            else
            {
                NavigationManager.NavigateTo($"barco/empresa/{codigoEmpresa}/tramites/{barco.CodigoBarco}");
            }
        }

        public async Task VerUsuariosBarco(EmpresasDto empresa)
        {
            var parameters = new DialogParameters { ["Empresa"] = empresa };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };

            var dialog = DialogService.ShowAsync<CrearUsuario>("Ver y Crear usuarios", parameters, options);
            var result = (await dialog);
        }

        private string  GetBadgeClass(string tipoExplotacion) 
        {
            return "1";
        }
    }
}
