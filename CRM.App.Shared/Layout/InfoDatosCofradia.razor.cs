
using CRM.App.Shared.Interfaces;
using CRM.App.Shared.Services;
using CRM.Dtos;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace CRM.App.Shared.Layout
{
    public partial class InfoDatosCofradia : ComponentBase, IDisposable
    {

        ////[CascadingParameter] public Task<AuthenticationStateProvider> AuthenticationStateProvider { get; set; } = default!;
        [Inject] private CofradiaState CofradiaState { get; set; } = default!;
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
        [Inject] private ICurrentUserService CurrentUserService { get; set; } = default!;

        public AuthenticationState contexto { get; set; }
        protected CofradiasDto cofradia;
        protected string? Direccion;

        protected override void OnInitialized()
        {
            CofradiaState.OnChange += HandleCofradiaChanged;
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadCofradiaAsync();
        }

        private async Task LoadCofradiaAsync()
        {
            CofradiasDto cofradiaResuelta = new();

            try
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var usuarioAutenticado = authState.User;

                if (usuarioAutenticado?.Identity?.IsAuthenticated == true)
                {
                    var usuario = await CurrentUserService.GetCurrentUserAsync(usuarioAutenticado);
                    cofradiaResuelta = usuario?.Gestoria;
                }
            }
            catch
            {
                cofradiaResuelta = null;
            }

            cofradia = cofradiaResuelta;
            CofradiaState.SetCofradia(cofradiaResuelta);
            if (CofradiaState.Cofradia is not null)
            {
                cofradia = CofradiaState.Cofradia;
            }
            else
            {
                CofradiaState.Clear();
                cofradia = null;
            }
            StateHasChanged();
            UpdateDireccion();
        }

        private void HandleCofradiaChanged()
        {
            _ = InvokeAsync(() =>
            {
                cofradia = CofradiaState.Cofradia;
                UpdateDireccion();
                StateHasChanged();
                return Task.CompletedTask;
            });
        }

        private void UpdateDireccion()
        {
            Direccion = cofradia is null
                ? null
                : string.Join(" ", new[]
                {
                    cofradia.Calle,
                    cofradia.Numero,
                    cofradia.CP,
                    cofradia.Provincia,
                    cofradia.Pais
                }.Where(segment => !string.IsNullOrWhiteSpace(segment)));
        }

        public void Dispose()
        {
            //CofradiaState.OnChange -= HandleCofradiaChanged;
        }
    }
}
