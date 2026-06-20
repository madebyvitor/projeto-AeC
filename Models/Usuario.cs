using System.ComponentModel.DataAnnotations;

namespace projetoAeC.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string UsuarioNome { get; set; } = string.Empty;

    [Required]
    public string SenhaHash { get; set; } = string.Empty;
}
