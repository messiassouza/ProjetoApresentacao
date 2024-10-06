using System.ComponentModel.DataAnnotations;

namespace ProjetoApresentacao.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        public string? Descricao { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; }
    }
}
