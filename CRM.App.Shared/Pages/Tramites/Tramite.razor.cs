
using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace CRM.App.Shared.Pages.Tramites
{
    public partial class Tramite: ComponentBase
    {
        [Parameter] public Guid? Id { get; set; }

        [Inject] private IApiClient<TramiteMasivoDto> servicioTramite { get; set; }
        [Inject] private IApiClient<EmpresasDto> servicioEmpresas { get; set; }
        [Inject] private IApiClient<PersonasDto> servicioPersonas { get; set; }
        [Inject] private IApiClient<BarcosDto> servicioBarcos { get; set; }
        [Inject] private IApiClient<BarcosTramitesDto> servicioBarcosTramites { get; set; }

        private bool _isEdit => Id.HasValue;
        private bool _success;
        private MudForm _form;

        private TramiteMasivoDto _masivoDto = new() { Tramite = new TramiteDto() };
        private List<EmpresasDto> _listaEmpresas = new();
        private IEnumerable<string> _empresasSeleccionadas = new HashSet<string>();

        private IEnumerable<EmpresasDto> _empresasObjetos = new HashSet<EmpresasDto>();

        // Función de búsqueda para el Autocomplete
        private async Task<IEnumerable<EmpresasDto>> SearchEmpresas(string value, CancellationToken token)
        {
            // Si la lista está vacía, intentamos cargarla de nuevo (por si falló el OnInitialized)
            if (_listaEmpresas == null || !_listaEmpresas.Any())
            {
                string[] includesEmpresas = new string[] { "Barco" };
                _listaEmpresas = (await servicioEmpresas.GetAllAsync("api/Empresa",null ,includesEmpresas)).ToList();
            }

            if (string.IsNullOrEmpty(value))
                return _listaEmpresas.AsEnumerable();

            return _listaEmpresas.Where(x =>
                x.Barco.NombreB.Contains(value, StringComparison.InvariantCultureIgnoreCase) ||
                x.Barco.CodigoBarco.Contains(value, StringComparison.InvariantCultureIgnoreCase) ||
                x.NombreArmador.Contains(value, StringComparison.InvariantCultureIgnoreCase) ||
                x.CodigoEmpresa.Contains(value, StringComparison.InvariantCultureIgnoreCase)).AsEnumerable();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                string[] includesEmpresas = new string[] { "Barco" };
                _listaEmpresas = (await servicioEmpresas.GetAllAsync("api/Empresa", null, includesEmpresas)).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar las empresas. {ex.Message}");
                throw;
            }

        }
        

        protected async Task OnInitializedAsync1()
        {
            if (_isEdit)
            {
                // Cargar trámite existente para editar
                _masivoDto.Tramite = await Http.GetFromJsonAsync<TramiteDto>($"api/Tramites/{Id}");
            }
            else
            {
                // Cargar lista de empresas para la asignación masiva
                _listaEmpresas = (await servicioEmpresas.GetAllAsync("api/Empresas")).ToList();
            }
        }

        private async Task Guardar()
        {
            await _form.Validate();
            if (!_form.IsValid) return;
            _masivoDto.EmpresasSeleccionadas = _empresasObjetos.Select(x => x.CodigoEmpresa).ToList();
            if (_isEdit)
            {
                await servicioTramite.UpdateAsync($"api/Tramites/{Id}", _masivoDto);
                Snackbar.Add("Trámite actualizado", Severity.Success);
            }
            else
            {
                //_masivoDto.EmpresasSeleccionadas = _empresasSeleccionadas.ToList();
                var response = await servicioTramite.CreateAsync("api/Tramites/masivo", _masivoDto);

                if (response.Success)
                    Snackbar.Add("Trámite masivo creado correctamente", Severity.Success);
                else
                    Snackbar.Add("Error al procesar la creación masiva", Severity.Error);
            }

            Volver();
        }

        private void Volver() => Nav.NavigateTo("/tramites");
    }
}
