using System.ComponentModel.DataAnnotations;

namespace Orcamento.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null;

        public int UserId { get; set; }
        public User ? User { get; set; }
        public List<Transaction> Transactions { get; set; } = new();
    }
}
