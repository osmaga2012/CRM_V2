using CRM.App.Shared.Interfaces;
using CRM.Dtos;
using CRM.Dtos.Response;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Security.Claims;

namespace CRM.App.Shared.Pages.CentrosTrabajo
{
    public partial class ListaTramitesBarco: ComponentBase
    {
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
        [CascadingParameter] private Task<AuthenticationState> AuthStateTask { get; set; } = default!;

        [Inject] private IApiClient<BarcosTramitesDto> servicioTramites { get; set; }
        [Inject] private IApiClient<ConfiguracionDto> servicioConfiguracion { get; set; }
        [Inject] private IApiClient<BarcosDto> servicioBarcos { get; set; }
        [Inject] private IApiClient<EmpresasDto> servicioEmpresa { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private IHttpClientFactory HttpClientFactory { get; set; } = default!;
        [Inject] private IJSRuntime JsRuntime { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IConfiguration Configuration { get; set; } = default!;
        [Inject] private ICurrentUserService currentUserService { get; set; } = default!;


        [Parameter] public string CodigoBarco { get; set; }
        [Parameter] public string CodigoEmpresa { get; set; }
        //[Parameter] public Guid? id { get; set; }
        [Parameter] public BarcosDto barco { get; set; }
        [Parameter] public Guid IdBarco { get; set; }

        private ClaimsPrincipal? _user;
        private string? _userId;
        private string? _userEmail;

        private ICollection<BarcosTramitesDto> tramites { get; set; } = default!;
        private BarcosTramitesDto tramite = new();
        private ConfiguracionDto configuracion = new();
        private string _filtroTramites = string.Empty;
        private MudForm form = default!;
        private bool success, ModoAlta;
        private string[] errors = { };
        private IBrowserFile? selectedFile;
        private string? selectedFileName;
        private string? existingFileName;
        private const long MaxFileSize = 20 * 1024 * 1024;
        private const string UploadEndpoint = "api/BarcosTramites/upload";
        private string? publicFilesBaseUrl;
        private string? _selectedFileName;
        private bool _confirmVisible;
        private BarcosTramitesDto _toDelete;
        private string emailEmpresa = "";

        private Breakpoint _bp = Breakpoint.Xs;
        private bool IsMobile = true;
        private bool IsEditMode => !ModoAlta;


        // ---- Campos/estado ----
        private string _emailInput = string.Empty;
        private string _emailError = string.Empty;
        private List<string> EmailsAviso = new();

        private void OnBreakpointChanged(Breakpoint breakpoint)
        {
            IsMobile = breakpoint < Breakpoint.Md;
            StateHasChanged();
        }        

        protected override async Task OnInitializedAsync()
        {
            publicFilesBaseUrl = Configuration["FileSettings:PublicBaseUrl"];
            IsMobile = _bp < Breakpoint.Md;

            var query = new Dictionary<string, string>();
            query.Add("CodigoEmpresa", CodigoEmpresa);

            //emailEmpresa = (await servicioEmpresa.GetAllAsync("api/Empresa/codigo")).FirstOrDefault().EMail1;
            await CargarTramites();
            _ = CargarUsuarioAsync();

            ////EmailsAviso.Clear();
            ////EmailsAviso.Add(emailEmpresa);

            StateHasChanged();
        }

        private async Task CargarUsuarioAsync()
        {
            var authState = await AuthStateTask;
            var user = authState.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                _user = user;

                // Intenta varios tipos de claim habituales
                _userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? user.FindFirst("sub")?.Value
                       ?? user.FindFirst("user_id")?.Value;

                _userEmail = user.FindFirst(ClaimTypes.Email)?.Value
                          ?? user.Identity?.Name;
            }
            else
            {
                _user = null;
                _userId = null;
                _userEmail = null;
            }
        }

        private async Task CargarTramites()
        {

            //Cargamos los datos del barco de la empresa seleccionada.
            barco = (await servicioBarcos.GetByIdAsync("api/Barcos/codigo", CodigoBarco));

            //cargamos la configuracion
            var configuraciones = await servicioConfiguracion.GetAllAsync("api/Configuracion")
                ?? Enumerable.Empty<ConfiguracionDto>();
            configuracion = configuraciones.FirstOrDefault() ?? new ConfiguracionDto();

            var filtro = new Dictionary<string, string>();
            filtro.Add("CodigoBarco", CodigoBarco);
            filtro.Add("CodigoEmpresa", CodigoEmpresa);

            tramites = (await servicioTramites.GetAllAsync("api/BarcosTramites",filtro)).ToList();

            tramite = new();
            tramite.DiasAvisoTramite = configuracion == null? 0: configuracion.DiasAntelacionAviso;

            ModoAlta = true;
            selectedFile = null;
            selectedFileName = null;
            existingFileName = null;
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

        private async Task EditarTramite (Guid Id)
        {

            // Encuentra el trámite en la colección 'tramites' por su Id
            var selectedTramite = tramites.FirstOrDefault(t => t.Id == Id);

            if (selectedTramite != null)
            {
                // Asigna una COPIA del trámite encontrado a 'tramite'
                // Esto es importante para que los cambios en el formulario no afecten directamente
                // al objeto en la lista hasta que se guarde.

                EmailsAviso = ParseEmails(selectedTramite?.ListaEmailsEnvioAviso).ToList();
                tramite = new BarcosTramitesDto
                {
                    Id = selectedTramite.Id,
                    CodigoBarco = selectedTramite.CodigoBarco,
                    CensoBarco = selectedTramite.CensoBarco,
                    Certificado = selectedTramite.Certificado,
                    FechaCreacion = selectedTramite.FechaCreacion,
                    FechaInicio = selectedTramite.FechaInicio,
                    FechaFin = selectedTramite.FechaFin,
                    FechaAviso = selectedTramite.FechaAviso,
                    UltimoEnvio = selectedTramite.UltimoEnvio,
                    //IdBarco  = selectedTramite.IdBarco,
                    IdGestoria = selectedTramite.IdGestoria,
                    EmpresaId = selectedTramite.EmpresaId,
                    idUsuario = selectedTramite.idUsuario,
                    //IdUsuarioCreacion = selectedTramite.IdUsuarioCreacion,
                    DiasAvisoTramite = selectedTramite.DiasAvisoTramite,
                    Observaciones = selectedTramite.Observaciones,
                    RutaFichero = selectedTramite.RutaFichero,
                    ListaEmailsEnvioAviso = EmailsAviso.ToString()

                    //ParseEmails(tramite?.ListaEmailsEnvioAviso).ToList();
                };

                ModoAlta = false; // Estamos en modo edición
                form?.ResetValidation(); // Limpia cualquier mensaje de validación anterior
                existingFileName = GetFileName(tramite.RutaFichero);
                selectedFile = null;
                selectedFileName = null;
                StateHasChanged(); // Forzar el refresco de la UI
            }
        }

        private async Task BorrarTramite(BarcosTramitesDto tramite)
        {

            // En lugar de DialogParameters, pasamos directamente el título, el mensaje
            // y el texto de los botones al ShowMessageBox.
            bool? dialogResult = await DialogService.ShowMessageBox(
                "Confirmar eliminación", // Título del cuadro de mensaje
                $"¿Estás seguro de que quieres eliminar el certificado {tramite.Certificado}? Esta acción no se puede deshacer.", // Contenido del mensaje
                yesText: "Eliminar", // Texto del botón de confirmación (afirmativo)
                cancelText: "Cancelar" // Texto del botón de cancelar (negativo/cancelar)
                                       //color: Color.Error // Color del botón principal (Eliminar)
            );

            // Si el usuario hace clic en "Eliminar" (o el botón principal del MessageBox)
            if (dialogResult == true) // MudMessageBox devuelve 'true' si se presiona el botón principal
            {
                await BorraTramiteId(tramite.Id);
            }
            else
            {
                Snackbar.Add("Eliminación cancelada.", Severity.Info);
            }

        }

        private async Task BorraTramiteId(Guid Id)
        {
            try
            {
                await servicioTramites.DeleteAsync("api/BarcosTramites", Id); ;
                Snackbar.Add("Tramite eliminado correctamente.", Severity.Success);
                await CargarTramites(); // Recargar la lista después de la eliminación
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error al eliminar el tipo de trámite: {ex.Message}", Severity.Error);
            }
        }

        // ---- Manejo de teclado (Enter) en el TextField ----
        private void OnEmailKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                AddEmailsFromInput();
                // No hace falta preventDefault aquí; evitamos efectos colaterales limpiando input.
            }
        }

