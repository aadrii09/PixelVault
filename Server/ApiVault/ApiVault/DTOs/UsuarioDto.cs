namespace ApiVault.DTOs
{
    public class UsuarioDto
    {
        public int idUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public bool EsAdmin { get; set; }

    }
}
