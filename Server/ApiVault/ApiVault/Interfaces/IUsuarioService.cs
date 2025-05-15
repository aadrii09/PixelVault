using ApiVault.DTOs;
using ApiVault.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiVault.Interfaces
{
    public interface IUsuarioService
    {
        // Obtener todos los usuarios (para administradores)
        Task<IEnumerable<Usuario>> GetUsuariosAsync();

        // Obtener un usuario específico por ID
        Task<Usuario> GetUsuarioAsync(int usuarioId);

        // Verificar si un email ya está en uso
        Task<bool> IsUniqueUsuarioAsync(string email);       

        // Eliminar un usuario
        Task<bool> EliminarAsync(int usuarioId);

        // Cambiar el rol de un usuario (administrador o no)
        Task<bool> ActualizarRolAsync(int usuarioId, bool esAdmin);

        Task<bool> ActualizarAsync(int usuarioId, UsuarioDto usuario);
    }
}
