using System.ComponentModel.DataAnnotations;

namespace FornecedoresApp.Models
{
  public enum Segmento { Comercio, Servico, Industria }

  public class Fornecedor
  {
    [Key]
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required, RegularExpression(@"^\d{14}$", ErrorMessage = "CNPJ deve ter 14 dígitos.")]
    public string Cnpj { get; set; } = string.Empty;

    [Required]
    public Segmento Segmento { get; set; }

    [Required, RegularExpression(@"^\d{8}$", ErrorMessage = "CEP deve ter 8 dígitos.")]
    public string Cep { get; set; } = string.Empty;

    [Required, StringLength(255)]
    public string Endereco { get; set; } = string.Empty;

    public string? FotoPath { get; set; }
  }
}
