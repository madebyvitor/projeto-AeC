using System.ComponentModel.DataAnnotations;

namespace projetoAeC.ViewModels.Enderecos;

public class EnderecoViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o CEP.")]
    [StringLength(9, ErrorMessage = "O CEP deve ter no máximo 9 caracteres.")]
    [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "Informe um CEP válido.")]
    [Display(Name = "CEP")]
    public string Cep { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o logradouro.")]
    [StringLength(150, ErrorMessage = "O logradouro deve ter no máximo 150 caracteres.")]
    public string Logradouro { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o número.")]
    [StringLength(20, ErrorMessage = "O número deve ter no máximo 20 caracteres.")]
    [Display(Name = "Número")]
    public string Numero { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "O complemento deve ter no máximo 100 caracteres.")]
    public string? Complemento { get; set; }

    [Required(ErrorMessage = "Informe o bairro.")]
    [StringLength(100, ErrorMessage = "O bairro deve ter no máximo 100 caracteres.")]
    public string Bairro { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a cidade.")]
    [StringLength(100, ErrorMessage = "A cidade deve ter no máximo 100 caracteres.")]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a UF.")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "A UF deve ter 2 caracteres.")]
    [Display(Name = "UF")]
    public string Uf { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "O código IBGE deve ter no máximo 20 caracteres.")]
    [Display(Name = "IBGE")]
    public string? Ibge { get; set; }
}
