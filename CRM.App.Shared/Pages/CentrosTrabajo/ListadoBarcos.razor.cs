using CRM.App.Shared.Pages.CentrosTrabajo.Usuarios;
using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using MudBlazor;

namespace CRM.App.Shared.Pages.CentrosTrabajo
{
    public partial class ListadoBarcos: ComponentBase
    {
        [Inject] private IApiClient<BarcosDto> servicioBarco { get; set; }
        [Inject] private IApiClient<EmpresasDto> servicioEmpresas { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private NavigationManager NavigationManager { get;set; }
        private ICollection<BarcosDto> barcos;
        private ICollection<EmpresasDto> empresas;
        private BarcosDto barcoSeleccionado;
        private string _filtroBarco;

        private string _emailInput = string.Empty;
        private string _emailError = string.Empty;
        private List<string> EmailsAviso = new();

        // Estado de paginación (15 elementos para barcos suele ser ideal)
        PaginationState paginacion = new PaginationState { ItemsPerPage = 15 };

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await CargarBarcos();

            }
            catch (OperationCanceledException)
            {

            }



            //return base.OnInitializedAsync();
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    await CargarBarcos();
        //    StateHasChanged();
        //}


        private async Task CargarBarcos()
        {
            //string[] includes = new string[] { "Empresa" };
            string[] includesEmpresas = new string[] { "Barco" };
            //barcos = (await servicioBarco.GetAllAsync("api/Barcos",null,null)).ToList();
            //empresas = (await servicioEmpresas.GetAllAsync("api/Empresa", null,includesEmpresas)).Where(y=>y.Barco != null).ToList();

            empresas = (await servicioEmpresas.GetAllAsync("api/Empresa", null, includesEmpresas)).ToList();
        }


        // quick filter - filter globally across multiple columns with the same input
        private Func<EmpresasDto, bool> filtroEmpresa => x =>
        {
            if (string.IsNullOrWhiteSpace(_filtroBarco))
            {
                return true; // Si no hay texto de búsqueda, mostrar todas las empresas
            }

            // Convertir a minúsculas para una búsqueda insensible a mayúsculas/minúsculas
            var textoBusquedaLower = _filtroBarco.ToLowerInvariant();

            // Aquí defines tu lógica de filtro:
            if (x.CodigoBarco.ToString().Contains(textoBusquedaLower, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.NombreArmador.Contains(textoBusquedaLower, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Barco.NombreA.Contains(textoBusquedaLower, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Barco.CapitanNombre.Contains(textoBusquedaLower, StringComparison.OrdinalIgnoreCase))
                return true;

            if ($"{x.CodigoBanco} {x.NombreArmador} {x.Barco.CapitanNombre} {x.Barco.CapitanNombre}".Contains(textoBusquedaLower))
                return true;

            return false;
        };

        public async Task VerTramites(string CodigoEmpresa,BarcosDto barco)
        {
            //var parameters = new DialogParameters { ["barco"] = barco };
            //var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };

            //var dialog = DialogService.ShowAsync<ListaTramitesBarco>("Editar Tramite", parameters, options);
            //var result = (await dialog);

            NavigationManager.NavigateTo($"barco/empresa/{CodigoEmpresa}/tramites/{barco.CodigoBarco}");
        }

        public async Task VerUsuariosBarco(EmpresasDto empresa)
        {
            var parameters = new DialogParameters { ["Empresa"] = empresa };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };

            var dialog = DialogService.ShowAsync<CrearUsuario>("Ver y Crear usuarios", parameters, options);
            var result = (await dialog);
        }
    }
}
