using CRM.Dtos;
using CRM.Dtos.Enums;
using CRM.Web.Services;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static MudBlazor.Colors;

namespace CRM.App.Shared.Pages.Tramites
{
    public partial class ListadoTramites : ComponentBase
    {

        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IApiClient<TramiteDto> tramiteService { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        private List<TramiteDto> Tramites = new List<TramiteDto>();
        private string SearchString = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            // Simular la carga de datos
            // Aquí es donde llamarías a tu servicio de backend para obtener los trámites reales
            // Por ahora, creamos algunos datos de ejemplo con tu estructura de DTO:

            //Tramites.Add(new TramiteDto
            //{
            //    Codigo = 1,
            //    Descripcion = "Solicitud de subvención anual",
            //    TipoTramiteId = 1, // Asume que ID 1 es "Solicitud"
            //    TipoTramite = new TipoTramiteDto { Id = 1, Nombre = "Solicitud", Descripcion = "Trámite de solicitud" },
            //    FechaInicio = DateTime.Today.AddDays(-10),
            //    FechaFin = null,
            //    Estado = EstadosTramite.EnProceso,
            //    Observaciones = "Pendiente de documentación adicional."
            //});

            //Tramites.Add(new TramiteDto
            //{
            //    Codigo = 2,
            //    Descripcion = "Comunicación de cambio de datos",
            //    TipoTramiteId = 2, // Asume que ID 2 es "Comunicación"
            //    TipoTramite = new TipoTramiteDto { Id = 2, Nombre = "Comunicación", Descripcion = "Trámite informativo" },
            //    FechaInicio = DateTime.Today.AddDays(-5),
            //    FechaFin = DateTime.Today.AddDays(-2),
            //    Estado = EstadosTramite.Cerrado,
            //    Observaciones = "Datos actualizados en el sistema."
            //});

            //Tramites.Add(new TramiteDto
            //{
            //    Codigo = 3,
            //    Descripcion = "Renovación de licencia de pesca",
            //    TipoTramiteId = 1,
            //    TipoTramite = new TipoTramiteDto { Id = 1, Nombre = "Solicitud", Descripcion = "Trámite de solicitud" },
            //    FechaInicio = DateTime.Today.AddDays(-20),
            //    FechaFin = null,
            //    Estado = EstadosTramite.Abierto,
            //    Observaciones = "Esperando la firma del director."
            //});
            await CargarTramites();
            StateHasChanged();

        }

        private async Task CargarTramites()
        {
            Tramites = (await tramiteService.GetAllAsync("/api/Tramites")).ToList();
            Snackbar.Add("Trámites cargados con éxito.", Severity.Success);
        }

        private async Task AgregarTramite()
        {
            //var resultado = DialogService.Show<MntTramite>("Nuevo tramite");
            //DialogResult result = await resultado.Result;

            //if (!result.Canceled && (bool)result.Data)
            //{
            //    Snackbar.Add("Tramite agregado correctamente.", Severity.Success);
            //    await CargarTramites(); // <<--- ¡Aquí se recarga la lista!
            //}
            //else if (result.Canceled)
            //{
            //    Snackbar.Add("Creación de tipo de trámite cancelada.", Severity.Info);
            //}

            NavigationManager.NavigateTo("tramite");

        }

        private void EditTramite(int codigo)
        {
            NavigationManager.NavigateTo($"/tramites/editar/{codigo}");
        }

        private async Task EditTramite(Guid id) 
        {
            //var parameters = new DialogParameters { ["IdTramite"] = id };
            //var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };

            //var dialog = DialogService.ShowAsync<MntTramite>("Editar Tramite", parameters, options);
            //var result = (await dialog);

            NavigationManager.NavigateTo($"tramite/{id}");

        }

        private void DeleteTramite(int codigo)
        {


            // Implementa aquí la lógica para eliminar un trámite por su código.
            // Después de la eliminación, deberías recargar la lista o eliminar el elemento de 'Tramites'.
            Snackbar.Add($"Funcionalidad de eliminación para el trámite {codigo} no implementada.", Severity.Warning);
        }
        private async Task DeleteTramite(Guid id, string nombreTramite)
        {
            // En lugar de DialogParameters, pasamos directamente el título, el mensaje
            // y el texto de los botones al ShowMessageBox.
            bool? dialogResult = await DialogService.ShowMessageBox(
                "Confirmar eliminación", // Título del cuadro de mensaje
                $"¿Estás seguro de que quieres eliminar el trámite {nombreTramite}? Esta acción no se puede deshacer.", // Contenido del mensaje
                yesText: "Eliminar", // Texto del botón de confirmación (afirmativo)
                cancelText: "Cancelar" // Texto del botón de cancelar (negativo/cancelar)
                                       //color: Color.Error // Color del botón principal (Eliminar)
            );

            // Si el usuario hace clic en "Eliminar" (o el botón principal del MessageBox)
            if (dialogResult == true) // MudMessageBox devuelve 'true' si se presiona el botón principal
            {
                await Borrar(id);
            }
            else
            {
                Snackbar.Add("Eliminación cancelada.", Severity.Info);
            }
        }

        private async Task Borrar(Guid id)
        {
            try
            {
                var respuesta = await tramiteService.DeleteAsync("api/Tramites", id); 

                if (respuesta.Success)
                {
                    Snackbar.Add("Trámite eliminado correctamente.", Severity.Success);
                    await CargarTramites(); // Recargar la lista después de la eliminación
                }
                else
                {
                    Snackbar.Add(respuesta.Message, Severity.Success);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error al eliminar el tipo de trámite: {ex.Message}", Severity.Error);
            }
        }

        // Filtro rápido para el MudDataGrid
        private Func<TramiteDto, bool> QuickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(SearchString))
                return true;

            if (x.Codigo.ToString().Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (x.NombreTramite?.Contains(SearchString, StringComparison.OrdinalIgnoreCase) == true)
                return true;
            //if (x.TipoTramite?.Nombre?.Contains(SearchString, StringComparison.OrdinalIgnoreCase) == true)
            //    return true;
            //if (x.Estado.ToString().Contains(SearchString, StringComparison.OrdinalIgnoreCase) == true)
            //    return true;
            //if (x.Observaciones?.Contains(SearchString, StringComparison.OrdinalIgnoreCase) == true)
            //    return true;

            return false;
        };


    }
}
