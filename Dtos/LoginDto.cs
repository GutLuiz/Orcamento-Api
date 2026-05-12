using System.ComponentModel.DataAnnotations;

namespace Orcamento.Dtos
{
    public class LoginDto
    {
        [Required] // campo obrigatorio
        [EmailAddress] // valida no formato de email
        public string Email { get; set; }

        [Required] // campo obnrigatorio
        public string Password { get; set; }
    }
}
