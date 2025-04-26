using ApiVault.DTOs;

namespace ApiVault.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegistroDto registroDto);
        Task<string> LoginAsync(LoginDto loginDto);

    }
}
