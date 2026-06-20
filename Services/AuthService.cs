using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using projetoAeC.Data;
using projetoAeC.Models;
using projetoAeC.ViewModels.Auth;

namespace projetoAeC.Services;

public class AuthService
{
    private readonly ApplicationDbContext _context;
    private readonly PasswordHasher<Usuario> _passwordHasher = new();

    public AuthService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(bool Success, string? ErrorMessage)> RegistrarAsync(RegisterViewModel model)
    {
        var usuarioNormalizado = NormalizarUsuario(model.Usuario);

        var usuarioExiste = await _context.Usuarios
            .AnyAsync(usuario => usuario.UsuarioNome == usuarioNormalizado);

        if (usuarioExiste)
        {
            return (false, "Este usuário já está cadastrado.");
        }

        var usuario = new Usuario
        {
            Nome = model.Nome.Trim(),
            UsuarioNome = usuarioNormalizado
        };

        usuario.SenhaHash = _passwordHasher.HashPassword(usuario, model.Senha);

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return (true, null);
    }

    public async Task<Usuario?> ValidarCredenciaisAsync(LoginViewModel model)
    {
        var usuarioNormalizado = NormalizarUsuario(model.Usuario);

        var usuario = await _context.Usuarios
            .SingleOrDefaultAsync(usuario => usuario.UsuarioNome == usuarioNormalizado);

        if (usuario is null)
        {
            return null;
        }

        var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, model.Senha);

        return resultado == PasswordVerificationResult.Failed ? null : usuario;
    }

    private static string NormalizarUsuario(string usuario)
    {
        return usuario.Trim().ToLowerInvariant();
    }
}
