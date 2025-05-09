using ApiVault.Data;
using ApiVault.DTOs;
using ApiVault.Interfaces;
using ApiVault.Models;
using ApiVault.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace ApiVault.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthService(ApplicationDbContext context, JwtHelper jwtHelper)
        {
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> RegisterAsync(RegistroDto registerDto)
        {
            var yaExiste = await _context.Usuarios.AnyAsync(u => u.Email == registerDto.Email);
            if (yaExiste) return "El usuario ya está registrado";


            var usuario = new Usuario
            {
                Nombre = registerDto.Nombre,
                Apellidos = registerDto.Apellidos,
                Email = registerDto.Email,
                FechaRegistro = DateTime.Now,
                EsAdmin = false,
                Direccion = "",
                Telefono = "",
                PasswordHashed = PasswordHashed.HashPassword(registerDto.Password),
            };

            // password hasher here

            usuario.Direccion = "";
            usuario.Telefono = "";
            usuario.EsAdmin = false;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return "Usuario registrado exitosamente";
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (usuario == null) return "Usuario no encontrado";

            if (!PasswordHashed.VerifyPassword(loginDto.Password, usuario.PasswordHashed))
                return "Contraseña incorrecta";

            // validación de contraseña pendiente aquí
            var token = _jwtHelper.GenerateToken(usuario);
            return token;
        }
    }
}
