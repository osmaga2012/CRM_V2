using CRM.App.Shared.Interfaces;
using CRM.App.Shared.Services;
using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using CRM.App.Shared.Componentes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Charts;
namespace CRM.App.Shared.Pages.CentrosTrabajo
{

    // 🚀 NUEVA ENUMERACIÓN PARA EL SELECTOR DE VISTA
    public enum TipoVista
    {
        Tramites,
        Registros
    }
    public partial class ListadoTramitesBarcoEmpresa : ComponentBase
    {
        [CascadingParameter] private Task<AuthenticationState> AuthStateTask { get; set; } = default!;
        [Inject] private IFormFactor PlatformService { get; set; } = default!;
        [Inject] private IPlatformNavigationService NavigationService { get; set; } = default!;
        [Inject] private IApiClient<BarcosTramitesDto> servicioTramites { get; set; }
        [Inject] private IApiClient<Empresa_RecordatoriosAvisosDto> servicioAvisosRecordatorios { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IJSRuntime JsRuntime { get; set; } = default!;

        private ICollection<BarcosTramitesDto> tramites { get; set; } = default!;
        private ICollection<Empresa_RecordatoriosAvisosDto> ListaRegistros { get; set; } = default!;

        private Breakpoint _bp = Breakpoint.Xs;
        private bool IsMobile = true;
        private string? publicFilesBaseUrl;
        private string? _selectedFileName;

        private string _filtroTramites = string.Empty;

        // ESTADO DE LA VISTA
        public TipoVista VistaSeleccionada { get; set; } = TipoVista.Tramites; // 🚀 NUEVA PROPIEDAD

        private void OnBreakpointChanged(Breakpoint breakpoint)
        {
            IsMobile = breakpoint < Breakpoint.Md;
            StateHasChanged();
        }
        override protected async Task OnInitializedAsync()
        {
            var authState = await AuthStateTask;
            var user = authState.User;
            //if (user.Identity is not null && user.Identity.IsAuthenticated)
            //{
            //    var empresaIdClaim = user.Claims.FirstOrDefault(c => c.Type == "EmpresaId");
            //    if (empresaIdClaim is not null)
            //    {
            //        int empresaId = int.Parse(empresaIdClaim.Value);
            //        var tramites = await servicioTramites.GetAllAsync($"/api/BarcosTramites/empresa/{empresaId}");
            //        tramites = tramites.ToList();
            //    }
            //}
            tramites = (await servicioTramites.GetAllAsync("api/BarcosTramites")).ToList();
            ListaRegistros = (await servicioAvisosRecordatorios.GetAllAsync("api/EmpresaRecordatoriosAvisos")).ToList();
            StateHasChanged();

        }


        protected bool FiltroTramites(BarcosTramitesDto tramite)
        {
            if (string.IsNullOrWhiteSpace(_filtroTramites))
            {
                return true; // Si no hay texto de búsqueda, mostrar todas las empresas
            }

            // Convertir a minúsculas para una búsqueda insensible a mayúsculas/minúsculas
            var textoBusquedaLower = _filtroTramites.ToLowerInvariant();

            // Aquí defines tu lógica de filtro:
            // Por ejemplo, buscar en el nombre O en el sector de la empresa

            return tramite.Certificado.ToLowerInvariant().Contains(textoBusquedaLower);


            /*return barco.CodigoBarco.ToString().ToLowerInvariant().Contains(textoBusquedaLower) ||
                   barco.NombreA.ToLowerInvariant().Contains(textoBusquedaLower) || barco.CapitanNombre.ToLowerInvariant().Contains(textoBusquedaLower);*/
            //return true;
        }

        // 2. FILTRO DE REGISTROS (Implementación similar, adaptada al nuevo DTO)
        // -------------------------------------------------------------------
        public bool FiltroRegistros(Empresa_RecordatoriosAvisosDto registro)
        {
            if (string.IsNullOrWhiteSpace(_filtroTramites)) // Usamos el mismo input de filtro
            {
                return true;
            }

            var textoBusquedaLower = _filtroTramites.ToLowerInvariant();
            // Filtrar por el nombre del registro o detalles
            return registro.Descripcion.ToLowerInvariant().Contains(textoBusquedaLower)
                || (registro.Nombre?.ToLowerInvariant().Contains(textoBusquedaLower) ?? false);
        }

        private void AgregarRegistro()
        {
            // Lógica para agregar un nuevo registro.
            // Esto típicamente abriría un modal o navegaría a una página de formulario.

            Snackbar.Add("Abriendo formulario para crear nuevo registro...", Severity.Info);
            NavigationManager.NavigateTo("mis-registros");
            // Ejemplo de Navegación:
            // NavigationManager.NavigateTo("/registros/crear");

            // Ejemplo de apertura de un modal (requeriría MudDialogService):
            // DialogService.Show<FormularioRegistro>("Crear Registro");
        }


        private static string? GetFileName(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            try
            {
                return Path.GetFileName(path);
            }
            catch
            {
                return path;
            }
        }

        private async Task OpenAdjuntoAsync(BarcosTramitesDto tramite)
        {
            if (tramite is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(tramite.RutaFichero))
            {
                Snackbar.Add("El trámite no tiene un adjunto disponible.", Severity.Info);
                return;
            }

            var fileUrl = BuildAttachmentUrl(tramite.RutaFichero);
            if (string.IsNullOrWhiteSpace(fileUrl))
            {
                Snackbar.Add("No se pudo determinar la URL pública del adjunto.", Severity.Error);
                return;
            }

            try
            {
                await JsRuntime.InvokeVoidAsync("open", fileUrl, "_blank");
            }
            catch (Exception ex)
            {
                Snackbar.Add($"No se pudo abrir el adjunto: {ex.Message}", Severity.Error);
            }
        }

        private string? BuildAttachmentUrl(string? originalPath)
        {
            var relativePath = GetRelativeFilePath(originalPath);
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return null;
            }

            var baseUrl = publicFilesBaseUrl;
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                baseUrl = NavigationManager.BaseUri;
            }

            if (!baseUrl.EndsWith('/'))
            {
                baseUrl += "/";
            }

            return $"{baseUrl}{relativePath.TrimStart('/')}";
        }

        private static string? GetRelativeFilePath(string? fullPath)
        {
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                return null;
            }

            var normalizedPath = fullPath.Replace('\\', '/');
            const string marker = "/Ficheros/";

            var markerIndex = normalizedPath.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (markerIndex >= 0)
            {
                return normalizedPath[markerIndex..];
            }

            if (!normalizedPath.StartsWith('/'))
            {
                normalizedPath = "/" + normalizedPath;
            }

            return normalizedPath;
        }
    }
}
