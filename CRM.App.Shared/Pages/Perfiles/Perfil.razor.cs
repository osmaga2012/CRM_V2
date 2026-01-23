using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Security.Claims;

namespace CRM.App.Shared.Pages.Perfiles
{
    public partial class Perfil: ComponentBase
    {

        [Inject] private IApiClient<UsuarioDto> usuarioService { get; set; }
        [Inject] private AuthenticationStateProvider authenticationStateProvider { get; set; }
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        private string userId { get; set; }
        public bool ModoEdicion = false;
        private bool EstaCargando = true;
        public string Errores;

        private UsuarioDto usuario { get; set; }
        private UsuarioDto _originalUsuario;

        private string fotoBase64; // Para mostrar la imagen en la UI (data URI)
        private string errorSubidaFichero; // Para errores específicos de la subida de archivo
        private long tamañoMaximo = 5 * 1024 * 1024; // 5 MB, ajusta según necesidad
        private string[] tiposPermitidos = new string[] { "image/jpeg", "image/png", "image/gif" };

        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {
                // Obtener el ID del usuario de los claims
                // Asegúrate de que el claim 'NameIdentifier' (o 'sub') esté presente en tu JWT
                userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    Errores = "No se pudo obtener el ID del usuario desde la sesión.";
                    EstaCargando = false;
                    return;
                }
                await LoadUserProfile();
            }
            else
            {
                // Redirigir al login si no está autenticado
                NavManager.NavigateTo("login");
            }
        }

        private async Task LoadUserProfile()
        {
            EstaCargando = true;
            Errores = null;
            try
            {
                // Asegúrate de que tu API tenga un endpoint como /api/Usuario/{id}
                // Y que tu ApiClient esté configurado para manejar GET by ID.
                usuario = await usuarioService.GetByIdAsync("/api/Usuarios", userId); // Asume que GetAllAsync puede filtrar por ID o necesitas un GetByIdAsync
                //usuario = usuarios.FirstOrDefault(); // Asume que solo habrá un usuario con ese ID

                if (usuario == null)
                {
                    Errores = "Perfil no encontrado o ID de usuario inválido.";
                }
                else
                {
                    // Clonar el objeto para permitir revertir cambios al cancelar la edición
                    //_originalUsuario = (UsuarioDto)usuario.MemberwiseClone();
                }

                UpdateProfilePictureDisplay(usuario.FotoPerfil);

                //StateHasChanged();
            }
            catch (Exception ex)
            {
                Errores = $"Error al cargar el perfil: {ex.Message}";
                snackbar.Add($"Error al cargar el perfil: {ex.Message}", Severity.Error);
            }
            finally
            {
                EstaCargando = false;
            }
        }

        private async Task CambiarModoEdicion()
        {
            ModoEdicion = !ModoEdicion;
            if (!ModoEdicion && _originalUsuario != null)
            {
                // Si cancelamos la edición, revertir a los valores originales
                //usuario = (UsuarioDto)_originalUsuario.MemberwiseClone();
                usuario = _originalUsuario;
            }
            Errores = null; // Limpiar mensaje de error al cambiar de modo
            StateHasChanged(); // Forzar la actualización de la UI
        }

        private async Task GuardarCambios()
        {
            EstaCargando = true;
            Errores = null;
            try
            {
                // Llamamos al UpdateAsync de nuestro servicio genérico (IBaseApiService<UsuarioDto>)
                var isSuccess = await usuarioService.UpdateAsync("api/usuarios",usuario, usuario.Id);

                if (isSuccess.Success)
                {
                    snackbar.Add("Perfil actualizado con éxito!", Severity.Success);
                    ModoEdicion = false;
                    // Actualiza el original después de guardar con éxito
                    // Copia manual de propiedades en lugar de MemberwiseClone()
                    _originalUsuario = new UsuarioDto
                    {
                        Id = usuario.Id,
                        EMail = usuario.EMail,
                        NombreUsuario = usuario.NombreUsuario,
                        //Apellidos = usuario.Apellidos,
                        //Telefono = usuario.Telefono
                        // Asegúrate de copiar TODAS las propiedades que existan en UsuarioDto
                        // que desees preservar/revertir
                        FechaUltimoAcceso = usuario.FechaUltimoAcceso
                    };
                }
                else
                {
                    Errores = "Error al actualizar el perfil en el servidor. Verifica la respuesta de la API.";
                    snackbar.Add("Error al actualizar el perfil.", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Errores = $"Error de conexión al guardar: {ex.Message}";
                snackbar.Add($"Error de conexión: {ex.Message}", Severity.Error);
            }
            finally
            {
                EstaCargando = false;
            }
        }


        // Nuevo método para manejar la selección de archivos
        IList<IBrowserFile> _files = new List<IBrowserFile>();
        private async void UploadFiles(IBrowserFile file)
        {
            errorSubidaFichero = null;

            if (file == null) return;

            if (file.Size > tamañoMaximo)
            {
                errorSubidaFichero = $"El tamaño máximo permitido es {tamañoMaximo / (1024 * 1024)} MB.";
                return;
            }

            if (!tiposPermitidos.Contains(file.ContentType))
            {
                errorSubidaFichero = $"Tipo de archivo no permitido. Solo se aceptan imágenes JPEG, PNG o GIF.";
                return;
            }

            try
            {
                using var stream = file.OpenReadStream(tamañoMaximo);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                usuario.FotoPerfil = memoryStream.ToArray();

                UpdateProfilePictureDisplay(usuario.FotoPerfil);

                snackbar.Add("Imagen seleccionada correctamente. ¡No olvides guardar los cambios!", Severity.Info);
            }
            catch (Exception ex)
            {
                errorSubidaFichero = $"Error al leer el archivo: {ex.Message}";
                snackbar.Add($"Error al leer la imagen: {ex.Message}", Severity.Error);
            }
        }
        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            errorSubidaFichero = null;
            var file = e.File;

            if (file == null) return;

            if (file.Size > tamañoMaximo)
            {
                errorSubidaFichero = $"El tamaño máximo permitido es {tamañoMaximo / (1024 * 1024)} MB.";
                return;
            }

            if (!tiposPermitidos.Contains(file.ContentType))
            {
                errorSubidaFichero = $"Tipo de archivo no permitido. Solo se aceptan imágenes JPEG, PNG o GIF.";
                return;
            }

            try
            {
                using var stream = file.OpenReadStream(tamañoMaximo);
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                // Asigna los bytes al DTO del usuario
                usuario.FotoPerfil = memoryStream.ToArray();

                // Actualiza la visualización de la imagen
                UpdateProfilePictureDisplay(usuario.FotoPerfil);

                snackbar.Add("Imagen seleccionada correctamente. ¡No olvides guardar los cambios!", Severity.Info);
            }
            catch (Exception ex)
            {
                errorSubidaFichero = $"Error al leer el archivo: {ex.Message}";
                snackbar.Add($"Error al leer la imagen: {ex.Message}", Severity.Error);
            }
        }

        // Método auxiliar para actualizar la cadena Base64 para la visualización
        private void UpdateProfilePictureDisplay(byte[] imageBytes)
        {
            if (imageBytes != null && imageBytes.Length > 0)
            {
                // Nota: Aquí se asume 'image/jpeg' o 'image/png'. Si la API guarda el tipo de imagen,
                // deberías usar ese tipo dinámicamente aquí.
                fotoBase64 = $"data:image/png;base64,{Convert.ToBase64String(imageBytes)}";
            }
            else
            {
                fotoBase64 = null;
            }
            StateHasChanged(); // Forzar la actualización de la UI del avatar
        }


    }


}
