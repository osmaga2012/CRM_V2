using CRM.App.Shared.Interfaces;
using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Globalization;

namespace CRM.App.Shared.Pages.Dashboard
{

    public partial class Dashboard : ComponentBase
    {

        [Inject] private IApiClient<BarcosTramitesDto> BarcosTramitesService { get; set; }
        [Inject] private IApiClient<TramiteDto> TramitesService { get; set; }

        private bool isLoading = true;

        private List<BarcosTramitesDto> TodosBarcosTramites { get; set; } = new();
        private List<TramiteDto> TodosTramites { get; set; } = new();

        private List<BarcosTramitesDto> BarcosTramitesProximosVencer { get; set; } = new();
        private List<TramiteDto> TramitesProximosVencer { get; set; } = new();

        private List<BarcosTramitesDto> UltimosBarcosTramites { get; set; } = new();
        private List<TramiteDto> UltimosTramites { get; set; } = new();

        // Charts
        private string[] MesesLabels { get; set; } = Array.Empty<string>();
        // Inicializar la colección para evitar null en el render del MudChart
        private List<ChartSeries> MesesLabels1 { get; set; } = new List<ChartSeries>();
        private double[][] MesesData { get; set; } = Array.Empty<double[]>();

        private string[] EstadoLabels { get; set; } = Array.Empty<string>();
        private double[][] EstadoData { get; set; } = Array.Empty<double[]>();

        private string[] TipoLabels { get; set; } = Array.Empty<string>();
        private double[][] TipoData { get; set; } = Array.Empty<double[]>();

        private bool MostrarEstadosYTipos { get; set; } = false;
        private DateTime CalendarReference { get; set; } = DateTime.Now;
        private Dictionary<DateTime, List<BarcosTramitesDto>> AgendaPorFecha { get; set; } = new();



        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task Reload()
        {
            await LoadData();
            StateHasChanged();
        }

        private async Task LoadData()
        {
            isLoading = true;
            try
            {
                var items = await BarcosTramitesService.GetAllAsync("api/BarcosTramites");
                var tramites = await TramitesService.GetAllAsync("api/Tramites");

                TodosBarcosTramites = items?.ToList() ?? new List<BarcosTramitesDto>();
                TodosTramites = tramites?.ToList() ?? new List<TramiteDto>();


                BuildCharts();
                BuildLists();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error cargando BarcosTramites: {ex.Message}");
                TodosBarcosTramites = new List<BarcosTramitesDto>();


                MesesLabels = Array.Empty<string>();
                MesesLabels1 = new();
                MesesData = Array.Empty<double[]>();
                EstadoLabels = Array.Empty<string>();
                EstadoData = Array.Empty<double[]>();
                TipoLabels = Array.Empty<string>();
                TipoData = Array.Empty<double[]>();
                BarcosTramitesProximosVencer.Clear();
                UltimosTramites.Clear();
            }
            finally
            {
                isLoading = false;
            }
        }

        private void BuildCharts()
        {
            var now = DateTime.UtcNow;
            var months = Enumerable.Range(0, 6)
                                   .Select(i => now.AddMonths(-5 + i))
                                   .ToArray();

            MesesLabels = months.Select(m => m.ToString("MMM yy", CultureInfo.CurrentCulture)).ToArray();

            var mesesCounts = months.Select(m =>
                TodosBarcosTramites.Count(item =>
                {
                    var date = item.FechaCreacionParser ??
                               item.FechaFinParser ??
                               item.FechaInicioParser ??
                               item.FechaCreacionParser;
                    return date.HasValue && date.Value.Month == m.Month && date.Value.Year == m.Year;
                })
            ).Select(c => (double)c).ToArray();

            MesesData = new[] { mesesCounts };

            // rellenar ChartSeries para MudChart
            MesesLabels1 = new List<ChartSeries>
            {
                new ChartSeries { Name = "Trámites", Data = mesesCounts }
            };

            //// Estados
            //var estados = TodosBarcosTramites.GroupBy(t => string.IsNullOrWhiteSpace(t.) ? "Desconocido" : t.Estado)
            //                      .OrderByDescending(g => g.Count())
            //                      .ToList();
            //EstadoLabels = estados.Select(g => g.Key).ToArray();
            //EstadoData = new[] { estados.Select(g => (double)g.Count()).ToArray() };

            // Tipos documento top5
            //var tipos = AllItems.GroupBy(t => string.IsNullOrWhiteSpace(t.TipoDocumento) ? "Otro" : t.TipoDocumento)
            //                    .OrderByDescending(g => g.Count())
            //                    .Take(5)
            //                    .ToList();
            //TipoLabels = tipos.Select(g => g.Key).ToArray();
            //TipoData = new[] { tipos.Select(g => (double)g.Count()).ToArray() };
        }

        private void BuildLists()
        {
            DateTime ahora = DateTime.UtcNow;
            BarcosTramitesProximosVencer = TodosBarcosTramites
                .Where(t => t.FechaFinParser.HasValue)
                .Select(t =>
                {
                    // calcular días con tolerancia de nulls
                    var dias = (int)Math.Ceiling((t.FechaFinParser.Value - ahora).TotalDays);
                    // inyectar propiedad auxiliar en el DTO no es posible aquí; en su lugar usamos LINQ con tuple y luego proyectamos al DTO original
                    return new { Item = t, Dias = dias };
                })
                .Where(x => x.Dias <= 30)
                .OrderBy(x => x.Dias)
                .Take(10)
                .Select(x => x.Item)
                .ToList();

            UltimosBarcosTramites = TodosBarcosTramites
                .OrderByDescending(t => t.FechaInicioParser ?? t.FechaFinParser ?? DateTime.MinValue)
                .Take(8)
                .ToList();

            // Construir agenda por fecha (por FechaFinParser si existe)
            AgendaPorFecha = TodosBarcosTramites
                .Where(t => t.FechaFinParser.HasValue)
                .GroupBy(t => t.FechaFinParser.Value.Date)
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        private void PrevMonth()
        {
            CalendarReference = CalendarReference.AddMonths(-1);
            StateHasChanged();
        }

        private void NextMonth()
        {
            CalendarReference = CalendarReference.AddMonths(1);
            StateHasChanged();
        }

        private void PrevYear()
        {
            CalendarReference = CalendarReference.AddYears(-1);
            StateHasChanged();
        }

        private void NextYear()
        {
            CalendarReference = CalendarReference.AddYears(1);
            StateHasChanged();
        }
    }
}