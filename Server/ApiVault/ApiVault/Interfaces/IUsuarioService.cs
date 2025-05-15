using ApiVault.DTOs;
using ApiVault.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiVault.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetUsuariosAsync();
        Task<Usuario> GetUsuarioAsync(int usuarioId);
        Task<bool> IsUniqueUsuarioAsync(string email);       
        Task<bool?> EliminarAsync(int usuarioId); 
        Task<bool?> ActualizarRolAsync(int usuarioId, bool esAdmin); 
        Task<bool> ActualizarAsync(int usuarioId, UsuarioDto usuario);
    }
}