using System.ComponentModel.DataAnnotations;

namespace Orcamento.Models
{
    public class RegisterDto
    {
        [Required] // campo obrigatorio
        [EmailAddress] // valida formato de email
        public string Email { get; set; }

        [Required] // campo obrigatorio tambem
        [MinLength(6)] // senha minima
        public string Password { get; set; }
    }
}
