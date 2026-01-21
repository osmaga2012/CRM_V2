using CRM.Dtos.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.App.Shared.Layout.Menus
{
    public partial class MainNavMenu : ComponentBase
    {
        [Parameter] public TipoUsuarioDto TipoUsuario { get; set; }
        [Parameter] public EventCallback ToggleDarkMode { get; set; }
        [Parameter] public bool IsDarkMode { get; set; }

        private bool collapseNavMenu = true;
        private bool ocultarItem = false;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        private async Task OnToggleDarkModeClicked()
        {
            if (ToggleDarkMode.HasDelegate)
            {
                await ToggleDarkMode.InvokeAsync();
            }
        }

    }
}
