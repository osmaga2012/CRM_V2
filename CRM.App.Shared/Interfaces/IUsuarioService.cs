using CRM.Dtos;
using CRM.App.Shared.Dtos;
using UsuarioDto = CRM.Dtos.UsuarioDto;

namespace CRM.App.Shared.Interfaces
{
    public interface IUsuarioService
    {
        Task<PerfilDto?> VerificarPerfilAsync();
        Task<CRM.Dtos.UsuarioDto?> ObtenerPerfilUsuarioAsync();
        Task<CRM.Dtos.UsuarioDto> CrearUsuario(UsuarioDto usuario);
    }
}
