using CRM.Dtos;
using CRM.Web.Shared.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace CRM.App.Shared.Pages.CentrosTrabajo.Usuarios
{
    public partial class CrearUsuario: ComponentBase
    {
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;

        //[Parameter] public BarcosDto barco { get; set; } // Parámetro para recibir el ID del barco
        [Parameter] public EmpresasDto empresa { get; set; }
        [Inject] private IApiClient<UsuarioDto> usuarioService { get; set; }

        private MudForm? form;
        private UsuarioDto usuario = new UsuarioDto();
        private bool success;
        private string[] errors = { };
        private BarcosDto? barcoSeleccionado;
        private List<UsuarioDto> usuariosEmpresa = new List<UsuarioDto>();
        private ClaimsPrincipal? currentUser;
        private bool assignAsPrincipalShip = true;
        private bool isLoadingUsers = true;

        protected override async Task OnInitializedAsync()
        {
            await CargarUsuariosBarco();

            //
            //usuario.EMail = empresa.NIFE;  //+ "@cofradiagandia.com";
            usuario.NIFAcceso = empresa.NIFE;

            StateHasChanged();
        }

        private async Task CargarUsuariosBarco()
        {

            //Dictionary<string,string> filtro = new Dictionary<string,string>();
            isLoadingUsers = false;


            try
            {
                Dictionary<string, string> filtro = new Dictionary<string, string>
                {
                    { "CodigoEmpresa", empresa.CodigoEmpresa }
                };

                var usuarios = await usuarioService.GetAllAsync("api/Usuarios", filtro);
                usuariosEmpresa = usuarios?.ToList() ?? new List<UsuarioDto>();
            }
            finally
            {
                isLoadingUsers = false;
                await InvokeAsync(StateHasChanged);
            }
            //filtro.Add("CodigoEmpresa", empresa.CodigoEmpresa);

           // usuariosEmpresa = (await usuarioService.GetAllAsync("api/Usuarios",filtro)).ToList();
            //isLoadingUsers = false;

            //StateHasChanged();
            
        }

        private async Task Guardar()
        {

            usuario.CodigoEmpresa = empresa.CodigoEmpresa;
            usuario.TipoUsuario = CRM.Dtos.Enums.TipoUsuarioDto.Empresa;

            var response = await usuarioService.CreateAsync("api/Usuarios/RegistrarUsuario", usuario);

            if (response.Success)
            {
                await CargarUsuariosBarco();
                StateHasChanged();
            }
        }
    }
}
