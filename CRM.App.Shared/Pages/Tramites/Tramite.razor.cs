
using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;

namespace CRM.App.Shared.Pages.Tramites
{
    public partial class Tramite: ComponentBase
    {
        [Parameter] public Guid? Id { get; set; }
        [Inject] private IDialogService MyDialogService { get; set; } // Nombre de la instancia
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
        private IEnumerable<EmpresasDto> _empresasSeleccionadas = new HashSet<EmpresasDto>();

        private IEqualityComparer<EmpresasDto> _comparer = new GenericComparer();

        // Clase auxiliar para que MudBlazor sepa comparar tus DTOs por código
        internal class GenericComparer : IEqualityComparer<EmpresasDto>
        {
            public bool Equals(EmpresasDto x, EmpresasDto y) => x?.CodigoEmpresa == y?.CodigoEmpresa;
            public int GetHashCode(EmpresasDto obj) => obj.CodigoEmpresa?.GetHashCode() ?? 0;
        }

        // Función de búsqueda para el Autocomplete
        private async Task<IEnumerable<EmpresasDto>> SearchEmpresas(string value, CancellationToken token)
        {
            if (_listaEmpresas == null || !_listaEmpresas.Any())
            {
                string[] includesEmpresas = new string[] { "Barco" };
                var result = await servicioEmpresas.GetAllAsync("api/Empresa", null, includesEmpresas);
                _listaEmpresas = result.ToList();
            }

            // 1. Obtenemos los códigos que ya están seleccionados
            var codigosSeleccionados = _empresasSeleccionadas.Select(s => s.CodigoEmpresa).ToList();

            // 2. Filtramos la lista base: que NO estén en los seleccionados
            var query = _listaEmpresas.Where(x => !codigosSeleccionados.Contains(x.CodigoEmpresa));

            // 3. Aplicamos la búsqueda por texto si hay valor
            if (!string.IsNullOrEmpty(value))
            {
                query = query.Where(x =>
                    (x.Barco?.NombreB?.Contains(value, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                    (x.Barco?.CodigoBarco?.Contains(value, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                    (x.NombreArmador?.Contains(value, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
                    (x.CodigoEmpresa?.Contains(value, StringComparison.InvariantCultureIgnoreCase) ?? false));
            }

            return query.ToList();
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
            _masivoDto.EmpresasSeleccionadas = _empresasSeleccionadas.Select(x => x.CodigoEmpresa).ToList();
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

        private async Task AbrirSelectorEmpresas()
        {
            var parameters = new DialogParameters<DialogoSeleccionEmpresas>
            {
                { x => x.EmpresasYaSeleccionadas, _empresasSeleccionadas.ToList() }
            };

            var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true, CloseButton = true };
            var dialog = await IDialogService.ShowAsync<DialogoSeleccionEmpresas>("Seleccionar Empresas", parameters, options);
            //var dialog = DialogService.ShowAsync<("Editar Tramite", parameters, options);

            var result = await dialog.Result;

            if (!result.Canceled && result.Data is IEnumerable<EmpresasDto> seleccion)
            {
                _empresasSeleccionadas = new HashSet<EmpresasDto>(seleccion);
                StateHasChanged();
            }
        }

        private void QuitarEmpresa(EmpresasDto emp)
        {
            _empresasSeleccionadas.Remove(emp);
        }
    }
}
