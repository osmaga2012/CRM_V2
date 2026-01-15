using CRM.Dtos;
using System.Security.Claims;

namespace CRM.App.Shared.Interfaces
{
    public interface ICurrentUserService
    {
        Task<UsuarioDto> GetCurrentUserAsync(ClaimsPrincipal userPrincipal); // ¡Ahora devuelve UsuarioDto!
        void ClearCachedUser();
    }
}
