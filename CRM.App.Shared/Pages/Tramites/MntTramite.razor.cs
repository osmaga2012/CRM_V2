using CRM.Dtos;
using CRM.Dtos.Response;

using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CRM.App.Shared.Pages.Tramites
{
    public partial class MntTramite : ComponentBase
    {
        [Inject] private IApiClient<TramiteDto> servicioTramite { get; set; }
        [Inject] private IApiClient<EmpresasDto> servicioEmpresas { get; set; }
        [Inject] private IApiClient<PersonasDto> servicioPersonas { get; set; }
        [Inject] private IApiClient<BarcosDto> servicioBarcos { get; set; }
        [Inject] private IApiClient<BarcosTramitesDto> servicioBarcosTramites { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        //[CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public Guid? IdTramite { get; set; }

        private string AsignacionDestinoProxy
        {
            get => tramite.AsignacionDestino;
            set
            {
                if (value == tramite.AsignacionDestino) return;

                // <-- pon aquí tu breakpoint
                tramite.AsignacionDestino = value;

                if (!DebeMostrarSelectorEmpresas)
                    EmpresasSeleccionadas.Clear();

                if (!DebeMostrarSelectorPersonas)
                    PersonasSeleccionadas = new HashSet<PersonasDto>();

                ActualizarPersonasDisponibles();
                StateHasChanged(); // asegura render inmediato
            }
        }

        private TramiteDto tramite = new();
        private MudForm form = default!;
        private bool success;
        private string[] errors = { };
        private bool ModoAlta = true;

        private const string DestinoTodos = "Todos";
        private const string DestinoEmpresas = "Empresas";
        private const string DestinoPersonasEmpresa = "Personas de una empresa";
        private const string DestinoPersonasSueltas = "Personas sueltas";

        private List<EmpresasDto> EmpresasDisponibles { get; set; } = new();
        private List<PersonasDto> TodasLasPersonas { get; set; } = new();
        private List<PersonasDto> PersonasDisponibles { get; set; } = new();
        private HashSet<string> EmpresasSeleccionadas { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<PersonasDto> PersonasSeleccionadas { get; set; } = new();

        private bool DebeMostrarSelectorEmpresas => EsDestinoEmpresas() || EsDestinoPersonasEmpresa();
        private bool DebeMostrarSelectorPersonas => EsDestinoPersonasEmpresa() || EsDestinoPersonasSueltas();
        private bool DebeDeshabilitarSelectorPersonas => EsDestinoPersonasEmpresa() && !EmpresasSeleccionadas.Any();

        protected override async Task OnInitializedAsync()
        {
            tramite.AsignacionDestino = string.IsNullOrWhiteSpace(tramite.AsignacionDestino)
                ? DestinoTodos
                : tramite.AsignacionDestino;

            await CargarDatosIniciales();

            if (IdTramite.HasValue && IdTramite.Value != Guid.Empty)
            {
                await CargarTramite();
            }

            SincronizarSeleccionesDesdeTramite();
            ActualizarPersonasDisponibles();
        }

        private async Task CargarTramite()
        {
            tramite = await servicioTramite.GetByIdAsync("api/Tramites", IdTramite) ?? new TramiteDto();
            tramite.AsignacionDestino = string.IsNullOrWhiteSpace(tramite.AsignacionDestino)
                ? DestinoTodos
                : tramite.AsignacionDestino;
            ModoAlta = false;
        }

        private async Task CargarDatosIniciales()
        {
            var empresasTask = servicioEmpresas.GetAllAsync("api/Empresa");
            var personasTask = servicioPersonas.GetAllAsync("api/Personas");

            var empresas = await empresasTask;
            EmpresasDisponibles = empresas?.OrderBy(e => e.Empresa).ToList() ?? new List<EmpresasDto>();

            var personas = await personasTask;
            TodasLasPersonas = personas?.OrderBy(p => p.Nombre)
                                       .ThenBy(p => p.Apellido1)
                                       .ThenBy(p => p.Apellido2)
                                       .ToList() ?? new List<PersonasDto>();
        }

        private void SincronizarSeleccionesDesdeTramite()
        {
            EmpresasSeleccionadas = tramite.EmpresasAsignadas?.Any() == true
                ? new HashSet<string>(tramite.EmpresasAsignadas, StringComparer.OrdinalIgnoreCase)
                : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var personasAsignadas = tramite.PersonasAsignadas?.Where(codigo => !string.IsNullOrWhiteSpace(codigo)).ToHashSet(StringComparer.OrdinalIgnoreCase)
                ?? new HashSet<string>();

            PersonasSeleccionadas = TodasLasPersonas
                .Where(p => !string.IsNullOrWhiteSpace(p.CodigoPersona) && personasAsignadas.Contains(p.CodigoPersona))
                .ToHashSet();

            SincronizarModeloConSelecciones();
        }

        private void ActualizarPersonasDisponibles()
        {
            IEnumerable<PersonasDto> personas = TodasLasPersonas;

            if (EsDestinoPersonasEmpresa())
            {
                if (EmpresasSeleccionadas.Any())
                {
                    personas = personas.Where(p => p.EmpresasVinculadas != null &&
                        p.EmpresasVinculadas.Any(ev => !string.IsNullOrWhiteSpace(ev.CodigoEmpresa) && EmpresasSeleccionadas.Contains(ev.CodigoEmpresa)));
                }
                else
                {
                    personas = Enumerable.Empty<PersonasDto>();
                }
            }

            PersonasDisponibles = personas.ToList();

            var codigosSeleccionados = PersonasSeleccionadas
                .Where(p => !string.IsNullOrWhiteSpace(p.CodigoPersona))
                .Select(p => p.CodigoPersona!)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            PersonasSeleccionadas = PersonasDisponibles
                .Where(p => !string.IsNullOrWhiteSpace(p.CodigoPersona) && codigosSeleccionados.Contains(p.CodigoPersona))
                .ToHashSet();

            SincronizarModeloConSelecciones();
        }

        private void SincronizarModeloConSelecciones()
        {
            if (EsDestinoEmpresas() || EsDestinoPersonasEmpresa())
            {
                tramite.EmpresasAsignadas = EmpresasSeleccionadas.ToList();
            }
            else
            {
                tramite.EmpresasAsignadas = new List<string>();
            }

            if (EsDestinoPersonasEmpresa() || EsDestinoPersonasSueltas())
            {
                tramite.PersonasAsignadas = PersonasSeleccionadas
                    .Where(p => !string.IsNullOrWhiteSpace(p.CodigoPersona))
                    .Select(p => p.CodigoPersona!)
                    .Distinct()
                    .ToList();
            }
            else
            {
                tramite.PersonasAsignadas = new List<string>();
            }
        }

        private bool EsDestinoEmpresas() => string.Equals(tramite.AsignacionDestino, DestinoEmpresas, StringComparison.OrdinalIgnoreCase);
        private bool EsDestinoPersonasEmpresa() => string.Equals(tramite.AsignacionDestino, DestinoPersonasEmpresa, StringComparison.OrdinalIgnoreCase);
        private bool EsDestinoPersonasSueltas() => string.Equals(tramite.AsignacionDestino, DestinoPersonasSueltas, StringComparison.OrdinalIgnoreCase);

        private Task OnAsignacionDestinoChanged(string value)
        {
            tramite.AsignacionDestino = value;

            if (!DebeMostrarSelectorEmpresas)
            {
                EmpresasSeleccionadas.Clear();
            }

            if (!DebeMostrarSelectorPersonas)
            {
                PersonasSeleccionadas = new HashSet<PersonasDto>();
            }

            ActualizarPersonasDisponibles();
            return Task.CompletedTask;
        }

        //private Task OnEmpresasSeleccionadasChanged(IEnumerable<string> valores)
        private Task OnEmpresasSeleccionadasChanged(IEnumerable<string> valores)
        {
            EmpresasSeleccionadas = valores?.ToHashSet(StringComparer.OrdinalIgnoreCase)
                ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            ActualizarPersonasDisponibles();
            return Task.CompletedTask;
        }

        private Task OnPersonasSeleccionadasChanged(IEnumerable<PersonasDto> valores)
        {
            PersonasSeleccionadas = valores?.ToHashSet()
                ?? new HashSet<PersonasDto>();
            SincronizarModeloConSelecciones();
            return Task.CompletedTask;
        }

        private Task<IEnumerable<PersonasDto>> BuscarPersonas(string value, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<PersonasDto>>(cancellationToken);
            }

            IEnumerable<PersonasDto> personas = PersonasDisponibles;

            if (!string.IsNullOrWhiteSpace(value))
            {
                personas = personas.Where(p =>
                    (!string.IsNullOrWhiteSpace(p.Nombre) && p.Nombre.Contains(value, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(p.Apellido1) && p.Apellido1.Contains(value, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(p.Apellido2) && p.Apellido2.Contains(value, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(p.CodigoPersona) && p.CodigoPersona.Contains(value, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(p.NIFT) && p.NIFT.Contains(value, StringComparison.OrdinalIgnoreCase)));
            }

            return Task.FromResult(personas.Take(50));
        }

        private string GetDescripcionPersona(PersonasDto persona)
        {
            if (persona == null)
            {
                return string.Empty;
            }

            var partesNombre = new[] { persona.Nombre, persona.Apellido1, persona.Apellido2 }
                .Where(p => !string.IsNullOrWhiteSpace(p));

            var nombreCompleto = string.Join(" ", partesNombre);
            var codigo = string.IsNullOrWhiteSpace(persona.CodigoPersona) ? string.Empty : persona.CodigoPersona;

            if (string.IsNullOrWhiteSpace(codigo))
            {
                return nombreCompleto;
            }

            return string.IsNullOrWhiteSpace(nombreCompleto)
                ? codigo
                : $"{codigo} - {nombreCompleto}";
        }

        private string GetEtiquetaEmpresa(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return string.Empty;
            }

            var empresa = EmpresasDisponibles.FirstOrDefault(e => string.Equals(e.CodigoEmpresa, codigo, StringComparison.OrdinalIgnoreCase));
            return empresa == null ? codigo : $"{empresa.CodigoEmpresa} - {empresa.Empresa}";
        }


        //private async void Submit()
        private async Task GuardarCambios()
        {
            SincronizarModeloConSelecciones();

            ResponseDto respuesta = new();
            await form.Validate();
            if (!form.IsValid)
            {
                Snackbar.Add("Por favor, corrige los errores del formulario.", Severity.Warning);
                return;
            }

            try
            {
                if (ModoAlta && tramite.Id == Guid.Empty)
                {
                    tramite.Id = Guid.NewGuid();
                }

                if (IdTramite != null)
                {
                    respuesta = await servicioTramite.UpdateAsync("api/tramites", tramite, IdTramite);
                }
                else
                {
                    respuesta = await servicioTramite.CreateAsync("api/tramites", tramite);
                }

                if (respuesta.Success)
                {
                    var esAlta = ModoAlta;
                    if (ModoAlta)
                    {
                        IdTramite = tramite.Id;
                        ModoAlta = false;
                    }

                    await AsignarTramiteAEmpresasAsync();
                    Snackbar.Add(esAlta ? "Trámite creado correctamente." : "Trámite actualizado correctamente", Severity.Success);
                    //MudDialog.Close(DialogResult.Ok(true)); // Cerrar diálogo con éxito
                }

            }
            catch (Exception ex)
            {
                Snackbar.Add($"Ocurrió un error inesperado: {ex.Message}", Severity.Error);
            }
        }

        //private void Cancel() => MudDialog.Cancel();
        private void QuitarEmpresa(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) return;
            if (EmpresasSeleccionadas.Remove(codigo))
            {
                ActualizarPersonasDisponibles(); // si dependes de empresas -> personas
                StateHasChanged();
            }
        }
        private async Task AsignarTramiteAEmpresasAsync()
        {
            if ((!EsDestinoEmpresas() && !EsDestinoPersonasEmpresa()) || !EmpresasSeleccionadas.Any())
            {
                return;
            }

            var nombreTramite = tramite.NombreTramite?.Trim();
            if (string.IsNullOrWhiteSpace(nombreTramite))
            {
                Snackbar.Add("No se pudo asignar el trámite porque no tiene nombre.", Severity.Warning);
                return;
            }

            foreach (var codigoEmpresa in EmpresasSeleccionadas)
            {
                if (string.IsNullOrWhiteSpace(codigoEmpresa))
                {
                    continue;
                }

                IEnumerable<EmpresasDto> empresas;

                try
                {
                    var filtroEmpresa = new Dictionary<string, string>
                    {
                        { nameof(EmpresasDto.CodigoEmpresa), codigoEmpresa }
                    };

                    empresas = (await servicioEmpresas.GetAllAsync("api/Empresa", filtroEmpresa, new[] { "Barco" }));
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"No se pudieron obtener los barcos de la empresa {codigoEmpresa}: {ex.Message}", Severity.Warning);
                    continue;
                }

                foreach (var barco in empresas)
                {
                    if (barco == null || string.IsNullOrWhiteSpace(barco.Barco.CodigoBarco))
                    {
                        continue;
                    }

                    bool yaAsignado;
                    try
                    {
                        var filtroAsignaciones = new Dictionary<string, string>
                        {
                            { nameof(BarcosTramitesDto.CodigoBarco), barco.Barco.CodigoBarco },
                            { nameof(BarcosTramitesDto.CodigoEmpresa), codigoEmpresa },
                            { nameof(BarcosTramitesDto.Certificado), nombreTramite }
                        };

                        var existentes = await servicioBarcosTramites.GetAllAsync("api/BarcosTramites", filtroAsignaciones);
                        yaAsignado = existentes.Any();
                    }
                    catch (Exception ex)
                    {
                        Snackbar.Add($"No se pudo comprobar la asignación para el barco {barco.CodigoBarco}: {ex.Message}", Severity.Warning);
                        continue;
                    }

                    if (yaAsignado)
                    {
                        continue;
                    }

                    var nuevoTramiteBarco = new BarcosTramitesDto
                    {
                        CodigoBarco = barco.Barco.CodigoBarco,
                        CensoBarco = barco.Barco.Censo.Value,
                        Certificado = nombreTramite,
                        CodigoEmpresa = codigoEmpresa,
                        DiasAvisoTramite = 0,
                        FechaAviso = default,
                        FechaFin = default
                    };

                    if (tramite.FechaFin.HasValue)
                    {
                        var fechaFin = DateOnly.FromDateTime(tramite.FechaFin.Value);
                        nuevoTramiteBarco.FechaFin = fechaFin;
                        nuevoTramiteBarco.FechaAviso = fechaFin;
                    }

                    try
                    {
                        var respuesta = await servicioBarcosTramites.CreateAsync("api/BarcosTramites", nuevoTramiteBarco);
                        if (!respuesta.Success)
                        {
                            Snackbar.Add($"No se pudo asignar el trámite al barco {barco.CodigoBarco}: {respuesta.Message}", Severity.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        Snackbar.Add($"Error al asignar el trámite al barco {barco.CodigoBarco}: {ex.Message}", Severity.Error);
                    }
                }
            }
        }
    }
}