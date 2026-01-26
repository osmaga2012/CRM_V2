using CRM.App.Shared.Interfaces;
using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace CRM.App.Shared.Pages.CentrosTrabajo
{
    public partial class Tramites: ComponentBase
    {
        [Inject] private IPlatformNavigationService NavigationManager { get; set; }
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        [Inject] private ISnackbar Snackbar { get; set; }
        [Inject] private IApiClient<TramiteDto> tramiteService { get; set; }

        [Parameter] public Guid? Id { get; set; }

        private IQueryable<TramiteDto>? lstTramites;
        private string SearchString = string.Empty;


        // Propiedad que QuickGrid usará para mostrar los datos filtrados
        protected IQueryable<TramiteDto>? FiltroTramites =>
            string.IsNullOrWhiteSpace(SearchString)
                ? lstTramites
                : lstTramites?.Where(t => t.NombreTramite.Contains(SearchString));

        protected override async Task OnInitializedAsync()
        {
            await CargarTramites(); 
            StateHasChanged();
        }

        private async Task CargarTramites()
        {

            lstTramites = (await tramiteService.GetAllAsync("/api/Tramites")).AsQueryable();
            Snackbar.Add("Trámites cargados con éxito.", Severity.Success);
        }

    }
}
