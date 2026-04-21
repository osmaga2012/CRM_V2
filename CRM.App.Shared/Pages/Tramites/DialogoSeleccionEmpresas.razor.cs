using CRM.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Charts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.App.Shared.Pages.Tramites
{
    public partial class DialogoSeleccionEmpresas : ComponentBase
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        [Parameter] public List<EmpresasDto> EmpresasYaSeleccionadas { get; set; } = new ();
        [Parameter] public List<BarcosDto> BarcosYaSeleccionados { get; set; } = new();

        private MudAutocomplete<BarcosDto> _autocomplete;
        private BarcosDto _barcoSeleccionado;
        private EmpresasDto _empresaSeleccionada;

        private List<EmpresasDto> _totalEmpresas = new();
        private List<EmpresasDto> _tempSeleccionEmpresas = new();   
        private List<BarcosDto> _totalBarcos = new();
        private List<BarcosDto> _tempSeleccionBarcos = new();

        protected override async Task OnInitializedAsync()
        {
            // Cargar datos desde servicios
            string[] includesEmpresas = new string[] { "Barco" };
            string[] includeEmpresaBarco = new string[] { "Empresa" };
            _totalEmpresas = (await servicioEmpresas.GetAllAsync("api/Empresa", null, includesEmpresas))?.ToList() ?? new();
            //_totalBarcos = (await servicioBarcos.GetAllAsync("api/Barcos", null, includeEmpresaBarco))?.ToList() ?? new();



            // Inicializar selección
            _tempSeleccionEmpresas = EmpresasYaSeleccionadas?.ToList() ?? new();
            _tempSeleccionBarcos = BarcosYaSeleccionados?.ToList() ?? new();
        }

        private Task<IEnumerable<EmpresasDto>> BuscarEmpresas(string value, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<EmpresasDto>>(cancellationToken);
            }

            // Filtrar valores nulos
            IEnumerable<EmpresasDto> resultados = _totalEmpresas.Where(e => e != null);

            if (!string.IsNullOrWhiteSpace(value))
            {
                var searchTerm = value.Trim();
                resultados = resultados.Where(e =>
                    (!string.IsNullOrWhiteSpace(e.Empresa) &&
                     e.Empresa.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(e.CodigoEmpresa) &&
                     e.CodigoEmpresa.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) || 
                    (!string.IsNullOrWhiteSpace(e.CodigoBarco.ToString()) && 
                     e.CodigoBarco.ToString().Contains(searchTerm,StringComparison.OrdinalIgnoreCase)) ||
                    (e.NombreB != null && e.NombreB.Contains(searchTerm,StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            // Filtrar los que ya están seleccionados
            var codigosSeleccionados = _tempSeleccionEmpresas
                .Select(e => e.CodigoEmpresa)
                .ToHashSet();

            resultados = resultados.Where(e => !codigosSeleccionados.Contains(e.CodigoEmpresa));
            return Task.FromResult(resultados.Take(100));
        }

        private Task<IEnumerable<BarcosDto>> BuscarBarcos(string value, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<IEnumerable<BarcosDto>>(cancellationToken);
            }

            IEnumerable<BarcosDto> resultados = _totalBarcos;

            // Para debug: descomentar para ver cuántos barcos hay
            // Console.WriteLine($"Total de barcos disponibles: {_totalBarcos.Count}");
            // Console.WriteLine($"Búsqueda con valor: '{value}'");

            if (!string.IsNullOrWhiteSpace(value))
            {
                var searchTerm = value.Trim();
                resultados = _totalBarcos.Where(b =>
                    (!string.IsNullOrWhiteSpace(b.NombreB) &&
                     b.NombreB.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (b.CodigoBarco.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(b.Matricula) &&
                     b.Matricula.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(b.CodigoEmpresa) &&
                     b.CodigoEmpresa.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (GetNombreEmpresa(b.CodigoEmpresa)?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();

                // Para debug: descomentar para ver resultados
                // Console.WriteLine($"Resultados encontrados: {resultados.Count()}");
            }

            // Filtrar los que ya están seleccionados
            var codigosSeleccionados = _tempSeleccionBarcos
                .Select(b => b.CodigoBarco)
                .ToHashSet();

            resultados = resultados.Where(b => !codigosSeleccionados.Contains(b.CodigoBarco));

            return Task.FromResult(resultados.Take(100));
        }

        private Task<IEnumerable<EmpresasDto>> BuscarEmpresas1(string value, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Task.FromResult(_totalEmpresas.AsEnumerable());

            var resultados = _totalEmpresas.Where(e =>
                e.Empresa.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                e.CodigoEmpresa.Contains(value, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(resultados);
        }

        private async Task AsignarEmpresaABarco(BarcosDto barco, EmpresasDto empresa)
        {
            if (barco == null || empresa == null) return;

            // Actualizar la empresa en el barco
            barco.CodigoEmpresa = empresa.CodigoEmpresa;
            barco.Empresa = empresa;

            // Aquí puedes agregar la lógica para guardar los cambios en el servidor
            await servicioBarcos.UpdateAsync($"api/Barcos/{barco.CodigoBarco}", barco);

            StateHasChanged();
        }

        private async Task OnKeyDown(KeyboardEventArgs e)
        {
            // Verificar si la tecla presionada es "Enter"
            // if (e.Key == "Enter" && _barcoSeleccionado != null)
            // {
            //     // Agregar el barco seleccionado
            //     AgregarBarco(_barcoSeleccionado);
            // }
        }

        private void OnBarcoSeleccionado(BarcosDto barco)
        {
            _barcoSeleccionado = barco;

            if (barco != null)
            {
                AgregarBarco(barco.Empresa);
            }
        }

        private void OnEmpresaSeleccionada(EmpresasDto empresa)
        {
            _empresaSeleccionada = empresa;

            if (empresa != null)
            {
                AgregarBarco(empresa);
            }
        }

        private void AgregarBarco(EmpresasDto empresa)
        {
            if (empresa.Barco == null) return;

            // Obtenemos el código de forma segura usando el operador nulo (?.)
            var codigoBarcoNuevo = empresa.Barco.CodigoBarco;

            // Si el barco que intentamos agregar ni siquiera tiene código, podemos detenernos aquí.
            if (codigoBarcoNuevo ==0 || string.IsNullOrWhiteSpace(codigoBarcoNuevo.ToString())) return;

            // Verificar que no esté ya agregado (protegido contra elementos y propiedades nulas)
            var yaExiste = _tempSeleccionBarcos.Any(b =>
            {
                var codigoExistente = b?.CodigoBarco;

                return !string.IsNullOrWhiteSpace(codigoExistente.ToString()) &&
                       string.Equals(codigoExistente.ToString(), codigoBarcoNuevo.ToString(), StringComparison.OrdinalIgnoreCase);
            });

            if (!yaExiste)
            {
                empresa.Barco.Empresa = empresa; // Asegurar que el barco tenga el código de empresa actualizado
                _tempSeleccionBarcos.Add(empresa.Barco);
            }

            // Limpiar la selección y forzar actualización
            _barcoSeleccionado = null;
            StateHasChanged();
        }

        private void RemoverBarcoPorCodigo(MudChip<string> chip)
        {
            if (chip?.Value != null)
            {
                var barcoARemover = _tempSeleccionBarcos.FirstOrDefault(b =>
                    string.Equals(b.CodigoBarco.ToString(), chip.Value, StringComparison.OrdinalIgnoreCase));

                if (barcoARemover != null)
                {
                    _tempSeleccionBarcos.Remove(barcoARemover);
                    StateHasChanged();
                }
            }
        }

        private void LimpiarTodo()
        {
            _tempSeleccionBarcos.Clear();
            _barcoSeleccionado = null;
            StateHasChanged();
        }

        private string GetDescripcionBarco(BarcosDto barco)
        {
            if (barco == null) return string.Empty;

            var nombre = string.IsNullOrWhiteSpace(barco.NombreB) ? "Sin nombre" : barco.NombreB;
            var codigo = string.IsNullOrWhiteSpace(barco.CodigoBarco.ToString()) ? "" : $" ({barco.CodigoBarco})";

            return $"{nombre}{codigo}";
        }
        private string GetDescripcionBarco(EmpresasDto empresa)
        {
            if (empresa == null) return string.Empty;

            var nombre = string.IsNullOrWhiteSpace(empresa.Barco?.NombreB) ? "Sin nombre" : empresa.Barco.NombreB;
            var codigo = string.IsNullOrWhiteSpace(empresa.Barco?.CodigoBarco.ToString()) ? "" : $" ({empresa.Barco.CodigoBarco})";

            return $"{nombre}{codigo}";
        }

        private string GetNombreEmpresa(string codigoEmpresa)
        {
            if (string.IsNullOrWhiteSpace(codigoEmpresa)) return "Sin empresa";

            var empresa = _totalEmpresas.FirstOrDefault(e =>
                string.Equals(e.CodigoEmpresa, codigoEmpresa, StringComparison.OrdinalIgnoreCase));

            return empresa?.Empresa ?? codigoEmpresa;
        }

        private void Aceptar() => MudDialog.Close(DialogResult.Ok(_tempSeleccionBarcos));

        private void Cancelar() => MudDialog.Cancel();
    }
}

