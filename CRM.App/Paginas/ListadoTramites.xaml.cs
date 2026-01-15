using CRM.App.Shared.Modelos;
using CRM.App.Sharted.Services;
using CRM.Dtos;
using CRM.Web.Shared.Interfaces;

namespace CRM.App.Paginas;

public partial class ListadoTramites : ContentPage
{
    private readonly IApiClient<TramiteDto> tramitesService;
    private readonly IApiClient<BarcosTramitesDto> tramitesBarcoService;

    public ListadoTramites(IApiClient<TramiteDto> tramitesService, IApiClient<BarcosTramitesDto> tramitesBarcoService)
	{
		InitializeComponent();
        this.tramitesService = tramitesService;
        this.tramitesBarcoService = tramitesBarcoService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ =  CargarDatos();
    }

    private async Task CargarDatos()
    {
        try
        {
            // Ejemplo de uso de tu API genérica
            var tramites = await tramitesService.GetAllAsync("api/tramites");
            var barcosTramite = await tramitesBarcoService.GetAllAsync("api/BarcosTramites");

            // 2. Unificar usando LINQ
            var listaUnificada = new List<TramiteDisplayItem>();

            // Transformamos los trámites normales
            var items1 = tramites.Select(t => new TramiteDisplayItem
            {
                ///Id = t.Id,
                Nombre = t.NombreTramite, // Ajusta a tus nombres reales de propiedad
                Tipo = 1,
                Detalles = t.NombreTramite,
                FechaFinalizacion = t.FechaFin.Value.ToShortDateString()
            });

            // Transformamos los trámites de barcos
            var items2 = barcosTramite.Select(b => new TramiteDisplayItem
            {
                //Id = b.Id,
                Nombre = b.Certificado,
                Tipo = 0,
                Detalles = b.Observaciones,
                FechaFinalizacion = b.FechaFin.ToShortDateString()
            });

            var resultadoFinal = items1.Concat(items2).OrderBy(x => x.Nombre).ToList();
            // Asignar a la UI (suponiendo que tienes un CollectionView llamado 'Lista')
            Lista.ItemsSource = resultadoFinal;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los trámites. {ex.Message}", "OK");
        }
    }
}