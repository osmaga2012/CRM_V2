using CRM.App.Shared.Interfaces;
using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Mail;
using System.Security.Claims;

namespace CRM.App.Shared.Pages.Empresa
{
    public partial class FormularioRecordatorio : ComponentBase
    {
        // 🚀 NUEVA INYECCIÓN/CASCADA PARA OBTENER DATOS DEL USUARIO
        [CascadingParameter] private Task<AuthenticationState> AuthStateTask { get; set; } = default!;
        [Inject] ISnackbar Snackbar { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IApiClient<Empresa_RecordatoriosAvisosDto> servicioRecordatorios { get; set; }
        [Inject] IJSRuntime JsRuntime { get; set; } = default!;
        [Inject] IDialogService DialogService { get; set; } = default!;
        [Inject] private ICurrentUserService currentUserService { get; set; } = default!;

        [Parameter] public Guid Id { get; set; } // Parámetro de ruta para edición


        private UsuarioDto CurrentUser = default!; // Para almacenar el usuario actual
        private string CodigoEmpresa = string.Empty;
        private string? _userId;
        private Empresa_RecordatoriosAvisosDto recordatorio { get; set; } = new Empresa_RecordatoriosAvisosDto();
        private DateTime? FechaSeleccionada { get; set; }


        // ---- Campos/estado ----
        private string _emailInput = string.Empty;
        private string _emailError = string.Empty;
        private List<string> EmailsAviso = new();


        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateTask;
            _ = CargarUsuarioAsync();

            if (Id != Guid.Empty)
            {
                try
                {
                    // Lógica para cargar el registro desde la API
                    var data = await servicioRecordatorios.GetByIdAsync($"/api/RecordatoriosAvisos", Id);
                    if (data != null)
                    {
                        recordatorio = data;
                        FechaSeleccionada = recordatorio.FechaFinalizacion;
                    }
                    else
                    {
                        Snackbar.Add("Recordatorio no encontrado.", Severity.Error);
                        NavigationManager.NavigateTo("/ruta-listado-recordatorios"); // Redirigir si no existe
                    }
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error al cargar: {ex.Message}", Severity.Error);
                }
            }
            else
            {
                // Si es nuevo, inicializar la fecha a hoy
                FechaSeleccionada = DateTime.Today;
            }
        }

        private async Task CargarUsuarioAsync()
        {
            var authState = await AuthStateTask;
            var user = authState.User;

            if (user?.Identity?.IsAuthenticated == true)
            {

                // Si está autenticado, intenta obtener el perfil.
                // Esta llamada usará el HttpClient con TokenRefreshHandler.
                CurrentUser = await currentUserService.GetCurrentUserAsync(user);
            }
        }
        private async void Guardar()
        {
            if (FechaSeleccionada.HasValue)
            {
                recordatorio.FechaFinalizacion = FechaSeleccionada.Value;
            }

            bool isNew = recordatorio.Id == Guid.Empty;
            string accion = isNew ? "creado" : "actualizado";

            try
            {
                if (isNew)
                {
                    // 1. Guardar/Crear nuevo registro (asume que devuelve el objeto con el ID)
                    recordatorio.Id = Guid.NewGuid(); // Generar un nuevo GUID para el ejemplo
                    recordatorio.idUsuario = CurrentUser.Id;
                    recordatorio.CodigoEmpresa = CurrentUser.CodigoEmpresa;
                    recordatorio.ListaEmailsEnvioAviso = string.Join(';', EmailsAviso);

                    var respuesta = await servicioRecordatorios.CreateAsync("/api/EmpresaRegistroAvisos", recordatorio);
                    //recordatorio = nuevoModelo; // Actualizar el modelo con el ID generado

                    if (respuesta.Success)
                    {
                        //recordatorio.Id = ((Empresa_RecordatoriosAvisosDto)respuesta.Data! ).Id;
                        Snackbar.Add($"Recordatorio '{recordatorio.Nombre}' {accion} con éxito.", Severity.Success);
                        // 2. Preguntar y agregar al calendario
                        //await PreguntarYAgregarAlCalendario();
                    }
                    else
                    {
                        Snackbar.Add($"Error al crear el recordatorio: {respuesta.Message}", Severity.Error);
                    }


                }
                else
                {
                    // 1. Actualizar registro existente
                    var respuesta = await servicioRecordatorios.UpdateAsync($"/api/EmpresaRegistroAvisos", recordatorio);
                    if (respuesta.Success)
                    {
                        Snackbar.Add($"Recordatorio '{recordatorio.Nombre}' {accion} con éxito.", Severity.Success);
                    }
                    else
                    {
                        Snackbar.Add($"Error al actualizar el recordatorio: {respuesta.Message}", Severity.Error);
                    }

                }
                // 3. Redirigir al listado después de guardar/actualizar
                NavigationManager.NavigateTo("/mis-tramites");
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error al guardar el recordatorio: {ex.Message}", Severity.Error);
            }
        }

        /// <summary>
        /// Muestra un diálogo de confirmación para preguntar si grabar en el calendario.
        /// </summary>
        private async Task PreguntarYAgregarAlCalendario()
        {
            // 🚀 CAMBIO: Usamos DialogService.ShowMessageBox para diálogos sencillos.
            // Este método maneja internamente la UI y los botones OK/Cancel.

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            var result = await DialogService.ShowMessageBox(
                "Guardar en Calendario", // Título del Diálogo
                "¿Deseas agregar este recordatorio/aviso a tu calendario personal para no olvidarlo?", // Contenido del mensaje
                yesText: "Sí, Agregar al Calendario", // Texto para el botón OK
                cancelText: "Ahora no", // Texto para el botón Cancelar
                options: options
            );

            // ShowMessageBox devuelve 'true' si se pulsa yesText, o 'false' si se pulsa cancelText o se cierra.
            if (result.GetValueOrDefault() == true)
            {
                await AgregarAlCalendarioAsync();
            }
        }

        /// <summary>
        /// Llama a la funcionalidad JS para crear el evento en el calendario.
        /// (Mismo método JS Interop que en la respuesta anterior)
        /// </summary>
        private async Task AgregarAlCalendarioAsync()
        {
            var titulo = $"Recordatorio: {recordatorio.Nombre} ({recordatorio.CodigoBarco})";
            var descripcion = recordatorio.Descripcion;
            var fecha = recordatorio.FechaFinalizacion.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                // Asegúrate de tener la función 'calendarInterop.createEvent' definida en tu JS.
                await JsRuntime.InvokeVoidAsync("calendarInterop.createEvent", titulo, descripcion, fecha);
                Snackbar.Add("El evento se ha generado para ser agregado a tu calendario.", Severity.Info);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error al intentar agregar al calendario: {ex.Message}", Severity.Warning);
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
    }
}
