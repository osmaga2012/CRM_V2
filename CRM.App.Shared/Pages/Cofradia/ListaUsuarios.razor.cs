using Blazored.Modal.Services;
using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using MudBlazor;
using System.Linq;

namespace CRM.App.Shared.Pages.Cofradia
{
    public partial class ListaUsuarios: ComponentBase
    {
        [Inject] private IApiClient<UsuarioDto> usuarioService {  get; set; }
        [Inject] private IApiClient<EmpresasDto> empresasService { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private ISnackbar SnackbarService { get; set; }

        [Parameter] public List<UsuarioDto> Usuarios { get; set; }

        [CascadingParameter] public IModalService Modal { get; set; }

        private UsuarioDto usuario { get; set; } = new UsuarioDto();
        private string _FiltroUsuarios;

        private bool isLoading = true;
        private bool IsEditMode = false;
        private List<EmpresasDto> Empresas = new List<EmpresasDto>();
        private UsuarioDto selectedUsuario;

        // --- Propiedades para el formulario principal ---
        private MudForm form = default!;
        private bool success;
        private string[] errors = { };
        private string? message;
        private bool isSuccess;
        private bool isLoaded = false;
        private bool _fixed = false;
        private string passCache;

        private OverflowBehavior _overflowBehavior = OverflowBehavior.FlipOnOpen;
        private DropdownSettings _dropdownSettings => new DropdownSettings() { Fixed = _fixed, OverflowBehavior = _overflowBehavior, };

        private EmpresasDto _selectedEmpresaCache;
        // Cuando cambia la selección, actualizamos la propiedad string del usuario.
        // Estado de paginación: 10 elementos por página
        PaginationState pagination = new PaginationState { ItemsPerPage = 10 };


        // Filtro para el nombre de usuario
        private string _filtroNombreUsuario = string.Empty;
        private EmpresasDto EmpresaSelecionada
        {
            get
            {
                // 1. Si ya tenemos un valor en caché, lo devolvemos (mejor para la reactividad)
                if (_selectedEmpresaCache != null)
                {
                    return _selectedEmpresaCache;
                }

                // 2. Si la caché está vacía, inicializamos desde el ID (modo Edición)
                if (!String.IsNullOrEmpty(usuario.CodigoEmpresa))
                {
                    // Buscamos y cacheamos el DTO
                    _selectedEmpresaCache = Empresas.FirstOrDefault(e => e.CodigoEmpresa == usuario.CodigoEmpresa);
                    return _selectedEmpresaCache;
                }
                return null;
            }
            set
            {
                // Lógica de escritura/actualización:

                // 1. Actualizar la variable de caché
                _selectedEmpresaCache = value;

                // 2. Actualizar la propiedad real del modelo (el Guid?)
                usuario.CodigoEmpresa = value?.CodigoEmpresa;

                // Nota: No necesitas llamar a StateHasChanged() manualmente aquí, 
                // ya que el @bind-Value del MudAutocomplete ya maneja el rendering.
            }
        }

        protected override async Task OnInitializedAsync()
        {
            //Usuarios = (await usuarioService.GetAllAsync("api/Usuarios")).Where(x=>x.TipoUsuario == TipoUsuarioDto.Administrador).ToList();
            Usuarios = (await usuarioService.GetAllAsync("api/Usuarios")).ToList();
            Empresas = (await empresasService.GetAllAsync("api/Empresa")).ToList();
            var usuarios = (await usuarioService.GetAllAsync("api/Usuarios")).ToList();

            if (Usuarios.Any()) isLoading = false;

            isLoading = false;

            StateHasChanged();
        }
        protected bool FiltroUsuario(UsuarioDto usuario)
        {
            if (string.IsNullOrWhiteSpace(_FiltroUsuarios))
            {
                return true; // Si no hay texto de búsqueda, mostrar todas las empresas
            }

            // Convertir a minúsculas para una búsqueda insensible a mayúsculas/minúsculas
            var textoBusquedaLower = _FiltroUsuarios.ToLowerInvariant();

            // Aquí defines tu lógica de filtro:
            // Por ejemplo, buscar en el nombre O en el sector de la empresa
            return usuario.NombreUsuario.ToLowerInvariant().Contains(textoBusquedaLower);
        }


        public async Task ConfirmarEliminar(UsuarioDto usuario)
        {
            // Opciones para el diálogo de confirmación
            bool? result = await DialogService.ShowMessageBox(
                "Confirmación de Eliminación", // Título del diálogo
                $"¿Estás seguro de que quieres eliminar la empresa '{usuario.NombreUsuario}'?", // Contenido del mensaje
                yesText: "Sí, eliminar",
                cancelText: "Cancelar",
                options: new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true } // Opciones adicionales
            );

            // Comprobar si el usuario confirmó (no canceló y no presionó el botón 'No')
            if (result == true)
            {
                await usuarioService.DeleteAsync("api/Usuarios", usuario.Id);
            }
            else
            {
                SnackbarService.Add("Eliminación cancelada.", Severity.Info);
            }
        }
        private void NuevoUsuario()
        {
            // 1. Instanciamos un nuevo objeto limpio
            usuario = new UsuarioDto
            {
                Activo = true,
                TipoUsuario = CRM.Dtos.Enums.TipoUsuarioDto.Empresa // Valor por defecto
            };

            // 2. Reseteamos la empresa seleccionada si existía
            EmpresaSelecionada = null;

            // 3. Cambiamos el modo de la UI
            IsEditMode = false;

            // 4. Limpiamos visualmente los errores del MudForm
            form?.ResetValidation();

            // Opcional: Si el formulario está en un diálogo o sección oculta, 
            // aquí podrías activar el flag para mostrarlo.
        }

        private async void Guardar(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {

            if (IsEditMode)
            {
                // Lógica para actualizar el usuario existente
                var pass = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash); //PasswordHelper.HashPassword(usuario.PasswordHash);
                usuario.PasswordHash = pass; // No actualizar la contraseña si no se ha cambiado


                await usuarioService.UpdateAsync("api/Usuarios",usuario, usuario.Id);
            }
            else
            {

                var pass = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash); //PasswordHelper.HashPassword(usuario.PasswordHash);
                usuario.PasswordHash = pass; // No actualizar la contraseña si no se ha cambiado
                // Lógica para crear un nuevo usuario
                await usuarioService.CreateAsync("api/Usuarios", usuario);
            }

            StateHasChanged();
        }

        private void OnCancel_Click(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            throw new NotImplementedException();
        }


        private async void CargarDatosUsuario(Guid id)
        {
            usuario = await usuarioService.GetByIdAsync("api/Usuarios", id);
            IsEditMode = true;

            passCache = usuario.PasswordHash;

            StateHasChanged();

        }

        private async Task<IEnumerable<EmpresasDto>> BuscarEmpresa(string value, CancellationToken token)
        {
            // Si el campo de búsqueda está vacío, muestra todos los ítems.
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return Empresas;

            return Empresas.Where(x => x.CodigoEmpresa.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.NombreB.Contains(value,StringComparison.InvariantCultureIgnoreCase));

            // Si no coincide con ningún criterio, ocúltalo.

        }
    }
}
