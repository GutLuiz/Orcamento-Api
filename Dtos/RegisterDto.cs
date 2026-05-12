using System.ComponentModel.DataAnnotations;

namespace Orcamento.Dtos
{
    public class RegisterDto
    {
        [Required] // campo obrigatorio
        [EmailAddress] // valida formato de email
        [MaxLength(150)] // maximo
        public string Email { get; set; }

        [Required] // campo obrigatorio tambem
        [MinLength(6)] // senha minima
        [MaxLength(100)] // maximo
        public string Password { get; set; }
    }
}
