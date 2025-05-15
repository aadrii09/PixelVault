using ApiVault.Data;
using ApiVault.DTOs;
using ApiVault.Interfaces;
using ApiVault.Models;
using ApiVault.Utilidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiVault.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public UsuarioService(ApplicationDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<bool?> ActualizarRolAsync(int usuarioId, bool esAdmin)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return null;

            // Si se va a quitar el rol de admin, comprobar si es el único
            if (usuario.EsAdmin && !esAdmin)
            {
                int adminCount = await _context.Usuarios.CountAsync(u => u.EsAdmin);
                if (adminCount == 1)
                    return false; // No permitir quitar el rol al único admin
            }

            usuario.EsAdmin = esAdmin;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool?> EliminarAsync(int usuarioId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                return null;

            // Si es admin, comprobar si es el único
            if (usuario.EsAdmin)
            {
                int adminCount = await _context.Usuarios.CountAsync(u => u.EsAdmin);
                if (adminCount == 1)
                    return false; // No permitir eliminar el único admin
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Usuario> GetUsuarioAsync(int usuarioId)
        {
            return await _context.Usuarios.FindAsync(usuarioId);
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<bool> IsUniqueUsuarioAsync(string email)
        {
            return !await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ActualizarAsync(int usuarioId, UsuarioDto usuario)
        {
            var usuarioDb = await _context.Usuarios.FindAsync(usuarioId);
            if (usuarioDb == null)
            {
                return false;
            }

            usuarioDb.Nombre = usuario.Nombre;
            usuarioDb.Apellidos = usuario.Apellidos;
            usuarioDb.Email = usuario.Email;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}