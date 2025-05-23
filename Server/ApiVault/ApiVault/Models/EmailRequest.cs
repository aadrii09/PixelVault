using System.ComponentModel.DataAnnotations;

namespace ApiVault.Models
{
    public class EmailRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