        // ---- Alta/baja de chips ----
        private void AddEmailsFromInput()
        {
            _emailError = string.Empty;

            if (string.IsNullOrWhiteSpace(_emailInput))
                return;

            // Permite pegar varios: separadores ; , espacio o salto de línea
            var nuevos = ParseEmails(_emailInput);

            var added = false;
            foreach (var email in nuevos)
            {
                if (!IsValidEmail(email))
                {
                    _emailError = $"Email no válido: {email}";
                    continue;
                }

                // Evita duplicados (case-insensitive)
                if (!EmailsAviso.Contains(email, StringComparer.OrdinalIgnoreCase))
                {
                    EmailsAviso.Add(email);
                    added = true;
                }
            }

            if (added)
            {
                // ordenar opcionalmente
                EmailsAviso = EmailsAviso.OrderBy(x => x).ToList();
            }

            _emailInput = string.Empty;
            StateHasChanged();
        }

        private void RemoveEmail(string email)
        {
            EmailsAviso.RemoveAll(x => string.Equals(x, email, StringComparison.OrdinalIgnoreCase));
            StateHasChanged();
        }

        // ---- Utilidades ----
        private static IEnumerable<string> ParseEmails(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return Enumerable.Empty<string>();

            var tokens = raw
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Split(new[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t));

            // Distinct case-insensitive
            return tokens.Distinct(StringComparer.OrdinalIgnoreCase);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return string.Equals(addr.Address, email, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        private async Task OnFileChanged(InputFileChangeEventArgs e)
        {
            if (e.FileCount == 0)
            {
                await ClearSelectedFile();
                return;
            }

            var file = e.File;
            if (file is null)
            {
                await ClearSelectedFile();
                return;
            }

            if (file.Size > MaxFileSize)
            {
                var maxSizeInMb = MaxFileSize / (1024 * 1024);
                Snackbar.Add($"El archivo supera el tamaño máximo permitido de {maxSizeInMb} MB.", Severity.Warning);
                await ClearSelectedFile();
                return;
            }

            selectedFile = file;
            selectedFileName = file.Name;
            existingFileName = null;
        }

        private Task ClearSelectedFile()
        {
            selectedFile = null;
            selectedFileName = null;
            existingFileName = null;
            tramite.RutaFichero = null;
            return Task.CompletedTask;
        }

        private async Task<bool> TryUploadAttachmentAsync()
        {
            if (selectedFile is null)
            {
                return true;
            }

            try
            {
                var response = await servicioTramites.UploadFileAsync(UploadEndpoint, selectedFile, MaxFileSize);
                if (response is null || !response.Success)
                {
                    var message = response?.Message;
                    Snackbar.Add(string.IsNullOrWhiteSpace(message) ? "No se pudo subir el archivo adjunto." : message, Severity.Error);
                    return false;
                }

                tramite.RutaFichero = response.Data?.ToString();
                existingFileName = GetFileName(tramite.RutaFichero) ?? selectedFile.Name;
                selectedFile = null;
                selectedFileName = null;
                return true;
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error al subir el archivo: {ex.Message}", Severity.Error);
                return false;
            }
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

        private void BorrarConfirmado()
        {
            _confirmVisible = false;
            if (_toDelete is null) return;
            // TODO: Borrar
        }

        private async void ConfirmarBorrado(BarcosTramitesDto item)
        {
            //_toDelete = item;
            //_confirmVisible = true;
            //StateHasChanged();


            var contenido = (MarkupString)$"¿Seguro que quieres borrar <b>{item.Certificado}</b>? Esta acción no se puede deshacer.";

            var options = new DialogOptions
            {
                CloseButton = false,
                MaxWidth = MaxWidth.ExtraSmall,
                FullWidth = true,
                BackdropClick = false,
            };

            bool? result = await DialogService.ShowMessageBox(
                title: "Confirmar eliminación",
                message: contenido.ToString(),                 // <-- MarkupString
                yesText: "Eliminar",
                cancelText: "Cancelar",
                options: options
            );

            if (result == true)
                await BorraTramiteId(item.Id);
            else
                Snackbar.Add("Eliminación cancelada.", Severity.Info);
        }

        private async Task Submit()
        {
            ResponseDto respuesta = new();
            await form.Validate();
            if (!form.IsValid)
            {
                Snackbar.Add("Por favor, corrige los errores del formulario.", Severity.Warning);
                return;
            }

            if (!await TryUploadAttachmentAsync())
            {
                return;
            }

            try
            {
                //tramite = barco;
                //tramite.IdBarco = barco.Id;
                tramite.CodigoEmpresa = CodigoEmpresa;
                tramite.FechaAviso = tramite.FechaFin.AddDays(tramite.DiasAvisoTramite * -1);
                tramite.ListaEmailsEnvioAviso = string.Join(';', EmailsAviso);
                if (!ModoAlta && tramite.Id != Guid.Empty)
                {

                    respuesta = await servicioTramites.UpdateAsync("api/BarcosTramites", tramite, tramite.Id);
                }
                else
                {
                    tramite.CodigoBarco = barco.CodigoBarco;
                 
                    if (barco.Censo.HasValue) tramite.CensoBarco = (int)barco.Censo.Value;

                    tramite.CodigoEmpresa = CodigoEmpresa;
                    //tramite.IdUsuarioCreacion = Guid.Parse(_userId);
                    //tramite.IdGestoria = barco.IdGestoria;

                    respuesta = await servicioTramites.CreateAsync("api/BarcosTramites", tramite);
                    //response = await Http.PostAsJsonAsync("api/tramite", tramiteFormModel);
                    
                }

                if (respuesta.Success)
                {
                    await form.ResetAsync();
                    Snackbar.Add(ModoAlta ? "Trámite creado correctamente." : "Trámite actualizado correctamente", Severity.Success);
                    existingFileName = GetFileName(tramite.RutaFichero);
                    selectedFile = null;
                    selectedFileName = null;
                    //MudDialog.Close(DialogResult.Ok(true)); // Cerrar diálogo con éxito
                    //StateHasChanged();
                    //EmailsAviso.Clear();
                    //EmailsAviso.Add(emailEmpresa);

                }

                await CargarTramites();

            }
            catch (Exception ex)
            {
                Snackbar.Add($"Ocurrió un error inesperado: {ex.Message}", Severity.Error);
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

        private async Task DownloadAdjuntoAsync(BarcosTramitesDto tramite)
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

            try
            {
                var client = HttpClientFactory.CreateClient("ApiClient");
                var requestUrl = $"api/BarcosTramites/download?filePath={Uri.EscapeDataString(tramite.RutaFichero)}";

                using var response = await client.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);

                if (response.Content.Headers.ContentType?.MediaType?.Contains("json", StringComparison.OrdinalIgnoreCase) == true)
                {
                    var payload = await response.Content.ReadFromJsonAsync<ResponseDto>();
                    if (payload is not null && !payload.Success)
                    {
                        var errorMessage = string.IsNullOrWhiteSpace(payload.Message)
                            ? "No se pudo descargar el adjunto."
                            : payload.Message;
                        Snackbar.Add(errorMessage, Severity.Error);
                        return;
                    }

                    Snackbar.Add("No se pudo descargar el adjunto.", Severity.Error);
                    return;
                }

                if (!response.IsSuccessStatusCode)
                {
                    Snackbar.Add($"No se pudo descargar el adjunto. Código {(int)response.StatusCode}.", Severity.Error);
                    return;
                }

                var fileName = response.Content.Headers.ContentDisposition?.FileNameStar
                    ?? response.Content.Headers.ContentDisposition?.FileName
                    ?? GetFileName(tramite.RutaFichero)
                    ?? "adjunto";

                fileName = fileName.Trim('"');

                var stream = await response.Content.ReadAsStreamAsync();
                using var streamRef = new DotNetStreamReference(stream);
                var module = await JsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/downloadHelper.js");
                await module.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error al descargar el adjunto: {ex.Message}", Severity.Error);
            }
        }
    }
}
