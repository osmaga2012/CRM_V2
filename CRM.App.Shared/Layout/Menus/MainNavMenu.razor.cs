using CRM.Dtos;
using CRM.App.Shared.Interfaces;
using CRM.Dtos.Enums;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.App.Shared.Layout.Menus
{
    public partial class MainNavMenu : ComponentBase
    {
        [Inject] private ICurrentUserService currentUserService { get; set; }
        [CascadingParameter] private Task<AuthenticationState> authenticationStateTask { get; set; }

        private UsuarioDto? _loggedInUser;

        // Propiedades computadas para limpiar el HTML
        private bool EsAdmin => _loggedInUser?.TipoUsuario == TipoUsuarioDto.Administrador;
        private bool EsEmpresa => _loggedInUser?.TipoUsuario == TipoUsuarioDto.Empresa;
        private bool EsGestion => EsAdmin || EsEmpresa;

        private bool collapseNavMenu = true;
        private bool ocultarItem = false;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationStateTask;
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                // El await ya gestiona la espera necesaria sin bloquear el hilo
                _loggedInUser = await currentUserService.GetCurrentUserAsync(user);

                // Notificamos a Blazor que los datos han llegado y debe repintar
                StateHasChanged();
            }
        }

    }
}
