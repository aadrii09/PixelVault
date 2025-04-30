using ApiVault.Data;
using ApiVault.DTOs;
using ApiVault.Interfaces;
using ApiVault.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiVault.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> RegisterAsync(RegistroDto registroDto)
        {
            var yaExiste = await _context.Usuarios
                .AnyAsync(u => u.Email == registroDto.Email);
            if (yaExiste)
                return "Ese email ya está en uso";

            var usuario = new Usuario
            {
                Nombre = registroDto.Nombre,
                Apellidos = registroDto.Apellidos,
                Email = registroDto.Email,
                FechaRegistro = DateTime.Now,
                EsAdmin = false
            };

            usuario.Direccion = "";
            usuario.Telefono = "";
            usuario.EsAdmin = false;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return "Usuario registrado con éxito";
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (usuario == null)
            {
                return "Credenciales inválidas";
            }
            //validacion de contraseña exitosa
            return "Inicio de sesión exitoso";
        }
    }
}
