using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace CRM.App.Shared.Pages.Cofradia
{
    public partial class DetalleCofradia : ComponentBase
    {
        //[Parameter] private string id { get; set; } = string.Empty;
        [Inject] private IApiClient<CofradiasDto> cofradiaService { get; set; }
        [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; } // Acceso al estado de autenticación


        private CofradiasDto? cofradia;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var usuario = (await authenticationStateTask).User;
                cofradia = (await cofradiaService.GetAllAsync("/api/Cofradias")).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error al cargar los datos: {ex.Message}", Severity.Error);
            }
        }

        private void Volver()
        {
            //NavigationManager.NavigateTo("/");
        }
    }
}
